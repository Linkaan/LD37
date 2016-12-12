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

	/* Specific key to unlock door (optional) */
	public Key key;

	public float angularSpeed;
	public bool locked;
	public Transform hinge;

	private Selectable selectable;
	private DoorState state;

	private float currentDoorRotation;
	private float doorRotGoal;

	private Player player;

	void Start () {
		state = DoorState.closed;
		selectable = this.GetComponent<Selectable> ();
		currentDoorRotation = 0;
		player = Transform.FindObjectOfType<Player> ();

		selectable.SetPreClickCallback (player.CanMoveToNextRoom);
	}
	
	void Update () {
		if (state == DoorState.closed && selectable.isClickedOn) {
			if (locked) {
				UnlockDoor ();
				selectable.isClickedOn = false;
			} else {
				/* Open door */
				state = DoorState.opening;
				doorRotGoal = door_open_angle;
				selectable.SetSelected (false);
				player.MoveToNextRoom ();
			}
		}

		if (currentDoorRotation != doorRotGoal) {
			RotateDoor ();
		}
	}

	void RotateDoor () {
		float diff = doorRotGoal - currentDoorRotation;
		float toRotate = angularSpeed * diff * Time.deltaTime;

		if ((state == DoorState.opening && currentDoorRotation + toRotate > doorRotGoal) || (state == DoorState.closing && currentDoorRotation + toRotate < doorRotGoal)) { 
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

	public void UnlockDoor () {
		if (key != null) {
			if (key.keyId == player.GetKeyId()) {
				locked = false;
			}
		} else {
			locked = false;
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
