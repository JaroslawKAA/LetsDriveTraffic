using UnityEngine;
using Waypoints;

public class CarControllerUsedMotor : MonoBehaviour
{
    [HideInInspector]
    public bool isDestinationInFront;

    private CarMotor _motor;
    private WaypointNavigator _navigator;
    private CarCollisionDetector _collisionDetector;

    private void Awake()
    {
        _motor = GetComponent<CarMotor>();
        _navigator = GetComponent<WaypointNavigator>();
        _collisionDetector = GetComponent<CarCollisionDetector>();
    }

    // Update is called once per frame
    void Update()
    {
        _motor.ResetInputs();

        if (_navigator.CurrentWaypoint != null)
        {
            if (_collisionDetector.IsCloseObstacle)
            {
                _motor.Brake();
            }
            else if (_collisionDetector.IsFarObstacle)
            {
                _motor.SlowDown();
            }
            else if (IsTargetInFront())
            {
                _motor.MoveForward();
            }
            else
            {
                _motor.Brake();
            }

            var destinationVector = (_navigator.CurrentWaypoint.transform.position - transform.position).normalized;
            float angleToDir = Vector3.SignedAngle(transform.forward, destinationVector, Vector3.up);

            if (angleToDir > 0)
                _motor.TurnRight();
            else
                _motor.TurnLeft();
        }
        else
        {
            // Reached target
            _motor.Brake();
            _motor.StopTurning();
        }
    }

    private bool IsTargetInFront()
    {
        var destinationVector = _navigator.CurrentWaypoint.transform.position - transform.position;
        float angle = Vector2.Angle(
            new Vector2(transform.forward.x, transform.forward.z),
            new Vector2(destinationVector.x, destinationVector.z));

        if (Mathf.Abs(angle) < 60)
        {
            isDestinationInFront = true;
            return true;
        }

        isDestinationInFront = false;
        return false;
    }

    private void OnDrawGizmos()
    {
        if (_navigator.CurrentWaypoint == null)
            return;

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position + new Vector3(0, 0.2f, 0), _navigator.CurrentWaypoint.transform.position);
    }
}