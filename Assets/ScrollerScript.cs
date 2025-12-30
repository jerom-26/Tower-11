using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ScrollerScript : MonoBehaviour
{
    [Header("Speed")]
    public float baseSpeed = 19f;
    public float multiplier = 0.05f;

    [Header("BG Img")]
    public Transform sky;
    public Transform skyDuplicate;

    [Header("Loop Settings")]
    public float buffer = 0.1f;

    private float tileWidth;
    private Camera cam;

    void Awake()
    {
        cam = Camera.main;

        if (sky == null || skyDuplicate == null)
        {
            enabled = false;
            return;
        }

        var sr = sky.GetComponent<SpriteRenderer>();
        if (sr == null)
        {
            enabled = false;
            return;
        }

        tileWidth = sr.bounds.size.x;
        if (tileWidth <= 0.0001f)
        {
            enabled = false;
        }
    }

    void Update()
    {
        float speed = baseSpeed * multiplier;
        Vector3 delta = Vector3.left * speed * Time.deltaTime;

        sky.position += delta;
        skyDuplicate.position += delta;

        float camLeftX = GetCameraLeftX();
        if (GetTileRightEdge(sky) < camLeftX - buffer)
        {
            sky.position = new Vector3(skyDuplicate.position.x + tileWidth, sky.position.y, sky.position.z);
        }

        if (GetTileRightEdge(skyDuplicate) < camLeftX - buffer)
        {
            skyDuplicate.position = new Vector3(sky.position.x + tileWidth, skyDuplicate.position.y, skyDuplicate.position.z);
        }
    }

    float GetCameraLeftX()
    {
        if (cam == null) cam = Camera.main;
        return cam.transform.position.x - (cam.orthographicSize * cam.aspect);
    }

    float GetTileRightEdge(Transform t)
    {
        return t.position.x + tileWidth * 0.5f;
    }
}
