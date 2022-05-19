using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents.Sensors;

/// <summary>
/// This script is at the root of every level, and it handles the global working of the game, like death of <see cref="Mario"/> or the different assignation of variables
/// </summary>
public class Environment : MonoBehaviour
{
    private Mario _mario;
    /// <summary>
    /// Reference the player of the current level
    /// </summary>
    public Mario MarioPlayer { get => _mario; set => _mario = value; }
    private Vector3 _marioInitPosition;
    public float MarioInitPositionX { get => _marioInitPosition.x; }

    private MainCamera _camera;
    /// <summary>
    /// Reference the Camera of the level
    /// </summary>
    public MainCamera Camera { get => _camera; set => _camera = value; }

    private CameraSensorComponent _sensor;
    /// <summary>
    /// Reference the componment CameraSensor for the <see cref="MLAgent"/>
    /// </summary>
    public CameraSensorComponent Sensor { get => _sensor; set => _sensor = value; }

    private MLAgent _marioAgent;
    /// <summary>
    /// Reference the <see cref="MLAgent"/> if it is in the <see cref="Environment"/>
    /// </summary>
    public MLAgent MarioAgent { get => _marioAgent; set => _marioAgent = value; }

    private GameObject _environmentPrefab;
    public GameObject EnvironmentPrefab { set => _environmentPrefab = value; }
    private string[] _names = { "Questionmarks", "Enemies", "Coins", "Conduits" };
    public List<GameObject> _objectsToInstantiate;
    private GameObject[] _objectsInTheScene;

    /// <summary>
    /// The game object of the <see cref="Checkpoint"/>
    /// </summary>
    public GameObject CheckpointSingle;

    private List<Checkpoint> _checkpointList;
    /// <summary>
    /// All the <see cref="Checkpoint"/> of the <see cref="Environment"/> are in this 
    /// </summary>
    public List<Checkpoint> CheckpointList { get => _checkpointList; }

    /// <summary>
    /// Used to know if <see cref="Mario"/> passed through the correct <see cref="Checkpoint"/>
    /// </summary>
    private int _nextCheckpointIndex;
    public int NextCheckpoitnIndex { set => _nextCheckpointIndex = value; }

    private Transform _flagTransform;
    /// <summary>
    /// Used to set up the maxStep value of <see cref="MLAgent"/>
    /// </summary>
    public Transform FlagTransform { get => _flagTransform; }


    /// <summary>
    /// This script is called at the beginning of the simulation to create the checkpoint system and the reset varaibles
    /// </summary>
    public void OnEnable()
    {
        MarioPlayer = GetComponentInChildren<Mario>();
        MarioAgent = GetComponentInChildren<MLAgent>();
        Sensor = GetComponentInChildren<CameraSensorComponent>();

        if (MarioAgent)
        {
            GameObject _checkpoints = new GameObject("Checkpoints");
            _checkpoints.transform.SetParent(transform);
            _checkpoints.transform.localPosition = Vector3.zero;


            _flagTransform = transform.Find("Flag");

            float pos = MarioPlayer.transform.localPosition.x + 2;
            while (pos < _flagTransform.localPosition.x)
            {
                Instantiate(CheckpointSingle, transform.localPosition + (new Vector3(pos, 7, 0)), Quaternion.identity, _checkpoints.transform);
                pos += 5;
            }

            InitCheckpoints();
        }
        if (!_environmentPrefab)
            _environmentPrefab = Resources.Load("Levels/" + gameObject.name) as GameObject;

        InitVariables();
        _objectsToInstantiate = new List<GameObject>();
        if (_environmentPrefab)
        {
            foreach (string name in _names)
            {
                if (_environmentPrefab.transform.Find(name) != null)
                    _objectsToInstantiate.Add(_environmentPrefab.transform.Find(name).gameObject);
            }
            _objectsInTheScene = new GameObject[_objectsToInstantiate.Count];

            for (int i = 0; i < _objectsInTheScene.Length; i++)
            {
                _objectsInTheScene[i] = transform.Find(_objectsToInstantiate[i].name).gameObject;
            }
        }
    }

