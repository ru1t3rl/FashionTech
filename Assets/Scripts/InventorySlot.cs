using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class InventorySlot : MonoBehaviour
{
    [SerializeField]
    private Material emptyMaterial;
    [SerializeField]
    private Material fullMaterial;

    public GameObject container;
    public Vector3 scale;
    public float upPositionOffset = 0.1f;

    private GameObject storedObject = null;
    private FixedJoint joint = null;
    public bool isAvailable = true;
    private bool useOffset = false;
    public GameObject offsetEmpty;


    // Start is called before the first frame update
    void Start()
    {
        SetContainerEmpty();
        scale = transform.localScale;
        Debug.Log(offsetEmpty);
        if (offsetEmpty != null) useOffset = true;
        Debug.Log(useOffset);
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

    public void AttachOnOffset(GameObject newObject)
    {
        if (storedObject) { return; }
        if (!useOffset) return;

        storedObject = newObject;

        storedObject.transform.position = offsetEmpty.transform.position + Vector3.up * upPositionOffset;
        storedObject.transform.rotation = Quaternion.identity;

        Rigidbody targetBody = storedObject.gameObject.GetComponent<Rigidbody>();
        SetContainerFull();
    }

    public void Detatch()
    {
        Debug.Log($"test {storedObject}");

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
        if (storedObject && IsHand(other))
        {
            storedObject.GetComponent<Storeable>().EnableCollisions(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (storedObject && IsHand(other))
        {
            storedObject.GetComponent<Storeable>().EnableCollisions(false);
        }
    }

    //all collision is based on the fingers or other objects within the hand, wich allways have a handcolider grandparent
    public bool IsHand(Collider other)
    {
        if (other.transform.parent && other.transform.parent.parent && other.transform.parent.parent.GetComponent<HandCollider>())
        {
            return true;
        }
        else
        {
            return false;
        }
    }


}
