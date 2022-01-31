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
