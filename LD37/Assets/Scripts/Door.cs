using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {
	 
	private const int door_open_angle = 90;

	/* States of door */
	private enum DoorState {
		closing = 2<<0,
		closed = 2<<1,
		opening = 2<<2,
		open = 2<<3
	}

	public float angularSpeed;
	public bool locked;
	public Transform hinge;

	private Selectable selectable;
	private DoorState state;

	private float currentDoorRotation;
	private float doorRotGoal;

	private Vector3 targetVector;
	private Quaternion targetRotation;
	private Vector3 originalPosition;
	private Quaternion originalRotation;

	private Player player;

	void Start () {
		state = DoorState.closed;
		selectable = this.GetComponent<Selectable> ();
		currentDoorRotation = 0;
		player = Transform.FindObjectOfType<Player> ();

		originalPosition = transform.position;
		originalRotation = transform.rotation;

		transform.RotateAround(hinge.position, Vector3.up, door_open_angle);
		targetVector = transform.position;
		targetRotation = transform.rotation;

		transform.position = originalPosition;
		transform.rotation = originalRotation;
	}
	
	void Update () {
		if (!locked && state == DoorState.closed && selectable.isClickedOn) {
			/* Open door */
			state = DoorState.opening;
			doorRotGoal = door_open_angle;
			selectable.SetSelected (false);
			player.MoveToNextRoom ();
		}

		if (currentDoorRotation != doorRotGoal) {
			RotateDoor ();
		}
	}

	int abs (float num) {	
		if (num > 0)
			return 1;
		else if (num < 0)
			return -1;
		return 0;
	}

	void RotateDoor () {
		float diff = doorRotGoal - currentDoorRotation;

		float direction = abs (diff);
		float toRotate = angularSpeed * diff * Time.deltaTime;

		if ((state == DoorState.opening && currentDoorRotation + toRotate > doorRotGoal) || (state == DoorState.closing && currentDoorRotation + toRotate < doorRotGoal)) { 
			/*
			if (state == DoorState.opening) {
				transform.position = targetVector;
				transform.rotation = targetRotation;
			} else {
				transform.position = originalPosition;
				transform.rotation = originalRotation;
			}
			*/
			//currentDoorRotation = doorRotGoal;
			toRotate = doorRotGoal - currentDoorRotation;
		}
		
		transform.RotateAround (hinge.position, Vector3.up, toRotate);
		currentDoorRotation = this.transform.rotation.eulerAngles.y;
		if (state == DoorState.opening) {
			state = DoorState.open;
			selectable.isClickedOn = false;
		} else if (state == DoorState.closing) {
			state = DoorState.closed;
			selectable.isClickedOn = false;
			Debug.Log ("door is closed!");
		}
	}

	public void CloseDoor () {
		Debug.Log ("Closing door on you!");
		if (state == DoorState.open || state == DoorState.opening) {
			state = DoorState.closing;
			doorRotGoal = 0;
		}
	}
}
