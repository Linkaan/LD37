using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public enum PlayerState {
		idle,
		moving_to_next_room,
		homer_is_ded
	}

	public SmoothMouseLook mouseScript;
	public Selector selector;
	public PlayerState state;
	public float speed;
	public float roomLength;

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
