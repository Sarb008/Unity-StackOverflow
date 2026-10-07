using UnityEngine;

public class Piece : MonoBehaviour
{
    public enum PieceType
    {
        Normal,
        GitRevert,
        GitRestore,
        K8sReplicate,
        PytorchTrain
    }

    [SerializeField] private PieceType pieceType = PieceType.Normal;

    private SpriteRenderer spriteRenderer;
    private PolygonCollider2D polygonCollider;

    public PieceType Type => pieceType;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        polygonCollider = GetComponent<PolygonCollider2D>();
    }

    public void Initialize(Sprite sprite, PieceType type)
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        if (polygonCollider == null)
            polygonCollider = GetComponent<PolygonCollider2D>();

        spriteRenderer.sprite = sprite;
        pieceType = type;

        UpdateCollider();
    }

    private void UpdateCollider()
    {
        if (polygonCollider == null || spriteRenderer.sprite == null)
            return;

        Sprite sprite = spriteRenderer.sprite;

        polygonCollider.pathCount = sprite.GetPhysicsShapeCount();

        for (int i = 0; i < polygonCollider.pathCount; i++)
        {
            var physicsShape = new System.Collections.Generic.List<Vector2>();

            sprite.GetPhysicsShape(i, physicsShape);

            polygonCollider.SetPath(i, physicsShape.ToArray());
        }
    }
}