using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public CharacterStatus playerStatus;

    List<IEndGameObserver> EndGameObservers = new List<IEndGameObserver>();

    public void RegisterPlayer(CharacterStatus player)
    {
        playerStatus = player;
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
}
