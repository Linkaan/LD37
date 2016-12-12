using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public enum PlayerState {
		idle,
		holding_object,
		moving_to_next_room,
		homer_is_ded
	}

	public SmoothMouseLook mouseScript;
	public Selector selector;
	public PlayerState state;
	public float speed;
	public float roomLength;

	public LayerMask raycastMask;
	public LayerMask obstacleMask;

	public Transform ground;

	public Room room;
	public Grabable holding;
	private Vector3 worldOffset;

	private Vector3 goal;
	private Vector3 initialRot;

	private Vector3 startPos;
	private float startTime;
	private float journeyLength;

	private float lastRotX;
	private float lastRotY;

	private Key key;

	void Start () {
		ChangeState (PlayerState.idle);
		initialRot = Camera.main.transform.eulerAngles;
	}
	
	void Update () {
		if (state == PlayerState.moving_to_next_room) {
			Camera.main.transform.eulerAngles = Vector3.Lerp (Camera.main.transform.eulerAngles, initialRot, Time.deltaTime);

			float distCovered = (Time.time - startTime) * speed;
			float fracJourney = distCovered / journeyLength;
			transform.position = Vector3.Lerp (startPos, goal, fracJourney);

			if (transform.position == goal) {
				RaycastHit hit;

				if (Physics.Raycast (transform.position, -transform.up, out hit, 12, raycastMask.value)) {
					room = hit.transform.GetComponentInParent<Room> ();
				} else {
					Debug.LogError ("Could not find room below me!");
				}
				mouseScript.inputEnabled = true;
				selector.inputEnabled = true;
				ChangeState (PlayerState.idle);
			}
		}

		if (state == PlayerState.holding_object) {
			if (Input.GetMouseButtonDown (0) || Input.GetKeyDown (KeyCode.Space)) {				
				ChangeState (PlayerState.idle);
				holding.Unselect ();
			} else {
				Vector3 newPos = holding.transform.position
				                + Vector3.forward * (lastRotY - mouseScript.rotationY)
				                + Vector3.right * (lastRotX - mouseScript.rotationX);
				if (newPos.x > room.x2)
					newPos.x = room.x2;
				if (newPos.x < room.x1 - worldOffset.x)
					newPos.x = room.x1 - worldOffset.x;
				if (newPos.z > room.y2)
					newPos.z = room.y2;
				if (newPos.z < room.y1 - worldOffset.y - room.length)
					newPos.z = room.y1 - worldOffset.y - room.length;

				holding.transform.position = newPos;
			}
			/*
			RaycastHit hit;

			Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
			Debug.DrawRay(ray.origin, ray.direction * 12);
			if (Physics.Raycast (ray, out hit, 12, raycastMask.value)) {
				holding.transform.position = /*holding.transform.InverseTransformPoint(hit.point;
			}
			*/
		}
		lastRotY = mouseScript.rotationY;
		lastRotX = mouseScript.rotationX;
	}

	public void GiveKey (Key key) {
		Debug.Log ("I CAN HAZ KEY!");
		this.key = key;
	}

	public string GetKeyId () {
		if (key == null)
			return null;
		return key.keyId;
	}

	public void ChangeState (PlayerState state) {
		Debug.Log ("change state to " + state);
		this.state = state;
		switch (state) {
		case PlayerState.holding_object:
			worldOffset = holding.GetComponent<Renderer>().bounds.size;
			break;
		default:
			break;
		}
	}

	public bool CanMoveToNextRoom () {
		float step = (Mathf.Abs (transform.position.y - ground.position.y) / 4);
		Vector3 pos1 = new Vector3 (transform.position.x, transform.position.y - step + step / 2, transform.position.z);
		Vector3 pos2 = new Vector3 (transform.position.x, transform.position.y - step * 2 + step / 2 , transform.position.z);
		Vector3 pos3 = new Vector3 (transform.position.x, transform.position.y - step * 3 + step / 2 , transform.position.z);
		Vector3 pos4 = new Vector3 (transform.position.x, transform.position.y - step * 4 + step / 2 , transform.position.z);

		//Debug.DrawRay (pos1, transform.forward * 12);
		//Debug.DrawRay (pos2, transform.forward * 12);
		//Debug.DrawRay (pos3, transform.forward * 12);
		//Debug.DrawRay (pos4, transform.forward * 12);

		return !(Physics.Raycast (pos1, transform.forward, 12, obstacleMask.value) ||
			Physics.Raycast (pos2, transform.forward, 12, obstacleMask.value) ||
			Physics.Raycast (pos3, transform.forward, 12, obstacleMask.value) ||
			Physics.Raycast (pos4, transform.forward, 12, obstacleMask.value));
	}

	public void MoveToNextRoom () {
		mouseScript.inputEnabled = false;
		selector.inputEnabled = false;
		startPos = transform.position;
		startTime = Time.time;
		goal = new Vector3 (transform.position.x, transform.position.y, transform.position.z - roomLength);
		journeyLength = Vector3.Distance (startPos, goal);
		state = PlayerState.moving_to_next_room;
	}
}
