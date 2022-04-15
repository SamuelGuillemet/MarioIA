using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goomba : Enemy
{
    //Used to disable the killing of Mrio if Goomba is stomped
    private bool _died = false;

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
        if (other.gameObject.tag == "Player" && !_died)
        {
            if (other.GetContact(0).normal.y <= -0.75f)
            {
                Stomped();
                _died = true;
                other.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(other.gameObject.GetComponent<Rigidbody2D>().velocity.x, other.gameObject.GetComponent<Mario>().SpeedJumpOnEnemy);
            }
            else
            {
                Debug.Log("Mario Died");
            }
        }
        else
        {
            CollisionHandler(other);
        }
    }
}
