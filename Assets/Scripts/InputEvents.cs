using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class InputEvents
{
    public static EventHandler<int> JumpButtonPressed;
    public static EventHandler<int> JumpButtonReleased;
    public static EventHandler<int> AttackButtonPressed;
    public static EventHandler<int> AttackButtonReleased;
    public static EventHandler<DirectionalEventArgs> JoystickMoved;
    
}

public class DirectionalEventArgs : EventArgs
{
    public int PlayerId { get; set; }
    public Vector2 JoystickPosition { get; set; }

    public DirectionalEventArgs(int playerId, Vector2 joystickPosition)
    {
        PlayerId = playerId;
        JoystickPosition = joystickPosition;
    }
}
