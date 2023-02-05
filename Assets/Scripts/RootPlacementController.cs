using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RootPlacementController : MonoBehaviour
{
    public string playerName;

    public Rigidbody2D player;
    public GameObject rootPiece;
    public GameObject rootNutrientPiece;
    public float spawnDistance;
    public int nutrients = 0;
    public PlantController plantController;
    
    private float tweenTime = 0.2f;
    private float retractionFactor = 0.2f;
    public Vector2 spawnLocation;

    private GameController gameController;
    private Transform lastLocation;
    private List<GameObject> roots = new List<GameObject>();

    private int rootCount = 0;

    void Start()
    {
        PlantRoot(rootPiece);
        spawnLocation = new Vector2(transform.position.x, transform.position.y);
        if (DoesTagExist("Game"))
        {
            gameController = GameObject.FindGameObjectWithTag("Game").GetComponent<GameController>();
        }
    }

    void FixedUpdate()
    {
        if (!player.gameObject.GetComponent<PlayerMovement>().isRetracting) {
            float distance = DistanceBetweenTwoTransforms(player.transform, lastLocation);
            if (distance > spawnDistance)
            {
                PlantRoot(rootPiece);
            }
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
        GameObject spawnedRoot = Instantiate(rootPrefab, player.transform.position, player.transform.rotation);
        
        spawnedRoot.name = "root_" + roots.Count().ToString();
        roots.Add(spawnedRoot);

        lastLocation = spawnedRoot.transform;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Nutrient"))
        {
            PlantRoot(rootNutrientPiece);
            GameController.nutrientSpawner?.NutrientConsumed();
            nutrients += 1;
            collision.gameObject.GetComponent<NutrientController>().Collect();
            Debug.Log("Collected Nutrients: " + nutrients.ToString());
            player.gameObject.GetComponent<PlayerMovement>().PlayCollectEffects();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Root"))
        {
            PlayerMovement playerMovement = player.gameObject.GetComponent<PlayerMovement>();
            playerMovement.PlayHitEffects();
            playerMovement.isRetracting = true;
            RetractRoots();
        }
    }

    void BankNutrients() {
        gameController?.AddScore(playerName, nutrients);
        plantController.AddGrowth(nutrients);
        nutrients = 0;
    }

    public bool RetractRoots()
    {
        rootCount = roots.Count();

        bool allAtCenter = true;

        for (int i = rootCount; i > 0; i--)
        {

            if (i == rootCount)
            {
                player.transform.position = Vector3.Lerp(player.transform.position, roots[i - 1].transform.position, tweenTime);
                player.transform.rotation = Quaternion.Slerp(player.transform.rotation, roots[i - 1].transform.rotation, tweenTime);
            }

            else
            { 
                roots[i].transform.position = Vector3.Lerp(roots[i].transform.position, roots[i - 1].transform.position, tweenTime);
                roots[i].transform.rotation = Quaternion.Slerp(roots[i].transform.rotation, roots[i - 1].transform.rotation, tweenTime);

                float pathDiff = Vector2.Distance(new Vector2(roots[i].transform.position.x, roots[i].transform.position.y), spawnLocation);
                if (pathDiff > retractionFactor)
                {
                    allAtCenter = false;
                }
            }
        }

        if (allAtCenter)
        {
            BankNutrients();
            foreach(GameObject root in roots)
            {
                Destroy(root);
            }
            roots.Clear();
            player.transform.position = spawnLocation;
            PlantRoot(rootPiece);

            return false;
        } else {
            if (nutrients > 0) {
                // Grow sound
                GetComponent<ChainedSounds>()?.PlayFor(0.5f);
            }
        }

        return true;
    }

    public static bool DoesTagExist(string aTag)
    {
        try
        {
            GameObject.FindGameObjectsWithTag(aTag);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
