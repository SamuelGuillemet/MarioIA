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

    private MainCamera _camera;
    /// <summary>
    /// Reference the Camera of the level
    /// </summary>
    public MainCamera Camera { get => _camera; set => _camera = value; }
    private CameraSensorComponent _sensor;
    public CameraSensorComponent Sensor { get => _sensor; set => _sensor = value; }

    private MLAgent _marioAgent;
    public MLAgent MarioAgent { get => _marioAgent; set => _marioAgent = value; }

    private GameObject _environmentPrefab;
    private string[] _names = { "Questionmarks", "Enemies", "Coins" };
    private List<GameObject> _objectsToInstantiate;
    private GameObject[] _objectsInTheScene;

    // Start is called before the first frame update
    void OnEnable()
    {
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
        MarioPlayer = GetComponentInChildren<Mario>();
        Camera = GetComponentInChildren<MainCamera>();
        MarioAgent = GetComponentInChildren<MLAgent>();
        Sensor = GetComponentInChildren<CameraSensorComponent>();

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
        InitVariables();
    }
}
