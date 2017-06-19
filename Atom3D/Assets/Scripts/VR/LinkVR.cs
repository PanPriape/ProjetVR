using UnityEngine;

public class LinkVR : MonoBehaviour
{

    private GameObject sphere1 = null;
    private GameObject sphere2 = null;

    //FALSE = simple | TRUE = double
    private bool type = false;

    public GameObject getSphere1()
    {
        return this.sphere1;
    }

    public void setSphere1(GameObject sphere)
    {
        this.sphere1 = sphere;
    }

    public GameObject getSphere2()
    {
        return this.sphere2;
    }

    public void setSphere2(GameObject sphere)
    {
        this.sphere2 = sphere;
    }

    public bool getType()
    {
        return this.type;
    }

    public void setType(bool type)
    {
        this.type = type;
    }

    void Start()
    {
        this.transform.parent = GameObject.FindGameObjectsWithTag("molecule")[0].transform;
        transform.localScale = new Vector3(0.15f, 1.0f, 0.15f);
    }

    void Update()
    {
        if (sphere1 == null || sphere2 == null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Vector3 dir = sphere2.transform.position - sphere1.transform.position;
            transform.position = sphere1.transform.position + 0.5f * dir;
            Vector3 scale = transform.localScale;
            scale.y = dir.magnitude * 0.5f;
            transform.localScale = scale;
            transform.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        }
    }
}
