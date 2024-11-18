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
    public bool shroomed = false;
    public Stack<Command> commandStack = new Stack<Command>();
    public Stack<Command> redoStack = new Stack<Command>();
    public Stack<Command> replayStack;
    [SerializeField] Button undoButton, redoButton, replayButton;
    [SerializeField] Text shroomedText;
    public float shroomedDuration;
    public double shroomStart;

    public static GameState gameState = 0;

    protected override void Awake()
    {
        base.Awake();
        undoButton.onClick.AddListener(() => UndoProcedure());
        redoButton.onClick.AddListener(() => RedoProcedure());
        replayButton.onClick.AddListener(() => StartCoroutine(Replay()));
    }

    private void Update()
    {
        // if the player is shroomed
        if (shroomed)
        {
            shroomedText.enabled = true;
            // get the remaining duration
            double shroomRemaining = shroomStart + shroomedDuration - Time.timeAsDouble;
            // and set text to show duration in whole seconds left
            shroomedText.text = $"You're Shroomed ({(int)shroomRemaining}) !";

            // text color starts from green and changes based on shroomed duration left
            // basically, it lerps the hue value to get all values starting from green and going counter clockwise around the color wheel
            // 1 - remaining/duration = basically t (timePassed/totalTime)
            // t*360 + 127 makes it so that we start at 127 (green) and as time goes by, hue value goes up
            // and at 360 it gets decremented by 360 as e.g. 362.5 % 360 would equal 2.5
            float hue = (float)((1-shroomRemaining/shroomedDuration)*360 + 127) % 360;
            hue /= 360; // get a value between 0 and 1
            shroomedText.color = Color.HSVToRGB(hue, 1, 1); // hue, saturation, value (brightness)
        }
        else { shroomedText.enabled = false; }
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
