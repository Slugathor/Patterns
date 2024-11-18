using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private int coinAmount = 0;
    private Vector3 playerPosStart;
    public static event Action<float> PlayerMoved;
    PlayerState playerState = PlayerState.STANDING;
    float speed = 1; // default movement speed

    [SerializeField] MoveCommand WKey, AKey, SKey, DKey;
    enum MoveCommand
    {
        MoveForward,
        MoveBackward,
        MoveLeft,
        MoveRight,
    }

    enum PlayerState
    {
        STANDING=0,
        CROUCHING,
        
    }

    Dictionary<KeyCode, MoveCommand> keyToCommand;
    void Start()
    {
        CoinManager.instance.CoinCollected += OnCoinCollected;

        keyToCommand = new Dictionary<KeyCode, MoveCommand>
        {
            { KeyCode.W, WKey },
            { KeyCode.A, AKey },
            { KeyCode.S, SKey },
            { KeyCode.D, DKey },
        };
        GameManager.instance.playerStartPos = transform.position;
        GameManager.instance.playerStartRota = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {

        // Inputs that work both while paused and while playing
        if (Input.GetKeyDown(KeyCode.T)) // Print achievements0
        {
            HelperFunctions.PrintAchievements();
        }
        if (Input.GetKeyDown(KeyCode.Escape)) // Go to main menu
        {
            MenuManager.GoToMainMenu();
        }
        if (Input.GetKeyDown(KeyCode.P)) // Pause toggle
        {
            MenuManager.TogglePauseGame();
        }
        // GAMESTATE PLAYING
        if (GameManager.gameState == GameState.PLAYING)
        {
            HandlePlayingInputs();

            // if player moved during the frame, calculate the distance travelled and raise event PlayerMoved
            if (playerPosStart != transform.position)
            {
                float distMoved = Math.Abs((playerPosStart - transform.position).magnitude);
                //Debug.Log("Distance moved this frame: "+distMoved.ToString());
                PlayerMoved?.Invoke(distMoved);
            }
        }
    }

    void HandlePlayingInputs() 
    {
        switch (playerState)
        {
            case PlayerState.STANDING:
                speed = 1;
                if (Input.GetKeyDown(KeyCode.C))
                {
                    Crouch();
                }
                break;
            case PlayerState.CROUCHING:
                speed = 0.5f;
                if (Input.GetKeyUp(KeyCode.C))
                {
                    StandUp();
                }
                break;
        }

        // save player pos at the start of the frame
        playerPosStart = transform.position;

        if (Input.GetKeyDown(KeyCode.Z)) { GameManager.instance.UndoProcedure(); }
        if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.Y)) { GameManager.instance.RedoProcedure(); }

        foreach (var entry in keyToCommand)
        {
            if (Input.GetKeyDown(entry.Key))
            {
                GameManager.instance.redoStack.Clear();
                
                ExecuteCommand(entry.Value, speed);
            }
        }
    }

    void ExecuteCommand(MoveCommand commandType, float _speed)
    {
        Command command = commandType switch
        {
            MoveCommand.MoveForward => new MoveForward(),
            MoveCommand.MoveBackward => new MoveBackward(),
            MoveCommand.MoveLeft => new MoveLeft(),
            MoveCommand.MoveRight => new MoveRight(),
            _ => null
        };

        if (command != null) 
        {
            command.Execute(transform, _speed);
            GameManager.instance.commandStack.Push(command);
        }
    }

    void Crouch()
    {
        transform.localScale = new Vector3(1, 0.6f, 1);
        playerState = PlayerState.CROUCHING;
    }
    void StandUp()
    {
        transform.localScale = new Vector3(1, 1, 1);
        playerState = PlayerState.STANDING;
    }

    void OnCoinCollected(int coinValue)
    {
        AddCoin(coinValue);
    }

    void AddCoin(int amount = 1)
    {
        coinAmount+=amount;
    }

}
