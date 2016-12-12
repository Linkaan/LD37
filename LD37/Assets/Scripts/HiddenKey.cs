using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (Selectable))]
public class HiddenKey : MonoBehaviour {

	public enum ActionOnSelected {
		move_forward,
		move_right,
		rotate_around_hinge,
		disappear
	}

	private enum ActionState {
		original_state,
		perform_action,
		performing_action,
		performed_action
	}

	public float moveSpeed = 5f;
	public float angularSpeed = 4f;

	public float moveForwardAmount = 1f;
	public float rotateAroundHingeAngle = 90f;

	public bool interactable;

	public bool hasKey;
	public ActionOnSelected action;

	public LayerMask obstacleMask;

	private Selectable selectable;
	private Player player;

	private Transform hinge;

	private ActionState actionState;
	private Vector3 goalPos;

	private float goalRotAngle;
	private float currentAngle;

	private Vector3 startPos;
	private float startTime;
	private float journeyLength;

	private Vector3 initialPos;
	private float initialRot;

	private bool actionToggle;

	void Start () {
		selectable = this.GetComponent<Selectable> ();
		selectable.SetOnClickCallback (DoAction);
		player = Transform.FindObjectOfType<Player> ();
		actionState = ActionState.original_state;
		actionToggle = false;
		initialPos = this.transform.position;
		initialRot = this.transform.eulerAngles.y;

		hinge = this.transform.FindChild ("Hinge");
	}

	void Update () {
		if (!interactable)
			return;
		if (actionState == ActionState.perform_action) {
			switch (action) {
			case ActionOnSelected.move_forward:
			case ActionOnSelected.move_right:
				startPos = transform.position;
				startTime = Time.time;
				journeyLength = Vector3.Distance (startPos, goalPos);
				break;
			case ActionOnSelected.rotate_around_hinge:
				startTime = Time.time;
				break;
			default:
				Debug.LogError ("Action " + action + " not yet implemented");
				break;
			}
			actionState = ActionState.performing_action;
		}

		if (actionState == ActionState.performing_action) {
			switch (action) {
			case ActionOnSelected.move_forward:
				DoMoveForward ();
				break;
			case ActionOnSelected.rotate_around_hinge:
				DoRotateObject ();
				break;
			default:
				Debug.LogError ("Action " + action + " not yet implemented");
				break;
			}
		}
	}

	void DoRotateObject () {
		float diff = goalRotAngle - currentAngle;
		float toRotate = angularSpeed * diff * (Time.time - startTime);

		if ((actionToggle && currentAngle + toRotate > goalRotAngle)) { 
			toRotate = goalRotAngle - currentAngle;
		}

		transform.RotateAround (hinge.position, Vector3.up, toRotate);
		currentAngle = this.transform.rotation.eulerAngles.y;
		if (Mathf.Abs(currentAngle - goalRotAngle) <= 0.01f || Mathf.Abs (toRotate) == 360) {
			if (!actionToggle)
				actionState = ActionState.performed_action;
			else
				actionState = ActionState.original_state;
		}
	}

	void DoMoveForward() {
		
		float distCovered = (Time.time - startTime) * moveSpeed;
		float fracJourney = distCovered / journeyLength;
		transform.position = Vector3.Lerp (startPos, goalPos, fracJourney);

		if (transform.position == goalPos) {
			if (!actionToggle)
				actionState = ActionState.performed_action;
			else
				actionState = ActionState.original_state;
		}
	}

	void DoAction () {
		RaycastHit hit;

		selectable.isClickedOn = false;
		if (!interactable) {			
			return;
		}

		if (GetComponent<Renderer> () != null) {
			Vector3 final = GetComponent<Renderer> ().bounds.center;
			final.y -= 2;

			if (Physics.Raycast (final, transform.forward, 12, obstacleMask.value)) {
				/* can't move because there is an obstacle on this object */
				return;
			} else {
				Debug.Log ("NOTHING ON ME!!!!");
			}
		}

		Debug.Log ("PERFORM ACTION: " + actionState);
		switch (action) {
		case ActionOnSelected.move_forward:
			if (actionState == ActionState.performed_action) {
				goalPos = initialPos;
				actionToggle = true;
			} else {
				goalPos = initialPos + Vector3.back * moveForwardAmount;
				actionToggle = false;
			}
			actionState = ActionState.perform_action;
			break;
		case ActionOnSelected.rotate_around_hinge:
			if (actionState == ActionState.performed_action) {
				goalRotAngle = initialRot;
				actionToggle = true;
			} else {
				goalRotAngle = rotateAroundHingeAngle;
				actionToggle = false;
			}
			actionState = ActionState.perform_action;
			break;
		default:
			Debug.LogError ("Action " + action + " not yet implemented");
			break;				
		}
		if (hasKey) {
			/* Print message or something */
			//Key key = this.GetComponentInChildren<Key> ();
			//player.GiveKey (key);
		}
	}
}
