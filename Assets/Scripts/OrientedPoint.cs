using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct OrientedPoint
{
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;


    public OrientedPoint(Vector3 position, Quaternion rotation, Vector3 scale)
    {
        this.position = position;
        this.rotation = rotation;
        this.scale = scale;
    }

    public OrientedPoint(Vector3 position, Vector3 forward, Vector3 scale)
    {
        this.position = position;
        this.rotation = Quaternion.LookRotation(forward);
        this.scale = scale;
    }

    public Vector3 LocalToWorldPosition(Vector3 localSpacePosition)
    {
        return position + rotation * localSpacePosition;

    }

    public Vector3 LocalToWorldVector(Vector3 localSpacePosition)
    {
        return rotation * localSpacePosition;

    }
}
