using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerBros : MonoBehaviour
{
    public int _countJump = 0;
    public bool ShouldJump = false;

    public bool ThrowHammer = false;
    private bool _launch;

    public GameObject hammer;
    private GameObject[] _hammersInTheScene;
    private int _indexOfNextSpawn;

    public Transform MarioTransform;

    private int _dir;

    // Start is called before the first frame update
    void Start()
    {
        _hammersInTheScene = new GameObject[4];
        _indexOfNextSpawn = 0;
        _dir = ((int)transform.localScale.x) * -1;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        foreach (GameObject item in _hammersInTheScene)
        {
            if (item != null)
                item.transform.eulerAngles = Vector3.forward * ((item.transform.eulerAngles.z + 15) % 360);
        }

        if (MarioTransform.position.x > transform.position.x + 2f)
            transform.localScale = new Vector2(-1, 1);
        else if (MarioTransform.position.x < transform.position.x - 2f)
            transform.localScale = new Vector2(1, 1);

        if (ThrowHammer && !_launch)
        {
            _launch = true;
            StartCoroutine("Throw");
        }

        if (ShouldJump)
        {
            float jumpforce = 6.5f;
            transform.parent.GetComponent<Rigidbody2D>().velocity = new Vector2(0, jumpforce);
        }

    }

    IEnumerator Throw()
    {
        yield return new WaitForSeconds(0.25f);
        if (_hammersInTheScene[_indexOfNextSpawn] != null)
            Destroy(_hammersInTheScene[_indexOfNextSpawn]);
        _hammersInTheScene[_indexOfNextSpawn] = Instantiate(hammer, transform.position + Vector3.up, Quaternion.identity, transform.parent);
        _hammersInTheScene[_indexOfNextSpawn].GetComponent<Rigidbody2D>().velocity = new Vector2(3.5f * _dir, 8);
        _indexOfNextSpawn = (_indexOfNextSpawn + 1) % 4;
        _launch = false;
    }

    /* 
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "PlateformDown" && (CountJump == 1 || CountJump == 2))
            {
                Physics2D.IgnoreCollision(GetComponent<Collider2D>(), other.transform.parent.GetComponent<Collider2D>(), true);
            }
            if (other.tag == "PlateformUp" && (CountJump == 3 || CountJump == 0) && jump)
            {
                Physics2D.IgnoreCollision(GetComponent<Collider2D>(), other.transform.parent.GetComponent<Collider2D>(), true);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.tag == "PlateformDown" && (CountJump == 1 || CountJump == 2))
            {
                Physics2D.IgnoreCollision(GetComponent<Collider2D>(), other.transform.parent.GetComponent<Collider2D>(), false);
            }
            if (other.tag == "PlateformUp" && (CountJump == 3 || CountJump == 0))
            {
                Physics2D.IgnoreCollision(GetComponent<Collider2D>(), other.transform.parent.GetComponent<Collider2D>(), false);
            }
        } */
}