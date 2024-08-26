using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    Button NewGameButton;
    Button ContinueButton;
    Button QuitButton;

    private void Awake()
    {
        NewGameButton = transform.GetChild(2).GetComponent<Button>();
        ContinueButton = transform.GetChild(3).GetComponent<Button>();
        QuitButton = transform.GetChild(4).GetComponent<Button>();

        NewGameButton.onClick.AddListener(NewGame);
        ContinueButton.onClick.AddListener(ContinueGame);
        QuitButton.onClick.AddListener(QuitGame);
    }

    void NewGame()
    {
        // Delete all saved data
        PlayerPrefs.DeleteAll();

        SceneController.Instance.TransitionToFirstLevel();
    }

    void ContinueGame()
    {
        SceneController.Instance.ContinueGame();
    }

    void QuitGame()
    {
        Application.Quit();
    }
}
