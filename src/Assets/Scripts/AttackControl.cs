using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttackControl : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Ammo;
    public Transform Region;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!Input.GetMouseButtonUp((int)MouseButton.Left)) return;
        var ammo = Instantiate(Ammo);
        var dir = transform.position.DirectionToMouse();
        Debug.Log(dir);
        ammo.GetComponent<Ammo>().Direction = dir;
        ammo.GetComponent<Ammo>().StartPosition = transform.position + dir.ToNormalized() * 0.75f;
        if (Region)
        {
            ammo.transform.SetParent(Region);
        }
    }
}
