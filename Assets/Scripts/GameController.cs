using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    [Header("REFERENCES")]
    [SerializeField] private MenuManager menuManager;

    private Player[] players;
    private PlayerController[] pathCreators;

    private bool isWin;
    private bool isRestarted;
    private bool firstPlay = true;
    private int checksCount = 0;

    private void Awake()
    {
        Instance = this;

        players = GetComponentsInChildren<Player>();
        pathCreators = GetComponentsInChildren<PlayerController>();
    }

    public void CheckFinish()
    {
        checksCount++;

        if (checksCount == players.Length)
        {
            checksCount = 0;
            isWin = true;
            foreach (Player player in players)
            {
                if (!player.isFinished)
                {
                    isWin = false;
                    break;
                }
            }

            LevelManager.Instance.PassedCurrentLevel(isWin);

            if (isWin) {
                menuManager.OpenMenu("Win");
            }
            else
            {
                menuManager.OpenMenu("Lose");
            }
        }
    }

    public void StartMove()
    {
        if(isRestarted || firstPlay)
        {
            isRestarted = false;
            firstPlay = false;

            foreach (PlayerController creator in pathCreators)
            {
                creator.StartMove();
            }
        }
    }

    public void ResetPath()
    {
        isRestarted = true;

        menuManager.OpenMenu("Main");

        foreach (PlayerController creator in pathCreators)
        {
            creator.ResetPath();
        }
    }
}
