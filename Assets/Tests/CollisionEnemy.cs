using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class CollisionEnemy
{
    Environment _currentEnvironment;


    [SetUp]
    public void Setup()
    {
        GameObject _gameObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Tests/UnitTesting3"));
        _currentEnvironment = _gameObject.GetComponentInChildren<Environment>();
    }

    [TearDown]
    public void TearDown()
    {
        Object.Destroy(_currentEnvironment.gameObject);
    }


    [UnityTest]
    public IEnumerator EnemeyCollisionDifferentDirection()
    {
        Enemy GoombaRight = _currentEnvironment.transform.Find("Enemies/GoombaRight").GetComponent<Enemy>();
        Enemy GoombaLeft = _currentEnvironment.transform.Find("Enemies/GoombaLeft").GetComponent<Enemy>();
        Vector2 GoombaRightDir = GoombaRight.Dir;
        Vector2 GoombaLeftDir = GoombaLeft.Dir;
        yield return new WaitForSeconds(2.5f);
        Assert.AreEqual(GoombaLeftDir, GoombaRight.Dir);
        Assert.AreEqual(GoombaRightDir, GoombaLeft.Dir);
    }

    [UnityTest]
    public IEnumerator EnemeyCollisionSameDirection()
    {
        Enemy GoombaRight = _currentEnvironment.transform.Find("Enemies/GoombaRight").GetComponent<Enemy>();
        Enemy GoombaLeft = _currentEnvironment.transform.Find("Enemies/GoombaLeft").GetComponent<Enemy>();
        GoombaRight.Dir = GoombaLeft.Dir;
        GoombaLeft.Dir = new Vector2(-2f, 0);
        Vector2 GoombaRightDir = GoombaRight.Dir;
        Vector2 GoombaLeftDir = GoombaLeft.Dir;
        yield return new WaitForSeconds(2.5f);
        Assert.AreEqual(GoombaRightDir, GoombaRight.Dir);
        Assert.AreNotEqual(GoombaLeftDir, GoombaLeft.Dir);
    }
}
