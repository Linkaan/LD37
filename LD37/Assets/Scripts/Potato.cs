using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (Selectable))]
public class Potato : MonoBehaviour {

	public Grabable obstacle;
	public HiddenKey hider;
	public GameObject particles;

	private Selectable selectable;
	private Player player;

	void Start () {
		selectable = this.GetComponent<Selectable> ();
		selectable.SetOnClickCallback (DoPickup);
		player = Transform.FindObjectOfType<Player> ();
	}

	void DoPickup () {
		obstacle.liftable = true;
		hider.interactable = true;
		Destroy (particles);
		Destroy (this.gameObject);
	}
}
