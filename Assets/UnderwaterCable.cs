using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class UnderwaterCable : MonoBehaviour
{

    [SerializeField] Transform start;
    [SerializeField] float cableEndHeight;
    [SerializeField] float maxSegmentLength;
    [SerializeField] int segmentCount;


    LineRenderer lr;


    void Start()
    {
        lr = GetComponent<LineRenderer>();

        lr.positionCount = segmentCount + 1;

        for (int i = 0; i < segmentCount + 1; i++)
        {
            float height = cableEndHeight / segmentCount * i;
            lr.SetPosition(i, start.position + Vector3.up * height);
        }
    }

    void Update()
    {
    }
}
