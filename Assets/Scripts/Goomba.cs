using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goomba : Enemy
{
    bool stomp = false;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "BabyMario")
        {
            GameObject player = collision.gameObject;
            //rebondissement de mario
            player.GetComponent<Rigidbody2D>().velocity = new Vector2(player.GetComponent<Rigidbody2D>().velocity.x, player.GetComponent<Mario>().SpeedJumpOnEnemy);
            stomp = true;
            //animation de mort de goomba
            _animator.SetBool("Died", stomp);
            Speed = 0;
            //delai avant disparition du Goomba aplati
            Invoke("Mort", 1);

        }
    }
      private void Mort()
    {
        Destroy(gameObject);
    }

    
}
