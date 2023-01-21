using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class exittogame : MonoBehaviour
{

    public void ExitButton()
    {
        Application.Quit();
    }

    void Start()
    {
        
    }

    void Update()
    {
        Debug.Log("Exit pressed!");
    }
}
