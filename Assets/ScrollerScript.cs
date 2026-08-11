using System.Collections;
using UnityEngine;

public class ScrollerScript : MonoBehaviour
{
    [Header("Speed")]
    [SerializeField] private float baseSpeed = 19f;
    [SerializeField] private float multiplier = 0.05f;

    [Header("Difficulty Scaling")]
    [SerializeField] private bool useDifficultyScaling = true;
    [SerializeField] private float maxSpeedMultiplier = 1.25f;
    [SerializeField] private int startScalingScore;

    [Header("Background Images")]
    [SerializeField] private Transform sky;
    [SerializeField] private Transform skyDuplicate;

    [Header("Loop Settings")]
    [SerializeField] private float overlap = 0.01f;

    private float tileWidth;
    private Camera mainCamera;
    private gameLogicScript gameLogic;

    private SpriteRenderer originalRenderer;
    private SpriteRenderer duplicateRenderer;

    private bool initialized;

    private void Awake()
    {
        mainCamera = Camera.main;

        GameObject logicObject =
            GameObject.FindGameObjectWithTag("Logic");

        if (logicObject != null)
        {
            gameLogic =
                logicObject.GetComponent<gameLogicScript>();
        }

        if (sky == null || skyDuplicate == null)
        {
            Debug.LogError(
                $"{name}: Scroller is missing an original or duplicate tile.",
                this
            );

            enabled = false;
            return;
        }

        originalRenderer = sky.GetComponent<SpriteRenderer>();
        duplicateRenderer = skyDuplicate.GetComponent<SpriteRenderer>();

        if (originalRenderer == null ||
            duplicateRenderer == null)
        {
            Debug.LogError(
                $"{name}: Both scrolling tiles need SpriteRenderer components.",
                this
            );

            enabled = false;
            return;
        }

        tileWidth = originalRenderer.bounds.size.x;

        if (tileWidth <= 0.001f)
        {
            Debug.LogError(
                $"{name}: Scroller tile width is invalid.",
                this
            );

            enabled = false;
        }
    }

    private IEnumerator Start()
    {
        if (!enabled)
        {
            yield break;
        }

        // Wait until WebGL/browser sizing has been applied.
        yield return null;
        yield return new WaitForEndOfFrame();

        mainCamera = Camera.main;
        tileWidth = originalRenderer.bounds.size.x;

        AlignTilesToCamera();
        CheckViewportCoverage();

        initialized = true;
    }

    private void AlignTilesToCamera()
    {
        if (mainCamera == null)
        {
            return;
        }

        float cameraLeftEdge = GetCameraLeftX();

        // Place the first tile so it begins slightly before
        // the camera's left edge.
        float firstTileX =
            cameraLeftEdge +
            tileWidth * 0.5f -
            overlap * 0.5f;

        sky.position = new Vector3(
            firstTileX,
            sky.position.y,
            sky.position.z
        );

        // Ensure both copies have identical vertical placement.
        skyDuplicate.position = new Vector3(
            firstTileX + tileWidth - overlap,
            sky.position.y,
            sky.position.z
        );
    }

    private void Update()
    {
        if (!initialized)
        {
            return;
        }

        if (gameLogic == null || !gameLogic.IsPlaying)
        {
            return;
        }

        float finalSpeed =
            baseSpeed + multiplier * gameLogic.playerScore;

        if (useDifficultyScaling &&
            gameLogic.playerScore >= startScalingScore)
        {
            float difficulty =
                gameLogic.GetDifficulty01();

            float speedScale = Mathf.Lerp(
                1f,
                maxSpeedMultiplier,
                difficulty
            );

            finalSpeed *= speedScale;
        }

        Vector3 movement =
            Vector3.left * finalSpeed * Time.deltaTime;

        sky.position += movement;
        skyDuplicate.position += movement;

        float cameraLeftEdge = GetCameraLeftX();

        if (GetTileRightEdge(sky) <= cameraLeftEdge)
        {
            MoveAfter(sky, skyDuplicate);
        }

        if (GetTileRightEdge(skyDuplicate) <= cameraLeftEdge)
        {
            MoveAfter(skyDuplicate, sky);
        }
    }

    private void MoveAfter(
        Transform tileToMove,
        Transform otherTile
    )
    {
        tileToMove.position = new Vector3(
            otherTile.position.x + tileWidth - overlap,
            otherTile.position.y,
            tileToMove.position.z
        );
    }

    private float GetCameraLeftX()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        if (mainCamera == null)
        {
            return 0f;
        }

        return mainCamera.transform.position.x -
               mainCamera.orthographicSize *
               mainCamera.aspect;
    }

    private float GetTileRightEdge(Transform tile)
    {
        return tile.position.x + tileWidth * 0.5f;
    }

    private void CheckViewportCoverage()
    {
        if (mainCamera == null)
        {
            return;
        }

        float cameraWidth =
            mainCamera.orthographicSize *
            mainCamera.aspect *
            2f;

        float combinedTileWidth =
            tileWidth * 2f - overlap;

        if (combinedTileWidth < cameraWidth)
        {
            Debug.LogWarning(
                $"{name}: Two tiles are too narrow. " +
                $"Coverage={combinedTileWidth:F2}, " +
                $"Camera={cameraWidth:F2}. " +
                "Use a wider sprite or three tiles.",
                this
            );
        }
    }
}