using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuntimeInfo : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        var mos = Input.mousePosition;
        Debug.Log($"X:{mos.x} Y:{mos.y}");
    }
}
