using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selectable : MonoBehaviour {

	public Material highlightedMat;

	/* To be updated by other scripts */
	public bool isClickedOn = false;
	public bool isSelected = false;

	private MeshRenderer[] childrenRenderers;
	private Dictionary<MeshRenderer, Material> originalMats;

	void Start () {
		this.childrenRenderers = this.GetComponentsInChildren<MeshRenderer> ();
		this.originalMats = new Dictionary<MeshRenderer, Material> ();
		foreach (MeshRenderer child in childrenRenderers) {
			this.originalMats [child] = child.material;
		}
	}

	void updateMats () {
		foreach (MeshRenderer child in childrenRenderers) {
			if (this.isSelected) {
				child.material = highlightedMat;
			} else {
				child.material = originalMats[child];
			}
		}
	}

	void Update () {
		if (isSelected) {
			if (Input.GetMouseButtonDown (0)) {
				isClickedOn = true;
			}
		}
	}
		
	public void SetSelected (bool isSelected) {
		this.isSelected = isSelected;
		updateMats ();
	}
		
	public void OnOverCallback () {
		if (!isClickedOn) {
			SetSelected (true);
		}
	}

	public void OnLeaveCallback () {
		if (!isClickedOn) {
			SetSelected (false);
		}
	}
}
