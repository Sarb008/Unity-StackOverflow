using UnityEngine;
using UnityEngine.InputSystem;

public class PieceController : MonoBehaviour
{
    [SerializeField] private float fallOutY = -16f;
    [SerializeField] private float cameraFalloutDistance = 12f;
    [SerializeField] private float moveSpeed = 5f;

    private Rigidbody2D rb;
    private bool isActive = true;
    private bool hasBeenDropped = false;
    private bool hasSettled = false;
    public bool IsSettled => hasSettled;

    private PieceSpawner spawner;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;

        float randomAngle = Random.Range(0f, 360f);
        transform.rotation = Quaternion.Euler(0f, 0f, randomAngle);
    }

    private void Update()
    {
        if (transform.position.y < fallOutY)
        {
            HandleFallOut();
            return;
        }

        if (!isActive)
            return;

        Move();

        if (Keyboard.current != null &&
            Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Drop();
        }
    }

    private void HandleFallOut()
    {
        if (hasSettled)
        {
            Destroy(gameObject);
            return;
        }

        if (spawner != null)
        {
            spawner.PieceFellOut();
        }

        Destroy(gameObject);
        enabled = false;
    }

    private void Move()
    {
        float horizontal = 0f;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.aKey.isPressed ||
                Keyboard.current.leftArrowKey.isPressed)
            {
                horizontal = -1f;
            }

            if (Keyboard.current.dKey.isPressed ||
                Keyboard.current.rightArrowKey.isPressed)
            {
                horizontal = 1f;
            }
        }

        Vector2 velocity = rb.linearVelocity;
        velocity.x = horizontal * moveSpeed;
        rb.linearVelocity = velocity;
    }

    private void Drop()
    {
        if (!isActive)
            return;

        isActive = false;
        hasBeenDropped = true;

        rb.gravityScale = 1f;
    }

    private float settleTimer = 0f;
    [SerializeField] private float settleTime = 0.35f;

    private float maximumSettleTimer = 0f;
    [SerializeField] private float maximumSettleTime = 2f;

    private void FixedUpdate()
    {
        if (!hasBeenDropped || hasSettled)
            return;

        maximumSettleTimer += Time.fixedDeltaTime;

        bool isMovingSlowly =
            rb.linearVelocity.magnitude < 0.2f &&
            Mathf.Abs(rb.angularVelocity) < 10f;

        if (isMovingSlowly)
        {
            settleTimer += Time.fixedDeltaTime;

            if (settleTimer >= settleTime)
            {
                Settle();
            }
        }
        else
        {
            settleTimer = 0f;
        }

        if (maximumSettleTimer >= maximumSettleTime &&
            !hasSettled)
        {
            Debug.Log(
                "PieceController: Maximum settle time reached. " +
                "Forcing piece to settle."
            );

            Settle();
        }
    }

    private void Settle()
    {
        if (hasSettled)
            return;

        hasSettled = true;
        enabled = false;
        Debug.Log("HEIGHT CHECK: Piece settled!");

        if (GameManager.Instance != null)
        {
            Collider2D collider = GetComponent<Collider2D>();

            if (collider != null)
            {
                GameManager.Instance.RegisterHeight(collider.bounds.max.y);
            }
        }

        Debug.Log("Piece has settled: " + gameObject.name);

        Piece piece = GetComponent<Piece>();

        if (piece != null && piece.Type != Piece.PieceType.Normal)
        {
            if (IsUpright())
            {
                Debug.Log("Special piece landed in ACTIVE orientation: " + piece.Type);

                if (spawner != null)
                {
                    spawner.ActivateSpecial(piece.Type);
                }
            }
            else
            {
                Debug.Log("Special piece landed in INACTIVE orientation. Power NOT activated.");
            }
        }
        /*
        if (GameManager.Instance != null)
        {
            GameManager.Instance.RecalculateCurrentHeight();
        }*/



        if (spawner != null)
        {
            spawner.PieceSettled();
        }
    }

    public void SetSpawner(PieceSpawner pieceSpawner)
    {
        spawner = pieceSpawner;
    }

    private bool IsUpright()
    {
        float angle = Mathf.Repeat(transform.eulerAngles.z, 360f);

        return angle <= 90f || angle >= 270f;
    }
}