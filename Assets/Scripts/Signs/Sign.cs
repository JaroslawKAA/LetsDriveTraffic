using Car;
using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// Object to place on the road
/// </summary>
public abstract class Sign : MonoBehaviour
{

    #region Fields
    
    [SerializeField] private Sprite _signSprite;
    [SerializeField] private Waypoint _previousWaypoint;
    [SerializeField] private Waypoint _nextWaypoint;

    #endregion

    #region Properties

    public Waypoint PreviousWaypoint
    {
        get => _previousWaypoint;
        set => _previousWaypoint = value;
    }

    public Waypoint NextWaypoint
    {
        get => _nextWaypoint;
        set => _nextWaypoint = value;
    }
    
    public Sprite SignSprite
    {
        get => _signSprite;
        private set => _signSprite = value;
    }

    #endregion

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

    private void Start()
    {
        Assert.IsNotNull(_signSprite);
    }

    private void OnTriggerEnter(Collider other)
    {
        Assert.IsNotNull(PreviousWaypoint);
        Assert.IsNotNull(NextWaypoint);
        
        if (other.transform.CompareTag("Car"))
        {
            WaypointNavigator carNavigator = other.GetComponent<WaypointNavigator>();
            // Check if car is on road with sign
            if (carNavigator.CurrentWaypoint == PreviousWaypoint 
                || carNavigator.CurrentWaypoint == NextWaypoint)
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