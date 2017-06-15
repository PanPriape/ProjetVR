using UnityEngine;

public class LeftLaserPoint : MonoBehaviour
{

    public float distanceSpawn;

    private SteamVR_TrackedObject trackedObj;

    public GameObject laserPrefab;
    private GameObject laser;
    private Transform laserTransform;
    private Vector3 hitPoint;

    private GameObject objSelected1;
    private GameObject objSelected2;

    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }

    private void ShowLaser(RaycastHit hit)
    {
        laser.SetActive(true);
        laserTransform.position = Vector3.Lerp(trackedObj.transform.position, hitPoint, .5f);
        laserTransform.LookAt(hitPoint);
        laserTransform.localScale = new Vector3(laserTransform.localScale.x, laserTransform.localScale.y,
            hit.distance);
    }

    void CreateAndGrab(Transform celltab)
    {
        Vector3 dControButton = celltab.position - this.transform.position;
        Vector3 spawnPosition = ((distanceSpawn / dControButton.magnitude) * dControButton) + this.transform.position;
        GameObject atomSpawned = celltab.gameObject.GetComponent<CellTab>().createAtom(spawnPosition);
        this.GetComponent<ControllerGrabObject>().collidingObject = atomSpawned;
        this.GetComponent<ControllerGrabObject>().GrabObject();
    }

    void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    void Start()
    {
        laser = Instantiate(laserPrefab);
        laserTransform = laser.transform;
    }

    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(trackedObj.transform.position, transform.forward, out hit, 100))
        {
            hitPoint = hit.point;
            ShowLaser(hit);
            Transform target = hit.transform;
            if (Controller.GetHairTriggerDown())
            {
                if (target.gameObject.CompareTag("celltab"))
                {
                    // Create new atom
                    CreateAndGrab(target);
                }
                if (target.gameObject.CompareTag("atom"))
                {
                    // Detroy atom & links related
                    Destroy(target.gameObject);
                }
                if (target.gameObject.CompareTag("link"))
                {
                    //Downgrade link or Destroy
                    Destroy(target.gameObject);
                }
            }
            if (Controller.GetPress(SteamVR_Controller.ButtonMask.Touchpad))
            {
                if (target.gameObject.CompareTag("atom"))
                {
                    // Create a link if 2nd atom selected, deselect if same atom
                    if (objSelected1.Equals(target.gameObject))
                    {
                        objSelected1 = null;
                    }
                    if (objSelected1 == null)
                    {
                        objSelected1 = target.gameObject;
                    }
                    else
                    {
                        objSelected2 = target.gameObject;
                        bool testLibre = objSelected1.GetComponent<Atom>().AddVoisin(objSelected2.GetComponent<Atom>());
                        if (testLibre)
                        {
                            GameObject link = GameObject.Instantiate(UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/Prefabs/LinkVR.prefab", typeof(GameObject))) as GameObject;
                            link.GetComponent<LinkVR>().setSphere1(objSelected1);
                            link.GetComponent<LinkVR>().setSphere2(objSelected2);
                        }
                        objSelected1 = null;
                        objSelected2 = null;
                    }
                }
                if (target.gameObject.CompareTag("link"))
                {
                    //Upgrade link
                }
            }
        }
        else
        {
            laser.SetActive(false);
        }
    }
}
