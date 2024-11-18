using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class Command
{
    protected Transform player;
    protected Quaternion lastRota;
    protected Vector3 lastPos;
    protected float speed = 1;

    public Command(float _speed=1)
    {
        speed = _speed;
    }
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
    public MoveForward(float _speed=1) : base(_speed) // <--- added float speed parameter to all move commands, because crouching slows speed to half
    { 
        speed = _speed; 
    }

    public override void Execute(Transform _transform)
    {
        base.Execute(_transform);
        _transform.rotation = Quaternion.Euler(0, 0, 0);
        _transform.position += Vector3.forward * speed;
    }
}

public class MoveBackward : Command
{
    public MoveBackward(float _speed = 1) : base(_speed)
    {
        speed = _speed;
    }

    public override void Execute(Transform _transform)
    {
        base.Execute(_transform);
        _transform.rotation = Quaternion.Euler(0, 180, 0);
        _transform.position += Vector3.back * speed;
    }
}

public class MoveLeft : Command
{
    public MoveLeft(float _speed = 1) : base(_speed)
    {
        speed = _speed;
    }

    public override void Execute(Transform _transform)
    {
        base.Execute(_transform);
        _transform.rotation = Quaternion.Euler(0, -90, 0);
        _transform.position += Vector3.left * speed;
    }
}

public class MoveRight : Command
{
    public MoveRight(float _speed = 1) : base(_speed)
    {
        speed = _speed;
    }

    public override void Execute(Transform _transform)
    {
        base.Execute(_transform);
        _transform.rotation = Quaternion.Euler(0, 90, 0);
        _transform.position += Vector3.right * speed;
    }
}

