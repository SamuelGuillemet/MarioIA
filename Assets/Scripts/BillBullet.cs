using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that describe the bullets of the <see cref="Bill"/>
/// </summary>
public class BillBullet : MonoBehaviour
{
    private Bill _bill;
    /// <summary>
    /// Reference the <see cref="Bill"/> that launched the <see cref="BillBullet"/>
    /// </summary>
    public Bill SetBill { set => _bill = value; }

    private int _index;
    /// <summary>
    /// Reference the index of spawn of the <see cref="BillBullet"/>
    /// </summary>
    public int Index { set => _index = value; }

    private int _dir;
    /// <summary>
    /// Reference rhe direction of spawn of the <see cref="BillBullet"/>
    /// </summary>
    public int Dir { set => _dir = value; }

    /// <summary>
    /// The speed of the <see cref="BillBullet"/>
    /// </summary>
    private float _speed = 5f;

    private void FixedUpdate()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(_dir * _speed, 0);
        transform.localScale = new Vector2(-_dir, 1);
    }

    /// <summary>
    /// The function that will be called when <see cref="Mario"/> collides with the bill
    /// </summary>
    /// <param name="other">The GameObject collider that collide with the <see cref="BillBullet"/></param>
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.name == "BabyMario")
        {
            if (other.GetContact(0).point.y > transform.position.y)
            {
                other.gameObject.GetComponent<Mario>().BounceEnemy();
                _bill.DeleteBillBullet(_index);
            }
            else
            {
                other.gameObject.GetComponent<Mario>().MarioDied();
            }
        }
    }
}
