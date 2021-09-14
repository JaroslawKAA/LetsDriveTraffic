using UnityEngine;

public class CarController : MonoBehaviour
{
    public bool reachedDestination;
    public bool isDestinationInFront;

    private float reachedTargetDistance = 2f;
    private Waypoint _target;
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

        float distanceToTarget = Vector3.Distance(transform.position, _target.transform.position);

        if (distanceToTarget > reachedTargetDistance)
        {
            if (_collisionDetector.IsCloseObstacle)
            {
                _motor.Brake();
            }else if (_collisionDetector.IsFarObstacle)
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
            
            var destinationVector = (_target.transform.position - transform.position).normalized;
            float angleToDir = Vector3.SignedAngle(transform.forward, destinationVector, Vector3.up);
            if (angleToDir > 0)
                _motor.TurnRight();
            else
                _motor.TurnLeft();
        }
        else
        {
            //Reached target
            reachedDestination = true;
            _motor.Brake();
            _motor.StopTurning();
        }


        if (Vector3.Distance(transform.position, _target.transform.position) < 1f)
        {
            reachedDestination = true;
        }

        // _motor.SetInputs(forwardAmount, turnAmount);
    }

    public void SetDestination(Waypoint destination)
    {
        if (destination != null)
        {
            _target = destination;
            reachedDestination = false;
        }
        else
        {
            CarSpawner.S.RemoveCar(gameObject);
        }
    }

    private bool IsTargetInFront()
    {
        var destinationVector = _target.transform.position - transform.position;
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
        if (_target == null)
            return;

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position + new Vector3(0, 0.2f, 0), _target.transform.position);
    }
}