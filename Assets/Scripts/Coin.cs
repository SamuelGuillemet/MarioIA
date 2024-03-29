using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// If <see cref="Mario"/> collects the coin, it will be destroyed
/// </summary>
public class Coin : MonoBehaviour
{
    private float _customReward = 0;
    public float CustomReward { get => _customReward; set => _customReward = value; }

    /// <summary>
    /// If <see cref="Mario"/> collects the coin, it will be destroyed
    /// </summary>
    /// <param name="other">Basically <see cref="Mario"/></param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Destroy(gameObject);
            if (other.TryGetComponent<MLAgent>(out MLAgent _marioAgent))
            {
                _marioAgent.GetReward(0.75f + CustomReward);
            }

        }
    }
}
