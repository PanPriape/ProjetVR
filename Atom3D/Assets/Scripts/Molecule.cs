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
        align();
		if (mouseState) {
			this.rotationMolecule();
		}
	}
	
	public void rotationMolecule() {
		Vector3 verticalaxis = transform.TransformDirection(Vector3.up);
		transform.RotateAround (centroid.position, verticalaxis, 5);
    }
    void align()
    {
        GameObject[] atoms = GameObject.FindGameObjectsWithTag("atom");
        Vector3[] positions = new Vector3[atoms.Length];
        for (int i = 0; i < atoms.Length; i++)
        {
            positions[i] = atoms[i].transform.position;
        }
        this.transform.position = centroid.position;
        for (int i = 0; i < atoms.Length; i++)
        {
            atoms[i].transform.position = positions[i];
        }
    }
}
