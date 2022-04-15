using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KoopaParatroopa : Enemy
{   
    bool stomp = false;
    public GameObject KoopaTroopaVert;
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
            
            
            Speed = 0;
            
            //transformatio du koopa en carapace
            Destroy(gameObject);
            GameObject a = Instantiate(KoopaTroopaVert, new Vector3(transform.position.x,transform.position.y -0.1f,transform.position.z), transform.rotation) as GameObject;
            
        }
    }
    private void Update()
    {
        if(GetComponent<Rigidbody2D>().velocity.y == 0) 
        {GetComponent<Rigidbody2D>().velocity = new Vector2(Dir.x * Speed, 7.0f);

        }

    }
      
    
}