using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

public class MainMenu : MonoBehaviour
{
    Button NewGameButton;
    Button ContinueButton;
    Button QuitButton;

    PlayableDirector MainDirector;

    private void Awake()
    {
        NewGameButton = transform.GetChild(2).GetComponent<Button>();
        ContinueButton = transform.GetChild(3).GetComponent<Button>();
        QuitButton = transform.GetChild(4).GetComponent<Button>();

        NewGameButton.onClick.AddListener(PlayTimeLine);
        ContinueButton.onClick.AddListener(ContinueGame);
        QuitButton.onClick.AddListener(QuitGame);

        MainDirector = FindObjectOfType<PlayableDirector>();
        MainDirector.stopped += NewGame; // Action Event
    }

    void PlayTimeLine()
    {
        MainDirector.Play();
    }

    void NewGame(PlayableDirector obj)
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
