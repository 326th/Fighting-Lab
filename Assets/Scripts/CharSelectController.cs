using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.SceneManagement;
public class CharSelectController : MonoBehaviour
{
    private string selectedChar;
    [SerializeField] Image bigImage;
    [SerializeField] TextMeshProUGUI bigText;
    [SerializeField] Button Confirm;
    public List<Character> characters = new List<Character>();
    void Start()
    {

    }


    void Update()
    {
        if(selectedChar == null)
        {
            CheckMouseOnCharacter();
        }
        
    }

    public void SelectCharacter(string Char)
    {
        foreach (Character character in characters)
        {
            if (Char == character.characterName)
            {
                bigImage.sprite = character.characterSprite;
                bigImage.gameObject.SetActive(true);
                bigText.text = Char;
                bigText.gameObject.SetActive(true);
                selectedChar = character.characterName;
                Confirm.gameObject.SetActive(true);
            }
        }

    }
    public void CheckMouseOnCharacter()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        if (results.Count > 0)
        {
            foreach (Character character in characters)
            {
                if (results[0].gameObject.transform.parent.name == character.characterName)
                {
                    bigImage.sprite = character.characterSprite;
                    bigImage.gameObject.SetActive(true);
                    bigText.text = character.characterName;
                    bigText.gameObject.SetActive(true);
                }
            }
        }
    }

    public void ConfirmCharacter()
    {
        if (GameManagement.mode == "play")
        {
            SceneManager.LoadScene("DifficultySelectionScene");
        }
        else if (GameManagement.mode == "training")
        {
            SceneManager.LoadScene("TrainingScene");
        }
        else if (GameManagement.mode == "practice")
        {
            SceneManager.LoadScene("PracticeScene");
        }
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("HomeScreen");
    }
}