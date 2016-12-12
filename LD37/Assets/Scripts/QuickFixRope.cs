using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickFixRope : MonoBehaviour {

	public float speed;
	public Transform goal;

	private new Rigidbody rigidbody;

	private Vector3 startPos;
	private float startTime;
	private float journeyLength;

	void Start () {
		rigidbody = this.GetComponent<Rigidbody> ();
		rigidbody.isKinematic = false;
		rigidbody.AddForce (-transform.right * 151f, ForceMode.Impulse);
		/*
		startPos = transform.position;
		startTime = Time.time;
		journeyLength = Vector3.Distance (startPos, goal.position);*/
	}

	void Update () {
		if (Time.time - startTime > 1f) {
			rigidbody.useGravity = true;
			this.enabled = false;
		}
	}

	/*
	void Update () {
		if (startPos != goal.position) {
			float distCovered = (Time.time - startTime) * speed;
			float fracJourney = distCovered / journeyLength;
			transform.position = Vector3.Lerp (startPos, goal.position, fracJourney);
		} else {
			this.enabled = false;
		}
	}*/
}
