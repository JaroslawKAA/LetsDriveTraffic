using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Waypoint : MonoBehaviour
{
    public Waypoint previousWaypoint;
    public Waypoint nextWaypoint;

    [Range(0f, 2f)] public float width = 1f;

    public List<Waypoint> branches;
    [Range(0f, 1f)] public float branchRatio = 0.5f;
    [SerializeField]
    private bool _isCarHere;

    public bool IsCarHere
    {
        get => _isCarHere;
        private set => _isCarHere = value;
    }

    public Vector3 GetPosition()
    {
        Vector3 minBound = transform.position + transform.right * width / 2f;
        Vector3 maxBound = transform.position - transform.right * width / 2f;

        return Vector3.Lerp(minBound, maxBound, Random.Range(0f, 1f));
    }

    private void Update()
    {
        Collider[] results = Physics.OverlapSphere(transform.position, 2);
        if (results.Any(c => c.transform.CompareTag("Car")))
        {
            IsCarHere = true;
        }
        else
        {
            IsCarHere = false;
        }
    }
}