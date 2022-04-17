using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script is at the root of evry level, and it handle the global working of the game, like death of <see cref="Mario"/> or the different assignation of variable
/// </summary>
public class Environment : MonoBehaviour
{
    private Mario _mario;
    public Mario MarioPlayer { get => _mario; set => _mario = value; }

    private MainCamera _camera;
    public MainCamera Camera { get => _camera; set => _camera = value; }

    // Start is called before the first frame update
    void Start()
    {
        MarioPlayer = GetComponentInChildren<Mario>();
        Camera = GetComponentInChildren<MainCamera>();

        Camera.PlayerTransform = MarioPlayer.transform;

        foreach (Enemy enemy in GetComponentsInChildren<Enemy>())
        {
            enemy.MainCameraTransform = Camera.transform;
        }

        foreach (HammerBros hammerBros in GetComponentsInChildren<HammerBros>())
        {
            hammerBros.MarioTransform = MarioPlayer.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
