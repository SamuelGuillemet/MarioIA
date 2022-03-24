using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    private Vector2 _posInit;
    public GameObject NextPrefab;
    public GameObject ToSpawn;
    private List<GameObject> _enemiesOnTop = new List<GameObject>();
    private bool _isAnimated;

    // Start is called before the first frame update
    void Start()
    {
        _posInit = transform.position;
        _isAnimated = false;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.name == "BabyMario")
        {
            if (col.GetContact(0).normal.y >= 0.5 && !_isAnimated) //if mario hits from under and animation not already started
            {
                foreach (GameObject enemy in _enemiesOnTop.ToArray())
                {
                    //enemy.FlipAndDie(); //method used in Enemy class
                    Debug.Log("FlipAndDie");
                }

                StartCoroutine("BrickHit");
            }
        }
    }


    private IEnumerator BrickHit()
    {
        _isAnimated = true;
        if (ToSpawn)
        {
            Instantiate(ToSpawn, _posInit, Quaternion.identity);
        }

        for (int i = 0; i < 16; i++)
        {
            this.transform.position = new Vector2(_posInit.x, _posInit.y + 0.03125f * i);
            yield return null;
        }

        for (int i = 16; i > 0; i--)
        {
            this.transform.position = new Vector2(_posInit.x, _posInit.y + 0.03125f * i);
            yield return null;
        }

        Destroy(gameObject);
        if (NextPrefab)
        {
            Instantiate(NextPrefab, _posInit, Quaternion.identity);
        }
    }

    //add or remove enemy from _enemiesOnTop
    void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            _enemiesOnTop.Add(other.gameObject);
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            _enemiesOnTop.Remove(other.gameObject);
        }
    }

    //modify block behaviour for hammerbros
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "BabyMario")
        {
            Physics2D.IgnoreCollision(other, gameObject.transform.GetComponent<Collider2D>());
        }
    }


}