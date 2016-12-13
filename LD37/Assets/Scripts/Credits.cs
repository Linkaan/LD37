using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credits : MonoBehaviour
{

    public DialogHandler handler;

    void OnTriggerEnter(Collider other)
    {
        handler.SetState(DialogHandler.DialogState.show_enddialog);
    }
}
