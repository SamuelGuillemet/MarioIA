using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int Speed = 2;
    public Vector2 Dir = Vector2.left;

    [HideInInspector]
    public Animator _animator;


    // Update is called once per frame
    void FixedUpdate()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(Dir.x * Speed, GetComponent<Rigidbody2D>().velocity.y);
        transform.localScale = new Vector2(Dir.x, transform.localScale.y);
    }

    // changement de direction apres collision
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Destination") || other.gameObject.CompareTag("Enemy"))
        {
            if (Dir == Vector2.left)
                Dir = Vector2.right;
            else
                Dir = Vector2.left;
        }
        if (other.gameObject.name == "BabyMario")
        {
            //mort de mario a coder au rassemblage 
            Debug.Log("Die");
        }
    }
}
