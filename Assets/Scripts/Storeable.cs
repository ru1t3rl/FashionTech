using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storeable : MonoBehaviour
{

    protected bool isAvailable = true;
    private InventorySlot container;

    public void SetContainer(InventorySlot newContainer)
    {
        container = newContainer;
    }

    public void Store()
    {
        if (container && container.GetAvailability())
        {
            container.Attatch(this.gameObject);
            isAvailable = false;
        }
    }

    public void PickUp()
    {
        if (container)
        {
            container.Detatch();
            isAvailable = true;

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "InventorySlot" && isAvailable)
        {
            SetContainer(other.gameObject.GetComponent<InventorySlot>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "InventorySlot" && other.gameObject.GetComponent<InventorySlot>() == container)
        {
            container = null;

        }
    }
}
