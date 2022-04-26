using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// If <see cref="Mario"/> collects the coin, it will be destroyed
/// </summary>
public class Coin : MonoBehaviour
{
    /// <summary>
    /// If <see cref="Mario"/> collects the coin, it will be destroyed
    /// </summary>
    /// <param name="other">Basically <see cref="Mario"/></param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
}
