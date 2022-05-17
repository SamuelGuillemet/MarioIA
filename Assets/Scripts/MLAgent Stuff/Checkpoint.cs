using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is used to set up a checkpoint where the IA gets points when it goes through
/// </summary>
public class Checkpoint : MonoBehaviour
{
    private Environment _currentEnvironment;

    /// <summary>
    /// Set the <see cref="Environment"/> which <see cref="Checkpoint"/> belong to
    /// </summary>
    /// <param name="currentEnvironment"></param>
    public void SetEnvironment(Environment currentEnvironment)
    {
        this._currentEnvironment = currentEnvironment;
    }

    /// <summary>
    /// This function handle the player enter the checkpoint
    /// </summary>
    public void EnterCheckpoint()
    {
        _currentEnvironment.PlayerThroughCheckpoint(this);
    }
}
