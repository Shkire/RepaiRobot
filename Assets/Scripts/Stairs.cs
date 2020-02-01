using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stairs : MonoBehaviour
{
    public Transform[] points = new Transform[2];

    public Vector3 bottomPosition
    {
        get
        {
            if (points[0].position.y < points[1].position.y)
            {
                return points[0].position;
            }
            return points[1].position;
        }
    }

    public Vector3 topPosition
    {
        get
        {
            if (points[0].position.y > points[1].position.y)
            {
                return points[0].position;
            }
            return points[1].position;
        }
    }
}