    /// <summary>
    /// This function is used to assign the different varaible to scripts which need the position of the camera or of mario 
    /// </summary>
    private void InitVariables()
    {
        MainCamera[] cameras = GetComponentsInChildren<MainCamera>();
        foreach (var cam in cameras)
        {
            cam.PlayerTransform = MarioPlayer.transform;
            if (cam.name == "CameraIA")
                Camera = cam;
        }
        if (!Camera)
            Camera = GetComponentInChildren<MainCamera>();


        _marioInitPosition = MarioPlayer.transform.localPosition;
        MarioPlayer.CurrentEnvironment = this;

        if (Camera)
            Camera.PlayerTransform = MarioPlayer.transform;

        if (Sensor)
            Sensor.Camera = Camera.GetComponent<Camera>();

        if (MarioAgent)
        {
            MarioPlayer.MarioAgent = MarioAgent;
            MarioAgent.CurrentEnvironment = this;
            MarioAgent.CurrentMario = MarioPlayer;
        }

        foreach (Enemy enemy in GetComponentsInChildren<Enemy>())
        {
            enemy.MainCameraTransform = Camera.transform;

        }

        foreach (HammerBros hammerBros in GetComponentsInChildren<HammerBros>())
        {
            hammerBros.MarioTransform = MarioPlayer.transform;
        }

        foreach (Pipe pipe in GetComponentsInChildren<Pipe>())
        {
            GameObject bounty = new GameObject("BountyPipe");
            bounty.transform.SetParent(pipe.transform.parent);
            bounty.transform.localPosition = new Vector3(1, 5, 0);
            bounty.AddComponent<Coin>();
            bounty.GetComponent<Coin>().CustomReward = 2.25f;
            bounty.AddComponent<SpriteRenderer>();
            bounty.GetComponent<SpriteRenderer>().sprite = CheckpointSingle.GetComponent<SpriteRenderer>().sprite;
            bounty.GetComponent<SpriteRenderer>().color = new Color(0.82f, 0.82f, 0.31f, 0.45f);
            bounty.GetComponent<SpriteRenderer>().drawMode = SpriteDrawMode.Tiled;
            bounty.GetComponent<SpriteRenderer>().size = new Vector2(0.25f, 14f);
            bounty.AddComponent<BoxCollider2D>();
            bounty.GetComponent<BoxCollider2D>().size = new Vector2(0.25f, 14f);
            bounty.GetComponent<BoxCollider2D>().isTrigger = true;

        }
    }

    /// <summary>
    /// This function handle the reset of the level when <see cref="Mario"/> hits an enemy or the flag
    /// </summary>
    public void Reset()
    {
        for (int i = 0; i < _objectsToInstantiate.Count; i++)
        {
            Destroy(_objectsInTheScene[i]);
            _objectsInTheScene[i] = Instantiate(_objectsToInstantiate.ToArray()[i], transform.position, Quaternion.identity, transform);
            _objectsInTheScene[i].name = _objectsToInstantiate.ToArray()[i].name;
        }
        MarioPlayer.transform.localPosition = _marioInitPosition;
        MarioPlayer.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        InitVariables();
        _nextCheckpointIndex = 0;
    }

    /// <summary>
    /// This function is called when the IA enter the checkpoint to give it some points
    /// </summary>
    /// <param name="checkpoint"></param>
    public void PlayerThroughCheckpoint(Checkpoint checkpoint)
    {
        if (_checkpointList.IndexOf(checkpoint) == _nextCheckpointIndex)
        {
            if (_nextCheckpointIndex % 10 == 0 && _nextCheckpointIndex != 0)
                _marioAgent.GetReward(5f);
            else
                _marioAgent.GetReward(1f);
            _nextCheckpointIndex++;
        }
        else
        {
            _marioAgent.GetReward(-0.75f);
        }
    }

    /// <summary>
    /// Called at start to init the system of the checkpoints
    /// </summary>
    private void InitCheckpoints()
    {
        Transform checkpointsTransform = transform.Find("Checkpoints");
        if (checkpointsTransform != null)
        {
            _checkpointList = new List<Checkpoint>();
            _nextCheckpointIndex = 0;
            foreach (Transform checkpointSingleTransform in checkpointsTransform)
            {
                Checkpoint checkpoint = checkpointSingleTransform.GetComponent<Checkpoint>();
                checkpoint.SetEnvironment(this);
                _checkpointList.Add(checkpoint);
            }
        }
    }

}
