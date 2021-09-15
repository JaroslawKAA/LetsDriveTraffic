using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class WaypointNavigator : MonoBehaviour
{
    #region Properties

    public Waypoint CurrentWaypoint
    {
        get => _currentWaypoint;
        private set
        {
            if (value == null)
            {
                CarSpawner.S.RemoveCar(gameObject);
            }
            _currentWaypoint = value;
        }
    }

    public bool ReachedTarget => ReachedCurrentWaypoint();

    #endregion

    #region Fields

    private float _reachedTargetDistance = 4f;
    private Waypoint _currentWaypoint;

    #endregion

    #region Events

    public Action<Waypoint> OnReachedCurrentWaypoint;

    #endregion

    // Update is called once per frame
    void Update()
    {
        if (ReachedCurrentWaypoint())
        {
            bool shouldBranch = false;
            if (CurrentWaypoint.branches != null && CurrentWaypoint.branches.Count > 0)
            {
                shouldBranch = Random.Range(0f, 1f) <= CurrentWaypoint.branchRatio ? true : false;
            }

            if (shouldBranch)
            {
                CurrentWaypoint = CurrentWaypoint.branches[Random.Range(0, CurrentWaypoint.branches.Count - 1)];
            }
            else
            {
                CurrentWaypoint = CurrentWaypoint.nextWaypoint;
            }

            OnReachedCurrentWaypoint?.Invoke(CurrentWaypoint);
        }
    }

    public void SetCurrentWaypoint(Waypoint waypoint)
    {
        this.CurrentWaypoint = waypoint;
    }

    private bool ReachedCurrentWaypoint()
    {
        if (CurrentWaypoint == null)
            return false;

        float distanceToTarget = Vector3.Distance(transform.position, CurrentWaypoint.transform.position);

        return !(distanceToTarget > _reachedTargetDistance);
    }
}