using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that describe the behaviour of <see cref="Goomba"/>, <see cref="Koopa"/>, <see cref="BuzzyBeetle"/> and <see cref="HammerBros"/>
/// </summary>
public class Enemy : MonoBehaviour
{
    /// <summary>
    /// The speed of most of the enemies except for <see cref="HammerBros"/> 
    /// </summary>
    public float Speed = 2f;

    /// <summary>
    /// Direction of the mooving of the enemies except for <see cref="HammerBros"/>
    /// </summary>
    public Vector2 Dir = new Vector2(-1, 0);

    /// <summary>
    /// The anmiator attached to evry <see cref="Enemy"/>
    /// </summary>
    private Animator _animator;
    public Animator Animator { get => _animator; set => _animator = value; }

    /// <summary>
    /// A bool that indicate if the current <see cref="Enemy"/> is a shell or not, useful inside <see cref="CollisionHandler"/>
    /// </summary>
    private bool _shell = false;
    public bool Shell { get => _shell; set => _shell = value; }

    /// <summary>
    /// A boolean that stop, the collision of the stomped <see cref="Goomba"/>, and, the behaviour of <see cref="HammerBros"/>
    /// </summary>
    private bool _dead = false;
    public bool Dead { get => _dead; set => _dead = value; }

    private Transform _mainCameraTransform;
    /// <summary>
    /// Used to wake up the enemy when <see cref="MainCamera"/> is close enough
    /// </summary>
    public Transform MainCameraTransform { set => _mainCameraTransform = value; }

    private void OnEnable()
    {
        GetComponentInParent<Rigidbody2D>().Sleep();
    }

    /// <summary>
    /// Used to kill the enemy if he fall off the screen and handle the WakeUp if <see cref="Mario"/> is close enough to the <see cref="Enemy"/>
    /// </summary>
    private void FixedUpdate()
    {
        if (transform.localPosition.y < -1)
        {
            Destroy(gameObject);
        }
        if (_mainCameraTransform.localPosition.x + 15 > transform.parent.localPosition.x + transform.localPosition.x)
        {
            GetComponentInParent<Rigidbody2D>().WakeUp();
            Speed = 2f;
        }
        else if (!_shell)
        {
            GetComponentInParent<Rigidbody2D>().Sleep();
            Speed = 0f;
        }
    }

    /// <summary>
    /// The function that handle the collison with the tag "destination" and with the tag "Enemy"
    /// </summary>
    /// <param name="coll"></param>
    public void CollisionHandler(Collision2D coll)
    {
        if (coll.gameObject.tag == "Destination" && (Mathf.Abs(coll.GetContact(0).normal.x) > 0.75f || coll.GetContact(0).normal.y < 0.25f))
        {
            transform.localScale = new Vector2(-1 * transform.localScale.x, transform.localScale.y);
            Dir = new Vector2(-1 * Dir.x, Dir.y);
        }
        else if (coll.gameObject.tag == "Enemy")
        {
            if (_shell)
            {
                coll.gameObject.GetComponent<Enemy>().FlipAndDie();
                return;
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
    /// The function that make the transition when an enemy is stomped by <see cref="Mario"/>
    /// </summary>
    public void Stomped()
    {
        _animator.SetTrigger("Stomp");
        Dir = Vector2.zero;
        Speed = 0f;
    }

    /// <summary>
    /// The function that handle the <see cref="FlipAndDie"/> animation before the death of the enemy
    /// </summary>
    public void FlipAndDie()
    {
        var agent = GetComponentInParent<Environment>().MarioAgent;
        if (agent != null)
            agent.GetReward(0.75f);

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
