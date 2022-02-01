using Godot;
using System;

public class Walker : Sprite
{
    NodeStateMachine<Walker> stateMachine;

    public override void _Ready()
    {
        stateMachine = new NodeStateMachine<Walker>(this, new WalkState());
        stateMachine.AddState(new IdleState());
    }

    public override void _Process(float delta)
    {
        stateMachine.Update(delta);
    }
}

public class WalkState : NodeState<Walker>
{
    float walk_timer = 3f;
    Vector2 dir = Vector2.Zero;
    float speed = 100f;
    RandomNumberGenerator rng;

    public override void Begin()
    {
        rng = new RandomNumberGenerator();
        rng.Randomize();
        dir = Vector2.Zero;
        while (dir == Vector2.Zero)
        {
            var x = rng.RandiRange(-1, 1);
            var y = rng.RandiRange(-1, 1);
            dir = new Vector2(x, y);
        }
        walk_timer = rng.RandiRange(3, 6);
        GD.Print("walking for " + walk_timer + " seconds");
    }
    public override void Update(float delta)
    {
        _context.GlobalPosition += dir * speed * delta;
        var pos = _context.GlobalPosition;

        if (_context.GlobalPosition.x < 0)
        {
            pos.x = 0;
            dir.x = -dir.x;
        }
        if (_context.GlobalPosition.y < 0)
        {
            pos.y = 0;
            dir.y = -dir.y;
        }
        if (_context.GlobalPosition.x > _context.GetViewportRect().End.x)
        {
            pos.x = _context.GetViewportRect().End.x;
            dir.x = -dir.x;
        }
        if (_context.GlobalPosition.y > _context.GetViewportRect().End.y)
        {
            pos.y = _context.GetViewportRect().End.y;
            dir.y = -dir.y;
        }
        walk_timer -= _context.GetProcessDeltaTime();
        if (walk_timer < 0)
        {
            _machine.ChangeState(typeof(IdleState));
        }
        _context.GlobalPosition = pos;
    }
    public override void End()
    {
        GD.Print("Stopping at " + _context.GlobalPosition);
    }
}

public class IdleState : NodeState<Walker>
{
    float idle_timer = 3f;
    public override void Begin()
    {
        GD.Print("Taking a rest for a few seconds");
        idle_timer = 3f;
    }
    public override void Update(float delta)
    {
        idle_timer -= delta;
        if (idle_timer < 0)
            _machine.ChangeState<WalkState>();
    }

    public override void End()
    {
        GD.Print("Getting back to it");
    }
}
