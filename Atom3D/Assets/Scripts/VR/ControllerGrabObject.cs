using UnityEngine;

public class ControllerGrabObject : MonoBehaviour
{

    public SteamVR_TrackedObject trackedObj;

    public SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }

    public GameObject collidingObject;
    public GameObject objectInHand;

    public GameObject molecule;

    public bool hair;

    public void SetCollidingObject(Collider col)
    {
        if (collidingObject || !col.GetComponent<Rigidbody>())
        {
            return;
        }
        collidingObject = col.gameObject;
    }

    public void OnTriggerEnter(Collider other)
    {
        SetCollidingObject(other);
    }

    public void OnTriggerStay(Collider other)
    {
        SetCollidingObject(other);
    }

    public void OnTriggerExit(Collider other)
    {
        if (!collidingObject)
        {
            return;
        }
        collidingObject = null;
    }

    public FixedJoint AddFixedJoint()
    {
        FixedJoint fx = gameObject.AddComponent<FixedJoint>();
        fx.breakForce = 20000;
        fx.breakTorque = 20000;
        return fx;
    }

    public void GrabObject()
    {
        objectInHand = collidingObject;
        objectInHand.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        collidingObject = null;
        var joint = AddFixedJoint();
        joint.connectedBody = objectInHand.GetComponent<Rigidbody>();
    }

    public void ReleaseObject()
    {
        if (GetComponent<FixedJoint>())
        {
            GetComponent<FixedJoint>().connectedBody = null;
            Destroy(GetComponent<FixedJoint>());
        }
        objectInHand.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        objectInHand = null;
    }

    void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
        hair = false;
    }

    void Update()
    {
        if (Controller.GetHairTriggerDown())
        {
            hair = true;
            if (collidingObject)
            {
                molecule.GetComponent<Container>().move = false;
                GrabObject();
            }
        }

        if (Controller.GetHairTriggerUp())
        {
            hair = false;
            if (objectInHand)
            {
                molecule.GetComponent<Container>().move = true;
                ReleaseObject();
            }
        }
    }
}
