using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float Speed = 2f;
    public Vector2 Dir = new Vector2(-1, 0);
    private Animator _animator;
    public Animator Animator { get => _animator; set => _animator = value; }
    private bool _shell = false;
    public bool Shell { get => _shell; set => _shell = value; }

    private bool _dead = false;
    public bool Dead { get => _dead; set => _dead = value; }

    /// <summary>
    /// Used to kill the enemy if he fall off the screen
    /// </summary>
    private void Update()
    {
        if (transform.localPosition.y < -1)
        {
            Destroy(gameObject);
        }
        //TODO Fix with global environment
        if (GameObject.Find("Main Camera").transform.localPosition.x + 15 > transform.parent.localPosition.x + transform.localPosition.x)
            GetComponent<Rigidbody2D>().WakeUp();
        else
            GetComponent<Rigidbody2D>().Sleep();
    }

    /// <summary>
    /// The function that handle the collison with the tag "destination" and with the tag Enemy
    /// </summary>
    /// <param name="coll"></param>
    public void CollisionHandler(Collision2D coll)
    {
        if (coll.gameObject.tag == "Destination" && coll.GetContact(0).normal.y < 0.1f)
        {
            transform.localScale = new Vector2(-1 * transform.localScale.x, transform.localScale.y);
            Dir = new Vector2(-1 * Dir.x, Dir.y);
        }
        else if (coll.gameObject.tag == "Enemy")
        {
            if (_shell)
            {
                coll.gameObject.GetComponent<Enemy>().FlipAndDie();
                FlipAndDie();
            }
            GameObject enemy = coll.gameObject;
            if (enemy.GetComponent<Enemy>().Dir.x == Dir.x)
            {
                if (gameObject.transform.localPosition.x > enemy.transform.localPosition.x && Dir.x == -1)
                {
                    transform.localScale = new Vector2(-1 * transform.localScale.x, transform.localScale.y);
                    Dir = new Vector2(-1 * Dir.x, Dir.y);
                }
                if (gameObject.transform.localPosition.x < enemy.transform.localPosition.x && Dir.x == 1)
                {
                    transform.localScale = new Vector2(-1 * transform.localScale.x, transform.localScale.y);
                    Dir = new Vector2(-1 * Dir.x, Dir.y);
                }
            }
            else
            {
                transform.localScale = new Vector2(-1 * transform.localScale.x, transform.localScale.y);
                Dir = new Vector2(-1 * Dir.x, Dir.y);
            }
        }
    }

    /// <summary>
    /// The function that make the transition when an enemy is stomped by Mario
    /// </summary>
    public void Stomped()
    {
        _animator.SetTrigger("Stomp");
        Dir = Vector2.zero;
        Speed = 0f;
    }

    /// <summary>
    /// The function that handle the FlipAndDie animation before the death of the enemy
    /// </summary>
    public void FlipAndDie()
    {
        _dead = true;
        GetComponentInChildren<Collider2D>().enabled = false;
        Dir = Vector2.zero;
        Speed = 0;
        transform.localScale = new Vector2(transform.localScale.x, -1);
        GetComponentInParent<Rigidbody2D>().velocity = new Vector2(0, 12f);
        GetComponentInChildren<Animator>().enabled = false;
        GetComponentInParent<Rigidbody2D>().gravityScale = 4;
    }
}
