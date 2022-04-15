using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuzzyBeetle : Enemy
{
    void Start()
    {
        Animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (GetComponent<Rigidbody2D>().IsAwake())
            GetComponent<Rigidbody2D>().velocity = new Vector2(Dir.x * Speed, GetComponent<Rigidbody2D>().velocity.y);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (other.GetContact(0).normal.y <= -0.75f)
            {
                if (Shell)
                    FlipAndDie();
                else
                {
                    Stomped();
                    Shell = true;
                }
                other.gameObject.GetComponent<Mario>().bounceEnemy();
            }
            else
            {
                if (Shell && Speed == 0f)
                {
                    Speed = 6f;
                    Dir = new Vector2(other.GetContact(0).normal.x, Dir.y);
                }
                else
                    other.gameObject.GetComponent<Mario>().marioDied();

            }
        }
        else
            CollisionHandler(other);
    }
}
