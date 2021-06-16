using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Valve.VR;

public class QualityManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI currentQuality;
    string[] qualityLevels;
    int currentLevel;

    private void Awake()
    {
        qualityLevels = QualitySettings.names;
        currentLevel = QualitySettings.GetQualityLevel();
        UpdateGUI();
    }

    public void SetQuality(string quality)
    {
        for (int i = 0; i < qualityLevels.Length; i++)
        {
            if (qualityLevels[i] == quality)
            {
                QualitySettings.SetQualityLevel(i, true);
                currentLevel = i;
                UpdateGUI();
                break;
            }
        }
    }

    private void UpdateGUI()
    {
        if (currentQuality != null)
            currentQuality.text = $"Current: {qualityLevels[currentLevel]}";
    }
}
