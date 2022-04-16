using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class allow the camera to follow Mario position and fix an the aspect of view
/// </summary>
public class MainCamera : MonoBehaviour
{
    //TODO Fix with global environment
    [SerializeField] private Transform _playerTransform;

    /// <summary>
    /// The target ascpect of the camera view on the screen
    /// </summary>
    public Vector2 targetAspects = new Vector2(16f, 15f);
    private float _yPos;

    // Start is called before the first frame update
    void Start()
    {
        _yPos = _playerTransform.localPosition.y + 5;
        float targetaspect = targetAspects.x / targetAspects.y;
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
    }

    void Update()
    {
        if (_playerTransform != null)
        {
            float playerPos = _playerTransform.localPosition.x;
            float camPos = transform.localPosition.x;
            if (playerPos > camPos)
            {
                transform.localPosition = new Vector3(playerPos, _yPos, transform.localPosition.z);
            }
            else if (playerPos < camPos - 5)
            {
                transform.localPosition = new Vector3(playerPos + 5, _yPos, transform.localPosition.z);
            }
        }

    }
}
