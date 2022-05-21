using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class describe the behaviour of the <see cref="Enemy"/> koopa
/// </summary>
public class Koopa : Enemy
{
    [Tooltip("If Fly is true, the koopa will be a koopa paratroopa.")]
    /// <summary>
    /// If Fly is true, the koopa will be a koopa paratroopa.
    /// </summary>
    public bool Fly;

    void Start()
    {
        Animator = GetComponent<Animator>();
        Animator.SetBool("Walk", Fly);
    }

    void Update()
    {
        if (GetComponent<Rigidbody2D>().IsAwake())
        {
            if (Fly && IsGrounded())
                StartCoroutine("Jump");

            GetComponent<Rigidbody2D>().velocity = new Vector2(Dir.x * Speed, GetComponent<Rigidbody2D>().velocity.y);
        }
    }

    /// <summary>
    /// This function handle the behaviour of the <see cref="Koopa"/> when he enter collision 
    /// </summary>
    /// <param name="other">The GameObject collider that enter the collision with the <see cref="Koopa"/></param>
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if ((transform.localPosition.y <= other.transform.localPosition.y - 0.5f))
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
                other.gameObject.GetComponent<Mario>().BounceEnemy();
            }
            else
            {
                if (Shell && Speed == 0f)
                {
                    Speed = 6f;
                    Dir = new Vector2(other.GetContact(0).normal.normalized.x, Dir.y);
                }
                else
                    other.gameObject.GetComponent<Mario>().MarioDied();
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
    private bool IsGrounded()
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
