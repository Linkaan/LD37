using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickFixKey : MonoBehaviour {

	public Transform goal;
	public float speed;

	private Vector3 goalPos;
	private Vector3 moveDir;

	// Use this for initialization
	void Start () {
		goalPos = goal.position;
		moveDir = (goalPos - transform.position).normalized * speed;
	}
	
	// Update is called once per frame
	void Update () {
		GetComponent<Rigidbody> ().MovePosition (transform.position + moveDir * Time.deltaTime);
		if (transform.position == goalPos) {
			this.enabled = false;
		}
	}
}
