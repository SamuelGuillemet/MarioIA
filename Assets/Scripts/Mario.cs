using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mario : MonoBehaviour
{
    enum input
    {
        arret,
        marche,
        course
    }
    private input _currentInput;

    //Composants associés au joueur
    private Rigidbody2D _rb;
    private Collider2D _col;
    private Animator _an;
    private Transform m_GroundCheck1, m_GroundCheck2;

    //Variable pour son déplacement
    private float _speedY;          //Vitesse verticale
    private float _absSpeedX;       //Vitesse absolue horizontale
    private float _jumpVelocityX;   //Vitesse au départ du saut

    private bool _jump;
    private bool _crouch;
    private bool _grounded;

    private int _direction;


    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _col = GetComponent<Collider2D>();
        _an = GetComponent<Animator>();
        m_GroundCheck1 = transform.Find("groundCheck1");
        m_GroundCheck2 = transform.Find("groundCheck2");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _speedY = _rb.velocity.y;
        _absSpeedX = Mathf.Abs(_rb.velocity.x);
        _grounded = Physics2D.OverlapPoint(m_GroundCheck1.position, LayerMask.GetMask("Default")) || Physics2D.OverlapPoint(m_GroundCheck2.position, LayerMask.GetMask("Default"));

        MoveMario();

        _rb.velocity = new Vector2(10f * _direction, _rb.velocity.y); //TODO Fix the accelration value

        _an.SetFloat("Speed", Mathf.Abs(_absSpeedX));
        _an.SetBool("Crouch", _crouch);
        _an.SetBool("Jumping", _jump);
    }

    private void MoveMario()
    {
        _jump = Input.GetKey(KeyCode.UpArrow) && _speedY >= 0;

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)) //Marche
        {
            _currentInput = input.marche;
            if (Input.GetKey(KeyCode.LeftArrow)) //Direction
                _direction = -1;
            else
                _direction = 1;
            transform.localScale = new Vector2(Mathf.Sign(_direction), transform.localScale.y);
        }
        else
        {
            _currentInput = input.arret;
            _direction = 0;
        }

        if (Input.GetKey(KeyCode.Space) && _currentInput == input.marche) //Run
            _currentInput = input.course;

        if (Input.GetKey(KeyCode.UpArrow) && _grounded) //Jump
        {
            _jumpVelocityX = _rb.velocity.x;
            _rb.velocity = new Vector2(_rb.velocity.x, 15); //TODO Fix the speed of jumping
        }

        if (Input.GetKey(KeyCode.DownArrow)) //Crouch
            _crouch = true;
        else
            _crouch = false;
    }
}
