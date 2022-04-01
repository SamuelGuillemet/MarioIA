using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plante : MonoBehaviour
{
    /// <summary>
    /// If mario collides with the plante, he will die
    /// </summary>
    /// <param name="other"></param>
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.name == "BabyMario")
        {
            other.gameObject.GetComponent<Mario>().marioDied();
        }
    }
}
