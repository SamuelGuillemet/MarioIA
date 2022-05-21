using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that handle the behaviour of blocks  and handle the death of enemies when <see cref="Mario"/> hits underneath
/// </summary>
public class Block : MonoBehaviour
{
    private Vector2 _posInit;

    /// <summary>
    /// The prefab that will be spawned after the <see cref="Block"/> will be destroyed.
    /// </summary>
    public GameObject NextPrefab;

    /// <summary>
    /// The prefab that will be spawned when <see cref="Mario"/> hit the block by underneath
    /// </summary>
    public GameObject ToSpawn;

    /// <summary>
    /// A list of GameObject that are on teh top of the current <see cref="Block"/>
    /// </summary>
    private List<GameObject> _enemiesOnTop = new List<GameObject>();

    /// <summary>
    /// Bool to avoid the animation to be played when the block is already animating
    /// </summary>
    private bool _isAnimated;

    void Start()
    {
        _posInit = transform.position;
        _isAnimated = false;
    }

    /// <summary>
    /// Checks if Mario hits a block, launches animation, and kills enemies on top of the <see cref="Block"/> based on the list <see cref="_enemiesOnTop"/>
    /// </summary>
    /// <param name="coll">The GameObject collider that enter the collision with the <see cref="Block"/></param>
    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            if (coll.GetContact(0).normal.y >= 0.5 && !_isAnimated) //if Mario hits from under and animation not already started
            {
                foreach (GameObject enemy in _enemiesOnTop.ToArray())
                {
                    if (enemy)
                        enemy.GetComponentInChildren<Enemy>().FlipAndDie();
                }


                if (coll.gameObject.TryGetComponent<MLAgent>(out MLAgent _marioAgent))
                {
                    StartCoroutine(BrickHit(_marioAgent));
                }
                else
                {
                    StartCoroutine(BrickHit());
                }
            }
        }

        if (coll.gameObject.tag == "Enemy")
            _enemiesOnTop.Add(coll.gameObject);
    }

    /// <summary>
    /// Animates the block and replaces it with <see cref="NextPrefab"/>, and spawn <see cref="ToSpawn"/>
    /// </summary>
    private IEnumerator BrickHit(MLAgent agent = null)
    {
        _isAnimated = true;
        if (ToSpawn)
        {
            Instantiate(ToSpawn, _posInit, Quaternion.identity, transform.parent);
            if (agent != null)
                agent.GetReward(0.5f);
        }
        else
        {
            if (agent != null)
                agent.GetReward(-0.25f);
        }

        for (int i = 0; i < 32; i++)
        {
            this.transform.position = new Vector2(_posInit.x, _posInit.y + (0.5f / 32f) * i);
            yield return null;
        }

        for (int i = 32; i > 0; i--)
        {
            this.transform.position = new Vector2(_posInit.x, _posInit.y + (0.5f / 32f) * i);
            yield return null;
        }


        if (NextPrefab)
        {
            GameObject nextPrefab = Instantiate(NextPrefab, _posInit, Quaternion.identity, transform.parent);
            nextPrefab.name = NextPrefab.name;
        }

        Destroy(gameObject);

    }

    /// <summary>
    /// Checks if enemies exit the block to remove them from the list <see cref="_enemiesOnTop"/>
    /// </summary>
    /// <param name="other">The GameObject collider that exit the collision with the <see cref="Block"/></param>

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
            _enemiesOnTop.Remove(other.gameObject);
    }

}