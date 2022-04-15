using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Koopa : Enemy
{
    public bool Fly;
    // Start is called before the first frame update
    void Start()
    {
        Animator = GetComponent<Animator>();
        Animator.SetBool("Walk", Fly);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Fly)
            if (isGrounded())
                StartCoroutine("Jump");

        GetComponent<Rigidbody2D>().velocity = new Vector2(Dir.x * Speed, GetComponent<Rigidbody2D>().velocity.y);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player" && !Died)
        {
            if (other.GetContact(0).normal.y <= -0.75f)
            {
                if (Fly)
                {
                    fly();
                    Fly = false;
                }
                else if (Shell)
                {
                    FlipAndDie();
                }
                else
                {
                    shell();
                }
                other.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(other.gameObject.GetComponent<Rigidbody2D>().velocity.x, other.gameObject.GetComponent<Mario>().SpeedJumpOnEnemy);
            }
            else
            {
                if (Shell && Speed == 0f)
                {
                    Speed = 6f;
                    Dir = new Vector2(other.GetContact(0).normal.x, Dir.y);
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

    IEnumerator Jump()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(Dir.x * Speed, 8f);
        yield return new WaitForSeconds(0.5f);
    }
}
