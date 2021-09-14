using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WaypointManagerWindow : EditorWindow
{
    [MenuItem("Tools/Waypoint Editor")]
    public static void Open()
    {
        GetWindow<WaypointManagerWindow>();
    }

    public Transform waypointRoot;

    private void OnGUI()
    {
        SerializedObject obj = new SerializedObject(this);

        EditorGUILayout.PropertyField(obj.FindProperty("waypointRoot"));

        if (waypointRoot == null)
        {
            EditorGUILayout.HelpBox("Root transform nust be selected. Please assign a root transform.",
                MessageType.Warning);
        }
        else
        {
            EditorGUILayout.BeginVertical("box");
            DrawButtons();
            EditorGUILayout.EndVertical();
        }

        obj.ApplyModifiedProperties();
    }

    private void DrawButtons()
    {
        if (GUILayout.Button("Create Waypoint"))
        {
            CreateWaypoint();
        }

        if (Selection.activeGameObject != null && Selection.activeGameObject.GetComponent<Waypoint>())
        {
            if (GUILayout.Button("Add branch Waypoint"))
            {
                CreateBranch();
            }
            if (GUILayout.Button("Create Waypoint Before"))
            {
                CreateWaypointBefore();
            }

            if (GUILayout.Button("Create Waipoint After"))
            {
                CreateWaypointAfter();
            }

            if (GUILayout.Button("Remove Selected Waypoint"))
            {
                RemoveWaypoint();
            }
        }
    }

    private void CreateBranch()
    {
        GameObject waypointObject = new GameObject("Waypoint_" + waypointRoot.childCount, typeof(Waypoint));
        waypointObject.transform.SetParent(waypointRoot, false);

        Waypoint waypoint = waypointObject.GetComponent<Waypoint>();

        Waypoint branchedFrom = Selection.activeGameObject.GetComponent<Waypoint>();
        if (branchedFrom.branches == null)
            branchedFrom.branches = new List<Waypoint>();
        branchedFrom.branches.Add(waypoint);

        waypoint.transform.position = branchedFrom.transform.position;
        waypoint.transform.forward = branchedFrom.transform.forward;
        waypoint.previousWaypoint = branchedFrom;

        Selection.activeGameObject = waypoint.gameObject;
    }

    private void CreateWaypoint()
    {
        GameObject waypointObject = new GameObject("Waypoint_" + waypointRoot.childCount, typeof(Waypoint));
        waypointObject.transform.SetParent(waypointRoot, false);

        Waypoint waypoint = waypointObject.GetComponent<Waypoint>();
        if (waypointRoot.childCount > 1)
        {
            if (Selection.activeGameObject != null && Selection.activeGameObject.GetComponent<Waypoint>())
            {
                waypoint.previousWaypoint = Selection.activeGameObject.GetComponent<Waypoint>();
            }
            else
            {
                waypoint.previousWaypoint = waypointRoot
                    .GetChild(waypointRoot.childCount - 2)
                    .GetComponent<Waypoint>();
            }

            waypoint.previousWaypoint.nextWaypoint = waypoint;
            // Place the waypoint at the last position
            waypoint.transform.position = waypoint.previousWaypoint.transform.position;
            waypoint.transform.forward = waypoint.previousWaypoint.transform.forward;
        }

        Selection.activeGameObject = waypointObject;
    }

    private void CreateWaypointAfter()
    {
        GameObject waypointObject = new GameObject("Waypoint_" + waypointRoot.childCount, typeof(Waypoint));
        waypointObject.transform.SetParent(waypointRoot, false);

        Waypoint waypoint = waypointObject.GetComponent<Waypoint>();

        Waypoint selectedWaypoint = Selection.activeGameObject.GetComponent<Waypoint>();

        waypointObject.transform.position = selectedWaypoint.transform.position;
        waypointObject.transform.forward = selectedWaypoint.transform.forward;

        waypoint.previousWaypoint = selectedWaypoint;

        if (selectedWaypoint.nextWaypoint != null)
        {
            selectedWaypoint.nextWaypoint.previousWaypoint = waypoint;
            waypoint.nextWaypoint = selectedWaypoint.nextWaypoint;
        }

        selectedWaypoint.nextWaypoint = waypoint;

        waypoint.transform.SetSiblingIndex(selectedWaypoint.transform.GetSiblingIndex());

        Selection.activeGameObject = waypoint.gameObject;
    }

    private void CreateWaypointBefore()
    {
        GameObject waypointObject = new GameObject("Waypoint_" + waypointRoot.childCount, typeof(Waypoint));
        waypointObject.transform.SetParent(waypointRoot, false);

        Waypoint waypoint = waypointObject.GetComponent<Waypoint>();

        Waypoint selectedWaypoint = Selection.activeGameObject.GetComponent<Waypoint>();

        waypointObject.transform.position = selectedWaypoint.transform.position;
        waypointObject.transform.forward = selectedWaypoint.transform.forward;

        if (selectedWaypoint.previousWaypoint != null)
        {
            waypoint.previousWaypoint = selectedWaypoint.previousWaypoint;
            selectedWaypoint.previousWaypoint.nextWaypoint = waypoint;
        }

        waypoint.nextWaypoint = selectedWaypoint;

        selectedWaypoint.previousWaypoint = waypoint;

        waypoint.transform.SetSiblingIndex(selectedWaypoint.transform.GetSiblingIndex());

        Selection.activeGameObject = waypoint.gameObject;
    }

    private void RemoveWaypoint()
    {
        Waypoint selectedWaipoint = Selection.activeGameObject.GetComponent<Waypoint>();

        if (selectedWaipoint.nextWaypoint != null)
        {
            selectedWaipoint.nextWaypoint.previousWaypoint = selectedWaipoint.previousWaypoint;
        }

        if (selectedWaipoint.previousWaypoint != null)
        {
            selectedWaipoint.previousWaypoint.nextWaypoint = selectedWaipoint.nextWaypoint;
        }
        
        DestroyImmediate(selectedWaipoint.gameObject);
    }
}