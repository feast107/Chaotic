using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;



public class ClickMoveOn : MonoBehaviour
{
    public enum Command
    {
        Start,
        Load,
        CloseMenu,
        Exit
    }

    public Command Trigger;

    private static readonly Dictionary<Command, Action<ClickMoveOn>> ClickEvents = new()
    {
        { Command.Start, (_) => { SceneManager.LoadScene("Fight"); } },
        { Command.Load, (context) =>
        {
            ReflectGroup = 1;
            GameObject.Find("Menu").transform.ApplyToChildren((trans) =>
            {
                trans.GetComponent<SpriteRenderer>().sortingOrder += 10;
                trans.position.Set(trans.position.x, trans.position.y, -context.Z);
            });
        }
        },
        { Command.Exit, (_) =>
        {
            Debug.Log("Quit");
            Application.Quit(1);
        }
        },
        { Command.CloseMenu , (context) =>
        {
            ReflectGroup = 0;
            GameObject.Find("Menu").transform.ApplyToChildren((trans) =>
            {
                trans.GetComponent<SpriteRenderer>().sortingOrder -= 10;
                trans.position.Set(trans.position.x, trans.position.y, context.Z);
                PostInitialize.Values.ForEach(x=>x());
            });

        } }
    };

    private static readonly Dictionary<uint, Tuple<Action<ClickMoveOn>, Action<ClickMoveOn>>> GroupEvents = new()
    {
        {
            0, new(
                context =>
                {
                    context.spriteRenderer.sortingOrder += context.LayerChange;
                    context.transform.position = Vector3.back * context.ZChange;
                }, context =>
                {
                    context.spriteRenderer.sortingOrder -= context.LayerChange;
                    context.transform.position = Vector3.forward * context.ZChange;
                })
        }
    };

    private SpriteRenderer spriteRenderer;

    private static readonly Dictionary<ClickMoveOn, Action> PostInitialize = new();

    public int Layer = 0;

    public int LayerChange = 10;

    public int Z = -10;

    public int ZChange = 20;

    public uint Group = 0;

    public static uint ReflectGroup = 0;

    // Start is called before the first frame update
    public void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = Layer;
        transform.position.Set(transform.position.x,transform.position.y,Z);
    }

    public ClickMoveOn()
    {
        PostInitialize[this] = Start;
    }

    public void OnMouseExit()
    {
        if (ReflectGroup != Group) return;
        GroupEvents[Group]?.Item2.Invoke(this);
    }

    public void OnMouseUp() => ClickEvents[Trigger]?.Invoke(this);

    public void OnMouseEnter()
    {
        if (ReflectGroup != Group) return;
        GroupEvents[Group]?.Item1.Invoke(this);
    }

    // Update is called once per frame
    public void Update()
    {
    }
}
