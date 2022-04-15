using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagement : MonoBehaviour
{
    public static GameManagement manager;
    string mode;

    private void Awake()
    {
        if (manager == null)
        {
            manager = this;
            DontDestroyOnLoad(this);

        }else if (manager != this)
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        mode = null;
    }
    public void LoadScene(string scene)
    {
        if (scene == "HomeScreen")
        {
            mode = null;
        }
        else if (scene == "PlayScene")
        {
            mode = "play";
        }
        else if (scene == "TrainingScene")
        {
            mode = "training";
        }
        else if (scene == "PracticeScene")
        {
            mode = "practice";
        }
        SceneManager.LoadScene(scene);
    }
    public void LoadCharacterSelectionScene(string newMode)
    {
        mode = newMode;
        SceneManager.LoadScene("CharacterSelectionScene");
    }
    public void ConfirmCharacter()
    {
        if (mode == "play")
        {
            SceneManager.LoadScene("DifficultySelectionScene");
        }
        else if (mode == "training")
        {
            SceneManager.LoadScene("TrainingScene");
        }
        else if (mode == "practice")
        {
            SceneManager.LoadScene("PracticeScene");
        }
    }

    public void Quit()
    {
        print("Quitting..");
        Application.Quit();
    }


    void Update()
    {
        
    }
}
