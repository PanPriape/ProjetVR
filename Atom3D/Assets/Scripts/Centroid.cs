﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Centroid : MonoBehaviour {

	// Use this for initialization
	void Start () {
		this.transform.parent = GameObject.FindGameObjectsWithTag("container")[0].transform;
		this.transform.position = new Vector3(0.0f,0.0f,0.0f);
	}
	
	// Update is called once per frame
	void Update () {
		this.calcPosition();
	}
	
	void calcPosition() {
		GameObject[] atoms = GameObject.FindGameObjectsWithTag("atom");
		Vector3 centroid = new Vector3(0.0f,0.0f,0.0f);
		
		if (atoms.Length > 0) {
			for (int index = 0; index < atoms.Length; index++ ) {
				centroid+=atoms[index].transform.position;
			}
		
			centroid/= atoms.Length;
		}
		
		this.transform.position = centroid;
	}
}
