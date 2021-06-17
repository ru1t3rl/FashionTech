using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RipplePlane : MonoBehaviour
{
    [SerializeField] int terrainLayerIndex;
    [SerializeField] float yOffset = 10f;

    private void Awake()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1000, 1 << terrainLayerIndex))
        {
            transform.position = new Vector3(
                transform.position.x,
                 hit.point.y + yOffset,
                 transform.position.z
                );
        }
    }
}
