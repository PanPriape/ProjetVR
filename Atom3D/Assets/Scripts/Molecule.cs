using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Molecule : MonoBehaviour {

	private Transform centroid;
	private bool mouseState;
	
	
	public void setMouseState(bool state) {
		this.mouseState = state;
	}

	// Use this for initialization
	void Start () {
		centroid = GameObject.FindGameObjectsWithTag("centroid")[0].transform;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void rotationMolecule() {
		Vector3 verticalaxis = transform.TransformDirection(Vector3.up);
		transform.RotateAround (centroid.position, verticalaxis, 10);
	}
}
