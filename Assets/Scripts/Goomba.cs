using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goomba : Enemy
{
public bool ecrase = false;
public Animator animator;
GameObject player;
private void OnTriggerEnter2D2(Collider2D collision){

  
    
     if(collision.gameObject.tag == "player")
        {
            float yOffset = 0.9f;
            if(transform.position.y + yOffset < collision.transform.position.y)
            {
              //rebondissement de mario
                player.GetComponent<Rigidbody2D>().velocity = Vector2.up * 7;
                ecrase = true;
                //animation de mort de goomba
                animator.SetBool("ecrase", ecrase);
                
                speed = 0;
                //delai avant disparition du Goomba aplati
                Invoke("Mort", 1);
            }
            else
            {
               //mort de mario a coder au rassemblage 
            }
        }
}
private void Mort()
    {
        Destroy(gameObject);
    }
} 
