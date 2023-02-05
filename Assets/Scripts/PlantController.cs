using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantController : MonoBehaviour
{
    public int currentGrowth = 0;
    public SpriteRenderer plantSpriteRenderer;
    public List<Sprite> plantSprites;
    public int nutrientMultiplier = 5;
    public void AddGrowth(int nutrients)
    {
        currentGrowth += nutrients;
        int plantSpriteID = (currentGrowth / nutrientMultiplier);
        if (plantSpriteID >= plantSprites.Count)
        {
            plantSpriteID = plantSprites.Count - 1;
        }
        plantSpriteRenderer.sprite = plantSprites[plantSpriteID];
    }
}
