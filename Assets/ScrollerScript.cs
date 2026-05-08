using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ScrollerScript : MonoBehaviour
{
    [Header("Speed")]
    public float baseSpeed = 19f;
    public float multiplier = 0.05f;

    [Header("Difficulty Scaling")]
    public bool useDifficultyScaling = true;
    public float maxSpeedMultiplier = 1.25f;
    public int startScalingScore = 0;

    [Header("BG Img")]
    public Transform sky;
    public Transform skyDuplicate;

    [Header("Loop Settings")]
    public float buffer = 0.1f;

    private float tileWidth;
    private Camera cam;
    private gameLogicScript gameLogic;


    void Awake()
    {
        cam = Camera.main;

        GameObject logicObj = GameObject.FindGameObjectWithTag("Logic");
        if (logicObj != null)
            gameLogic = logicObj.GetComponent<gameLogicScript>();

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
            return;
        }
    }

    void Update()
    {

        float finalSpeed = baseSpeed;

        // score-based extra speed
        if (gameLogic != null)
            finalSpeed += multiplier * gameLogic.playerScore;

        // Difficulty-based scaling
        if (useDifficultyScaling && gameLogic != null && gameLogic.playerScore >= startScalingScore)
        {
            float d = gameLogic.GetDifficulty01(); // 0..1
            float speedScale = Mathf.Lerp(1f, maxSpeedMultiplier, d);
            finalSpeed *= speedScale;
        }

        Vector3 delta = Vector3.left * finalSpeed * Time.deltaTime;

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

    void LoopIfNeeded(Transform a, Transform b)
    {
        if (a.position.x <= b.position.x - tileWidth + buffer)
        {
            a.position = new Vector3(b.position.x + tileWidth - buffer, a.position.y, a.position.z);
        }
    }

}
