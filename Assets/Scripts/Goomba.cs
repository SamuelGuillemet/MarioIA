using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class describe the behaviour of the <see cref="Enemy"/> Goomba
/// </summary>
public class Goomba : Enemy
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

    /// <summary>
    /// This function handle the behaviour of the <see cref="Goomba"/> when he enter collision 
    /// </summary>
    /// <param name="other">The GameObject collider that enter the collision with the <see cref="Goomba"/></param>
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player" && !Dead)
        {
            if (other.GetContact(0).normal.y <= -0.75f)
            {
                Stomped();
                Dead = true;
                other.gameObject.GetComponent<Mario>().BounceEnemy();
            }
            else
                other.gameObject.GetComponent<Mario>().MarioDied();
        }
        else
            CollisionHandler(other);
    }
}
