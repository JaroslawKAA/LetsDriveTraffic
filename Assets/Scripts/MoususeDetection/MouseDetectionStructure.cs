using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class MouseDetectionStructure : MonoBehaviour
{
    public GameObject mouseSnapperPrefab;
    public GameObject snappersParent;

    private void Awake()
    {
        Assert.IsNotNull(mouseSnapperPrefab);
        Assert.IsNotNull(snappersParent);
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in transform)
        {
            Waypoint waypoint = child.GetComponent<Waypoint>();
            if (waypoint.nextWaypoint != null)
            {
                InstantiateSnapper(waypoint, waypoint.nextWaypoint);
            }

            if (waypoint.branches != null && waypoint.branches.Count > 0)
            {
                foreach (Waypoint branch in waypoint.branches)
                {
                    if (branch.nextWaypoint != null)
                    {
                        InstantiateSnapper(waypoint, branch);
                    }
                }
            }
        }
    }

    private void InstantiateSnapper(Waypoint start, Waypoint end)
    {
        Vector3 instantiatePosition =
            Vector3.Lerp(start.transform.position, end.transform.position, .5f);
        Quaternion instantiationRotation = Quaternion.LookRotation(end.transform.position
                                                                   - start.transform.position);
        GameObject snapper = Instantiate(mouseSnapperPrefab, instantiatePosition, instantiationRotation,
            snappersParent.transform);
        snapper.transform.localScale =
            new Vector3(4, 4, Vector3.Distance(start.transform.position, end.transform.position));
        MouseSnaper mouseSnapper = snapper.GetComponent<MouseSnaper>();
        mouseSnapper.start = start;
        mouseSnapper.end = end;
    }
}