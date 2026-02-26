using UnityEngine;

public struct MoveEvent : IEvent
{
    public Vector2 moveInput;
}

public struct RunEvent : IEvent
{
    public bool isPressed;
}

public struct RollEvent : IEvent
{
    public bool isPressed;
}