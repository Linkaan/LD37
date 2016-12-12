using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (Selectable))]
public class Key : MonoBehaviour {

	public string keyId;

	private Selectable selectable;
	private Player player;

	void Start () {
		selectable = this.GetComponent<Selectable> ();
		selectable.SetOnClickCallback (DoPickup);
		player = Transform.FindObjectOfType<Player> ();
	}

	void DoPickup () {
		player.GiveKey (this);
		Destroy (this.gameObject);
	}
}
