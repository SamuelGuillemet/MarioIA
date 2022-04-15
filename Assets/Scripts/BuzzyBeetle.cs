using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuzzyBeetle : Enemy
{
    // Start is called before the first frame update
    void Start()
    {
        Animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(Dir.x * Speed, GetComponent<Rigidbody2D>().velocity.y);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player" && !Died)
        {
            if (Shell)
            {
                if (other.GetContact(0).normal.y <= -0.75f)
                {
                    FlipAndDie();
                }
                else
                {
                    Speed = 6f;
                    Dir = new Vector2(other.GetContact(0).normal.x, Dir.y);
                }
            }
            else
            {
                if (other.GetContact(0).normal.y <= -0.75f)
                {
                    shell();
                    other.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(other.gameObject.GetComponent<Rigidbody2D>().velocity.x, other.gameObject.GetComponent<Mario>().SpeedJumpOnEnemy);
                }
                else
                {
                    Debug.Log("Mario Died");
                }
            }
        }
        else
        {
            encounterEnemy(other);
        }

    }
}
