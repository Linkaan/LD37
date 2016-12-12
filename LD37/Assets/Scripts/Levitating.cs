using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Levitating : MonoBehaviour {

	public float height;
	public float levitationSpeed;

	private bool toggle;

	private float initHeight;

	void Start () {
		initHeight = transform.position.y;
	}

	void Update () {
		Vector3 final = transform.position;
		final.y = initHeight + height * Mathf.Sin (levitationSpeed * Time.time);
		transform.position = final;
	}
}
