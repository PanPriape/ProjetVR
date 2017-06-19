using UnityEngine;

public class MoleculeManager : MonoBehaviour
{

    public SteamVR_TrackedObject trackedObj;

    public SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }

    public GameObject molecule;
    
    public FixedJoint AddFixedJoint()
    {
        FixedJoint fx = gameObject.AddComponent<FixedJoint>();
        fx.breakForce = 20000;
        fx.breakTorque = 20000;
        return fx;
    }

    public void GrabObject()
    {
        molecule.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        var joint = AddFixedJoint();
        joint.connectedBody = molecule.GetComponent<Rigidbody>();
    }

    public void ReleaseObject()
    {
        if (GetComponent<FixedJoint>())
        {
            GetComponent<FixedJoint>().connectedBody = null;
            Destroy(GetComponent<FixedJoint>());
        }
        molecule.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
    }

    void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    void Update()
    {
        if (Controller.GetHairTriggerDown())
        {
                GrabObject();
        }

        if (Controller.GetHairTriggerUp())
        {
                ReleaseObject();
        }
    }
}
