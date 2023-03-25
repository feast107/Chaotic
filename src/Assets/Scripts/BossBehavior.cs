using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossBehavior : MonoBehaviour
{
    public ValueController Health;
    // Start is called before the first frame update
    private void Start()
    {
        Health.OnEmptyEvent += () => { Debug.Log("DIED"); };
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log($"{other} hit");
        Health.Cast(2f);
    }

    // Update is called once per frame
    private void Update()
    {
        
    }
}
