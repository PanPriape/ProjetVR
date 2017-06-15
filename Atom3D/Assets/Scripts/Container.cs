using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : MonoBehaviour {

	private bool _mouseState;
	
	private float strength = 5.0f;
	
    public Vector3 screenSpace;
    public Vector3 offset;
	
	private GameObject target;
	private GameObject[] atoms;

	
	public bool getMouseState() {
		return this._mouseState;
	}
	
	public void setMouseState(bool state) {
		this._mouseState = state;
	}
	
	public GameObject[] getAtoms() {
			return this.atoms;
	}
		 
	public void setTableau(GameObject[] atoms) {
			 this.atoms = atoms;
	}
	
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
		this.atoms = GameObject.FindGameObjectsWithTag("atom");
		//transform.RotateAround (GameObject.FindGameObjectsWithTag("centroid")[0].transform.position, Vector3.up, 20 * Time.deltaTime);
		// Debug.Log(_mouseState);
        if (_mouseState) {
            //keep track of the mouse position
            var curScreenSpace = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenSpace.z);
 
            //convert the screen mouse position to world point and adjust with offset
            var curPosition = UnityEngine.Camera.main.ScreenToWorldPoint (curScreenSpace) + offset;
 
            //update the position of the object in the world
            target.transform.position = curPosition;
        }
		
		this.masseRessort();
		
    }
	
	GameObject GetClickedObject (out RaycastHit hit) {
        GameObject target = null;
        Ray ray = UnityEngine.Camera.main.ScreenPointToRay (Input.mousePosition);
        if (Physics.Raycast (ray.origin, ray.direction * 10, out hit)) {
            target = hit.collider.gameObject;
        }

        return target;
    }

	void OnMouseEnter() {
		GameObject[] gobjs = GameObject.FindGameObjectsWithTag ("electron");
		foreach (GameObject gobj in gobjs) {
			gobj.GetComponent<Renderer> ().material.color = Color.red;
		}
	}
	void OnMouseExit() {
		GameObject[] gobjs = GameObject.FindGameObjectsWithTag ("electron");
		foreach (GameObject gobj in gobjs) {
			gobj.GetComponent<Renderer> ().material.color = Color.yellow;
		}
	}
	
	public void deplacement(GameObject target) {
        _mouseState = true;
		this.target = target;
        screenSpace = UnityEngine.Camera.main.WorldToScreenPoint (target.transform.position);
        offset = target.transform.position - UnityEngine.Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenSpace.z));
    }
	
	public void masseRessort() {
		
		for (int index = 0; index < atoms.Length; index++) {
			atoms[index].GetComponent<Rigidbody>().velocity = Vector3.zero;
			atoms[index].GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
		}
		
		for (int index1 = 0; index1 < atoms.Length; index1++) {
			for (int index2 = index1; index2 < atoms.Length; index2++) {
				Vector3 direction1 = atoms[index1].transform.position - atoms[index2].transform.position;
				Vector3 direction2 = atoms[index2].transform.position - atoms[index1].transform.position;
				if (direction1.magnitude < 5.0f) {
					atoms[index1].GetComponent<Rigidbody>().AddForce(strength * direction1.normalized);
				} else if (direction2.magnitude > 5.1f) {
					atoms[index1].GetComponent<Rigidbody>().AddForce(strength * direction2.normalized);
				}
			}
		}		
	}
}
