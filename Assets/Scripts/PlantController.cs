using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantController : MonoBehaviour
{
    public SpriteRenderer plantSpriteRenderer;
    public List<Sprite> plantSprites;
    public List<float> plantScales;
    public int nutrientMultiplier = 5;
    public float scaleBoost = 3f;

    public int targetGrowth = 0;
    private int currentGrowth = 0;
    private Cooldown growthCooldown = new Cooldown(0.1f);

    public void AddGrowth(int nutrients)
    {
        targetGrowth += nutrients;
    }

    void Update() {
        growthCooldown.Update();
        if (targetGrowth > currentGrowth && growthCooldown.Expired()) {
            currentGrowth += 1;
            growthCooldown.Reset();
            
            int plantSpriteID = (currentGrowth / nutrientMultiplier);
            if (plantSpriteID >= plantSprites.Count)
            {
                plantSpriteID = plantSprites.Count - 1;
            }
            plantSpriteRenderer.enabled = (currentGrowth > 0);
            plantSpriteRenderer.sprite = plantSprites[plantSpriteID];
            float scale = (float)(currentGrowth + 1) / (float)(plantSprites.Count * nutrientMultiplier);
            transform.localScale = scale * plantScales[plantSpriteID] * scaleBoost * Vector3.one;
        }
    }
}
