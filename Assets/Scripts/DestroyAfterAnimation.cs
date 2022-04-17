using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is called when the animation of death of the <see cref="Goomba"/> and the coin spawned by the questionmark is over to destroy the GameObject
/// </summary>
public class DestroyAfterAnimation : StateMachineBehaviour
{
    /// <summary>
    /// Called when the animation controller enter a state with this behaviour attached to him, destroy the GameObject.
    /// </summary>
    /// <param name="animator"></param>
    /// <param name="stateInfo"></param>
    /// <param name="layerIndex"></param>
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Destroy(animator.gameObject);
    }
}
