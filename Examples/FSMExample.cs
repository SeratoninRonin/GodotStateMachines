using Godot;
using System;
using System.Collections.Generic;

public class FSMExample : Node2D
{
    Sprite sprite;
    SimpleFSM fsm;
    RandomNumberGenerator rng;
    Vector2 dir;
    float idle_timer = 3f;
    float walk_timer = 3f;
    [Export]
    public float Speed = 10f;
    public override void _Ready()
    {
        rng = new RandomNumberGenerator();
        sprite = GetNode<Sprite>("Sprite");
        fsm = GetNode<SimpleFSM>("FSM");
        fsm.CurrentState = "Walk";
        rng.Randomize();
    }

    public void Walk_Enter()
    {
        GD.Print("Entering walk state.");
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

    public void Walk_PhysicsProcess()
    {
        sprite.GlobalPosition += dir * Speed * GetPhysicsProcessDeltaTime();
        var pos = sprite.GlobalPosition;

        if (sprite.GlobalPosition.x<0)
        {
            pos.x = 0;
            dir.x = -dir.x;
        }
        if (sprite.GlobalPosition.y<0)
        {
            pos.y = 0;
            dir.y = -dir.y;
        }
        if (sprite.GlobalPosition.x>GetViewportRect().End.x)
        {
            pos.x = GetViewportRect().End.x;
            dir.x = -dir.x;
        }
        if (sprite.GlobalPosition.y>GetViewportRect().End.y)
        {
            pos.y = GetViewportRect().End.y;
            dir.y = -dir.y;
        }
        walk_timer -= GetPhysicsProcessDeltaTime();
        if(walk_timer<0)
        {
            fsm.CurrentState = "Idle";
        }
        sprite.GlobalPosition = pos;
    }

    public void Walk_Exit()
    {
        GD.Print("Stopping at " + sprite.GlobalPosition);
    }

    public void Idle_Enter()
    {
        GD.Print("Taking a rest for a few seconds");
        idle_timer = 3f;
    }

    public void Idle_Process()
    {
        idle_timer -= GetProcessDeltaTime();
        if(idle_timer<0)
        {
            fsm.CurrentState = "Walk";
        }
    }

    public void Idle_Exit()
    {
        GD.Print("Getting back to it");
    }
}
