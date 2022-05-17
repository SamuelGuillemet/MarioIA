using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class allow the camera to follow Mario position and fix an the aspect of view
/// </summary>
public class MainCamera : MonoBehaviour
{
    private Transform _playerTransform;
    public Transform PlayerTransform { set => _playerTransform = value; }

    /// <summary>
    /// The target aspect of the camera view on the screen
    /// </summary>
    public Vector2 TargetAspects = new Vector2(16f, 15f);
    private float _yPos;

    // Start is called before the first frame update
    void Start()
    {
        _yPos = _playerTransform.localPosition.y + 5;
        float targetaspect = TargetAspects.x / TargetAspects.y;
        float windowaspect = (float)Screen.width / (float)Screen.height;
        float scaleheight = windowaspect / targetaspect;

        Camera camera = GetComponent<Camera>();

        if (scaleheight < 1.0f)
        {
            Rect rect = camera.rect;
            rect.width = 1.0f;
            rect.height = scaleheight;
            rect.x = 0;
            rect.y = (1.0f - scaleheight) / 2.0f;
            camera.rect = rect;

        }
        else
        {
            float scalewidth = 1.0f / scaleheight;
            Rect rect = camera.rect;
            rect.width = scalewidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scalewidth) / 2.0f;
            rect.y = 0;
            camera.rect = rect;
        }

        transform.localPosition = new Vector3(_playerTransform.localPosition.x, _yPos, transform.localPosition.z);
    }

    void Update()
    {
        if (_playerTransform != null)
        {
            float playerPos = _playerTransform.localPosition.x;
            float camPos = transform.localPosition.x;
            if (playerPos > camPos - 3.5f)
            {
                transform.localPosition = new Vector3(playerPos + 3.5f, _yPos, transform.localPosition.z);
            }
            else if (playerPos < camPos - 5)
            {
                transform.localPosition = new Vector3(playerPos + 5, _yPos, transform.localPosition.z);
            }
        }

    }
}
