using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorCloser : MonoBehaviour {

	public Door door;

	void OnTriggerEnter (Collider other) {
		door.CloseDoor ();
	}
}
