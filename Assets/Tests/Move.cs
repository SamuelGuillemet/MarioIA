using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class Move
{
    Environment _currentEnvironment;


    [SetUp]
    public void Setup()
    {
        GameObject _gameObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Tests/UnitTesting1"));
        _currentEnvironment = _gameObject.GetComponentInChildren<Environment>();
    }

    [TearDown]
    public void TearDown()
    {
        Object.Destroy(_currentEnvironment.gameObject);
    }


    [UnityTest]
    public IEnumerator MarioMoveLeft()
    {
        float _marioInitPosition = _currentEnvironment.MarioPlayer.transform.localPosition.x;
        yield return CoroutineScript.PressKeys(-1, 0, 0, _currentEnvironment.MarioPlayer);

        Assert.Less(_currentEnvironment.MarioPlayer.transform.localPosition.x, _marioInitPosition);
    }

    [UnityTest]
    public IEnumerator MarioMoveRight()
    {
        float _marioInitPosition = _currentEnvironment.MarioPlayer.transform.localPosition.x;
        yield return CoroutineScript.PressKeys(1, 0, 0, _currentEnvironment.MarioPlayer);

        Assert.Greater(_currentEnvironment.MarioPlayer.transform.localPosition.x, _marioInitPosition);
    }

    [UnityTest]
    public IEnumerator MarioJump()
    {
        float _marioInitPosition = _currentEnvironment.MarioPlayer.transform.localPosition.y;
        yield return CoroutineScript.PressKeys(0, 0, 1, _currentEnvironment.MarioPlayer);

        Assert.Greater(_currentEnvironment.MarioPlayer.transform.localPosition.y, _marioInitPosition);
    }

    [UnityTest]
    public IEnumerator MarioRun()
    {
        yield return CoroutineScript.PressKeys(1, 1, 0, _currentEnvironment.MarioPlayer);

        Assert.True(_currentEnvironment.MarioPlayer.CurrentVelocityX == Mario.VelocityX.course);
    }
}