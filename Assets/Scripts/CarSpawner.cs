using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
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

    // Start is called before the first frame update
    void Start()
    {
        if (S != null)
            throw new Exception("Duplicated Singleton!");
        
        S = this;
        _cars = new List<GameObject>();
        StartCoroutine(InitSpawn());
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

    private void Spawn(Transform waypointToSpawn)
    {
        GameObject obj = Instantiate(carPrefab);
        _cars.Add(obj);
        Transform child = waypointToSpawn;
        obj.GetComponent<WaypointNavigator>().currentWaypoint = child.GetComponent<Waypoint>();
        obj.transform.position = child.position;
        // Set car rotation to waypoint rotation 
        if (child.GetComponent<Waypoint>().nextWaypoint != null)
        {
            var lookPosition = child.GetComponent<Waypoint>().nextWaypoint.transform.position - child.position;
            lookPosition.y = 0;
            var lookRotation = Quaternion.LookRotation(lookPosition);
            obj.transform.rotation = lookRotation;
        }
        else if (child.GetComponent<Waypoint>().previousWaypoint != null)
        {
            var lookPosition = child.position - child.GetComponent<Waypoint>().previousWaypoint.transform.position;
            lookPosition.y = 0;
            var lookRotation = Quaternion.LookRotation(lookPosition);
            obj.transform.rotation = lookRotation;
        }
    }

    private IEnumerator InitSpawn()
    {
        List<Transform> availablePlacesToSpawn = transform.GetAllChildren().ToList();
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
    }

    public void RemoveCar(GameObject obj)
    {
        _cars.Remove(obj);
        Destroy(obj);
    }
}