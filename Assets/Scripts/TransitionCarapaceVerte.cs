using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionCarapaceVerte : Enemy
{   
    bool stomp = false;
    public GameObject CarapaceKoopaTroopaVert;
    private void Start()
    {   Speed = 8;
        Dir = Vector2.right;
        _animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "BabyMario")
        { 
            GameObject player = collision.gameObject;
            //rebondissement de mario
            player.GetComponent<Rigidbody2D>().velocity = new Vector2(player.GetComponent<Rigidbody2D>().velocity.x, player.GetComponent<Mario>().SpeedJumpOnEnemy);
            Destroy(gameObject);
            GameObject a = Instantiate(CarapaceKoopaTroopaVert, new Vector3(transform.position.x,transform.position.y + 0.1f,transform.position.z), transform.rotation) as GameObject;
        }
    }

    private void Update()
    { if (GetComponent<Rigidbody2D>().velocity.x > 0)
       { 
           Dir = Vector2.right;
           Speed = 8;
       }
       
       if (GetComponent<Rigidbody2D>().velocity.x < 0)
       { 
           Dir = Vector2.left;
           Speed = 8;
       }     
    }       
       
}