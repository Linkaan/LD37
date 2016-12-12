using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnRopeCut : MonoBehaviour {

	public GameObject armature;
	public Rigidbody keyBody;
	public Door door;

	public bool ropeCut = false;

	private new Rigidbody rigidbody;
	private bool hasBeenCut = false;

	private float startTime;

	// Use this for initialization
	void Start () {
		rigidbody = this.GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (ropeCut && !hasBeenCut) {			
			this.transform.parent = armature.transform;
			Destroy(GetComponent<HingeJoint>());
			/*
			Rigidbody[] bodies = this.GetComponentsInChildren<Rigidbody> ();
			rigidbody.AddForce (transform.right * 30f, ForceMode.Impulse);
			rigidbody.AddForce (transform.up * 20f, ForceMode.Impulse);
			//keyBody.mass = 10;
			foreach (Rigidbody body in bodies) {
				body.useGravity = false;
				body.AddForce (transform.up * 15f, ForceMode.Impulse);
				//if (body.gameObject.name == "Bone.030")
				//	break;
			}
			keyBody.mass = 10;
			keyBody.useGravity = true;
			*/
			HingeJoint[] joints = this.GetComponentsInChildren<HingeJoint> ();
			foreach (HingeJoint joint in joints) {
					Destroy (joint);
			}
			door.key = keyBody.gameObject.AddComponent<Key> ();
			keyBody.useGravity = false;
			keyBody.transform.parent = this.transform;
			keyBody.AddTorque (Vector3.forward * 10, ForceMode.Impulse);
		}
	}
}
