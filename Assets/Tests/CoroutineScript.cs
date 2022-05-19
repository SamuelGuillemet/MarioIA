using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineScript
{
    static public IEnumerator PressKeys(int dir, int run, int jump, Mario MarioPlayer)
    {
        float timePassed = 0;
        while (timePassed < 1f)
        {
            MarioPlayer.MLMoveMario(dir, run, jump);
            timePassed += Time.deltaTime;
            yield return null;
        }
    }
}