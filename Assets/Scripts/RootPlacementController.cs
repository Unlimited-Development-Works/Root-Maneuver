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
    public Transform spawnLocation;
    public float tweenTime = 0.01f;
    public float retractionFactor = 1f;

    private int rootCount = 0;

    void Start()
    {
        PlantRoot(rootPiece);
        spawnLocation = transform;
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
        
        spawnedRoot.name = "root_" + roots.Count().ToString();
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

    public bool RetractRoots()
    {
        rootCount = roots.Count();
        Vector2 location = spawnLocation.position;
        Vector2 delta = player.position - location;
        


/*        if (delta.magnitude < retractionFactor)
        {
            Debug.Log("got here");
            return false;
        }*/

        Debug.Log(rootCount);
        for (int i = rootCount; i > 0; i--)
        {
            if (i == rootCount)
            {
                player.transform.position = Vector3.Lerp(player.transform.position, roots[i - 1].transform.position, tweenTime);
                player.transform.rotation = Quaternion.Slerp(player.transform.rotation, roots[i - 1].transform.rotation, tweenTime);
            }
            roots[i-1].transform.position = Vector3.Lerp(roots[i-1].transform.position, roots[i-2].transform.position, tweenTime);
            roots[i-1].transform.rotation = Quaternion.Slerp(roots[i-1].transform.rotation, roots[i-2].transform.rotation, tweenTime);
        }

        return true;
    }
}
