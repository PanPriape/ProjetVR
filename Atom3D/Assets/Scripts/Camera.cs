using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour {

	public float speed = 0.1f;
	
	private bool isSelected1;
	
	private GameObject objSelected1;
	private GameObject objSelected2;
	private GameObject objSelected;

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

		
		if (Input.GetMouseButtonDown (0)) {
			RaycastHit hitInfo;
            objSelected = GetClickedObject (out hitInfo);
			
			if (objSelected.CompareTag("molecule")) {
				objSelected.GetComponent<Molecule>().deplacement(objSelected);
			} else if (objSelected.CompareTag("celltab")) {
				objSelected.GetComponent<CellTab>().popAtom();
			} else if (objSelected.CompareTag("atom")) {
				objSelected.GetComponent<Atom>().deplacement(objSelected);
			} else if (objSelected.CompareTag("link")) {
				Debug.Log("Lien Double");
				if (objSelected.GetComponent<Link>().getType()) {
					Debug.Log("Simple vers double");
					objSelected.GetComponent<Link>().getSphere1().GetComponent<Atom>().removeVoisin(objSelected.GetComponent<Link>().getSphere2().GetComponent<Atom>());
					objSelected.GetComponent<Renderer>().material.color = Color.blue;
					objSelected.GetComponent<Link>().setType(false);
				} else {
					Debug.Log("Double vers simple");
					if (objSelected.GetComponent<Link>().getSphere1().GetComponent<Atom>().AddVoisin(objSelected.GetComponent<Link>().getSphere2().GetComponent<Atom>())) {
						objSelected.GetComponent<Renderer>().material.color = Color.red;
						objSelected.GetComponent<Link>().setType(true);
					}
				}
			}
			
        }
		
		if (Input.GetMouseButtonUp (0) && objSelected != null) {
			if (objSelected.CompareTag("atom")) {
				objSelected.GetComponent<Atom>().setMouseState(false);
			} else if (objSelected.CompareTag("molecule")) {
				objSelected.GetComponent<Molecule>().setMouseState(false);
			}
        }
		
		if (Input.GetMouseButtonDown (1)) {
			
			RaycastHit hitInfo;
			objSelected = GetClickedObject(out hitInfo);
			
			if (objSelected.CompareTag("molecule")) {
				Debug.Log("molecule rotation");
			} else if (objSelected.CompareTag("link")) {
				Destroy(objSelected);
			} else if (objSelected.CompareTag("atom")) {
				if (!isSelected1) {
					objSelected1 = null;
					objSelected2 = null;
					//sélection du premier atome
					objSelected1 = objSelected;
					if (objSelected1 != null) {
						isSelected1 = true;
					}
				} else if (isSelected1) {
					//sélection du deuxième atom
					objSelected2 = objSelected;
					if (objSelected2 != null) {
						if (objSelected1 != objSelected2) {
							
							//Test electron disponible
							bool testLibre = objSelected1.GetComponent<Atom>().AddVoisin(objSelected2.GetComponent<Atom>());
							
							//construction du liens
							if (testLibre) {
								GameObject.Instantiate( UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Link.prefab", typeof(GameObject)));
							}
							
							isSelected1 = false;
						}
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
