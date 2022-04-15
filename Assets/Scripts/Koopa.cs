using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Koopa : Enemy
{
    //If Fly is true, the koopa will be a koopa paratroopa.
    [Tooltip("If Fly is true, the koopa will be a koopa paratroopa.")]
    public bool Fly;

    void Start()
    {
        Animator = GetComponent<Animator>();
        Animator.SetBool("Walk", Fly);
    }

    void FixedUpdate()
    {
        if (Fly && isGrounded())
            StartCoroutine("Jump");

        GetComponent<Rigidbody2D>().velocity = new Vector2(Dir.x * Speed, GetComponent<Rigidbody2D>().velocity.y);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (other.GetContact(0).normal.y <= -0.75f)
            {
                if (Fly)
                {
                    Animator.SetBool("Walk", false);
                    Fly = false;
                }
                else if (Shell)
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

    /// <summary>
    /// This function is called when the koopa paratroopa should jump
    /// </summary>
    /// <returns></returns>
    private IEnumerator Jump()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(Dir.x * Speed, 8f);
        yield return new WaitForSeconds(0.5f);
    }

    /// <summary>
    /// This function is called when the koopa paratroopa should jump to see if it is on the ground
    /// </summary>
    /// <returns>If the enemy is on the ground</returns>
    private bool isGrounded()
    {
        if (!GetComponent<Collider2D>().enabled)
            return false;
        Bounds bounds = GetComponent<Collider2D>().bounds;
        float range = bounds.size.y * 0.25f;
        Vector2 v = new Vector2(bounds.center.x, bounds.min.y - range);
        RaycastHit2D hit = Physics2D.Linecast(v, bounds.center);
        return (hit.collider.gameObject != gameObject);
    }
}
