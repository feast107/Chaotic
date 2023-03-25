using System;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    // Start is called before the first frame update
    [Serializable]
    public struct SpriteConfig
    {
        public Direction Direction;
        public Sprite Sprite;
    }

    public List<SpriteConfig> Configures;

    public Animator Animator;
    public SpriteRenderer Surface;
    public Vector3 StartPosition = Vector3.zero;
    public Direction Direction;
    public BoxCollider2D BoxCollider;
    public float Speed = 2f;

    private DateTime? destroyTime;
    private TimeSpan later;

    private bool stop = false;
    public int Life = 5;
    private void Start()
    {
        if(Animator == null) Animator = GetComponent<Animator>();
        Animator.enabled = false;
        if (Surface == null) Surface = GetComponent<SpriteRenderer>();
        if (BoxCollider == null) BoxCollider = GetComponent<BoxCollider2D>();
        later = TimeSpan.FromSeconds( Animator.GetCurrentAnimatorClipInfo(0)[0].clip.length);
        destroyTime = DateTime.Now + TimeSpan.FromSeconds(Life);
        var f = Configures.Find(x => x.Direction == Direction).Sprite;
        if (f != null)
        {
            Surface.sprite = f;
        }

        if (StartPosition != Vector3.zero)
        {
            transform.position = StartPosition;
        }
    }

    private void OnCollisionEnter2D(Collision2D another)
    {
        stop = true;
        Animator.enabled = true;
        destroyTime = DateTime.Now + later;
    }
    // Update is called once per frame
    private void Update()
    {
        if (stop) return;
        BoxCollider.transform.position += Speed * Time.deltaTime * Direction.ToNormalized();
    }

    private void FixedUpdate()
    {
        if (destroyTime != null && DateTime.Now > destroyTime)
        {
            Destroy(gameObject);
        }
    }
}
