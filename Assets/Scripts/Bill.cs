using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bill : MonoBehaviour
{
    //Stuff for BulletBills that will be spawned
    public GameObject BulletBill;
    private GameObject[] _bulletBillsInTheScene;
    private int _indexOfNextSpawn;
    private int _dir;                       //1 or -1
    private bool launch = false;            //Bool to prevent multiple launch

    public AnimationCurve behaviour;        //Curve that handle the behaviour of the bill
    private float _temp;                    //Random time for spawning

    void Start()
    {
        _bulletBillsInTheScene = new GameObject[4];
        _indexOfNextSpawn = 0;
        _dir = -1;
        _temp = Random.Range(0f, 1f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float step = behaviour.Evaluate(((float)Time.fixedTimeAsDouble) + _temp);

        if (step > 0.99 && !launch)
        {
            launch = true;
            StartCoroutine("Throw");
        }
    }

    /// <summary>
    /// The function taht delete BillBullet in the scene aand in the array <data>_bulletBillsInTheScene</data> 
    /// </summary>
    /// <param name="billBulletIndex"></param>
    public void DeleteBillBullet(int billBulletIndex)
    {
        Destroy(_bulletBillsInTheScene[billBulletIndex]);
        _bulletBillsInTheScene[billBulletIndex] = null;

    }

    /// <summary>
    /// The function that will be called when we want to launch the bill
    /// </summary>
    IEnumerator Throw()
    {
        yield return new WaitForSeconds(0.15f);
        if (_bulletBillsInTheScene[_indexOfNextSpawn] != null)
            Destroy(_bulletBillsInTheScene[_indexOfNextSpawn]);
        _bulletBillsInTheScene[_indexOfNextSpawn] = Instantiate(BulletBill, transform.position + Vector3.right * _dir, Quaternion.identity, transform.parent);
        _bulletBillsInTheScene[_indexOfNextSpawn].GetComponent<BillBullet>().Index = _indexOfNextSpawn;
        _bulletBillsInTheScene[_indexOfNextSpawn].GetComponent<BillBullet>().Bill = this;
        _bulletBillsInTheScene[_indexOfNextSpawn].GetComponent<BillBullet>().Dir = _dir;
        _indexOfNextSpawn = (_indexOfNextSpawn + 1) % _bulletBillsInTheScene.Length;
        _dir = -_dir;
        launch = false;
    }
}
