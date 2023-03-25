using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThisInfo : MonoBehaviour
{
    // Start is called before the first frame update

    public bool PrintPosition = true;
    private BoxCollider2D boxCollider2D;
    void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(PrintPosition){ Debug.Log($"X:[{boxCollider2D.transform.localPosition.x}] Y:[{boxCollider2D.transform.localPosition.y}]"); }
    }
}
