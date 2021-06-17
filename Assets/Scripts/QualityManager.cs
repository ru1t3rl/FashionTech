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

    [SerializeField] Material waterMaterial;
    [SerializeField] Quality[] qualities;

    private void Awake()
    {
        qualityLevels = QualitySettings.names;
        currentLevel = QualitySettings.GetQualityLevel();
        UpdateGUI();
    }

    private void Start()
    {
        SetQuality(currentLevel.ToString());
    }

    public void SetQuality(string quality)
    {
        for (int i = 0; i < qualities.Length; i++)
        {
            if (qualities[i].reflectionProbe != null)
            {
                qualities[i].reflectionProbe.SetActive(false);
            }
        }

        for (int i = 0; i < qualityLevels.Length; i++)
        {
            if (qualityLevels[i] == quality)
            {
                waterMaterial.SetFloat("_Metallic", qualities[i].waterMetalic);
                waterMaterial.SetFloat("Smoothness", qualities[i].waterSmoothness);
                waterMaterial.SetFloat("Normal_Strength", qualities[i].waterNormal);

                if (qualities[i].reflectionProbe != null)
                {
                    qualities[i].reflectionProbe.SetActive(true);
                }

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

    [System.Serializable]
    internal class Quality
    {
        public string name;
        public float waterMetalic;
        public float waterSmoothness;
        public float waterNormal;
        public GameObject reflectionProbe;
    }
}
