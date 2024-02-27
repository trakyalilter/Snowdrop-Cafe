using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class OptionsManager : MonoBehaviour
{
    public Button backButton;
    
    void Start()
    {
        backButton.onClick.AddListener(Back);
    }
    void Back()
    {
        // Load the MainScreen scene
        SceneManager.LoadScene("Menu");
    }

    
}
