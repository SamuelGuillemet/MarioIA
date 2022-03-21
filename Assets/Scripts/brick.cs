using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class brick : MonoBehaviour
{
    private Vector2 _posInit;
    public GameObject NextPrefab;
    private List<GameObject> _enemies = new List<GameObject>();
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
            if (NextPrefab)
            {
                //if mario hits from under and animation not already started
                if (_posInit[0] - 0.5 <= col.GetContact(0).point[0] && col.GetContact(0).point[0] <= _posInit[0] + 0.5 && col.GetContact(0).point[1] <= _posInit[1] && !_isAnimated)
                {

                    foreach (GameObject enemy in _enemies.ToArray())
                    {
                        FlipAndKill(enemy);
                    }

                    StartCoroutine("BrickHit");


                }




            }

        }
    }
    private IEnumerator BrickHit()
    {
        _isAnimated = true;
        for (int i = 0; i < 16; i++)
        {
            this.transform.position = new Vector2(_posInit[0], _posInit[1] + 0.03125F * i);
            yield return null;
        }
        for (int i = 16; i > 0; i--)
        {
            this.transform.position = new Vector2(_posInit[0], _posInit[1] + 0.03125F * i);
            yield return null;
        }

        Instantiate(NextPrefab, _posInit, Quaternion.identity);
        Destroy(gameObject);

    }

    void FlipAndKill(GameObject enemy)
    {

    }

}