using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int speed;
    public bool isright = false;
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("player");
    }
  
    // Update is called once per frame
    void Update()
    {
        if(isright)
        {
        transform.Translate(2 * Time.deltaTime * speed, 0, 0); 
        }
        else
        {
        transform.Translate(-2 * Time.deltaTime * speed, 0, 0); 
        }
        
    }
    // changement de direction apres collision
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle") || collision.gameObject.CompareTag("Enemies"))
        {
            if (isright)
            {
                isright = false;
            }
            else
            {
                isright = true;
            }
        }
        }
}
