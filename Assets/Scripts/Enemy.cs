using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed = 10f;
    public Vector2 Dir = new Vector2(-1, 0);
    public bool Shell = false;


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private bool isGrounded()
    {
        Bounds bounds = GetComponent<Collider2D>().bounds;
        float range = bounds.size.y * 0.25f;
        Vector2 v = new Vector2(bounds.center.x, bounds.min.y - range);
        RaycastHit2D hit = Physics2D.Linecast(v, bounds.center);
        return (hit.collider.gameObject != gameObject);
    }

    public void encounterEnemy(Collider2D coll)
    {
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
