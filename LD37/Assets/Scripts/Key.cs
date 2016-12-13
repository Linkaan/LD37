using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (Selectable))]
public class Key : MonoBehaviour {

    public HiddenKey thisHider;
    public string keyId;

    public AudioClip keys;

    private Selectable selectable;
	private Player player;

	void Start () {
		selectable = this.GetComponent<Selectable> ();
		selectable.SetOnClickCallback (DoPickup);
		player = Transform.FindObjectOfType<Player> ();
	}

	void DoPickup () {
        AudioSource.PlayClipAtPoint(keys, transform.position);
        Transform.FindObjectOfType<UserNotifier>().ShowText("Maybe I can use this key to unlock the door...", 2);
        player.GiveKey (this.keyId);
        thisHider.hasKey = false;
        Destroy (this.gameObject);
	}
}
