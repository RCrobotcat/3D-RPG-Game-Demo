using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public CharacterStatus playerStatus;

    private CinemachineFreeLook freeLookCam;
    private CinemachineVirtualCamera virtualCam;

    List<IEndGameObserver> EndGameObservers = new List<IEndGameObserver>();

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    public void RegisterPlayer(CharacterStatus player)
    {
        playerStatus = player;

        freeLookCam = FindObjectOfType<CinemachineFreeLook>();
        virtualCam = FindObjectOfType<CinemachineVirtualCamera>();

        if (freeLookCam != null)
        {
            freeLookCam.Follow = playerStatus.transform.GetChild(2);
            freeLookCam.LookAt = playerStatus.transform.GetChild(2);
        }

    }

    public void AddObserver(IEndGameObserver observer)
    {
        EndGameObservers.Add(observer);
    }

    public void RemoveObserver(IEndGameObserver observer)
    {
        EndGameObservers.Remove(observer);
    }

    public void NotifyObserver()
    {
        foreach (var observer in EndGameObservers)
        {
            observer.EndNotify();
        }
    }

    public Transform GetEntrance()
    {
        foreach (var item in FindObjectsOfType<TransitionDestination>())
        {
            if (item.destinationTag == TransitionDestination.DestinationTag.ENTER)
            {
                return item.transform;
            }
        }
        return null;
    }
}
