using System.Collections;
using UnityEngine;

/// <summary>
/// This class handle the behaviour of the pipe, with teleporation of <see cref="Mario"/> and <see cref="Plante"/> growing
/// </summary>
public class Pipe : MonoBehaviour
{

    [Header("Mario sinking")]
    [Tooltip("To create the animation of mario sinking into the pipe")]
    /// <summary>
    /// To create the animation of <see cref="Mario"/> sinking into the pipe
    /// </summary>
    public AnimationCurve MarioCurve;

    [Tooltip("The pipe of destinantion ; could be Null")]
    /// <summary>
    /// The destinantion pipe
    /// </summary>
    public GameObject Destination;
    private Vector2 _dest;                  //Field linked to the destination of the pipe when mario goes in

    //Plante stuff
    [Header("Plante stuff")]
    [Tooltip("To create the animation of the plante sinking into the pipe")]
    /// <summary>
    /// To create the animation of the <see cref="Plante"/> sinking into the pipe
    /// </summary>
    public AnimationCurve PlanteCurve;

    [Tooltip("The plante GameObject that will be spawned ; could be Null")]
    /// <summary>
    /// The <see cref="Plante"/> GameObject that will be spawned
    /// </summary>
    public GameObject PlanteGameObject;
    private GameObject _newPlante;          // The plante associated to the pipe

    /// <summary>
    /// Random start pos to avoid multiple same exact behaviour
    /// </summary>
    private float _addy;

    /// <summary>
    /// Random start time to avoid multiple same exact behaviour
    /// </summary>     
    private float _randomAddTime;


    private void Start()
    {
        if (Destination)
            _dest = Destination.transform.parent.localPosition + Destination.transform.localPosition + new Vector3(0.5f, 1.015f, 0);
        else if (PlanteGameObject)
            CreatePlante();
    }

    /// <summary>
    /// Function called at start to create the <see cref="Plante"/>
    /// </summary>
    public void CreatePlante()
    {
        _newPlante = Instantiate(PlanteGameObject, transform.localPosition + new Vector3(0, 0.5f) + transform.parent.position, Quaternion.identity, transform.parent);
        _addy = _newPlante.transform.localPosition.y - 0.5f;
        _randomAddTime = Random.Range(0f, 5f);
    }

    /// <summary>
    /// Run on evry frame to move the <see cref="Plante"/> according to <see cref="PlanteCurve"/>
    /// </summary>
    private void FixedUpdate()
    {
        if (!Destination && PlanteGameObject)
        {
            double time = Time.realtimeSinceStartupAsDouble + _randomAddTime;
            float posPlante = PlanteCurve.Evaluate((float)time) * 2 + _addy;
            _newPlante.transform.localPosition = new Vector3(_newPlante.transform.localPosition.x, posPlante, 0);
        }

    }

    /// <summary>
    /// Handle the detection of <see cref="Mario"/> crouching on top of a pipe
    /// </summary>
    /// <param name="coll">Will be <see cref="Mario"/> most of the time</param>
    void OnCollisionStay2D(Collision2D coll)
    {
        GameObject mario = coll.gameObject;
        if (mario.tag == "Player" && coll.GetContact(0).normal == Vector2.down && Mathf.Abs(mario.transform.localPosition.x - 0.5f - gameObject.transform.parent.localPosition.x) < 0.4f)
            if (mario.GetComponent<Mario>().Crouch && Destination)
                StartCoroutine("PipeTeleportation", mario);
    }

    /// <summary>
    /// Move mario to the next pipe with the animation based on <see cref="MarioCurve"/>
    /// </summary>
    /// <param name="mario"></param>
    IEnumerator PipeTeleportation(GameObject mario)
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
