using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class taht handle the launch <see cref="BillBullet"/>
/// </summary>
public class Bill : MonoBehaviour
{
    /// <summary>
    /// The GameObjet BulletBill that will be spawned
    /// </summary>
    public GameObject BulletBill;

    /// <summary>
    /// All the BulletBills in the scene, max is 4
    /// </summary>
    private GameObject[] _bulletBillsInTheScene;

    /// <summary>
    /// The index of the next BulletBill to be spawn in the array <see cref="_bulletBillsInTheScene"/>
    /// </summary>
    private int _indexOfNextSpawn;

    /// <summary>
    /// Direction of the next <see cref="BillBullet"/>, could be 1 or -1
    /// </summary>
    private int _dir;

    /// <summary>
    /// This bool prevent multiple launch
    /// </summary>
    private bool _launch = false;

    /// <summary>
    /// This Animation curve handle the behaviour of the bill
    /// </summary>
    public AnimationCurve Behaviour;

    /// <summary>
    /// This is a random time adds for spawning <see cref="BillBullet"/>, to prevent multiple <see cref="Bill"/> to act at the exact same time
    /// </summary>
    private float _temp;

    private void Start()
    {
        _bulletBillsInTheScene = new GameObject[4];
        _indexOfNextSpawn = 0;
        _dir = -1;
        _temp = Random.Range(0f, 1f);
    }

    private void FixedUpdate()
    {
        float step = Behaviour.Evaluate(((float)Time.fixedTimeAsDouble) + _temp);

        if (step > 0.99 && !_launch)
        {
            _launch = true;
            StartCoroutine("Throw");
        }
    }

    /// <summary>
    /// The function that delete the <see cref="BillBullet"/> in the scene and in the array <see cref="_bulletBillsInTheScene"/> 
    /// </summary>
    /// <param name="billBulletIndex"></param>
    public void DeleteBillBullet(int billBulletIndex)
    {
        Destroy(_bulletBillsInTheScene[billBulletIndex]);
        _bulletBillsInTheScene[billBulletIndex] = null;

    }

    /// <summary>
    /// The function that will be called when we want to launch the <see cref="BillBullet"/>, called by the time of the <see cref="Behaviour"/>
    /// </summary>
    IEnumerator Throw()
    {
        yield return new WaitForSeconds(0.15f);
        if (_bulletBillsInTheScene[_indexOfNextSpawn] != null)
            Destroy(_bulletBillsInTheScene[_indexOfNextSpawn]);
        _bulletBillsInTheScene[_indexOfNextSpawn] = Instantiate(BulletBill, transform.position + Vector3.right * _dir, Quaternion.identity, transform.parent);
        _bulletBillsInTheScene[_indexOfNextSpawn].GetComponent<BillBullet>().Index = _indexOfNextSpawn;
        _bulletBillsInTheScene[_indexOfNextSpawn].GetComponent<BillBullet>().SetBill = this;
        _bulletBillsInTheScene[_indexOfNextSpawn].GetComponent<BillBullet>().Dir = _dir;
        _indexOfNextSpawn = (_indexOfNextSpawn + 1) % _bulletBillsInTheScene.Length;
        _dir = -_dir;
        _launch = false;
    }
}
