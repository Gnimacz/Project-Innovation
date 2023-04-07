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
    public static EventHandler<int> ClientConnected;
    public static EventHandler<int> ClientDisconnected;
    
}

public class DirectionalEventArgs : EventArgs
{
    public int PlayerId { get; set; }
    public Vector2 JoystickPosition { get; set; }
    public JoystickAngle JoystickDirection { get; set; }
    public enum JoystickAngle
    {
        Up,
        Down,
        Left,
        Right,
        Neutral
    }

    public DirectionalEventArgs(int playerId, Vector2 joystickPosition, string joystickDirection)
    {
        PlayerId = playerId;
        JoystickPosition = joystickPosition;
        JoystickDirection = Enum.TryParse<JoystickAngle>(joystickDirection, out var result) ? result : JoystickAngle.Neutral;
    }
}
