using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    private Vector2 _posInit;
    public GameObject NextPrefab;
    public GameObject ToSpawn;
    private List<GameObject> _enemiesOnTop = new List<GameObject>();
    //To avoid the animation to be played when the block is already animating
    private bool _isAnimated;

    // Start is called before the first frame update
    void Start()
    {
        _posInit = transform.position;
        _isAnimated = false;
    }
    /// <summary>
    /// Checks if Mario hits a block, launches animation, and kills enemies on top of said block.
    /// </summary>
    /// <param name="col"></param>
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.name == "BabyMario")
        {
            if (col.GetContact(0).normal.y >= 0.5 && !_isAnimated) //if Mario hits from under and animation not already started
            {
                foreach (GameObject enemy in _enemiesOnTop.ToArray())
                {
                    enemy.GetComponentInChildren<Enemy>().FlipAndDie();
                }
                StartCoroutine("BrickHit");
            }
        }

        if (col.gameObject.tag == "Enemy")
            _enemiesOnTop.Add(col.gameObject);
    }

    /// <summary>
    /// Animates the block and replaces it with the block it should spawn next
    /// </summary>
    /// <returns></returns>
    private IEnumerator BrickHit()
    {
        _isAnimated = true;
        if (ToSpawn)
            Instantiate(ToSpawn, _posInit, Quaternion.identity);

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
            GameObject nextPrefab = Instantiate(NextPrefab, _posInit, Quaternion.identity);
            nextPrefab.name = NextPrefab.name;
        }

        Destroy(gameObject);

    }

    /// <summary>
    /// Checks if enemies exit the block to remove them from the list
    /// </summary>
    /// <param name="other"></param>

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
            _enemiesOnTop.Remove(other.gameObject);
    }
}