using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.ProBuilder;
using UnityEngine.SceneManagement;

public class SceneController : Singleton<SceneController>, IEndGameObserver
{
    public GameObject playerPrefab;

    GameObject player;
    NavMeshAgent playerAgent;

    bool fadeFinished;

    public SceneFader sceneFaderPrefab;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        GameManager.Instance.AddObserver(this);
        fadeFinished = true;
    }

    public void TransitToDestination(TransitionPoint transitionPoint)
    {
        switch (transitionPoint.transitionType)
        {
            case TransitionPoint.TransitionType.SameScene:
                StartCoroutine(Transition(SceneManager.GetActiveScene().name, transitionPoint.destinationTag));
                break;
            case TransitionPoint.TransitionType.DifferentScene:
                StartCoroutine(Transition(transitionPoint.sceneName, transitionPoint.destinationTag));
                break;
        }
    }

    IEnumerator Transition(string SceneName, TransitionDestination.DestinationTag destinationTag)
    {
        SaveManager.Instance.SavePlayerData();
        InventoryManager.Instance.SaveData();

        if (SceneName != SceneManager.GetActiveScene().name)
        {
            yield return SceneManager.LoadSceneAsync(SceneName);
            yield return Instantiate(playerPrefab, GetDestination(destinationTag).transform.position,
                GetDestination(destinationTag).transform.rotation);
            SaveManager.Instance.LoadPlayerData();
            yield break; // Exit the coroutine
        }
        else
        {
            player = GameManager.Instance.playerStatus.gameObject;
            playerAgent = player.GetComponent<NavMeshAgent>();
            playerAgent.enabled = false;
            player.transform.SetPositionAndRotation(GetDestination(destinationTag).transform.position,
                GetDestination(destinationTag).transform.rotation);
            playerAgent.enabled = true;
            yield return null;
        }
    }

    // Find the destination entrance in the new scene
    private TransitionDestination GetDestination(TransitionDestination.DestinationTag destinationTag)
    {
        var entrances = FindObjectsOfType<TransitionDestination>();
        foreach (var entrance in entrances)
        {
            if (entrance.destinationTag == destinationTag)
            {
                return entrance;
            }
        }
        return null;
    }

    public void TransitionToFirstLevel()
    {
        StartCoroutine(LoadLevel("GameScene"));
    }

    public void ContinueGame()
    {
        if (SaveManager.Instance.SceneName != "")
            StartCoroutine(LoadLevel(SaveManager.Instance.SceneName));
    }

    public void ReturnToMainMenu()
    {
        StartCoroutine(LoadMainMenu());
    }

    IEnumerator LoadLevel(string stringName)
    {
        SceneFader fade = Instantiate(sceneFaderPrefab);
        if (stringName != "")
        {
            if (PlayerPrefs.HasKey("PlayerX") && PlayerPrefs.HasKey("PlayerY") && PlayerPrefs.HasKey("PlayerZ"))
            {
                yield return StartCoroutine(fade.FadeOut(1.5f));
                yield return SceneManager.LoadSceneAsync(stringName);
                Vector3 PlayerPos = new Vector3(PlayerPrefs.GetFloat("PlayerX"),
                    PlayerPrefs.GetFloat("PlayerY"), PlayerPrefs.GetFloat("PlayerZ"));
                yield return player = Instantiate(playerPrefab, PlayerPos,
                    GameManager.Instance.GetEntrance().rotation);

                // Save Data
                SaveManager.Instance.SavePlayerData();
                yield return StartCoroutine(fade.FadeIn(1.5f));
                yield break;
            }
            else
            {
                yield return StartCoroutine(fade.FadeOut(1.5f));
                yield return SceneManager.LoadSceneAsync(stringName);
                yield return player = Instantiate(playerPrefab, GameManager.Instance.GetEntrance().position,
                    GameManager.Instance.GetEntrance().rotation);

                // Save Data
                SaveManager.Instance.SavePlayerData();
                yield return StartCoroutine(fade.FadeIn(1.5f));
                yield break;
            }
        }
    }

    IEnumerator LoadMainMenu()
    {
        SceneFader fade = Instantiate(sceneFaderPrefab);
        yield return StartCoroutine(fade.FadeOut(3.5f));
        yield return SceneManager.LoadSceneAsync("MainMenu");
        yield return StartCoroutine(fade.FadeIn(1.5f));
        yield break;
    }

    public void EndNotify()
    {
        if (fadeFinished)
        {
            fadeFinished = false;
            StartCoroutine(LoadMainMenu());
        }
    }
}
