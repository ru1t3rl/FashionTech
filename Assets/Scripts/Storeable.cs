using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Storeable : MonoBehaviour
{
    private Vector3 defaultScale = Vector3.one;
    public Vector3 storedScale = Vector3.one;
    public Collider objectCollider;

    protected bool isAvailable = true, blocked = false;
    private InventorySlot container;

    public void Awake()
    {
        defaultScale = this.gameObject.transform.localScale;
    }
    public void SetContainer(InventorySlot newContainer)
    {
        container = newContainer;
    }

    public void onDetachFromHand()
    {
        if (container && container.CompareTag("PlantSlot") && transform.CompareTag("Plant"))
        {
            LockInPlace();
        }
        else if (container && container.CompareTag("InventorySlot"))
        {
            Store();
        }
    }

    public void Store()
    {
        Debug.Log($"test3Store storing {container} {container.GetAvailability()}");

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
            container.AttachOnOffset(this.gameObject);

            Destroy(transform.GetComponent<Throwable>());
            Destroy(transform.GetComponent<Interactable>());
            Destroy(transform.GetComponent<Rigidbody>());
        }

        var plantComponent = GetComponent<Plant>();

        if (container.CompareTag("PlantSlot") &&
            transform.CompareTag("Plant") &&
            plantComponent != null)
        {
            plantComponent.PlacedPlant();
        }
    }

    public void PickUp()
    {
        Debug.Log($"test2 {container}");

        if (container)
        {
            RemoveFromContainer();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("x enter");

        if (transform.CompareTag("Plant") && other.tag == "PlantSlot" && isAvailable && !blocked)
        {
            SetContainer(other.gameObject.GetComponent<InventorySlot>());
            blocked = true;
        }

        if (transform.CompareTag("Storable") || transform.CompareTag("Plant") && other.tag == "InventorySlot" && isAvailable && !blocked)
        {
            SetContainer(other.gameObject.GetComponent<InventorySlot>());
            blocked = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("x exit");

        if (other.CompareTag("InventorySlot"))
        {
            blocked = false;
            if (container)
            {
                RemoveFromContainer();
            }
        }
    }

    public void EnableCollisions(bool enable)
    {
        objectCollider.enabled = enable;
    }

    public void RemoveFromContainer ()
    {
        EnableCollisions(true);
        container.Detatch();
        this.transform.localScale = defaultScale;
        isAvailable = true;
        container = null;
    }
}
