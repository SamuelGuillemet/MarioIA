using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarapaceKoopaTroopa : Enemy
{   
    bool stomp = false;
    public GameObject CarapaceKoopaTroopaVert;
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
            
            //arrêt du koopa
            
            Speed = 0;
            
            //transformatio du koopa en carapace
            Destroy(gameObject);
            GameObject a = Instantiate(CarapaceKoopaTroopaVert, transform.position, transform.rotation) as GameObject;
            
        }
    }
      
    
}