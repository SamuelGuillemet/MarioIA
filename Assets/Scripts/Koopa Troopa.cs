using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Koopa : Enemy {
	public GameObject KoopaShell;
    //deplacement normal et collision avec obstacle
	private void OnTriggerEnter2D3(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle") || collision.gameObject.CompareTag("enemies") || collision.CompareTag("shell"))
        {
            if (isright)
                Flip();
            else
                Flip();
        }
        //collision avec mario
        if (collision.CompareTag("player"))
        {
            float yOffset = 0.9f;
            if(transform.position.y + yOffset < collision.transform.position.y)
            {
                player.GetComponent<Rigidbody2D>().velocity = Vector2.up * 7;
                Instantiate(shell, shellSpawn.position, shellSpawn.rotation);
                Destroy(gameObject);
            }
            else { // cette partie est a coder plus tard determine la résultante de l'interaction joueur monstre (retrecissement ou mort) donc besoin
            // d'une boucle else
            }
        }
        // collision avec carapace en mouvement
        if(collision.CompareTag("shellAttack"))
        {
            animator.SetBool("Death", true);
            GetComponent<Collider2D>().isTrigger = true;

            float countdown = 0.5f;
            countdown -= Time.deltaTime;
            if(countdown > 0f)
            {
                rb.velocity = new Vector2(1f, 5f);
            }
        }
    }
//changement de direction du koopa aprés obstacle (difference avec le goomba car le goomba n'a pas a être flip il est symetrique graphiquement)
    private void Flip()
    {
        isright = !isRight;
        Vector2 ls = gameObject.transform.localScale;
        ls.x *= -1;
        transform.localScale = ls;
    }

    void Death()
    {
        Destroy(gameObject);
    }
}