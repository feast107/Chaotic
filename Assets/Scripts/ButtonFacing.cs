using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonFacing : MonoBehaviour
{
    [Serializable]
    public struct Configure
    {
        public Direction Direction;
        public Transform Animator;
        public Sprite Stop;

        public SpriteRenderer Renderer => renderer = renderer != null ? renderer : Animator.GetComponent<SpriteRenderer>();
        [CanBeNull] private SpriteRenderer renderer;
    }

    public List<Configure> Configures;

    public Transform Body;

    private SpriteRenderer stop;
    private SpriteRenderer move;
    public int SourceOrder = -10;
    public int TargetOrder = 10;

    // Start is called before the first frame update
    void Start()
    {
        this.Claim();
        stop = (Body ?? transform).GetComponent<SpriteRenderer>();
        Configures.ForEach(x=>x.Renderer.sortingOrder = SourceOrder);
    }

    private readonly List<Configure> queue = new();
    // Update is called once per frame
    void Update()
    {
        Configures.ForEach(x =>
        {
            var key = x.Direction.ToKey();
            if (Input.GetKeyDown(key) && !queue.Contains(x))
            {
                move = x.Renderer;
                if (queue.Count > 0) { queue[0].Renderer.sortingOrder = SourceOrder; }
                queue.Insert(0, x);
            }
            if (Input.GetKeyUp(key))
            {
                x.Renderer.sortingOrder = SourceOrder;
                queue.Remove(x);
            }
            
        });
        if (queue.Count == 0)
        {
            if (move != null)
            {
                move.sortingOrder = SourceOrder;
            }
            stop.sortingOrder = TargetOrder;
        }
        else
        {
            stop.sprite = queue[0].Stop;
            stop.sortingOrder = SourceOrder;
            queue[0].Renderer.sortingOrder = TargetOrder;
        }
    }
}
