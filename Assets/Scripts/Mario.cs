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

    public enum VelocityX
    {
        lowSpeed,
        marche,
        course
    }
    private VelocityX _currentVelocityX;                     //Velocité du joueur
    private VelocityX _jumpVelocityX;                        //Vélocité au départ du saut

    enum BodyDirection
    {
        gauche = -1,
        arret = 0,
        droite = 1,
    }
    private BodyDirection _currentBodyDirection;        //Direction voulue par le joueur

    //Composants associés au joueur
    private Rigidbody2D _rb;
    private Collider2D _col;
    private Animator _an;
    private Transform m_GroundCheck1, m_GroundCheck2;

    //Variable pour son déplacement
    private float _speedY;          //Vitesse verticale
    private float _absSpeedX;       //Vitesse absolue horizontale

    private bool _jump;
    private bool _crouch;
    private bool _grounded;

    private int _inputDirection;         //Direction selon l'input du joueur
    private float gravity;          //Current Gravity
    private float acceleration;     //Current acceleration

    //Données pour la physqiue custom
    float maxFallSpeed = -16.2f;
    float arretSpeed = 0.4f; //Vitesse sous laquelle le personnage s'arrête
    float marcheSpeed = 5.85f; //Vitesse max de marche
    float courseSpeed = 9.61f; //Vitesse max de course
    float lowSpeed = 3.5f; //Vitesse max de petite vitesse 

    float[] AccelerationX = { 0, 8.34f, 12.52f, 10f }; // Marche, course, back
    float[] SlidingDecceleration = { -11.42f, 22.85f }; // Release button, backward

    float[] InitialJumpVelocity = { 17f, 17f, 20f, 15f, 26.25f }; //Arret, marche, course, bounce 
    float[] HoldingJumpGravity = { 28.125f, 26.36f, 35.1f }; // Arret, marche, course
    float[] FallingGravity = { 88f, 74f, 100f }; // Arret, marche, course

    public float SpeedJumpOnEnemy { get => InitialJumpVelocity[3]; }


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
        _grounded = Physics2D.OverlapPoint(m_GroundCheck1.position, LayerMask.GetMask("Default")) || Physics2D.OverlapPoint(m_GroundCheck2.position, LayerMask.GetMask("Default"));

        SetConstant();
        MoveMario();

        _rb.gravityScale = AdjustGravity() / 9.81f;

        _rb.velocity = new Vector2(5f * _inputDirection, _rb.velocity.y); //TODO Fix the accelration value
        LimitSpeed();

        _an.SetFloat("Speed", Mathf.Abs(_absSpeedX));
        _an.SetBool("Crouch", _crouch);
        _an.SetBool("Jumping", _jump);
    }

    /// <summary>
    /// The function that handle the deplacement of Mario
    /// </summary>
    private void MoveMario()
    {
        _jump = Input.GetKey(KeyCode.UpArrow) && _speedY >= 0;

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)) //Marche
        {
            _currentInput = input.marche;
            if (Input.GetKey(KeyCode.LeftArrow)) //Direction
                _inputDirection = -1;
            else
                _inputDirection = 1;
            transform.localScale = new Vector2(Mathf.Sign(_inputDirection), transform.localScale.y);
        }
        else
        {
            _currentInput = input.arret;
            _inputDirection = 0;
        }

        if (Input.GetKey(KeyCode.Space) && _currentInput == input.marche) //Run
            _currentInput = input.course;

        if (Input.GetKey(KeyCode.UpArrow) && _grounded) //Jump
        {
            _jumpVelocityX = _currentVelocityX;
            _rb.velocity = new Vector2(_rb.velocity.x, 15); //TODO Fix the speed of jumping
        }

        if (Input.GetKey(KeyCode.DownArrow)) //Crouch
            _crouch = true;
        else
            _crouch = false;
    }

    /// <summary>
    /// The function that handle the different constant
    /// </summary>
    private void SetConstant()
    {
        if (_rb.velocity.x > 0)
            _currentBodyDirection = BodyDirection.droite;
        if (_rb.velocity.x == 0)
            _currentBodyDirection = BodyDirection.arret;
        if (_rb.velocity.x < 0)
            _currentBodyDirection = BodyDirection.gauche;

        if (_absSpeedX < lowSpeed)
            _currentVelocityX = VelocityX.lowSpeed;
        if (_absSpeedX > lowSpeed && _absSpeedX < marcheSpeed)
            _currentVelocityX = VelocityX.marche;
        if (_absSpeedX > marcheSpeed)
            _currentVelocityX = VelocityX.course;

        _speedY = _rb.velocity.y;
        _absSpeedX = Mathf.Abs(_rb.velocity.x);
    }

    /// <summary>
    /// Function that handle the maximum of speed based on <data> _currentInput </data>
    /// </summary>
    private void LimitSpeed()
    {
        if (_absSpeedX > courseSpeed)
            _rb.velocity = new Vector2(courseSpeed * ((int)_currentBodyDirection), _rb.velocity.y);   //Vitesse de course max
        if (_currentInput == input.marche && _absSpeedX > marcheSpeed)
            _rb.velocity = new Vector2(marcheSpeed * ((int)_currentBodyDirection), _rb.velocity.y);   //Vitesse de marche max
        if (_speedY < maxFallSpeed)
            _rb.velocity = new Vector2(_rb.velocity.x, maxFallSpeed + 0.4f);                          //Vitesse de chute max
    }

    /// <summary>
    /// The function that handle the custom gravity
    /// </summary>
    /// <returns> Returns the value of gravity which has to be divided by 9.81f </returns>
    private float AdjustGravity()
    {
        if (_grounded)
        {
            return 9.81f;                   //Au sol
        }
        _an.SetBool("Jumping", false);
        if (_jump)
        {
            _an.SetBool("Jumping", true);
            return HoldingJumpGravity[((int)_jumpVelocityX)];   //En l'air avec le jump de tenu (dépend de la vitesse initiale du saut)

        }
        return FallingGravity[((int)_jumpVelocityX)];           //En l'air avec le jump non tenu (dépend de la vitesse initiale du saut)
    }
}

