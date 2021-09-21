using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Waypoints
{
    public class WaypointNavigator : MonoBehaviour
    {
        #region Properties

        public Waypoint PreviousWaypoint
        {
            get => _previousWaypoint;
            private set { _previousWaypoint = value; }
        }

        public Waypoint CurrentWaypoint
        {
            get => _currentWaypoint;
            private set
            {
                _currentWaypoint = value;
            }
        }

        public Waypoint NextWaypoint
        {
            get => _nextWaypoint;
            private set
            {
                if (value == null)
                    CarSpawner.S.RemoveCar(gameObject);

                _nextWaypoint = value;
            }
        }

        public bool ReachedTarget => ReachedCurrentWaypoint();

        #endregion

        #region Fields

        private float _reachedTargetDistance = 4f;
        private Waypoint _currentWaypoint;
        private Waypoint _previousWaypoint;
        private Waypoint _nextWaypoint;

        #endregion

        #region Events

        public Action<Waypoint> OnReachedCurrentWaypoint;

        #endregion

        public void SetCurrentWaypoint(Waypoint waypoint)
        {
            this.PreviousWaypoint = waypoint.previousWaypoint;
            this.CurrentWaypoint = waypoint;
            this.NextWaypoint = GetNextWaypoint();
        }
    
        void Update()
        {
            if (ReachedCurrentWaypoint())
            {
                PreviousWaypoint = CurrentWaypoint;

                CurrentWaypoint = NextWaypoint;

                NextWaypoint = GetNextWaypoint();

                OnReachedCurrentWaypoint?.Invoke(CurrentWaypoint);
            }
        }

        private Waypoint GetNextWaypoint()
        {
            bool shouldBranch = false;
            if (CurrentWaypoint.branches != null && CurrentWaypoint.branches.Count > 0)
            {
                shouldBranch = Random.Range(0f, 1f) <= CurrentWaypoint.branchRatio;
            }

            if (shouldBranch)
            {
                return CurrentWaypoint.branches[Random.Range(0, CurrentWaypoint.branches.Count - 1)];
            }

            return CurrentWaypoint.nextWaypoint;
        }

        private bool ReachedCurrentWaypoint()
        {
            if (CurrentWaypoint == null)
                return false;

            float distanceToTarget = Vector3.Distance(transform.position, CurrentWaypoint.transform.position);

            return !(distanceToTarget > _reachedTargetDistance);
        }
    }
}