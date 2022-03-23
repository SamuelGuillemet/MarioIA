using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    private Vector2 _posInit;
    public GameObject NextPrefab;
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
            //if mario hits from under and animation not already started
            if (col.GetContact(0).normal.y >= 0.5 && !_isAnimated)

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

        for (int i = 0; i < 16; i++)
        {
            this.transform.position = new Vector2(_posInit.x, _posInit.y + 0.03125F * i);
            yield return null;
        }

        for (int i = 16; i > 0; i--)
        {
            this.transform.position = new Vector2(_posInit.x, _posInit.y + 0.03125F * i);
            yield return null;
        }

        if (NextPrefab)
        {
            if (NextPrefab.name == "Fixed")
            {
                //animate coin
            }
            Instantiate(NextPrefab, _posInit, Quaternion.identity);
        }
        else
        {
            //animate destruction
        }

        Destroy(gameObject);
    }

    //modify block behaviour for hammerbros

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
}