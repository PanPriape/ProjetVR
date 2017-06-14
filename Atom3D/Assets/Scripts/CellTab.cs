using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellTab : MonoBehaviour {

	//public Transform atomPrefab;
	string path;
	public Vector3 place;
    int nextnumber = 0;
    string clone = "(Clone)";

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
	
	public void popAtom(){
		GameObject.Instantiate( UnityEditor.AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)), place, Quaternion.identity); 
		//Instantiate(atomPrefab, place, Quaternion.identity);
	}

    public GameObject createAtom(Vector3 zone)
    {
        GameObject tmp;
        tmp = GameObject.Instantiate(UnityEditor.AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)), zone, Quaternion.identity) as GameObject;
        tmp.name = tmp.name + nextnumber;
        tmp.name = tmp.name.Replace(clone, "");
        nextnumber++;
        return tmp;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
