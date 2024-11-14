using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public enum GameState
{
    MENU,
    PLAYING,
    PAUSED,
    GAMEOVER
}

public class GameManager : GenericSingleton<GameManager>
{
    public GameObject player;
    public Vector3 playerStartPos;
    public Quaternion playerStartRota;
    public Stack<Command> commandStack = new Stack<Command>();
    public Stack<Command> redoStack = new Stack<Command>();
    public Stack<Command> replayStack;
    [SerializeField] Button undoButton, redoButton, replayButton;

    public static GameState gameState = 0;

    protected override void Awake()
    {
        base.Awake();
        undoButton.onClick.AddListener(() => UndoProcedure());
        redoButton.onClick.AddListener(() => RedoProcedure());
        replayButton.onClick.AddListener(() => StartCoroutine(Replay()));
    }
    public void UndoProcedure()
    {
        if (commandStack.Count > 0)
        {
            Command lastCommand = commandStack.Pop();
            lastCommand.Undo();
            redoStack.Push(lastCommand);
        }
    }
    public void RedoProcedure()
    {
        if (redoStack.Count > 0)
        {
            Command lastCommand = redoStack.Pop();
            lastCommand.Execute(player.transform);
            commandStack.Push(lastCommand);
        }
    }
    IEnumerator Replay()
    {
        if (commandStack.Count > 0)
        {
            replayStack = new Stack<Command>();
            Debug.Log("Returning player to start");
            player.transform.position = playerStartPos;
            player.transform.rotation = playerStartRota;
            redoStack.Clear();
            while (commandStack.Count > 0)
            {
                replayStack.Push(commandStack.Pop());
            }

            while (replayStack.Count > 0)
            {
                ReplayMove();
                yield return new WaitForSeconds(0.2f);
            }
        }

    }
    void ReplayMove()
    {
        Command nextCommand = replayStack.Pop();
        nextCommand.Execute(player.transform);
        commandStack.Push(nextCommand);
    }
}
