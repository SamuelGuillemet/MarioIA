using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class brick : MonoBehaviour
{
    private Vector2 _posInit;

    // Start is called before the first frame update
    void Start()
    {
        _posInit = transform.position;


    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
        }
    }

}
