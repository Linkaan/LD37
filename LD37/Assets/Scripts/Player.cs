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

	public GameObject holding;
	private Vector3 worldOffset;

	private Vector3 goal;
	private Vector3 initialRot;

	private Vector3 startPos;
	private float startTime;
	private float journeyLength;

	void Start () {
		state = PlayerState.idle;
		initialRot = Camera.main.transform.eulerAngles;
	}
	
	void Update () {
		if (state == PlayerState.moving_to_next_room) {
			Camera.main.transform.eulerAngles = Vector3.Lerp (Camera.main.transform.eulerAngles, initialRot, Time.deltaTime);

			float distCovered = (Time.time - startTime) * speed;
			float fracJourney = distCovered / journeyLength;
			transform.position = Vector3.Lerp (startPos, goal, fracJourney);

			if (transform.position == goal) {
				mouseScript.inputEnabled = true;
				selector.inputEnabled = true;
				state = PlayerState.idle;
			}
		}

		if (state == PlayerState.holding_object) {
			Vector3 pos = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0)) + worldOffset;
			Vector3 new_pos = new Vector3 (pos.x, holding.transform.position.y, pos.y);
			holding.transform.position = new_pos;
		}
	}

	public void ChangeState (PlayerState state) {
		this.state = state;
		switch (state) {
		case PlayerState.holding_object:
			Vector3 screenSpace = Camera.main.WorldToViewportPoint (holding.transform.position);
			worldOffset = holding.transform.position - Camera.main.ViewportToWorldPoint (new Vector3 (0.5f, 0.5f, screenSpace.z));
			break;
		default:
			break;
		}
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
