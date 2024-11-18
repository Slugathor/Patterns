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
    float jumpForceMultiplier = 1;
    bool doubleJumped=false;
    Rigidbody rigidbody;
    PlayerState stateLastFrame;
    public MoveCommand WKey, AKey, SKey, DKey;
    public enum MoveCommand
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
        IN_AIR
    }

    public Dictionary<KeyCode, MoveCommand> keyToCommand;
    void Start()
    {
        CoinManager.instance.CoinCollected += OnCoinCollected;
        rigidbody = GetComponent<Rigidbody>();

        keyToCommand = new Dictionary<KeyCode, MoveCommand>
        {
            { KeyCode.W, WKey }, // WKey is an enum variable "MoveCommand" exposed to the editor. This lets one change the functionality of the w key
            { KeyCode.A, AKey }, // the shroom swaps these around 
            { KeyCode.S, SKey },
            { KeyCode.D, DKey },
        };
        GameManager.instance.playerStartPos = transform.position;
        GameManager.instance.playerStartRota = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (stateLastFrame != playerState)
        {
            Debug.Log(playerState.ToString());
            stateLastFrame = playerState;
        }

        // Inputs that work both while paused and while playing
        if (Input.GetKeyDown(KeyCode.T)) // Print achievements
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
            HandlePlayingInputs(); // Movement and other player controls

            // if player moved during the frame, calculate the distance travelled and raise event PlayerMoved
            if (playerPosStart != transform.position)
            {
                float distMoved = Math.Abs((playerPosStart - transform.position).magnitude);
                //Debug.Log("Distance moved this frame: "+distMoved.ToString());
                PlayerMoved?.Invoke(distMoved);
                playerPosStart = transform.position;
            }
        }
    }

    void HandlePlayingInputs() 
    {
        switch (playerState)
        {
            case PlayerState.STANDING:
                speed = 1;
                jumpForceMultiplier = 1;
                if (Input.GetKey(KeyCode.C))
                {
                    Crouch();
                }
                HandleMovementCommands();
                break;
            case PlayerState.CROUCHING:
                speed = 0.5f;
                jumpForceMultiplier = 1.5f;
                if (Input.GetKeyUp(KeyCode.C))
                {
                    StandUp();
                }
                HandleMovementCommands();
                break;
            case PlayerState.IN_AIR:
                speed = 0;
                HandleMovementCommands();
                break;
        }

        
    }

    void HandleMovementCommands()
    {
        // save player pos at the start of the frame
        playerPosStart = transform.position;

        if (Input.GetKeyDown(KeyCode.Z)) { GameManager.instance.UndoProcedure(); }
        if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.Y)) { GameManager.instance.RedoProcedure(); }
        if (Input.GetKeyDown(KeyCode.Space) && !doubleJumped) 
        {
            if (playerState == PlayerState.IN_AIR)
            {
                doubleJumped = true;
            }
            Jump();
            return;
        }

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
            MoveCommand.MoveForward => new MoveForward(_speed),
            MoveCommand.MoveBackward => new MoveBackward(_speed),
            MoveCommand.MoveLeft => new MoveLeft(_speed),
            MoveCommand.MoveRight => new MoveRight(_speed),
            _ => null
        };

        if (command != null) 
        {
            command.Execute(transform);
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

    void Jump()
    {
        Debug.Log("JumpForceMultiplier: " + jumpForceMultiplier);
        rigidbody.AddForce(transform.up * jumpForceMultiplier* 5, ForceMode.Impulse);
        transform.localScale = new Vector3(1, 1, 1);
        playerState =PlayerState.IN_AIR;
        jumpForceMultiplier = 0.75f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(playerState == PlayerState.IN_AIR)
        {
            if(collision.collider.tag == "Ground" ||  collision.collider.tag == "Floor")
            {
                doubleJumped = false;
                StandUp();
            }
        }
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
