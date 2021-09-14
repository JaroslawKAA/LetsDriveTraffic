using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class WaypointEditor
{
    private static Color waypointColor = Color.magenta;
    
    [DrawGizmo(GizmoType.NonSelected | GizmoType.Selected | GizmoType.Pickable)]
    public static void OnDrawSceneGizmo(Waypoint waypoint, GizmoType gizmoType)
    {
        if (gizmoType == GizmoType.Selected)
        {
            Gizmos.color = Color.yellow;
        }
        else
        {
            Gizmos.color = Color.yellow * 0.5f;
        }

        Gizmos.DrawSphere(waypoint.transform.position, .1f);

        Gizmos.color = waypointColor;
        Gizmos.DrawLine(waypoint.transform.position + (waypoint.transform.right * waypoint.width / 2f),
            waypoint.transform.position - (waypoint.transform.right * waypoint.width / 2f));

        if (waypoint.nextWaypoint != null)
            DrawArrowToNextWaypoint(waypoint);

        if (waypoint.branches != null)
        {
            foreach (Waypoint branch in waypoint.branches)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(waypoint.transform.position, branch.transform.position);
            }
        }
    }

    private static void DrawArrowToNextWaypoint(Waypoint waypoint)
    {
        float arrowSize = 1;
        
        Vector3 nextWaypointVector =
            (waypoint.nextWaypoint.transform.position - waypoint.transform.position).normalized * -2;
        
        var rightArrowWing = Quaternion.Euler(0, 30, 0) * nextWaypointVector * arrowSize;
        var leftArrowWing = Quaternion.Euler(0, -30, 0) * nextWaypointVector * arrowSize;
        
        Gizmos.DrawRay(waypoint.nextWaypoint.transform.position, rightArrowWing);
        Gizmos.DrawRay(waypoint.nextWaypoint.transform.position, leftArrowWing);

        Vector3 rightArrowCorner = waypoint.nextWaypoint.transform.position
                                   + (rightArrowWing);
        Vector3 leftArrowCorner = waypoint.nextWaypoint.transform.position
                                  + leftArrowWing;

        Gizmos.DrawLine(waypoint.transform.position, waypoint.nextWaypoint.transform.position + nextWaypointVector);
        Gizmos.DrawLine(leftArrowCorner, rightArrowCorner);
    }
}