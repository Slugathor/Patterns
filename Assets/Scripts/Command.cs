using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class Command
{
    protected Transform player;
    protected Quaternion lastRota;
    protected Vector3 lastPos;
    public virtual void Execute(Transform _transform) 
    {
        player = _transform;
        lastRota = _transform.rotation;
        lastPos = _transform.position;
    }
    public virtual void Undo()
    {
        player.rotation = lastRota;
        player.position = lastPos;
    }
}

public class MoveForward : Command
{
    public override void Execute(Transform _transform)
    {
        base.Execute(_transform);
        _transform.rotation = Quaternion.Euler(0, 0, 0);
        _transform.position += Vector3.forward;
    }
}

public class MoveBackward : Command
{
    public override void Execute(Transform _transform)
    {
        base.Execute(_transform);
        _transform.rotation = Quaternion.Euler(0, 180, 0);
        _transform.position += Vector3.back;
    }
}

public class MoveLeft : Command
{
    public override void Execute(Transform _transform)
    {
        base.Execute(_transform);
        _transform.rotation = Quaternion.Euler(0, -90, 0);
        _transform.position += Vector3.left;
    }
}

public class MoveRight : Command
{
    public override void Execute(Transform _transform)
    {
        base.Execute(_transform);
        _transform.rotation = Quaternion.Euler(0, 90, 0);
        _transform.position += Vector3.right;
    }
}

public class DoNothing : Command
{
}