using System;
using UnityEngine;
using UnityEngine.AI;

public class CarController : MonoBehaviour
{
    #region Fields

    private float speedMax = 20f;

    private NavMeshAgent _navMeshAgent;
    private WaypointNavigator _navigator;
    private CarCollisionDetector _collisionDetector;

    #endregion Fields

    #region Events

    public Action OnNavMeshAgentEnabled;

    #endregion

    public void OnNavMeshAgentEnabled_Invoke()
    {
        OnNavMeshAgentEnabled?.Invoke();
    }

    #region Private Methods

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navigator = GetComponent<WaypointNavigator>();
        _collisionDetector = GetComponent<CarCollisionDetector>();

        OnNavMeshAgentEnabled += SetNextDestination;
        _navigator.OnReachedCurrentWaypoint += SetNextDestination;
    }

    private void Update()
    {
        _navMeshAgent.destination = _navigator.CurrentWaypoint.transform.position;
        
        if (_collisionDetector.IsCarInFront)
        {
            if (_collisionDetector.IsCloseObstacle)
            {
                _navMeshAgent.speed = 0f;
            }
            else
            {
                _navMeshAgent.speed = speedMax / 2f;
            }
        }
        else
        {
            _navMeshAgent.speed = speedMax;
        }
        
    }

    private void SetNextDestination()
    {
        _navMeshAgent.destination = _navigator.CurrentWaypoint.transform.position;
    }

    private void SetNextDestination(Waypoint waypoint)
    {
        if (waypoint == null)
            return;
        
        _navMeshAgent.destination = waypoint.transform.position;
    }

    private void OnDrawGizmos()
    {
        Vector3 lineOffset = new Vector3(0, .3f, 0);
        
        Debug.DrawLine(transform.position + lineOffset, _navMeshAgent.destination, Color.cyan);
    }

    #endregion
}