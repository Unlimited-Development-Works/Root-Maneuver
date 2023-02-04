using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RootPlacementController : MonoBehaviour
{
    public Rigidbody2D player;
    public GameObject rootPiece;
    public GameObject rootNutrientPiece;
    private Transform lastLocation;
    private List<GameObject> roots = new List<GameObject>();
    public float spawnDistance;
    void Start()
    {
        lastLocation = transform;
    }

    void FixedUpdate()
    {
        float distance = DistanceBetweenTwoTransforms(player.transform, lastLocation);
        if (distance > spawnDistance)
        {
            GameObject spawnedRoot = Instantiate(rootPiece, player.transform.position, player.transform.rotation);
            roots.Add(spawnedRoot);
            lastLocation = spawnedRoot.transform;
        }
    }

    public void SetLastLocation(Transform location)
    {
        lastLocation = location;
        Debug.Log("Set lastLocation to");
        Debug.Log(lastLocation);
    }

    float DistanceBetweenTwoTransforms(Transform source, Transform target)
    {
        Vector2 delta = source.position - target.position;
        return delta.magnitude;
    }
}
