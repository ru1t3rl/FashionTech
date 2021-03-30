using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Storeable : MonoBehaviour
{
    private Vector3 defaultScale = Vector3.one;
    public Vector3 storedScale = Vector3.one;
    public Collider objectCollider;

    protected bool isAvailable = true;
    private InventorySlot container;

    public void Awake()
    {
        defaultScale = this.gameObject.transform.localScale;
    }
    public void SetContainer(InventorySlot newContainer)
    {
        container = newContainer;
    }

    public void Store()
    {
        EnableCollisions(false);
        if (container && container.GetAvailability())
        {
            isAvailable = false;
            container.Attatch(this.gameObject);
            transform.localScale = Vector3.Scale(container.scale, storedScale);
        }
    }

    public void LockInPlace()
    {
        if (container && container.GetAvailability())
        {
            isAvailable = false;
            container.AttachOnTop(this.gameObject);

           

            Destroy(transform.GetComponent<Throwable>());
            Destroy(transform.GetComponent<Interactable>());
            Destroy(transform.GetComponent<Rigidbody>());

            Debug.Log(transform);

        }
        
    }

    public void PickUp()
    {
        if (container)
        {
            EnableCollisions(true);
            container.Detatch();
            this.transform.localScale = defaultScale;
            isAvailable = true;
            container = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "InventorySlot" && isAvailable)
        {
            SetContainer(other.gameObject.GetComponent<InventorySlot>());
        }
    }

    public void EnableCollisions(bool enable)
    {
            objectCollider.enabled = enable;
    }
}
