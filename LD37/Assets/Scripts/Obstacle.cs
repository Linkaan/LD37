using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour {

	public bool liftable = true;

	private Player player;
	private Selectable selectable;

	void Start () {
		player = Transform.FindObjectOfType<Player> ();
		selectable = this.GetComponent<Selectable> ();
	}
	
	void Update () {
		if (player.state == Player.PlayerState.idle && selectable.isClickedOn) {
			player.holding = this.gameObject;
			player.ChangeState (Player.PlayerState.holding_object);
		}
	}
}
