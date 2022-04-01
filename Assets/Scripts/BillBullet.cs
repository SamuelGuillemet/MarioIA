using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBullet : MonoBehaviour
{
    private Bill _bill;
    public Bill Bill { set => _bill = value; }

    private int _index;
    public int Index { set => _index = value; }

    private int _dir;
    public int Dir { set => _dir = value; }

    private float _speed = 5f;

    private void FixedUpdate()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(_dir * _speed, 0);
        transform.localScale = new Vector2(-_dir, 1);
    }

    /// <summary>
    /// The function that will be called when mario collides with the bill
    /// </summary>
    /// <param name="other"></param>
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.name == "BabyMario")
        {
            if (other.GetContact(0).point.y > transform.position.y)
            {
                other.gameObject.GetComponent<Mario>().bounceEnemy();
                _bill.DeleteBillBullet(_index);
            }
            else
            {
                other.gameObject.GetComponent<Mario>().marioDied();
            }
        }
    }
}
