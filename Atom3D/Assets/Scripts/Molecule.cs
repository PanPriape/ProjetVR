using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Molecule : MonoBehaviour {

	private bool _mouseState;
	private GameObject target;
    public Vector3 screenSpace;
    public Vector3 offset;

	
	public bool getMouseState() {
		return this._mouseState;
	}
	
	public void setMouseState(bool state) {
		this._mouseState = state;
	}
	
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
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
		foreach (Transform child in transform)
		{
			foreach (Transform child_child in child) {
				MeshFilter mf = child_child.GetComponent<MeshFilter> ();
				mf.GetComponent<Renderer> ().material.color = Color.red;
			}
		}
	}
	void OnMouseExit() {
		foreach (Transform child in transform)
		{
			foreach (Transform child_child in child) {
				MeshFilter mf = child_child.GetComponent<MeshFilter> ();
				mf.GetComponent<Renderer> ().material.color = Color.yellow;
			}
		}
	}
	
	public void deplacement(GameObject target) {
        _mouseState = true;
		this.target = target;
        screenSpace = UnityEngine.Camera.main.WorldToScreenPoint (target.transform.position);
        offset = target.transform.position - UnityEngine.Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenSpace.z));
    }
}
