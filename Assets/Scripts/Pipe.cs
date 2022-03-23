using System.Collections;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    public AnimationCurve MarioCurve;
    public AnimationCurve PlanteCurve;

    public GameObject destination;
    public GameObject plante;

    private Vector2 _dest;

    private GameObject _newPlante;
    private float _addy;
    private float _randomAddTime;

    private Transform _marioTransform;

    /// <summary>
    /// We create the plante at start
    /// </summary>
    private void Start()
    {
        if (destination)
            _dest = destination.transform.parent.localPosition + destination.transform.localPosition + new Vector3(0.5f, 1.015f, 0);
        else if (plante)
            createPlante();
        _marioTransform = transform.parent.parent.Find("BabyMario");
    }

    private void FixedUpdate()
    {
        if (!destination && plante)
        {
            double time = Time.realtimeSinceStartupAsDouble + _randomAddTime;
            float posPlante = PlanteCurve.Evaluate((float)time) * 2 + _addy;
            if (Mathf.Abs(_marioTransform.transform.localPosition.x - 0.5f - gameObject.transform.parent.localPosition.x) > 4f)
                _newPlante.transform.localPosition = new Vector3(_newPlante.transform.localPosition.x, posPlante, 0);
        }

    }

    /// <summary>
    /// Move mario to the next pipe
    /// </summary>
    /// <param name="mario"></param>
    /// <returns></returns>
    IEnumerator pipe(GameObject mario)
    {
        Vector2 pos = mario.transform.localPosition;

        for (float t = 0; t < MarioCurve.keys[MarioCurve.length - 1].time; t += Time.deltaTime)
        {
            mario.transform.localPosition = new Vector2(pos.x, pos.y - MarioCurve.Evaluate(t));
            yield return null;
        }
        pos = _dest;
        for (float t = 0; t < MarioCurve.keys[MarioCurve.length - 1].time; t += Time.deltaTime)
        {
            mario.transform.localPosition = new Vector2(pos.x, pos.y + MarioCurve.Evaluate(t));
            yield return null;
        }

    }

    void OnCollisionStay2D(Collision2D coll)
    {
        GameObject mario = coll.gameObject;
        if (mario.name == "BabyMario" && coll.GetContact(0).normal == Vector2.down && Mathf.Abs(mario.transform.localPosition.x - 0.5f - gameObject.transform.parent.localPosition.x) < 0.4f)
        {
            if (mario.GetComponent<Mario>().Crouch && destination)
            {
                StartCoroutine("pipe", mario);
            }
        }

    }

    public void createPlante()
    {
        _newPlante = Instantiate(plante, transform.localPosition + new Vector3(0, 0.5f) + transform.parent.position, Quaternion.identity, transform.parent);
        _addy = _newPlante.transform.localPosition.y - 0.5f;
        _randomAddTime = Random.Range(0f, 5f);
    }
}
