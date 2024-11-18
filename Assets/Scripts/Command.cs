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
    public virtual void Execute(Transform _transform, float _speed = 1f) 
    {
        player = _transform;
        lastRota = _transform.rotation;
        lastPos = _transform.position;
        speed = _speed;
    }
    public virtual void Undo()
    {
        player.rotation = lastRota;
        player.position = lastPos;
    }
}

public class MoveForward : Command
{
    public override void Execute(Transform _transform, float _speed = 1f)
    {
        base.Execute(_transform, _speed);
        _transform.rotation = Quaternion.Euler(0, 0, 0);
        _transform.position += Vector3.forward * speed;
    }
}

public class MoveBackward : Command
{
    public override void Execute(Transform _transform, float _speed = 1f)
    {
        base.Execute(_transform, _speed);
        _transform.rotation = Quaternion.Euler(0, 180, 0);
        _transform.position += Vector3.back * speed;
    }
}

public class MoveLeft : Command
{
    public override void Execute(Transform _transform, float _speed = 1f)
    {
        base.Execute(_transform, _speed);
        _transform.rotation = Quaternion.Euler(0, -90, 0);
        _transform.position += Vector3.left * speed;
    }
}

public class MoveRight : Command
{
    public override void Execute(Transform _transform, float _speed = 1f)
    {
        base.Execute(_transform, _speed);
        _transform.rotation = Quaternion.Euler(0, 90, 0);
        _transform.position += Vector3.right * speed;
    }
}

