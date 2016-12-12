using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour {

	public Renderer roomRenderer;
	public int roomNr;

	public float x1;
	public float x2;
	public float y1;
	public float y2;

	public float width;
	public float length;

	void Start () {
		x1 = transform.position.x;
		y1 = transform.position.z;
		Vector3 bounds = roomRenderer.bounds.size;
		x2 = x1 + bounds.x / 2;
		y2 = y1 - bounds.z / 2;
		width = bounds.x;
		length = bounds.z;
	}

}
				