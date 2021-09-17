using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseSnaper : MonoBehaviour
{
    public Waypoint start;
    public Waypoint end;

    public Vector3 GetSnappedPosition(Vector3 mousePosition)
    {
        mousePosition.y = 0;
        
        float mouseDistanceToStart = Vector3.Distance(mousePosition, start.transform.position);
        float mouseDistanceToEnd = Vector3.Distance(mousePosition, end.transform.position);
        float mouseDistancesSum = mouseDistanceToStart + mouseDistanceToEnd;

        float mouseDistanceProportion = mouseDistanceToStart / mouseDistancesSum;

        return Vector3.Lerp(start.transform.position, end.transform.position, mouseDistanceProportion);
    }
}
