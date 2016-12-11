using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : MonoBehaviour {

	public bool inputEnabled = true;

	private Selectable lastSelectable;

	void Update () {
		if (!inputEnabled)
			return;

		RaycastHit hit;

		Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
		Debug.DrawRay(ray.origin, ray.direction * 100);
		if (Physics.Raycast (ray, out hit)) {
			GameObject obj = hit.transform.gameObject;
			Selectable selectable = obj.GetComponent<Selectable> ();
			if (selectable != null) {
				if (lastSelectable != selectable) {
					selectable.OnLeaveCallback ();
				} else {
					selectable.OnOverCallback ();
				}
				lastSelectable = selectable;
			}
		} else if (lastSelectable != null && !lastSelectable.isClickedOn && lastSelectable.isSelected) {
			lastSelectable.OnLeaveCallback ();
		}
	}
}
