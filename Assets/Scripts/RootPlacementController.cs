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
    public int nutrients = 0;
    void Start()
    {
        PlantRoot(rootPiece);
    }

    void FixedUpdate()
    {
        float distance = DistanceBetweenTwoTransforms(player.transform, lastLocation);
        if (distance > spawnDistance)
        {
            PlantRoot(rootPiece);
        }
    }

    public void SetLastLocation(Transform location)
    {
        lastLocation = location;
    }

    float DistanceBetweenTwoTransforms(Transform source, Transform target)
    {
        Vector2 delta = source.position - target.position;
        return delta.magnitude;
    }

    void PlantRoot(GameObject rootPrefab)
    {
        GameObject spawnedRoot = Instantiate(rootPiece, player.transform.position, player.transform.rotation);
        roots.Add(spawnedRoot);
        lastLocation = spawnedRoot.transform;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Nutrient"))
        {
            PlantRoot(rootNutrientPiece);
            nutrients += 1;
            collision.gameObject.GetComponent<NutrientController>().Collect();
            Debug.Log("Collected Nutrients: " + nutrients.ToString());
        }
    }
}
