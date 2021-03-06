﻿using UnityEngine;
using System.Collections.Generic;

public class Atom : MonoBehaviour
{

    public int id;

    public int nbProtons;
    public int nbNeutrons;
    public int nbElectrons;
    public int nbElectronslibres;

    private bool _mouseState;

    private GameObject target;

    public Vector3 screenSpace;
    public Vector3 offset;

    private int nbElectronsRestants;


    public List<Atom> voisins;

    public bool AddVoisin(Atom a)
    {
        if (voisins.Count < nbElectronslibres && a.voisins.Count < a.nbElectronslibres)
        {
            voisins.Add(a);
            a.voisins.Add(this);
            this.nbElectronsRestants--;
            a.setnbElectronsRestants(a.getnbElectronsRestants() - 1);
            //this.displayElectron();
            //a.displayElectron();
            return true;
        }
        else
        {
            Debug.Log("nombre de liaisons maximal atteint");
            return false;
        }
    }

    public bool FindVoisin(Atom a)
    {
        return voisins.Contains(a);
    }

    public void removeVoisin(Atom a)
    {
        voisins.Remove(a);
        a.voisins.Remove(this);
        //on ajoute/revèle un électron libre sur chaque atomes faisant parti de la liaison cassée/enlevée
        this.nbElectronsRestants++;
        a.setnbElectronsRestants(a.getnbElectronsRestants() + 1);
        //this.displayElectron();
        //a.displayElectron();
    }

    public void Break()
    {
        foreach (Atom a in voisins)
        {
            voisins.Remove(a);
            a.voisins.Remove(this);
        }
    }

    public bool getMouseState()
    {
        return this._mouseState;
    }

    public void setMouseState(bool state)
    {
        this._mouseState = state;
    }

    public void displayElectron()
    {
        int i = 1;
        MeshFilter[] mfs = this.GetComponentsInChildren<MeshFilter>();
        foreach (MeshFilter mf in mfs)
        {
            if (i == 1)
            {
                i--;
            }
            else
            {
                mf.GetComponent<Renderer>().enabled = true;
            }
        }
       
        foreach (MeshFilter mf in mfs)
        {
            if (i == 1)
            {
                i--;
            }
            else
            {
                mf.GetComponent<Renderer>().enabled = true;
            }
        }
        /*for (int j = 1; j <= (this.nbElectronslibres - this.nbElectronsRestants); j++)
        {
            mfs[j].GetComponent<Renderer>().enabled = false;
        }*/
    }

    public int getnbElectronsRestants()
    {
        return this.nbElectronsRestants;
    }

    public void setnbElectronsRestants(int nbElec)
    {
        this.nbElectronsRestants = nbElec;
    }

    // Use this for initialization
    void Start()
    {
        this.transform.parent = GameObject.FindGameObjectsWithTag("molecule")[0].transform;
    }

    // Update is called once per frame
    void Update()
    {
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
        int i = 1;
        MeshFilter[] mfs = this.GetComponentsInChildren<MeshFilter>();
        foreach (MeshFilter mf in mfs)
        {
            if (i == 1)
            {
                i--;
            }
            else
            {
                mf.GetComponent<Renderer>().material.color = Color.red;
            }
        }
    }
    void OnMouseExit()
    {
        int i = 1;
        MeshFilter[] mfs = this.GetComponentsInChildren<MeshFilter>();
        foreach (MeshFilter mf in mfs)
        {
            if (i == 1)
            {
                i--;
            }
            else
            {
                mf.GetComponent<Renderer>().material.color = Color.yellow;
            }
        }
    }

    public void deplacement(GameObject target)
    {
        _mouseState = true;
        this.target = target;
        screenSpace = UnityEngine.Camera.main.WorldToScreenPoint(target.transform.position);
        offset = target.transform.position - UnityEngine.Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z));
    }

}

