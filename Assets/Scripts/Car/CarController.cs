using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Waypoints;

namespace Car
{
    public class CarController : MonoBehaviour
    {
        #region Fields

        [SerializeField] private float speedMax = 20f;
        [SerializeField] private float targetSpeed;
    
        private NavMeshAgent _navMeshAgent;
        private WaypointNavigator _navigator;
        private CarCollisionDetector _collisionDetector;

        #endregion Fields

        public float SpeedMax => speedMax;

        #region Events

        public Action OnNavMeshAgentEnabled;

        #endregion

        // TODO move it to nav mesh generator
        public void OnNavMeshAgentEnabled_Invoke()
        {
            OnNavMeshAgentEnabled?.Invoke();
        }

        public void Stop()
        {
            targetSpeed = 0f;
        }


        private bool _isLimitSpeedInvoked = false;
        public void LimitSpeed(int limit)
        {
            if (_isLimitSpeedInvoked)
                return;

            _isLimitSpeedInvoked = true;
            targetSpeed = limit;
            StartCoroutine(ResetSpeed(10));
        }

        #region Private Methods

        private void Awake()
        {
            targetSpeed = speedMax;
        
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _navigator = GetComponent<WaypointNavigator>();
            _collisionDetector = GetComponent<CarCollisionDetector>();

            OnNavMeshAgentEnabled += SetNextDestination;
            _navigator.OnReachedCurrentWaypoint += SetNextDestination;
        }

        private void Update()
        {
            _navMeshAgent.destination = _navigator.CurrentWaypoint.transform.position;
            
            if (_collisionDetector.IsCarInFront && _collisionDetector.IsCloseObstacle)
            {
                _navMeshAgent.speed = 0f;
            }
            else if ((_collisionDetector.IsCarInFront
                      || _collisionDetector.IsBuildingInFront)
                     && _collisionDetector.IsFarObstacle)
            {
                _navMeshAgent.speed = targetSpeed / 2f;
            }
            else
            {
                _navMeshAgent.speed = targetSpeed;
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

        public void ResetSpeed()
        {
            targetSpeed = speedMax;
            _isLimitSpeedInvoked = false;
        }
        
        private IEnumerator ResetSpeed(float afterSeconds)
        {
            yield return new WaitForSeconds(afterSeconds);

            targetSpeed = speedMax;
            _isLimitSpeedInvoked = false;
        }

        private void OnDrawGizmos()
        {
            Vector3 lineOffset = new Vector3(0, .3f, 0);

            Debug.DrawLine(transform.position + lineOffset, _navMeshAgent.destination, Color.cyan);
        }

        #endregion
    }
}