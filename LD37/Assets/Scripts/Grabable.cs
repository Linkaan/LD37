using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabable : MonoBehaviour {

	public bool liftable = true;

	private Player player;
	private Selectable selectable;

	void Start () {
		player = Transform.FindObjectOfType<Player> ();
		selectable = this.GetComponent<Selectable> ();
	}
	
	void LateUpdate () {
		if (liftable) {
			if (player.state == Player.PlayerState.idle && selectable.isClickedOn) {
				player.holding = this;
				player.ChangeState (Player.PlayerState.holding_object);
			}
		} else if (selectable.isClickedOn) {
            Transform.FindObjectOfType<UserNotifier>().ShowText("This obstacle is too heavy to lift!", 2);
            selectable.isClickedOn = false;
		}
	}

	public void Unselect () {
		selectable.isClickedOn = false;
		selectable.SetSelected (false);
	}
}
