using UnityEngine;

public class Container : MonoBehaviour
{

    private bool _mouseState;

    private float strength = 120.0f;

    public Vector3 screenSpace;
    public Vector3 offset;

    private GameObject target;
    private GameObject[] atoms;
    private GameObject[] links;

    public bool move = true;

    public bool getMouseState()
    {
        return this._mouseState;
    }

    public void setMouseState(bool state)
    {
        this._mouseState = state;
    }

    public GameObject[] getAtoms()
    {
        return this.atoms;
    }

    public void setTableau(GameObject[] atoms)
    {
        this.atoms = atoms;
    }

    public GameObject[] getLinks()
    {
        return this.links;
    }

    public void setTabLinks(GameObject[] links)
    {
        this.links = links;
    }

    // Update is called once per frame
    void Update()
    {
        this.atoms = GameObject.FindGameObjectsWithTag("atom");
        this.links = GameObject.FindGameObjectsWithTag("link");
        //transform.RotateAround (GameObject.FindGameObjectsWithTag("centroid")[0].transform.position, Vector3.up, 20 * Time.deltaTime);
        // Debug.Log(_mouseState);
        if (_mouseState)
        {
            //keep track of the mouse position
            var curScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z);

            //convert the screen mouse position to world point and adjust with offset
            var curPosition = UnityEngine.Camera.main.ScreenToWorldPoint(curScreenSpace) + offset;

            //update the position of the object in the world
            target.transform.position = curPosition;
        }
        if (move)
        {
            this.masseRessort();
        }
    }

    GameObject GetClickedObject(out RaycastHit hit)
    {
        GameObject target = null;
        Ray ray = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray.origin, ray.direction * 10, out hit))
        {
            target = hit.collider.gameObject;
        }
        return target;
    }

    void OnMouseEnter()
    {
        GameObject[] gobjs = GameObject.FindGameObjectsWithTag("electron");
        foreach (GameObject gobj in gobjs)
        {
            gobj.GetComponent<Renderer>().material.color = Color.red;
        }
    }
    void OnMouseExit()
    {
        GameObject[] gobjs = GameObject.FindGameObjectsWithTag("electron");
        foreach (GameObject gobj in gobjs)
        {
            gobj.GetComponent<Renderer>().material.color = Color.yellow;
        }
    }

    public void deplacement(GameObject target)
    {
        _mouseState = true;
        this.target = target;
        screenSpace = UnityEngine.Camera.main.WorldToScreenPoint(target.transform.position);
        offset = target.transform.position - UnityEngine.Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z));
    }

    public void masseRessort()
    {

        for (int index = 0; index < atoms.Length; index++)
        {

            atoms[index].GetComponent<Rigidbody>().velocity = Vector3.zero;
            atoms[index].GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }

        for (int i = 0; i < links.Length; i++)
        {

            GameObject atom1 = links[i].GetComponent<LinkVR>().getSphere1();
            GameObject atom2 = links[i].GetComponent<LinkVR>().getSphere2();


            Vector3 direction1 = atom1.transform.position - atom2.transform.position;
            Vector3 direction2 = atom2.transform.position - atom1.transform.position;

            double longeurliaison = 2.0f;

            //Définition de la longueur de liaison en fonction de la nature des atomes.

            if (atom1.name == "Carbon(Clone)" && atom2.name == "Carbon(Clone)")
            {
                if (links[i].GetComponent<LinkVR>().getType())
                {
                    longeurliaison = 0.96f;
                }
                else
                {
                    longeurliaison = 1.10f;
                }
            }
            else if ((atom1.name == "Carbon(Clone)" && atom2.name == "Oxygen(Clone)") || (atom2.name == "Carbon(Clone)" && atom1.name == "Oxygen(Clone)"))
            {
                if (links[i].GetComponent<LinkVR>().getType())
                {
                    longeurliaison = 0.88f;
                }
                else
                {
                    longeurliaison = 1.02f;
                }
            }
            else if (atom1.name == "Oxygen(Clone)" && atom2.name == "Oxygen(Clone)")
            {
                if (links[i].GetComponent<LinkVR>().getType())
                {
                    longeurliaison = 0.86f;
                }
                else
                {
                    longeurliaison = 1.06f;
                }
            }
            else if ((atom1.name == "Carbon(Clone)" && atom2.name == "Hydrogen(Clone)") || (atom2.name == "Carbon(Clone)" && atom1.name == "Hydrogen(Clone)"))
            {
                longeurliaison = 0.78f;
            }
            else if ((atom1.name == "Oxygen(Clone)" && atom2.name == "Hydrogen(Clone)") || (atom2.name == "Oxygen(Clone)" && atom1.name == "Hydrogen(Clone)"))
            {
                longeurliaison = 0.69f;
            }
            else if ((atom1.name == "Hydrogen(Clone)" && atom2.name == "Hydrogen(Clone)") || (atom2.name == "Hydrogen(Clone)" && atom1.name == "Hydrogen(Clone)"))
            {
                longeurliaison = 0.53f;
            }
            else
            {
                longeurliaison = 1.0f;
            }

            longeurliaison = longeurliaison * this.transform.GetChild(1).localScale.x;

            Debug.Log(this.transform.GetChild(1).localScale.x);

            atom1.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            atom2.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;

            if (direction1.magnitude < longeurliaison)
            {

                atom1.GetComponent<Rigidbody>().AddForce(strength * direction1.normalized);
                atom2.GetComponent<Rigidbody>().AddForce(strength * direction2.normalized);
            }
            else if (direction2.magnitude > longeurliaison + 0.1f)
            {
                atom1.GetComponent<Rigidbody>().AddForce(strength * direction2.normalized);
                atom2.GetComponent<Rigidbody>().AddForce(strength * direction1.normalized);
            }
            else
            {
                atom1.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                atom2.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            }
        }
    }
}
