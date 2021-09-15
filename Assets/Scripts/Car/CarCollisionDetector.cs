using System.Linq;
using UnityEngine;

public class CarCollisionDetector : MonoBehaviour
{
    public float frontDetectionDistance = 10f;

    public bool IsFarObstacle
    {
        get => _isFarObstacle;
        private set => _isFarObstacle = value;
    }

    public bool IsCloseObstacle
    {
        get => _isCloseObstacle;
        private set => _isCloseObstacle = value;
    }

    public bool IsCarInFront
    {
        get => _isCarInFront;
        private set => _isCarInFront = value;
    }

    public bool IsBuildingInFront
    {
        get => _isBuildingInFront;
        private set => _isBuildingInFront = value;
    }

    public bool Crashed { get; private set; }

    private Renderer[] _renderers;
    private Collider _collider;
    [Header("Defined dynamically")]
    [SerializeField] private bool _isFarObstacle;
    [SerializeField] private bool _isCloseObstacle;
    [SerializeField] private bool _isCarInFront;
    [SerializeField] private bool _isBuildingInFront;

    private void Awake()
    {
        _renderers = transform
            .GetAllChildren()
            .Select(x => x.GetComponent<Renderer>())
            .ToArray();
        _collider = GetComponent<Collider>();
    }

    private void FixedUpdate()
    {
        IsCarInFront = false;
        IsBuildingInFront = false;
        IsFarObstacle = false;
        IsCloseObstacle = false;
        
        Ray ray = new Ray();
        ray.origin = transform.position + new Vector3(0, .36f, 0);
        ray.direction = transform.forward;
        _collider.enabled = false;
        if (Physics.Raycast(ray, out RaycastHit hit, frontDetectionDistance))
        {
            if (hit.transform.CompareTag("Car") || hit.transform.CompareTag("Building"))
            {
                float distanceToObstacle = Vector3.Distance(transform.position, hit.transform.position);

                if (distanceToObstacle > frontDetectionDistance / 2)
                {
                    IsFarObstacle = true;
                }
                else
                {
                    IsCloseObstacle = true;
                }
            }
            
            if (hit.transform.CompareTag("Car"))
            {
                Debug.DrawRay(ray.origin, ray.direction * frontDetectionDistance, Color.white);
                IsCarInFront = true;
            }
            else if (hit.transform.CompareTag("Building"))
            {
                Debug.DrawRay(ray.origin, ray.direction * frontDetectionDistance, Color.yellow);
                IsBuildingInFront = true;
            }
        }
        else
        {
            Debug.DrawRay(ray.origin, ray.direction * frontDetectionDistance, Color.black);
        }

        _collider.enabled = true;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!Crashed && (other.transform.CompareTag("Car") || other.transform.CompareTag("Building")))
        {
            Crashed = true;
            SetCrashedColor();
        }
    }

    private void SetCrashedColor()
    {
        foreach (Renderer car in _renderers)
        {
            car.material.SetColor("_Color", Color.red);
        }
    }
}