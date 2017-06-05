using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceRepulsion : MonoBehaviour {

         public GameObject[] repulseTo;
         public float strengthOfAttraction = 5.0f;
     
         void Start () {
			repulseTo = GameObject.FindGameObjectsWithTag("atom");
         }
     
         void FixedUpdate () {
			 
			repulseTo = GameObject.FindGameObjectsWithTag("atom");
			
			for (int index = 0; index < repulseTo.Length; index++) {
				Vector3 direction = repulseTo[index].transform.position - transform.position;
                gameObject.GetComponent<Rigidbody>().AddForce(strengthOfAttraction * direction);
			}
         
         }
}
