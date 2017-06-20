using UnityEngine;

public class LaserPoint : MonoBehaviour
{
    public float distanceSpawn;

    private SteamVR_TrackedObject trackedObj;

    public GameObject laserPrefab;
    private GameObject laser;
    private Transform laserTransform;
    private Vector3 hitPoint;

    public GameObject objSelected1;
    public GameObject objSelected2;

    public GameObject molecule;
    public GameObject innermolecule;

    Vector3 dControButton;



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

        Vector3 spawnPosition = ((distanceSpawn / dControButton.magnitude) * dControButton) + this.transform.position;
        GameObject atomSpawned = celltab.gameObject.GetComponent<CellTab>().createAtom(spawnPosition);
        float trueSize = atomSpawned.transform.localScale.x * innermolecule.transform.localScale.x;
        atomSpawned.transform.localScale = new Vector3(trueSize,trueSize, trueSize);
        this.GetComponent<ControllerGrabObject>().collidingObject = atomSpawned;
        this.GetComponent<ControllerGrabObject>().GrabObject();
    }

    void DestroyAtom(GameObject atom)
    {
        atom.GetComponent<Atom>().Break();
        Destroy(atom);
    }

    void DownLink(GameObject link)
    {
        LinkVR linkComponent = link.GetComponent<LinkVR>();
        if (linkComponent.getType())
        {
            linkComponent.setType(false);
            linkComponent.getSphere1().gameObject.GetComponent<Atom>().removeVoisin(linkComponent.getSphere2().gameObject.GetComponent<Atom>());
            link.GetComponent<Renderer>().material.color = Color.blue;
        }
        else
        {
            linkComponent.getSphere1().gameObject.GetComponent<Atom>().removeVoisin(linkComponent.getSphere2().gameObject.GetComponent<Atom>());
            Destroy(link);
        }
    }

    void CreateLink(GameObject atom)
    {
        if (objSelected1 == atom)
        {
            objSelected1 = null;
        }
        if (objSelected1 == null)
        {
            objSelected1 = atom;
        }
        else
        {
            objSelected2 = atom;
            if (!objSelected1.GetComponent<Atom>().FindVoisin(objSelected2.GetComponent<Atom>()))
            {
                bool testLibre = objSelected1.GetComponent<Atom>().AddVoisin(objSelected2.GetComponent<Atom>());
                if (testLibre)
                {
                    GameObject link = GameObject.Instantiate(UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/Prefabs/LinkVR.prefab", typeof(GameObject))) as GameObject;
                    link.GetComponent<LinkVR>().setSphere1(objSelected1);
                    link.GetComponent<LinkVR>().setSphere2(objSelected2);
                }
            }
            objSelected1 = null;
            objSelected2 = null;
        }
    }

    void UpLink(GameObject link)
    {
        LinkVR linkComponent = link.GetComponent<LinkVR>();
        if (!linkComponent.getType())
        {
            bool testLibre = linkComponent.getSphere1().gameObject.GetComponent<Atom>().AddVoisin(linkComponent.getSphere2().gameObject.GetComponent<Atom>());
            if (testLibre)
            {
                linkComponent.setType(true);
                link.GetComponent<Renderer>().material.color = Color.red;
            }
        }
    }

    void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    void Start()
    {
        laser = Instantiate(laserPrefab);
        laserTransform = laser.transform;
        objSelected1 = null;
        objSelected2 = null;
    }

    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(trackedObj.transform.position, transform.forward, out hit, 10000))
        {
            hitPoint = hit.point;
            ShowLaser(hit);
            Transform target = hit.transform;

            if (Controller.GetPressDown(SteamVR_Controller.ButtonMask.Grip))
            {
                if (target.gameObject.CompareTag("atom"))
                {
                    // Detroy atom & links related
                    DestroyAtom(target.gameObject);
                }
                if (target.gameObject.CompareTag("link"))
                {
                    //Downgrade link or Destroy
                    DownLink(target.gameObject);
                }
            }

            if (Controller.GetHairTriggerDown() && target.gameObject.CompareTag("celltab"))
            {

                dControButton = target.position - this.transform.position;
                CreateAndGrab(target);
            }

            if (Controller.GetPress(SteamVR_Controller.ButtonMask.Touchpad))
            {
                if (target.gameObject.CompareTag("atom"))
                {
                    // Create a link if 2nd atom selected, deselect if same atom
                    CreateLink(target.gameObject);
                }
                if (target.gameObject.CompareTag("link"))
                {
                    //Upgrade link
                    UpLink(target.gameObject);
                }
            }
        }
        else
        {
            laser.SetActive(false);
        }
    }
}
