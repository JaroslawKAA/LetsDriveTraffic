using UnityEngine;

public class CarMotorOLD : MonoBehaviour
{
    #region Fields
    
    private float speed;
    private float speedMin = -50f;
    private float speedMax = 70f;
    private float acceleration = 2f;
    private float brakeSpeed = 2f;
    private float reverseSpeed = 30f;
    private float idleSlowdown = 10f;

    private float turnSpeed;
    private float turnSpeedMax = 300f;
    private float turnSpeedAcceleration = 300f;
    private float turnIdleSlowDown = 500f;
    
    private float targetSpeed = 50;
    private float rotationSpeed = 5f;

    private float forwardAmount;
    private float turnAmount;
    
    private Rigidbody _rigidbody;
    private bool _isMoving;
    private Vector3 _movement;
    
    #endregion
   
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!_isMoving)
        {
            DecreaseSpeed();
        }

        _rigidbody.MovePosition(transform.position + transform.rotation * _movement);
    }

    public void MoveForward()
    {
        IncreaseSpeed();
    }

    public void MoveBackward()
    {
        IncreaseSpeed();
    }

    public void Brake()
    {
        DecreaseSpeed(brakeSpeed);
    }

    public void RotateRight()
    {
        _rigidbody.MoveRotation(Quaternion.Euler(
            transform.rotation.eulerAngles.x,
            transform.rotation.eulerAngles.y + rotationSpeed * Time.deltaTime,
            transform.rotation.eulerAngles.z));
    }

    public void RotateLeft()
    {
        _rigidbody.MoveRotation(Quaternion.Euler(
            transform.rotation.eulerAngles.x,
            transform.rotation.eulerAngles.y - rotationSpeed * Time.deltaTime,
            transform.rotation.eulerAngles.z));
    }

    public void LookAt(Vector3 target)
    {
        var lookPos = target - transform.position;
        lookPos.y = 0;
        var targetRotation = Quaternion.LookRotation(lookPos);
        var currentRotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        
        _rigidbody.MoveRotation(currentRotation);
    }
    
    public void LookAt(Waypoint target)
    {
        var lookPos = target.transform.position - transform.position;
        lookPos.y = 0;
        var targetRotation = Quaternion.LookRotation(lookPos);
        var currentRotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        
        _rigidbody.MoveRotation(currentRotation);
    }

    public void TurnOf()
    {
        _isMoving = false;
    }

    private void IncreaseSpeed()
    {
        _isMoving = true;
        speed += acceleration * Time.deltaTime;
        speed = Mathf.Clamp(speed, 0, targetSpeed);
        _movement = new Vector3(0, 0, speed * Time.deltaTime);
    }

    private void DecreaseSpeed()
    {
        speed -= acceleration * 4 * Time.deltaTime;
        speed = Mathf.Clamp(speed, 0, targetSpeed);
        _movement = new Vector3(0, 0, speed * Time.deltaTime);
    }
    
    private void DecreaseSpeed(float decreasePower)
    {
        speed -= acceleration * 4 * Time.deltaTime * decreasePower;
        speed = Mathf.Clamp(speed, 0, targetSpeed);
        _movement = new Vector3(0, 0, speed * Time.deltaTime);
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Car") || collision.transform.CompareTag("Building")) {
            speed = Mathf.Clamp(speed, 0f, 20f);
        }
    }
}