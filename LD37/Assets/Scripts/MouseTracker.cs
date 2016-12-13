    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseTracker : MonoBehaviour {

    public Texture hand;
    public Texture hand_closed;

    private RawImage image;

    void Start () {
        image = this.GetComponent<RawImage>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButton(0))
        {
            image.texture = hand_closed;
        } else
        {
            image.texture = hand;
        }
        transform.position = Input.mousePosition;
    }
}
