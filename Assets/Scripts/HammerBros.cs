using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class describe the behaviour of the <see cref="Enemy"/> HammerBros
/// </summary>
public class HammerBros : Enemy
{
    [HideInInspector]
    /// <summary>
    /// This boolean is controlled by the animation and said if <see cref="HammerBros"/> should jump
    /// </summary>
    public bool ShouldJump = false;

    /// <summary>
    /// This boolean prevent from multiple jumps
    /// </summary>
    private bool _isJumping = false;

    /// <summary>
    /// Between 0 and 3 included and describe the cycle of the <see cref="HammerBros"/> and where he is on the plateform
    /// </summary>
    public int CountJump = 0;

    /// <summary>
    /// The prefab that will be thrown by <see cref="HammerBros"/>
    /// </summary>
    public GameObject Hammer;

    /// <summary>
    /// All hammers in the scene are refernced in this list, maximu is 4 at the same time in the scene
    /// </summary>
    private GameObject[] _hammersInTheScene;
    private int _indexOfNextSpawn;

    [HideInInspector]
    /// <summary>
    /// This boolean is controlled by the animation and said if <see cref="HammerBros"/> should throw hammer
    /// </summary>
    public bool ShouldThrowHammer = false;

    /// <summary>
    /// This boolean prevent from multiple throw
    /// </summary>
    private bool _isThrowingHammer = false;

    //Stuff linked to the position
    private Transform _marioTransform;
    /// <summary>
    /// This is used to inverse the orientation of hammerbros
    /// </summary>
    public Transform MarioTransform { get => _marioTransform; set => _marioTransform = value; }

    /// <summary>
    /// The direction of the next hammer that will be spawned, based on <see cref="Mario"/> position
    /// </summary>
    private int _dir;

    void Start()
    {
        _hammersInTheScene = new GameObject[2];
        _indexOfNextSpawn = 0;
        _dir = ((int)transform.localScale.x) * -1;
    }

    void FixedUpdate()
    {
        foreach (GameObject item in _hammersInTheScene)
        {
            if (item != null)
            {
                item.transform.eulerAngles = Vector3.forward * ((item.transform.eulerAngles.z + 15) % 360);
                if (item.GetComponent<Collider2D>().IsTouching(MarioTransform.gameObject.GetComponent<BoxCollider2D>()))
                {
                    MarioTransform.gameObject.GetComponent<Mario>().MarioDied();
                }
                if (item.transform.localPosition.y < -1)
                    Destroy(item);
            }
        }
        if (!Dead)
        {
            if (MarioTransform.position.x > transform.position.x + 2f)
                transform.localScale = new Vector2(-1, 1);
            else if (MarioTransform.position.x < transform.position.x - 2f)
                transform.localScale = new Vector2(1, 1);
            _dir = ((int)transform.localScale.x) * -1;

            if (ShouldThrowHammer && !_isThrowingHammer)
            {
                _isThrowingHammer = true;
                StartCoroutine(Throw());
            }

            if (ShouldJump && !_isJumping)
            {
                _isJumping = true;
                StartCoroutine(Jump());
            }
        }
    }

    /// <summary>
    /// The function that will be called when <see cref="HammerBros"/> has to throw a hammer
    /// </summary>
    IEnumerator Throw()
    {
        yield return new WaitForSeconds(0.25f);
        if (_hammersInTheScene[_indexOfNextSpawn] != null)
            Destroy(_hammersInTheScene[_indexOfNextSpawn]);
        _hammersInTheScene[_indexOfNextSpawn] = Instantiate(Hammer, transform.position + Vector3.up, Quaternion.identity, transform.parent);
        _hammersInTheScene[_indexOfNextSpawn].GetComponent<Rigidbody2D>().velocity = new Vector2(3.5f * _dir, 8);
        _indexOfNextSpawn = (_indexOfNextSpawn + 1) % _hammersInTheScene.Length;
        _isThrowingHammer = false;
    }

    /// <summary>
    /// The function that will be called when <see cref="HammerBros"/> has to jump
    /// </summary>
    IEnumerator Jump()
    {
        float jumpforce = 10f;
        if (CountJump >= 2)
            jumpforce = 3.5f;
        transform.parent.GetComponent<Rigidbody2D>().velocity = new Vector2(0, jumpforce);
        CountJump = (CountJump + 1) % 4;
        yield return new WaitForSeconds(1f);
        _isJumping = false;
    }

    /// <summary>
    /// The function that will be called when <see cref="Mario"/> collides with the HammerBros
    /// </summary>
    /// <param name="other"><see cref="Mario"/> most of the time</param>
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (other.GetContact(0).point.y + 1 > transform.position.y)
            {
                other.gameObject.GetComponent<Mario>().BounceEnemy();
                FlipAndDie();
            }
            else
            {
                other.gameObject.GetComponent<Mario>().MarioDied();
            }
        }
    }

    /// <summary>
    /// This function handle the collision with the plateforms
    /// </summary>
    /// <param name="other"> The collider attached to the <see cref="Block"/></param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "PlatformDown" && (CountJump == 1 || CountJump == 2))
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), other.transform.parent.GetComponent<Collider2D>(), true);
        }
        if (other.tag == "PlatformUp" && (CountJump == 3 || CountJump == 0) && _isJumping)
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), other.transform.parent.GetComponent<Collider2D>(), true);
        }
    }

    /// <summary>
    /// This function handle the collision with the plateforms
    /// </summary>
    /// <param name="other"> The collider attached to the <see cref="Block"/></param>
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "PlatformDown" && (CountJump == 1 || CountJump == 2))
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), other.transform.parent.GetComponent<Collider2D>(), false);
        }
        if (other.tag == "PlatformUp" && (CountJump == 3 || CountJump == 0))
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), other.transform.parent.GetComponent<Collider2D>(), false);
        }
    }
}