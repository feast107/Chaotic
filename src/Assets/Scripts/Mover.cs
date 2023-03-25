using System;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    // Start is called before the first frame update
    [Serializable]
    public class LimitConfigure
    {
        public Direction Direction;

        public float Maximum;
    }

    public List<LimitConfigure> Configure;

    public Transform Target;

    public float Speed = 1.0f;

    

    private void MoveByKey(KeyCode code)
    {
    }

    private void Start()
    {
        if (Target == null) Target = transform;
    }

    // Update is called once per frame
    private void Update()
    {
        var dir = Direction.None;
        Configure.ForEach(x =>
        {
            var key = x.Direction.ToKey();
            if (!x.Direction.IsValid(Target.position, x.Maximum)) return;
            if (Input.GetKey(key)) dir |= key.ToDirection();
        });
        Target.position += Speed * Time.deltaTime * dir.ToNormalized();
    }
}
