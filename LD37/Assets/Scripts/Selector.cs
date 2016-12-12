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
		Debug.DrawRay(ray.origin, ray.direction * 12);
		if (Physics.Raycast (ray, out hit)) {
			GameObject obj = hit.transform.gameObject;
			Selectable selectable = obj.GetComponent<Selectable> ();
			if (selectable != null && (lastSelectable == null || !lastSelectable.isClickedOn)) {
				if (lastSelectable != selectable || lastSelectable.isSelected == false) {
					if (lastSelectable != null) {
						lastSelectable.OnLeaveCallback ();
					}
					selectable.OnOverCallback ();
				}
				lastSelectable = selectable;
			}
		} else if (lastSelectable != null && !lastSelectable.isClickedOn && lastSelectable.isSelected) {
			lastSelectable.OnLeaveCallback ();
		}
	}
}
