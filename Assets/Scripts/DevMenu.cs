using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DevMenu : MonoBehaviour
{
    [SerializeField] GameObject devMenu;
    [SerializeField] bool startVisible = false;
    [SerializeField] TextMeshProUGUI text;

    private void Awake()
    {
        devMenu.SetActive(false);
    }


    public void Activate()
    {
        devMenu.SetActive(true);
        text.text = "<";
    }

    public void DeActivate()
    {
        devMenu.SetActive(false);
        text.text = ">";
    }

    public void Toggle()
    {
        if (devMenu.activeSelf)
        {
            DeActivate();
        }
        else
        {
            Activate();
        }
    }
}
 