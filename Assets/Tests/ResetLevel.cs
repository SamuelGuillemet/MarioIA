using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class ResetLevel
{
    Environment _currentEnvironment;


    [SetUp]
    public void Setup()
    {
        GameObject _gameObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Tests/UnitTesting2"));
        _currentEnvironment = _gameObject.GetComponentInChildren<Environment>();
        _currentEnvironment.EnvironmentPrefab = Resources.Load<GameObject>("Tests/UnitTesting2");
        _currentEnvironment.OnEnable();
    }

    [TearDown]
    public void TearDown()
    {
        Object.Destroy(_currentEnvironment.gameObject);
    }


    [UnityTest]
    public IEnumerator ResetMarioPos()
    {
        float _marioInitPosition = _currentEnvironment.MarioPlayer.transform.localPosition.x;
        yield return CoroutineScript.PressKeys(-1, 0, 0, _currentEnvironment.MarioPlayer);
        _currentEnvironment.Reset();
        yield return new WaitForSeconds(0.0167f);
        float diff1 = Mathf.Abs(_marioInitPosition - _currentEnvironment.MarioPlayer.transform.localPosition.x);
        Assert.LessOrEqual(diff1, 0.1f);
    }

    [UnityTest]
    public IEnumerator ResetElement()
    {
        Vector3 _enemiesPosition = _currentEnvironment.transform.Find("Enemies").localPosition;
        float _enemiesID = _currentEnvironment.transform.Find("Enemies").GetInstanceID();
        _currentEnvironment.Reset();
        yield return new WaitForSeconds(0.0167f);
        Assert.AreEqual(_enemiesPosition, _currentEnvironment.transform.Find("Enemies").localPosition);
        Assert.AreNotEqual(_enemiesID, _currentEnvironment.transform.Find("Enemies").GetInstanceID());
    }
}
