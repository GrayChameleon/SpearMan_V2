using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraplingHook : MonoBehaviour
{

    float distanceToClosest;
    public float hookRange;
    float currentDistance;
    public GameObject nearestGrabPlace;
    public List<GameObject> grabPlases = new List<GameObject>();


    private void Start()
    {
        foreach (GameObject item in GameObject.FindGameObjectsWithTag("GrabPlace"))
        {
            grabPlases.Add(item);
        }
        findNearestGrabPlace();

    }

    public void findNearestGrabPlace()
    {
        distanceToClosest = hookRange;
        foreach (GameObject item in grabPlases)
        {
            currentDistance = Vector2.Distance(transform.position, item.transform.position);
            if (currentDistance<distanceToClosest)
            {
                distanceToClosest = currentDistance;
                nearestGrabPlace = item;
            }
        }
    }

}
