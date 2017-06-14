using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserPoint : MonoBehaviour {

    public float distanceSpawn;

    private SteamVR_TrackedObject trackedObj;

    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }

    void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    // 1
    public GameObject laserPrefab;
    // 2
    private GameObject laser;
    // 3
    private Transform laserTransform;
    // 4
    private Vector3 hitPoint;

    private void ShowLaser(RaycastHit hit)
    {
        // 1
        laser.SetActive(true);
        // 2
        laserTransform.position = Vector3.Lerp(trackedObj.transform.position, hitPoint, .5f);
        // 3
        laserTransform.LookAt(hitPoint);
        // 4
        laserTransform.localScale = new Vector3(laserTransform.localScale.x, laserTransform.localScale.y,
            hit.distance);
    }

    void Start()
    {
        laser = Instantiate(laserPrefab);
        // 2
        laserTransform = laser.transform;
    }

    // Update is called once per frame
    void Update () {
        if (Controller.GetPress(SteamVR_Controller.ButtonMask.Touchpad))
        {
            RaycastHit hit;

            
            if (Physics.Raycast(trackedObj.transform.position, transform.forward, out hit, 100))
            {
                hitPoint = hit.point;
                ShowLaser(hit);
                if (Controller.GetHairTriggerDown())
                {
                    if (hit.transform.gameObject.CompareTag("Button") )
                    {
                        Vector3 dControButton = hit.transform.position - this.transform.position;
                        Vector3 spawnPosition = ((distanceSpawn / dControButton.magnitude) * dControButton) + this.transform.position;
                        GameObject atomSpawned = hit.transform.gameObject.GetComponent<CellTab>().createAtom(spawnPosition);
                        this.GetComponent<ControllerGrabObject>().collidingObject = atomSpawned;
                        this.GetComponent<ControllerGrabObject>().GrabObject();
                    }
                    if (hit.transform.gameObject.CompareTag("atom"))
                    {
                        Destroy(hit.transform.gameObject);
                    }
                }
            }


        }
        else
        {
            laser.SetActive(false);
        }
    }
}
