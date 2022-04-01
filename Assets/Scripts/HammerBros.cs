using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerBros : MonoBehaviour
{
    private int _currentPlatform;
    public int _nextPlatform; // à chaque saut, modifier cette valeur via animator



    // Start is called before the first frame update
    void Start()
    {
        _currentPlatform = Mathf.FloorToInt(transform.position.y / 4);
        _nextPlatform = _currentPlatform;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name.Contains("Brick_hammer") && _currentPlatform != _nextPlatform)
        {
            Debug.Log("entré");
            Physics2D.IgnoreCollision(other.gameObject.GetComponentsInParent<Collider2D>()[0], gameObject.transform.GetComponent<Collider2D>(), true);
            _currentPlatform = _nextPlatform;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.name.Contains("Brick_hammer") && _currentPlatform == _nextPlatform)
        {
            Physics2D.IgnoreCollision(other.gameObject.GetComponentsInParent<Collider2D>()[0], gameObject.transform.GetComponent<Collider2D>(), false);
        }
    }


    /*quand l'animation est lancé, ignorer toutes collisions des blocs taggés et réactiver quand il recommence à tomber
    sinon utilisation des layers  est plus opti car IgnoreCollision agit bien trop tard
    
*/
}




