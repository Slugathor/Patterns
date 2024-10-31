using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]private int coinAmount =0;
    event Action onCoinCollected;
    [SerializeField] MoveCommand WKey, AKey, SKey, DKey;
    enum MoveCommand
    {
        MoveForward,
        MoveBackward,
        MoveLeft,
        MoveRight,
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
        if (Input.GetKeyDown(KeyCode.Z)) { GameManager.instance.UndoProcedure(); }
        if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.Y)) { GameManager.instance.RedoProcedure(); }

        foreach (var entry in keyToCommand)
        {
            if (Input.GetKeyDown(entry.Key))
            {
                GameManager.instance.redoStack.Clear();
                ExecuteCommand(entry.Value);
            }
        }
    }

    void ExecuteCommand(MoveCommand commandType)
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
            command.Execute(transform);
            GameManager.instance.commandStack.Push(command);
        }
    }

    void OnCoinCollected()
    {
        AddCoin();
    }

    void AddCoin(int amount = 1)
    {
        coinAmount+=amount;
    }
}
