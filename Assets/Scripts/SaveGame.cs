using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveGame : MonoBehaviour
{
    public static int level = 1;

    private void Awake()
    {
        if (!PlayerPrefs.HasKey("isStart"))
        {
            PlayerPrefs.SetInt("isStart", 1);

            PlayerPrefs.SetInt("money", 0);
            PlayerPrefs.SetInt("allMoney", 10000);

            PlayerPrefs.SetInt("level", 1);

            PlayerPrefs.SetInt("recordText", 0);
        }
    }
}
