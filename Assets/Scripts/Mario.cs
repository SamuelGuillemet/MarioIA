using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mario : MonoBehaviour
{
    private Rigidbody2D _rb;
    private Collider2D _col;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _col = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }
}
