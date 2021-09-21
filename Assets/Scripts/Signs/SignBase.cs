using Car;
using UnityEngine;
using UnityEngine.Assertions;
using Waypoints;

namespace Signs
{
    public abstract class SignBase : MonoBehaviour
    {
        [SerializeField] protected Sprite _signSprite;
        [SerializeField] private Waypoint _previousWaypoint;
        [SerializeField] private Waypoint _nextWaypoint;

        public Sprite SignSprite
        {
            get => _signSprite;
            private set => _signSprite = value;
        }

        private Waypoint PreviousWaypoint
        {
            get => _previousWaypoint;
            set => _previousWaypoint = value;
        }

        private Waypoint NextWaypoint
        {
            get => _nextWaypoint;
            set => _nextWaypoint = value;
        }

        private void Start()
        {
            Assert.IsNotNull(_signSprite);
            gameObject.layer = LayerMask.NameToLayer("Sign");
        }

        public void SetContext(MouseSnapper snapper)
        {
            PreviousWaypoint = snapper.start;
            NextWaypoint = snapper.end;
            float signArrowRotation = Quaternion.LookRotation(
                    PreviousWaypoint.transform.position - NextWaypoint.transform.position)
                .eulerAngles.y;
            GetComponentInChildren<SignArrow>().SetRotation(signArrowRotation + 180);
        }

        public void Drag(Vector3 targetPosition)
        {
            transform.position = new Vector3(targetPosition.x, 0, targetPosition.z);
        }

        private void OnTriggerStay(Collider other)
        {
            Assert.IsNotNull(PreviousWaypoint);
            Assert.IsNotNull(NextWaypoint);

            if (other.transform.TryGetComponent(out WaypointNavigator carNavigator))
            {
                // Check if car is on road with sign
                if ((carNavigator.CurrentWaypoint == PreviousWaypoint
                     || carNavigator.CurrentWaypoint == NextWaypoint)
                    && (carNavigator.PreviousWaypoint == PreviousWaypoint
                        || carNavigator.PreviousWaypoint == NextWaypoint))
                {
                    CarController car = other.GetComponent<CarController>();
                    SendMessageToCar(car);
                }
            }
        }

        /// <summary>
        /// Send message to car.
        /// </summary>
        /// <param name="car"></param>
        protected abstract void SendMessageToCar(CarController car);
    }
}