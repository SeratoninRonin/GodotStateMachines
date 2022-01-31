using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class StateMethodCache
{
    public Action EnterState;
    public Action Tick;
    public Action PhysicsTick;
    public Action ExitState;
}

public class SimpleFSM : Node
{
    private Dictionary<string, StateMethodCache> _stateCache;
    private StateMethodCache _stateMethods;

    public float ElapsedTimeInState = 0f;

    [Export]
    public List<string> States = new List<string>();

    public string PreviousState { get; protected set; }
    private string _currentState;
    public string CurrentState
    {
        get => _currentState;
        set
        {
            //don't change to the current state or an invalid one
            if (_currentState == value || !States.ToList().Contains(value) || _stateCache == null)
            {
                return;
            }

            PreviousState = _currentState;
            _currentState = value;

            if (PreviousState == null)
            {
                PreviousState = CurrentState;
            }

            if (_stateMethods != null && _stateMethods.ExitState != null)
            {
                _stateMethods.ExitState();
            }

            ElapsedTimeInState = 0;
            _stateMethods = _stateCache[_currentState];

            if (_stateMethods != null && _stateMethods.EnterState != null)
            {
                _stateMethods.EnterState();
            }
        }
    }

    public string InitialState
    {
        set
        {
            _currentState = value;
            _stateMethods = _stateCache[_currentState];

            if (_stateMethods.EnterState != null)
            {
                _stateMethods.EnterState();
            }
        }
    }


    public override void _Ready()
    {
        States = States.ToList();
        _stateCache = new Dictionary<string, StateMethodCache>();
        foreach (var state in States)
        {
            ConfigureAndCacheState(state);
        }
    }

    public override void _Process(float delta)
    {

        if (_currentState != null)
        {
            ElapsedTimeInState += delta;
            if (_stateMethods.Tick != null)
            {
                _stateMethods.Tick();
            }
        }
    }

    public override void _PhysicsProcess(float delta)
    {
        if (_currentState != null)
        {
            if (_stateMethods.PhysicsTick != null)
            {
                _stateMethods.PhysicsTick();
            }
        }
    }

    public void ConfigureAndCacheState(string stateName)
    {
        var state = new StateMethodCache();
        state.EnterState = GetDelegateForMethod(stateName + "_Enter");
        state.Tick = GetDelegateForMethod(stateName + "_Process");
        state.PhysicsTick = GetDelegateForMethod(stateName + "_PhysicsProcess");
        state.ExitState = GetDelegateForMethod(stateName + "_Exit");

        _stateCache[stateName] = state;
    }

    public Action GetDelegateForMethod(string methodName)
    {
        var methodInfo = ReflectionUtils.GetMethodInfo(this.GetParent(), methodName);
        if (methodInfo != null)
        {
            return ReflectionUtils.CreateDelegate<Action>(this.GetParent(), methodInfo);
        }

        return null;
    }
}
