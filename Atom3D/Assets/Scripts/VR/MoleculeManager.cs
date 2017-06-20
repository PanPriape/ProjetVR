using UnityEngine;

public class MoleculeManager : MonoBehaviour
{

    public SteamVR_TrackedObject trackedObj;

    public SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }

    public GameObject molecule;
    public GameObject innermolecule;
    public GameObject rightController;
    public bool scale;
    public float baseDistance;
    public float size;


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
        //rightController = GameObject.Find("Controller (right)");
        scale = false;
    }

    void Update()
    {
        if (scale)
        {
            float newDistance = (this.transform.position - rightController.transform.position).magnitude;
            float newSize = 0;
            if (newDistance >= baseDistance) { newSize = 1 * size * newDistance / baseDistance; }
            else { newSize = 1 *size * newDistance / baseDistance; }
            if (newSize < 0.10) { newSize = 0.10f; }
            innermolecule.transform.localScale = new Vector3(newSize, newSize, newSize);
            if (Controller.GetHairTriggerUp() || !rightController.GetComponent<ControllerGrabObject>().hair)
            {
                scale = false;
            }
        }
        else
        {
            if (Controller.GetHairTriggerDown())
            {
                if (rightController.GetComponent<ControllerGrabObject>().hair)
                {
                    scale = true;
                    baseDistance = (this.transform.position - rightController.transform.position).magnitude;
                    size = innermolecule.transform.localScale.x;
                }
                else
                {
                    GrabObject();
                }
            }

            if (Controller.GetHairTriggerUp())
            {
                ReleaseObject();
            }
            if (Controller.GetPress(SteamVR_Controller.ButtonMask.Touchpad))
            {
                molecule.transform.localScale += new Vector3(0.001f, 0.001f, 0.001f);
            }
        }
    }
}
