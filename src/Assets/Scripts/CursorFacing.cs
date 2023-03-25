using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CursorFacing : MonoBehaviour
{
    // Start is called before the first frame update
    [Serializable]
    public class Configure
    {
        public Direction Direction;
        public Transform Animator;
        public Sprite Stop;

        public SpriteRenderer Renderer => renderer??=Animator.GetComponent<SpriteRenderer>();
        [CanBeNull] private SpriteRenderer renderer;
    }

    public List<Configure> Configures;

    public Transform Body;

    private SpriteRenderer stop;
    private SpriteRenderer move;
    public int SourceOrder = -10;
    public int TargetOrder = 10;

    private Direction direction = Direction.Up;

    private void Start()
    {
        this.Claim();
        stop = (Body ?? transform).GetComponent<SpriteRenderer>();
    }

    private void SetDirection(float x,float y)
    {
        direction = Math.Abs(y / x) > 1 
            ? y > 0 
                ? Direction.Up 
                : Direction.Down 
            : x > 0
                ? Direction.Right
                : Direction.Left;
    }

    private void FixedUpdate()
    {
        var p = ScriptExtension.MousePositionToWorld;
        SetDirection(p.x - transform.position.x, p.y - transform.position.y);
        var moving = false;
        Configures.ForEach(x =>
        {
            if (Input.GetKey(x.Direction.ToKey()))
            {
                moving = true;
            }

            if (x.Direction == direction)
            {
                stop.sprite = x.Stop;
                move = x.Renderer;
            }
            else
            {
                x.Renderer.sortingOrder = SourceOrder;
            }
        });
        stop.sortingOrder = moving ? SourceOrder : TargetOrder;
        move.sortingOrder = moving ? TargetOrder : SourceOrder;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
