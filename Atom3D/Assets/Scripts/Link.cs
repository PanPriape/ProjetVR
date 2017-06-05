using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Link : MonoBehaviour {

	public Transform sphere1 = null;
	public Transform sphere2 = null;
     
	void Start() {
        transform.localScale = new Vector3 (0.25f, 1.0f, 0.25f);
		sphere1 = GameObject.Find ("Main Camera").GetComponent<Camera>().getObj1().transform;
		sphere2 = GameObject.Find ("Main Camera").GetComponent<Camera>().getObj2().transform;
    }
     
    void Update() {
         Vector3 dir = sphere2.position - sphere1.position;
         transform.position = sphere1.position + 0.5f * dir;
         Vector3 scale = transform.localScale;
         scale.y = dir.magnitude * 0.5f;
         transform.localScale = scale;
         transform.rotation = Quaternion.FromToRotation(Vector3.up, dir);
    }
}
