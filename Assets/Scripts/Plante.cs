using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class handle the detection of <see cref="Mario"/> touching the <see cref="Plante"/>
/// </summary>
public class Plante : MonoBehaviour
{
    /// <summary>
    /// If <see cref="Mario"/> collides with the plante, he will die
    /// </summary>
    /// <param name="other">It will be <see cref="Mario"/> most of the time</param>
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.name == "BabyMario")
        {
            other.gameObject.GetComponent<Mario>().MarioDied();
        }
    }
}
