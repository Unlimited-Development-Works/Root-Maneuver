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
    private GameController gameController;
    public string playerName = "";
    public Transform spawnLocation;
    public float tweenTime = 0.1f;
    public float retractionFactor = 0.00001f;

    private int rootCount = 0;

    private float pathDiff; 

    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("Game").GetComponent<GameController>();
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
            gameController.SetScore(playerName, nutrients);
        }
    }

    public bool RetractRoots()
    {
        rootCount = roots.Count();

        pathDiff = DistanceBetweenTwoTransforms(player.transform, spawnLocation);
        Debug.Log(pathDiff);

        if ( 100f< retractionFactor)
        {
            return false;
        }


        for (int i = rootCount; i > 0; i--)
        {

            if (i == rootCount)
            {
                Debug.Log("hello");
                player.transform.position = Vector3.Lerp(player.transform.position, roots[i - 1].transform.position, tweenTime);
                player.transform.rotation = Quaternion.Slerp(player.transform.rotation, roots[i - 1].transform.rotation, tweenTime);
            }

            else
            { 

                roots[i].transform.position = Vector3.Lerp(roots[i].transform.position, roots[i - 1].transform.position, tweenTime);
                roots[i].transform.rotation = Quaternion.Slerp(roots[i].transform.rotation, roots[i - 1].transform.rotation, tweenTime);
                
            }
            
        }
        return true;
    }
}
