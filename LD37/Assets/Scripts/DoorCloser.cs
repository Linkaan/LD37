using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorCloser : MonoBehaviour {

	public Door door;
    public DialogHandler.DialogState state;

    void OnTriggerEnter (Collider other) {
		door.CloseDoor ();
        if (state != DialogHandler.DialogState.do_nothing)
        {
            Transform.FindObjectOfType<DialogHandler>().SetState(state);
        }
	}
}
