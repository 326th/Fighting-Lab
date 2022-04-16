using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadCharSelectScene : MonoBehaviour
{
    private GameManagement gameManagement;
    // Start is called before the first frame update
    void Start()
    {
        GameObject.FindObjectOfType<GameManagement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadCharacterSelectionScene(string newMode)
    {
        GameManagement.mode = newMode;
        SceneManager.LoadScene("CharacterSelectionScene");
    }

    public void Quit()
    {
        print("Quitting..");
        Application.Quit();
    }
}
