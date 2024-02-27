using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Ensure this is included for UI elements like Buttons
using UnityEngine.SceneManagement;
using System.IO; // Include this for file operations


public class FirstScreenManager : MonoBehaviour
{
    public GameObject continueCanvas;
    public Button continueButton;
    public Button newGameButton; // Assign in Inspector
    public Button optionsButton; // Assign in Inspector
    public Button exitButton; // Assign in Inspector
    private string path;
    void Start()
    {
        path = Application.persistentDataPath + "/counts.json";
        newGameButton.onClick.AddListener(NewGame);
        if (File.Exists(path))
        {
            continueButton.onClick.AddListener(ContinueGame);
            continueCanvas.SetActive(true);
        }
        else
        {
            continueCanvas.SetActive(false);
        }
         
        optionsButton.onClick.AddListener(OpenOptions);
        exitButton.onClick.AddListener(ExitGame);

    }
    void NewGame()
    {
        
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        SceneManager.LoadScene("MainScreen");
    }
    void ContinueGame()
    {
        
        SceneManager.LoadScene("MainScreen");
    }

    void OpenOptions()
    {
        
        SceneManager.LoadScene("Options");
    }

    void ExitGame()
    {
        
                #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false; // This line is for quitting the game in the Unity Editor
            #else
        UnityEngine.Application.Quit(); 
            #endif
    }

}
