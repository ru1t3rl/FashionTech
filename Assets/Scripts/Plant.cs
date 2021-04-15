using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    public int startStage = 0;
    private int currentStage, finalStage;
    private Vector3 sizeToGrowTo;

    public int[] growingStageSizes = { 3, 4, 5, 6 };
    private bool growing, placed;

    // Start is called before the first frame update
    void Start()
    {
        growing = false;
        placed = false;

        finalStage = growingStageSizes.Length;
    }

    public void PlacedPlant()
    {
        placed = true;
        StartGrowing();
    }

    private void StartGrowing()
    {
        currentStage = startStage;
        GrowToNextStage(true);
    }

    private void GrowToNextStage(bool firstTime)
    {
        if (!firstTime) currentStage++;

        if (currentStage >= finalStage) return;

        Debug.Log("growing to " + growingStageSizes[currentStage] + "  " + currentStage);

        growing = true;
        sizeToGrowTo.Set(growingStageSizes[currentStage], growingStageSizes[currentStage], growingStageSizes[currentStage]);
    }

    private void FixedUpdate()
    {
        if (growing && transform.localScale.x < sizeToGrowTo.x && 
            transform.localScale.y < sizeToGrowTo.y && transform.localScale.z < sizeToGrowTo.z)
        {
            transform.localScale *= 1.01f;
        }
        else if (growing) {
            growing = false;
            GrowToNextStage(false);
        }
    }
}
