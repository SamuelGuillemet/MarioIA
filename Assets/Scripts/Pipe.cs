using System.Collections;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    //Mario teleporation Stuff
    [Header("Mario sinking")]
    [Tooltip("To create the animation of mario sinking into the pipe")]
    public AnimationCurve MarioCurve;       //To create the animation of mario sinking into the pipe
    [Tooltip("The pipe of destinantion ; could be Null")]
    public GameObject Destination;          //The destinantion pipe
    private Vector2 _dest;                  //Field linked to the destination of the pipe when mario goes in
    private Transform _marioTransform;      //The position of mario

    //Plante stuff
    [Header("Plante stuff")]
    [Tooltip("To create the animation of the plante sinking into the pipe")]
    public AnimationCurve PlanteCurve;      //To create the animation of the plante sinking into the pipe
    [Tooltip("The plante GameObject that will be spawned ; could be Null")]
    public GameObject Plante;               //The plante GameObject that will be spawned
    private GameObject _newPlante;          // The plante associated to the pipe
    private float _addy;                    //Random start pos
    private float _randomAddTime;           //Random strat time


    /// <summary>
    /// We create the plante at start
    /// </summary>
    private void Start()
    {
        if (Destination)
            _dest = Destination.transform.parent.localPosition + Destination.transform.localPosition + new Vector3(0.5f, 1.015f, 0);
        else if (Plante)
            createPlante();
        _marioTransform = transform.parent.parent.Find("BabyMario"); //TODO Get the position of mario by the main camera
    }

    /// <summary>
    /// Function called at start to create the plante
    /// </summary>
    public void createPlante()
    {
        _newPlante = Instantiate(Plante, transform.localPosition + new Vector3(0, 0.5f) + transform.parent.position, Quaternion.identity, transform.parent);
        _addy = _newPlante.transform.localPosition.y - 0.5f;
        _randomAddTime = Random.Range(0f, 5f);
    }

    /// <summary>
    /// Run on evry frame to move the plante according to its Curve
    /// </summary>
    private void FixedUpdate()
    {
        if (!Destination && Plante)
        {
            double time = Time.realtimeSinceStartupAsDouble + _randomAddTime;
            float posPlante = PlanteCurve.Evaluate((float)time) * 2 + _addy;
            if (Mathf.Abs(_marioTransform.transform.localPosition.x - 0.5f - gameObject.transform.parent.localPosition.x) > 3f || _newPlante.transform.localPosition.y != 0)
                _newPlante.transform.localPosition = new Vector3(_newPlante.transform.localPosition.x, posPlante, 0);
        }

    }

    /// <summary>
    /// Handle the detection of mario crouching on top of a pipe
    /// </summary>
    /// <param name="coll"></param>
    void OnCollisionStay2D(Collision2D coll)
    {
        GameObject mario = coll.gameObject;
        if (mario.name == "BabyMario" && coll.GetContact(0).normal == Vector2.down && Mathf.Abs(mario.transform.localPosition.x - 0.5f - gameObject.transform.parent.localPosition.x) < 0.4f)
            if (mario.GetComponent<Mario>().Crouch && Destination)
                StartCoroutine("pipe", mario);
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
}
