using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuzzyBeetle : Enemy
{   
    bool stomp = false;
    public GameObject CarapaceBeetle;
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
            
            //arrêt du beetle
            
            Speed = 0;
            
            //transformation du beetle en carapace
            Destroy(gameObject);
            GameObject a = Instantiate(CarapaceBeetle, new Vector3(transform.position.x,transform.position.y + 0.2f,transform.position.z), transform.rotation) as GameObject;
            
        }
    }
      
    
}