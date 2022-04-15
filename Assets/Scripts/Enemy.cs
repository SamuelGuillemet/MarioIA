using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    public float Speed = 2f;
    public Vector2 Dir = new Vector2(-1, 0);
    private bool _shell = false;
    private bool _died = false;

    private Animator _animator;

    public bool Died { get => _died; set => _died = value; }
    public Animator Animator { get => _animator; set => _animator = value; }
    public bool Shell { get => _shell; set => _shell = value; }

    private void FixedUpdate()
    {
        if (transform.localPosition.y < -1)
        {
            Destroy(gameObject);
        }
    }

    public bool isGrounded()
    {
        Bounds bounds = GetComponent<Collider2D>().bounds;
        float range = bounds.size.y * 0.25f;
        Vector2 v = new Vector2(bounds.center.x, bounds.min.y - range);
        RaycastHit2D hit = Physics2D.Linecast(v, bounds.center);
        return (hit.collider.gameObject != gameObject);
    }

    public void encounterEnemy(Collision2D coll)
    {
        if (coll.gameObject.tag == "Destination" && coll.GetContact(0).normal.y < 0.1f)
        {
            transform.localScale = new Vector2(-1 * transform.localScale.x, transform.localScale.y);
            Dir = new Vector2(-1 * Dir.x, Dir.y);
        }
        else if (coll.gameObject.tag == "Enemy")
        {
            if (Shell)
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

    public void stomp()
    {
        _animator.SetTrigger("Stomp");
        _died = true;
        Dir = Vector2.zero;
    }

    public void shell()
    {
        _animator.SetTrigger("Stomp");
        StartCoroutine("changeShell");
        Speed = 0f;
    }

    public void fly()
    {
        _animator.SetBool("Walk", false);
    }

    public void FlipAndDie()
    {
        GetComponent<Collider2D>().enabled = false;
        Dir = Vector2.zero;
        transform.localScale = new Vector2(transform.localScale.x, -1);
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, 4f);
        _died = true;
    }


    IEnumerator changeShell()
    {
        yield return new WaitForSeconds(0.5f);
        _shell = true;
    }
}
