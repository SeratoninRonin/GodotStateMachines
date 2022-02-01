# GodotStateMachines

Two different state machines to use in Godot C#

# SimpleFSM

This node offers a quick and simple way to get AI up and running.  Simply add this node as a child, and then in the inspector/code, find the "States" array.  This is a list of strings, one for each state you want your object to be in.

![SimpleFSM](https://user-images.githubusercontent.com/61599196/151866978-86ec6ed3-edd8-4aeb-9ecb-788c02cb91f7.png)

For each state, SimpleFSM will try to call 4 different methods:
- stateName_Enter: Called when entering this state
- stateName_Exit: Called when exiting this state
- stateName_Process: Called each frame 
- stateName_PhysicsProcess: Called each fixed physics frame

If a method is not implemented it is ignored/not called.

Example:

```C#
public class MyNode : Node
{
  SimpleFSM _fsm;
  
  public override void _Ready() { _fsm = GetNode<SimpleFSM>("SimpleFSM"); }
  
  public void Walk_Enter() { GD.Print("Starting to walk!"); }
  
  public void Walk_Process() { GD.Print("Still walking..."); }
  
  public void Walk_Exit() { GD.Print("We've stopped!); }
  
  public void Idle_Enter() { GD.Print("Time for a rest!); }
  
  public void Idle_Exit() { GD.Print("Back to it!"); }
}
```
# NodeStateMachine

The next step up is NodeStateMachine which implements the "states as objects" pattern. NodeStateMachine uses separate classes for each state so it is a better choice for more complex systems.

We start to get into the concept of a context with NodeStateMachine. In coding, the context is just the class used to satisfy a generic constraint. in a List<string> the string would be the context class, the class that the list operates on. With NodeStateMachine you get to specify the context class. It could be your Enemy class, Player class or any other class that derives from Node.
  
Example:
Here is a simple example showing the usage (with the State subclasses omitted for brevity):

```C#
// create a state machine that will work with an object of type SomeClass as the focus with an initial state of PatrollingState
var machine = new SKStateMachine<SomeClass>( someClass, new PatrollingState() );

// we can now add any additional states
machine.AddState( new AttackState() );
machine.AddState( new ChaseState() );

// this method would typically be called in an update of an object
machine.Update(delta);

// change states. the state machine will automatically create and cache an instance of the class (in this case ChasingState)
machine.ChangeState<ChasingState>();
```

# Examples
  
An example of a sprite walking around has been implemented using both state machines and is included in the /Examples folder
  
# Acknowledgements

These machines were inspired by/adapted from the excellent Nez framework for MonoGame: https://github.com/prime31/Nez
  
# License
This code is MIT licensed and provided as-is.
