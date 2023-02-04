using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootPlacementController : MonoBehaviour
{
    public Rigidbody2D player;
    public GameObject rootPiece;
    public GameObject rootNutrientPiece;
    private Transform lastLocation;
    private float distance = 0;
    public float spawnDistance;
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        distance = DistanceBetweenTwoTransforms(player.transform, lastLocation);
        if (distance > spawnDistance)
        {
            GameObject spawnedRoot = Instantiate(rootPiece, player.transform.position, player.transform.rotation);
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
