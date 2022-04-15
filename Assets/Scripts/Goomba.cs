using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goomba : Enemy
{
    void Start()
    {
        Animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(Dir.x * Speed, GetComponent<Rigidbody2D>().velocity.y);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player" && !Dead)
        {
            if (other.GetContact(0).normal.y <= -0.75f)
            {
                Stomped();
                Dead = true;
                other.gameObject.GetComponent<Mario>().bounceEnemy();
            }
            else
                other.gameObject.GetComponent<Mario>().marioDied();
        }
        else
            CollisionHandler(other);
    }
}
