using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate bool PreClick ();
public delegate void OnClick ();

public class Selectable : MonoBehaviour {

	public Material highlightedMat;

	/* To be updated by other scripts */
	public bool isClickedOn = false;
	public bool isSelected = false;

	private PreClick PreClickCallback;
	private OnClick OnClickCallback;
	private MeshRenderer[] childrenRenderers;
	private Dictionary<MeshRenderer, Material> originalMats;

	void Start () {
		this.childrenRenderers = this.GetComponentsInChildren<MeshRenderer> ();
		this.originalMats = new Dictionary<MeshRenderer, Material> ();
		foreach (MeshRenderer child in childrenRenderers) {
			this.originalMats [child] = child.material;
		}
		if (this.PreClickCallback == null)
			this.PreClickCallback = DefaultPreClickCallback;
		if (this.OnClickCallback == null)
			this.OnClickCallback = DefaultOnClickCallback;
	}

	bool DefaultPreClickCallback () {
		return true;
	}

	void DefaultOnClickCallback () { }

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
			if (Input.GetMouseButtonDown (0) || Input.GetKeyDown (KeyCode.Space)) {
				if (PreClickCallback ()) {
					isClickedOn = true;
					OnClickCallback ();
				}
			}
		}
	}

	public void SetPreClickCallback (PreClick callback) {
		this.PreClickCallback = callback;
	}

	public void SetOnClickCallback (OnClick callback) {
		this.OnClickCallback = callback;
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
