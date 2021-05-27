using System;
using UnityEngine;

public class Plant : MonoBehaviour
{
    public int startStage = 0;
    private int currentStage, finalStage;
    private Vector3 sizeToGrowTo, startScale;

    private DateTime growthEndTime, startTime;

    public GrowingStage[] growingStages;
    private bool growing, placed;

    void Start()
    {
        growing = false;
        placed = false;

        startScale = transform.localScale;

        finalStage = growingStages.Length;
    }

    public void PlacedPlant()
    {
        placed = true;
        StartGrowing();
    }

    private void StartGrowing()
    {
        currentStage = startStage;

        startTime = DateTime.Now;

        GrowToNextStage(true);
    }

    private void GrowToNextStage(bool firstTime)
    {
        //TODO: add possible sprite/model changes here

        if (!firstTime) currentStage++;

        if (currentStage >= finalStage) return;

        growthEndTime = DateTime.Now.AddMinutes((double)(growingStages[currentStage].durationInMinutes * 1.0f));

        growing = true;
        sizeToGrowTo.Set(growingStages[currentStage].sizeIncrement + transform.localScale.x, growingStages[currentStage].sizeIncrement + transform.localScale.y, growingStages[currentStage].sizeIncrement + transform.localScale.z);
    }

    private void FixedUpdate()
    {
        if (growing && transform.localScale.x < sizeToGrowTo.x && 
            transform.localScale.y < sizeToGrowTo.y && transform.localScale.z < sizeToGrowTo.z)
        {
            GrowPlant();
        }
        else if (growing) {
            growing = false;
            GrowToNextStage(false);
        }
    }

    private void GrowPlant()
    {
        int elapsedTime = DateTime.Now.TotalSeconds() - startTime.TotalSeconds();
        int duration = growthEndTime.TotalSeconds() - startTime.TotalSeconds();

        transform.localScale = ((elapsedTime * 1.0f) / (duration * 1.0f)) * sizeToGrowTo + startScale;
    }
}

[System.Serializable]
public class GrowingStage
{
    [SerializeField]
    private string name;
    public GameObject model;
    public int durationInMinutes;
    public int sizeIncrement;
}