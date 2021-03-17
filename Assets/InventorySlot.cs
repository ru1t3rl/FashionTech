using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    public Material emptyMaterial;
    public Material fullMaterial;

    public GameObject container;
    public Vector3 scale;

    private GameObject storedObject = null;
    private FixedJoint joint = null;
    public bool isAvailable = true;


    // Start is called before the first frame update
    void Start()
    {
        SetContainerEmpty();
        scale = transform.localScale;
    }

    void Awake()
    {
        joint = GetComponent<FixedJoint>();
    }

    public void Attatch(GameObject newObject)
    {
        if (storedObject) { return; }
        storedObject = newObject;

        storedObject.transform.position = transform.position;
        storedObject.transform.rotation = Quaternion.identity;

        Rigidbody targetBody = storedObject.gameObject.GetComponent<Rigidbody>();
        joint.connectedBody = targetBody;
        SetContainerFull();
    }

    public void Detatch()
    {
        if (!storedObject) { return; }
        joint.connectedBody = null;
        storedObject.GetComponent<Storeable>().SetContainer(null);
        storedObject = null;
        SetContainerEmpty();
    }

    public void SetContainerEmpty()
    {
        container.GetComponent<Renderer>().material = emptyMaterial;
        isAvailable = true;
    }

    public void SetContainerFull()
    {
        container.GetComponent<Renderer>().material = fullMaterial;
        isAvailable = false;
    }

   
    public bool GetAvailability()
    {
        return isAvailable;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (storedObject)
        {
            //storedObject.GetComponent<Storeable>().EnableCollisions(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (storedObject)
        {
            //storedObject.GetComponent<Storeable>().EnableCollisions(false);
        }
    }


}
