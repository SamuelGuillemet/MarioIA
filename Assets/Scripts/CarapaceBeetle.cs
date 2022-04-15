using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarapaceBeetle : Enemy
{   
    bool stomp = false;
    private void Start()
    {   Speed = 0;
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