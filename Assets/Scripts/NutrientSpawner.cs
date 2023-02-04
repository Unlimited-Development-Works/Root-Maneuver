using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NutrientSpawner : MonoBehaviour
{
    public float width = 10;
    public float height = 10;
    public int maxNumberOfNutrients = 50;
    public float spawnDelaySeconds = 0.1f;
    public GameObject nutrientPrefab;
    public List<string> avoidTags = new List<string>();
    public float avoidRadius = 0.2f;
    
    private int numberOfNutrients = 0;
    private Cooldown spawnCooldown;

    private const int maxSpawnAttempts = 100;

    void Start()
    {
        spawnCooldown = new Cooldown(spawnDelaySeconds);
    }

    void Update()
    {
        spawnCooldown.Update(spawnDelaySeconds);
        if (spawnCooldown.Expired() && numberOfNutrients < maxNumberOfNutrients) {
            SpawnNutrient();
            spawnCooldown.Reset();
        }
    }

    private bool IsPointCloseToOtherPoints2D(Vector3 point, List<Vector3> pointsToAvoid) {
        foreach (Vector3 pos in pointsToAvoid) {
            if (Vector2.Distance(point, pos) < avoidRadius) {
                // Found a point that was too close
                return true;
            }
        }
        // No points that were too close
        return false;
    }

    private (bool, Vector3) RandomSpawnLocationAvoidingPoints(List<Vector3> pointsToAvoid) {
        for (int i=0; i < maxSpawnAttempts; i++) {
            Vector3 spawnLocation = new Vector3(
                Random.Range(-width/2, width/2),
                Random.Range(-height/2, height/2),
                0
            );
            if (!IsPointCloseToOtherPoints2D(spawnLocation, pointsToAvoid)) {
                // Found a good point
                return (true, spawnLocation);
            }
        }
        // Couldn't find a good point within `maxSpawnAttempts` tries
        return (false, Vector3.zero);
    }

    private void SpawnNutrient() {
        if (!nutrientPrefab) {
            Debug.LogError("No prefab for nutrient on NutrientSpawner");
            return;
        }

        // Find objects to avoid
        List<Vector3> pointsToAvoid = new List<Vector3>();
        foreach (string tag in avoidTags) {
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag(tag)) {
                pointsToAvoid.Add(obj.transform.position);
            }
        }

        // Select random spot in space
        (bool success, Vector3 spawnLocation) = RandomSpawnLocationAvoidingPoints(pointsToAvoid);
        if (!success) {
            // Failed to find a good point, bail on spawning
            return;
        }

        // Spawn nutrient
        GameObject nutrient = Instantiate(nutrientPrefab, spawnLocation, Quaternion.identity);
        numberOfNutrients += 1;
        nutrient.name = "Nutrient " + numberOfNutrients;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawWireCube(transform.position, new Vector3(width, height, 0));
    }
}
