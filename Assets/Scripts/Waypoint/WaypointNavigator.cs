using UnityEngine;
using Random = UnityEngine.Random;

public class WaypointNavigator : MonoBehaviour
{
    public Waypoint currentWaypoint;

    private CarController _controller;

    private void Awake()
    {
        _controller = GetComponent<CarController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _controller.SetDestination(currentWaypoint);
    }

    // Update is called once per frame
    void Update()
    {
        if (_controller.reachedDestination)
        {
            bool shouldBranch = false;
            if (currentWaypoint.branches != null && currentWaypoint.branches.Count > 0)
            {
                shouldBranch = Random.Range(0f, 1f) <= currentWaypoint.branchRatio ? true : false;
            }

            if (shouldBranch)
            {
                currentWaypoint = currentWaypoint.branches[Random.Range(0, currentWaypoint.branches.Count - 1)];
                
            }
            else
            {
                currentWaypoint = currentWaypoint.nextWaypoint;
            }
            
            _controller.SetDestination(currentWaypoint);
        }
    }
}