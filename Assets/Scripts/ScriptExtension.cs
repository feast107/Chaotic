using System;
using System.Collections.Generic;
using UnityEngine;
using Debug = UnityEngine.Debug;

[Flags]
public enum Direction
{
    None = 0,
    Up = 0b0001,
    Down = 0b0010,
    Left = 0b0100,
    Right = 0b1000,
    LeftUp = 0b0101,
    RightUp = 0b1001,
    LeftDown = 0b0110,
    RightDown = 0b1010,
}
public enum Change
{
    Increment,
    Decrement
}
public enum Line
{
    Vertical,
    Horizontal,
}

public static class ScriptExtension
{
    public static Direction ToDirection(this KeyCode key) =>
        key switch
        {
            KeyCode.A => Direction.Left,
            KeyCode.D => Direction.Right,
            KeyCode.W => Direction.Up,
            KeyCode.S => Direction.Down,
            _ => Direction.None
        };

    public static Vector3 ToNormalized(this Direction direction)
    {
        var ret = new Vector3();
        if (direction.HasFlag(Direction.Up)) ret += Vector3.up;
        if (direction.HasFlag(Direction.Down)) ret += Vector3.down;
        if (direction.HasFlag(Direction.Left)) ret += Vector3.left;
        if (direction.HasFlag(Direction.Right)) ret += Vector3.right;
        return ret.normalized;
    }

    public static KeyCode ToKey(this Direction direction) =>
        direction switch
        {
            Direction.Up => KeyCode.W,
            Direction.Left => KeyCode.A,
            Direction.Right => KeyCode.D,
            Direction.Down => KeyCode.S,
            _ => KeyCode.None
        };

    public static Direction DirectionToMouse(this Vector3 position)
    {
        var mp = MousePositionToWorld;
        return (Math.Atan2(mp.y - position.y, mp.x - position.x) * 180 / Math.PI) switch
        {
            >= 157.5 => Direction.Left,
            >= 112.5 => Direction.LeftUp,
            >= 67.5 => Direction.Up,
            >= 22.5 => Direction.RightUp,
            >= -22.5 => Direction.Right,
            >= -67.5 => Direction.RightDown,
            >= -112.5 => Direction.Down,
            >= -157.5 => Direction.LeftDown,
            _ => Direction.Left,
        };
    }

    public static bool IsValid(this Direction direction, Vector3 position, float limit) =>
        direction switch
        {
            Direction.Up => position.y < limit,
            Direction.Down => position.y > limit,
            Direction.Left => position.x > limit,
            Direction.Right => position.x < limit,
            _ => throw new NotSupportedException()
        };

    public static Vector3 MousePositionToWorld => Camera.main.ScreenToWorldPoint(Input.mousePosition);

    public static Vector3 ToVector3(this Change change) =>
        change switch
        {
            Change.Increment => Vector3.right,
            Change.Decrement => Vector3.left,
            _ => throw new NotSupportedException()
        };

    public static int ToFeature(this Change change) =>
        change switch
        {
            Change.Increment => 1,
            Change.Decrement => -1,
            _ => throw new NotSupportedException()
        };

    public static void ApplyToChildren(this Transform transform, Action<Transform> action)
    {
        for (var c = 0; c < transform.childCount; c++)
        {
            var child = transform.GetChild(c);
            action(child);
            if (child.childCount > 0)
            {
                ApplyToChildren(child, action);
            }
        }
    }

    /// <summary>
    /// 循环
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="action"></param>
    public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
    {
        foreach (var item in source)
        {
            action(item);
        }
    }

    /// <summary>
    /// 打印自己的信息
    /// </summary>
    /// <typeparam name="TMonoScript"></typeparam>
    /// <param name="script"></param>
    public static void Claim<TMonoScript>(this TMonoScript script) where TMonoScript : MonoBehaviour =>
        Debug.LogWarning($"This is [{typeof(TMonoScript).Name}] on GameObject : [{script.transform.name}]");
}
