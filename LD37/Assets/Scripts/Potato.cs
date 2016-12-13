using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (Selectable))]
public class Potato : MonoBehaviour {

    public HiddenKey thisHider;

    public AudioClip eat;

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
        AudioSource.PlayClipAtPoint(eat, player.transform.position);
        Transform.FindObjectOfType<UserNotifier>().ShowText("That potato was delicious! I feel stronger!", 3);
        obstacle.liftable = true;
		hider.interactable = true;
        thisHider.hasKey = false;
		Destroy (particles);
		Destroy (this.gameObject);
	}
}
