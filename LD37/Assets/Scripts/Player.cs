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
	private Quaternion initialRot;

	private Vector3 startPos;
	private float startTime;
	private float journeyLength;

	private float lastRotX;
	private float lastRotY;

    private Vector3 screenPoint;
    private Vector3 offset;

    private DialogHandler handler;

    private string key;

	void Start () {
		ChangeState (PlayerState.idle);
		initialRot = Camera.main.transform.rotation;
        handler = Transform.FindObjectOfType<DialogHandler>();
	}
	
	void Update () {
		if (state == PlayerState.moving_to_next_room) {
			Camera.main.transform.rotation = Quaternion.Lerp (Camera.main.transform.rotation, initialRot, Time.deltaTime);

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
                if (mouseScript == null)
                {
                    mouseScript = Camera.main.gameObject.AddComponent<SmoothMouseLook>();
                    selector.inputEnabled = true;
                }
                selector.inputEnabled = true;
				ChangeState (PlayerState.idle);
			}
		} else if (handler.dialogState != DialogHandler.DialogState.do_nothing && handler.dialogState != DialogHandler.DialogState.shown_enddialog)
        {
            selector.inputEnabled = false;
            Destroy(mouseScript);
            mouseScript = null;
        } else
        {
            if (mouseScript == null)
            {
                mouseScript = Camera.main.gameObject.AddComponent<SmoothMouseLook>();
                selector.inputEnabled = true;
            }
        }

        if (state == PlayerState.holding_object) {
			if (Input.GetMouseButtonUp (0) || Input.GetKeyUp (KeyCode.Space)) {				
				ChangeState (PlayerState.idle);
				holding.Unselect ();
			} else {
                float y = holding.transform.position.y;
                Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);

                Vector3 newPos = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
                newPos.y = y;
               // Vector3 newPos = holding.transform.position
				 //               + Vector3.forward * (lastRotY - mouseScript.rotationY) * 1.5f
				   //             + Vector3.right * (lastRotX - mouseScript.rotationX) * 1.5f;
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
        if (mouseScript != null)
        {
            lastRotY = mouseScript.rotationY;
            lastRotX = mouseScript.rotationX;
        }
	}

    public void GiveKey (string key) {
		Debug.Log ("I CAN HAZ KEY!");
		this.key = key;
	}

	public string GetKeyId () {
		return key;
	}

	public void ChangeState (PlayerState state) {
		Debug.Log ("change state to " + state);
		this.state = state;
		switch (state) {
		case PlayerState.holding_object:
			worldOffset = holding.GetComponent<Renderer>().bounds.size;
            screenPoint = Camera.main.WorldToScreenPoint(holding.transform.position);
            offset = holding.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
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

        bool canMove = !(Physics.Raycast(pos1, transform.forward, 12, obstacleMask.value) ||
            Physics.Raycast(pos2, transform.forward, 12, obstacleMask.value) ||
            Physics.Raycast(pos3, transform.forward, 12, obstacleMask.value) ||
            Physics.Raycast(pos4, transform.forward, 12, obstacleMask.value));

        if (!canMove && state == PlayerState.idle)
            Transform.FindObjectOfType<UserNotifier>().ShowText("Hmm, seems like there is a obstacle in the way.", 2);

        return canMove;
	}

	public void MoveToNextRoom () {
		selector.inputEnabled = false;
        Destroy(mouseScript);
        mouseScript = null;
        startPos = transform.position;
		startTime = Time.time;
		goal = new Vector3 (transform.position.x, transform.position.y, transform.position.z - roomLength);
		journeyLength = Vector3.Distance (startPos, goal);
		state = PlayerState.moving_to_next_room;
	}
}
