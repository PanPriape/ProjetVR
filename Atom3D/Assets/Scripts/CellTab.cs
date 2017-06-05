using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellTab : MonoBehaviour {

	//public Transform atomPrefab;
	string path;
	public Vector3 place;

	// Use this for initialization
	void Start () {
		path = "Assets/Prefabs/"+this.gameObject.name+".prefab";
		switch (this.gameObject.name)
		{
			case "Hydrogen":
				place = new Vector3(-2,0,-5);
				break;
			case "Carbon":
				place = new Vector3(0,0,-5);
				break;
			default:
				place = new Vector3(2,0,-5);
				break;
		}
	}
	
	 void OnMouseDown(){
		GameObject.Instantiate( UnityEditor.AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)), place, Quaternion.identity); 
		//Instantiate(atomPrefab, place, Quaternion.identity);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
