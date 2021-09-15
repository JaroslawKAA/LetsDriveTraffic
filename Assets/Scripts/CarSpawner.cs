using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class CarSpawner : MonoBehaviour
{
    public static CarSpawner S;
    
    public GameObject carPrefab;
    public int carToSpawn;
    public float spawnDelay = 1f;

    private List<GameObject> _cars;
    private List<Transform> _firstsWaypoints;
    private float spawnTimer = 1f;
    private bool _isInitSpawnDone;

    // Start is called before the first frame update
    void Start()
    {
        if (S != null)
            throw new Exception("Duplicated Singleton!");
        
        S = this;
        _cars = new List<GameObject>();
        GameManager.S.OnLevelGenerated += () =>
        {
            StartCoroutine(InitSpawn());
        };
        _firstsWaypoints = transform.GetAllChildren()
            .Where(c => c.GetComponent<Waypoint>().previousWaypoint == null)
            .ToList();
        foreach (Transform o in _firstsWaypoints)
        {
            SphereCollider collider = o.gameObject.AddComponent<SphereCollider>();
            collider.isTrigger = true;
            collider.radius = 2;
        }
    }

    private void Update()
    {
        if(!_isInitSpawnDone) return;
        
        // Spawn car when the count in scene is lower than carToSpawn
        if (_cars.Count < carToSpawn)
        {
            if (spawnTimer > 0)
            {
                spawnTimer -= Time.deltaTime;
            }
            else
            {
                Transform spawnWaypoint = _firstsWaypoints[Random.Range(0, _firstsWaypoints.Count)];
                if (!spawnWaypoint.GetComponent<Waypoint>().IsCarHere)
                {
                    Spawn(spawnWaypoint);
                    spawnTimer = spawnDelay;
                }
            }
        }
    }

    public void RemoveCar(GameObject obj)
    {
        _cars.Remove(obj);
        Destroy(obj);
    }

    private void Spawn(Transform waypointToSpawn)
    {
        GameObject obj = Instantiate(carPrefab);
        _cars.Add(obj);
        obj.GetComponent<WaypointNavigator>().SetCurrentWaypoint(waypointToSpawn.GetComponent<Waypoint>());
        obj.transform.position = waypointToSpawn.position;
        
        // Set car rotation to waypoint rotation 
        if (waypointToSpawn.GetComponent<Waypoint>().nextWaypoint != null)
        {
            var lookPosition = waypointToSpawn.GetComponent<Waypoint>().nextWaypoint.transform.position - waypointToSpawn.position;
            lookPosition.y = 0;
            var lookRotation = Quaternion.LookRotation(lookPosition);
            obj.transform.rotation = lookRotation;
        }
        else if (waypointToSpawn.GetComponent<Waypoint>().previousWaypoint != null)
        {
            var lookPosition = waypointToSpawn.position - waypointToSpawn.GetComponent<Waypoint>().previousWaypoint.transform.position;
            lookPosition.y = 0;
            var lookRotation = Quaternion.LookRotation(lookPosition);
            obj.transform.rotation = lookRotation;
        }

        StartCoroutine(EnableNavMeshAgent(obj));
    }

    private IEnumerator InitSpawn()
    {
        yield return new WaitForSeconds(1);
        
        List<Transform> availablePlacesToSpawn = transform.GetAllChildren()
            .Where(t => t.GetComponent<Waypoint>().nextWaypoint != null)
            .ToList();
        availablePlacesToSpawn = availablePlacesToSpawn.Shuffle().ToList();

        int count = 0;
        while (count < carToSpawn)
        {
            if (count >= transform.childCount)
                break;

            Spawn(availablePlacesToSpawn.First());
            availablePlacesToSpawn.RemoveAt(0);

            yield return new WaitForEndOfFrame();

            count++;
        }

        _isInitSpawnDone = true;
    }

    private IEnumerator EnableNavMeshAgent(GameObject car)
    {
        if (car == null)
        {
            Debug.Log("");
        }
        
        yield return new WaitForSeconds(.25f);
        
        car.GetComponent<NavMeshAgent>().enabled = true;
        car.GetComponent<CarController>().OnNavMeshAgentEnabled_Invoke();
    }
}