using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    private int _inputDirection;        //Direction selon l'input du joueur
    private float _gravity;             //Current Gravity
    private float _acceleration;        //Current acceleration

    //Données pour la physqiue custom
    float maxFallSpeed = -16.2f;    //Vitesse de chute max
    float arretSpeed = 0.4f;        //Vitesse sous laquelle le personnage s'arrête
    float marcheSpeed = 5.85f;      //Vitesse max de marche
    float courseSpeed = 9.61f;      //Vitesse max de course
    float lowSpeed = 3.5f;          //Vitesse max de petite vitesse 

    float[] AccelerationX = { 0, 8.34f, 12.52f, 10f };      // Marche, course, back
    float[] SlidingDecceleration = { -11.42f, 22.85f };     // Release button, backward

    float[] InitialJumpVelocity = { 17f, 17f, 20f, 15f, 26.25f };   //Arret, marche, course, bounce, trampoline 
    float[] HoldingJumpGravity = { 28.125f, 26.36f, 35.1f };        // Arret, marche, course
    float[] FallingGravity = { 88f, 74f, 100f };                    // Arret, marche, course

    public float SpeedJumpOnEnemy { get => InitialJumpVelocity[3]; }
    public bool Crouch { get => _crouch; }

    private bool _onTrampoline = false;

    //Debug mode
    public bool Debug = false;
    private Text _infoDebug;


    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _col = GetComponent<Collider2D>();
        _an = GetComponent<Animator>();
        m_GroundCheck1 = transform.Find("groundCheck1");
        m_GroundCheck2 = transform.Find("groundCheck2");

        //Initialisation des variables
        _currentInput = input.arret;
        _currentBodyDirection = BodyDirection.arret;
        _currentVelocityX = VelocityX.lowSpeed;
        _jumpVelocityX = VelocityX.lowSpeed;

        //Debug mode
        if (Debug)
            _infoDebug = GameObject.Find("Debug").GetComponent<Text>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _grounded = Physics2D.OverlapPoint(m_GroundCheck1.position, LayerMask.GetMask("Blocks")) || Physics2D.OverlapPoint(m_GroundCheck2.position, LayerMask.GetMask("Blocks"));

        SetConstant();
        MoveMario();

        _gravity = AdjustGravity() / 9.81f;
        _rb.gravityScale = _gravity;
        _acceleration = AdjustAcceleration();

        _rb.velocity = _rb.velocity + new Vector2(_acceleration * Time.deltaTime, 0);
        LimitSpeed();

        //Animations
        _an.SetFloat("Speed", Mathf.Abs(_absSpeedX));
        _an.SetBool("Crouch", _crouch);
        _an.SetBool("Jumping", _jump);

        //Debug mode
        if (Debug)
            _infoDebug.text = "acceleration = " + _acceleration + "\nspeedX = " + _rb.velocity.x + "\nspeedY = " + _speedY + "\ngravity = " + _gravity + "\njump = " + _jump + "\ngrounded = " + _grounded + "\naddVitesse = " + _acceleration * Time.deltaTime;

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
            _rb.velocity = new Vector2(_rb.velocity.x, InitialJumpVelocity[((int)_currentVelocityX)]);
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
        if (_absSpeedX > marcheSpeed + 0.3f)
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

    /// <summary>
    /// The function taht handle the custom acceleration
    /// </summary>
    /// <returns> Return the value of the acceleration</returns>
    private float AdjustAcceleration()
    {
        if (_grounded)
        {
            if (_currentInput == input.arret)
            {
                if (_absSpeedX > arretSpeed)
                {
                    return SlidingDecceleration[0] * ((int)_currentBodyDirection);
                }
                else
                {
                    _rb.velocity = new Vector2(0, _rb.velocity.y);
                    return 0;
                }
            }
            else
            {
                if (_inputDirection != ((int)_currentBodyDirection) && _currentBodyDirection != BodyDirection.arret)
                {
                    return SlidingDecceleration[1] * _inputDirection;
                }
                else
                {
                    return AccelerationX[((int)_currentInput)] * _inputDirection;
                }
            }
        }
        else
        {
            if (_currentInput == input.arret)
            {
                return 0;
            }
            else if (_currentVelocityX == VelocityX.course)
            {
                return AccelerationX[2] * _inputDirection;
            }
            else
            {
                if (_inputDirection != ((int)_currentBodyDirection))
                {
                    return AccelerationX[3] * _inputDirection;
                }
                else
                {
                    return AccelerationX[1] * _inputDirection;
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.name.Contains("Trampoline") && !_onTrampoline)
        {
            _onTrampoline = true;
            other.gameObject.GetComponent<Animator>().SetTrigger("activate");
            Invoke("bounceTrampoline", 0.200f);         //Time to end the animation/2
        }
    }

    private void bounceTrampoline()
    {
        _rb.velocity = new Vector2(_rb.velocity.x, InitialJumpVelocity[4]);
        _onTrampoline = false;
    }
}

