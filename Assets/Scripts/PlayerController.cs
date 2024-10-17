using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //public Command wCommand = new MoveForward();
    //public Command aCommand = new MoveLeft();
    //public Command sCommand = new MoveBackward();
    //public Command dCommand = new MoveRight();
    //public Command doNothingCommand = new DoNothing();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            Command wCommand = new MoveForward();
            wCommand.Execute(this.transform);
            GameManager.instance.commandStack.Push(wCommand);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            Command aCommand = new MoveLeft();
            aCommand.Execute(this.transform);
            GameManager.instance.commandStack.Push(aCommand);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Command sCommand = new MoveBackward();
            sCommand.Execute(this.transform);
            GameManager.instance.commandStack.Push(sCommand);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            Command dCommand = new MoveRight();
            dCommand.Execute(this.transform);
            GameManager.instance.commandStack.Push(dCommand);
        }
    }
}
