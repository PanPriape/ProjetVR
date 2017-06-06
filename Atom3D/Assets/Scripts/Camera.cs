using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour {

	public float speed = 0.1f;
	private bool isSelected1;
	private GameObject objSelected1;
	private GameObject objSelected2;

	// Use this for initialization
	void Start () {
		
	}

	public GameObject getObj1() {
		return objSelected1;
	}

	public GameObject getObj2() {
		return objSelected2;
	}

	// Update is called once per frame
	void Update () {
		
		var scroll = Input.GetAxis("Mouse ScrollWheel");
		
         if(Input.GetKey(KeyCode.RightArrow))
         {
             transform.position = new Vector3(transform.position.x + speed, transform.position.y, transform.position.z);
         }
         if(Input.GetKey(KeyCode.LeftArrow))
         {
             transform.position = new Vector3(transform.position.x - speed, transform.position.y, transform.position.z);
         }
         if(Input.GetKey(KeyCode.DownArrow))
         {
             transform.position = new Vector3(transform.position.x, transform.position.y - speed, transform.position.z);
         }
         if(Input.GetKey(KeyCode.UpArrow))
         {
             transform.position = new Vector3(transform.position.x, transform.position.y + speed, transform.position.z);
         }
		 if ( scroll != 0) {
			transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + scroll);
		}

		if (Input.GetMouseButtonDown (1)) {
			if (!isSelected1) {
				objSelected1 = null;
				objSelected2 = null;
				//sélection du premier atome
				RaycastHit hitInfo;
				objSelected1 = GetClickedObject (out hitInfo);
				if ((objSelected1 != null) && (objSelected1.CompareTag ("atom"))) {
					isSelected1 = true;
				}
			} else if (isSelected1) {
				//sélection du deuxième atom
				RaycastHit hitInfo;
				objSelected2 = GetClickedObject (out hitInfo);
				if ((objSelected2 != null) && (objSelected2.CompareTag ("atom"))) {
					if (objSelected1 != objSelected2) {
						//construction du liens
						GameObject.Instantiate( UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Link.prefab", typeof(GameObject)));
						isSelected1 = false;
					}
				}
			} 
			Debug.Log ("select1 : " + isSelected1.ToString ());
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
}
