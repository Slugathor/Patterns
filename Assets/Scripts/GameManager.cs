using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject player;
    public Stack<Command> commandStack = new Stack<Command>();
    public Stack<Command> redoStack = new Stack<Command>();
    [SerializeField] Button undoButton;
    [SerializeField] Button redoButton;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else { Destroy(this); }

        undoButton.onClick.AddListener(() => {
            if (commandStack.Count > 0)
            {
                Command lastCommand = commandStack.Pop();
                lastCommand.Undo();
                redoStack.Push(lastCommand);
            }
        });
    }
}
