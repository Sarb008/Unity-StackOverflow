using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float smoothSpeed = 3f;
    [SerializeField] private float gameOverZoom = 5f;

    private float targetY;
    public float CurrentY => transform.position.y;

    private void Start()
    {
        targetY = transform.position.y;
    }

    public void SetTargetHeight(float height)
    {
        targetY = height + 3f;
    }

    public void ShowGameOverView()
    {
        Camera cam = GetComponent<Camera>();

        if (cam == null)
            return;

        GameManager gameManager =
            GameManager.Instance;

        if (gameManager == null)
            return;

        float towerHeight =
            gameManager.CurrentHeight;

        float margin = 5f;

        cam.orthographicSize =
            (towerHeight / 2f) + margin;

        // Move camera down from the normal "top of tower" position
        targetY -= (towerHeight / 2f) + 3f;
    }

    private void LateUpdate()
    {
        Vector3 position = transform.position;

        position.y = Mathf.Lerp(
            position.y,
            targetY,
            smoothSpeed * Time.deltaTime
        );

        transform.position = position;
    }

    public bool HasReachedTarget()
    {
        return Mathf.Abs(
            transform.position.y - targetY
        ) < 0.05f;
    }
}