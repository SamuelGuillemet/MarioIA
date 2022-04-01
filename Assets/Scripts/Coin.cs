using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    /// <summary>
    /// If mario collects the coin, it will be destroyed
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "BabyMario")
        {
            Destroy(gameObject);
        }
    }
}
