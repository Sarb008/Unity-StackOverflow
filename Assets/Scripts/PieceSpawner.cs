using System.Collections.Generic;
using UnityEngine;

public class PieceSpawner : MonoBehaviour
{
    [SerializeField] private float firstSpawnHeight = 1f;
    [SerializeField] private QueueUI queueUI;
    [SerializeField] private GameObject piecePrefab;
    [SerializeField] private SpecialPopupUI specialPopupUI;

    [Header("Normal Piece Sprites")]
    [SerializeField] private Sprite[] normalSprites;
    [Header("Special Piece Sprites")]
    [SerializeField] private Sprite gitRevertSprite;
    [SerializeField] private Sprite gitRestoreSprite;
    [SerializeField] private Sprite k8sReplicateSprite;
    [SerializeField] private Sprite pytorchTrainSprite;
    [Header("OSPC Sprites")]
    [SerializeField] private Sprite[] oSprites;
    [SerializeField] private Sprite[] sSprites;
    [SerializeField] private Sprite[] pSprites;
    [SerializeField] private Sprite[] cSprites;
    [Header("Other Normal Piece Sprites")]
    [SerializeField] private Sprite[] stairSprites;
    [SerializeField] private Sprite[] slabSprites;
    [SerializeField] private Sprite[] cubeSprites;

    [System.Serializable]
    public class QueuedPiece
    {
        public Sprite sprite;
        public Piece.PieceType type;

        public QueuedPiece(Sprite sprite, Piece.PieceType type)
        {
            this.sprite = sprite;
            this.type = type;
        }
    }

    private List<QueuedPiece> pieceQueue = new List<QueuedPiece>();
    private List<QueuedPiece> placedHistory = new List<QueuedPiece>();

    private GameObject currentPiece;

    /*private void Start()
    {
        CreateQueue();
        UpdateQueueUI();
        SpawnPiece();
    }*/

    private bool gameStarted = false;

    public void StartGame()
    {
        if (gameStarted)
            return;

        gameStarted = true;

        CreateQueue();
        UpdateQueueUI();
        SpawnPiece();
    }

    private void UpdateQueueUI()
    {
        if (queueUI == null)
            return;

        queueUI.UpdateQueue(pieceQueue.ToArray());
        queueUI.UpdateQueueCount(pieceQueue.Count);
    }
    /*
    private void CreateQueue()
    {
        pieceQueue.Clear();

        // Add exactly one of each special piece.
        pieceQueue.Add(new QueuedPiece(
            gitRevertSprite,
            Piece.PieceType.GitRevert
        ));

        pieceQueue.Add(new QueuedPiece(
            gitRestoreSprite,
            Piece.PieceType.GitRestore
        ));

        pieceQueue.Add(new QueuedPiece(
            k8sReplicateSprite,
            Piece.PieceType.K8sReplicate
        ));

        pieceQueue.Add(new QueuedPiece(
            pytorchTrainSprite,
            Piece.PieceType.PytorchTrain
        ));

        // Add 12 normal pieces:
        // 5 Box, 5 Cube, 2 Other (Stair/Slab)

        for (int i = 0; i < 5; i++)
        {
            pieceQueue.Add(new QueuedPiece(
                GetRandomSprite(normalSprites),
                Piece.PieceType.Normal
            ));
        }

        for (int i = 0; i < 5; i++)
        {
            pieceQueue.Add(new QueuedPiece(
                GetRandomSprite(cubeSprites),
                Piece.PieceType.Normal
            ));
        }

        for (int i = 0; i < 2; i++)
        {
            pieceQueue.Add(new QueuedPiece(
                GetRandomOtherSprite(),
                Piece.PieceType.Normal
            ));
        }

        // Shuffle the 16 non-OSPC pieces.
        for (int i = pieceQueue.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);

            QueuedPiece temp = pieceQueue[i];
            pieceQueue[i] = pieceQueue[randomIndex];
            pieceQueue[randomIndex] = temp;
        }

        // Create the guaranteed O-S-P-C sequence.
        List<QueuedPiece> ospcSequence = new List<QueuedPiece>
        {
            new QueuedPiece(
                GetRandomShapeSprite(oSprites, "O"),
                Piece.PieceType.Normal
            ),

            new QueuedPiece(
                GetRandomShapeSprite(sSprites, "S"),
                Piece.PieceType.Normal
            ),

            new QueuedPiece(
                GetRandomShapeSprite(pSprites, "P"),
                Piece.PieceType.Normal
            ),

            new QueuedPiece(
                GetRandomShapeSprite(cSprites, "C"),
                Piece.PieceType.Normal
            )
        };

        // Insert O-S-P-C at a random position.
        int insertIndex = Random.Range(0, pieceQueue.Count + 1);

        pieceQueue.InsertRange(insertIndex, ospcSequence);

        Debug.Log(
            "Queue created with guaranteed consecutive O-S-P-C sequence " +
            "starting at index " + insertIndex
        );
    }
    */

    private void CreateQueue()
    {
        pieceQueue.Clear();

        if (GameManager.Instance == null)
        {
            Debug.LogError("PieceSpawner: GameManager instance not found!");
            return;
        }

        string difficulty =
            GameManager.Instance.SelectedDifficulty;

        int queueNumber = Random.Range(1, 3);

        Debug.Log(
            "Selected difficulty: " +
            difficulty +
            " | Queue: " +
            queueNumber
        );

        if (difficulty == "EASY")
        {
            if (queueNumber == 1)
                CreateEasy1();
            else
                CreateEasy2();
        }
        else if (difficulty == "MEDIUM")
        {
            if (queueNumber == 1)
                CreateMedium1();
            else
                CreateMedium2();
        }
        else if (difficulty == "HARD")
        {
            if (queueNumber == 1)
                CreateHard1();
            else
                CreateHard2();
        }
    }

    private void CreateEasy1()
    {
        pieceQueue.Add(new QueuedPiece(
            GetSpriteByName("box_yellow"),
            Piece.PieceType.Normal
        ));

        pieceQueue.Add(new QueuedPiece(
            gitRestoreSprite,
            Piece.PieceType.GitRestore
        ));

        pieceQueue.Add(new QueuedPiece(
            GetSpriteByName("cube_cyan"),
            Piece.PieceType.Normal
        ));

        pieceQueue.Add(new QueuedPiece(
            GetSpriteByName("box_green"),
            Piece.PieceType.Normal
        ));

        pieceQueue.Add(new QueuedPiece(
            gitRevertSprite,
            Piece.PieceType.GitRevert
        ));

        pieceQueue.Add(new QueuedPiece(
            GetSpriteByName("c_blue"),
            Piece.PieceType.Normal
        ));

        pieceQueue.Add(new QueuedPiece(
            GetSpriteByName("cube_orange"),
            Piece.PieceType.Normal
        ));

        pieceQueue.Add(new QueuedPiece(
            k8sReplicateSprite,
            Piece.PieceType.K8sReplicate
        ));

        pieceQueue.Add(new QueuedPiece(
            GetSpriteByName("p_violet"),
            Piece.PieceType.Normal
        ));

        pieceQueue.Add(new QueuedPiece(
            GetSpriteByName("box_pink"),
            Piece.PieceType.Normal
        ));

        pieceQueue.Add(new QueuedPiece(
            pytorchTrainSprite,
            Piece.PieceType.PytorchTrain
        ));

        pieceQueue.Add(new QueuedPiece(
            GetSpriteByName("s_red"),
            Piece.PieceType.Normal
        ));

        pieceQueue.Add(new QueuedPiece(
            GetSpriteByName("cube_green"),
            Piece.PieceType.Normal
        ));

        pieceQueue.Add(new QueuedPiece(
            GetSpriteByName("o_pink"),
            Piece.PieceType.Normal
        ));

        pieceQueue.Add(new QueuedPiece(
            GetSpriteByName("box_blue"),
            Piece.PieceType.Normal
        ));

        pieceQueue.Add(new QueuedPiece(
            GetSpriteByName("cube_violet"),
            Piece.PieceType.Normal
        ));

        pieceQueue.Add(new QueuedPiece(
            GetSpriteByName("box_cyan"),
            Piece.PieceType.Normal
        ));

        pieceQueue.Add(new QueuedPiece(
            GetSpriteByName("cube_yellow"),
            Piece.PieceType.Normal
        ));

        pieceQueue.Add(new QueuedPiece(
            GetSpriteByName("box_orange"),
            Piece.PieceType.Normal
        ));

        pieceQueue.Add(new QueuedPiece(
            GetSpriteByName("cube_red"),
            Piece.PieceType.Normal
        ));
    }

    private void CreateEasy2()
    {
        pieceQueue.Add(new QueuedPiece(
            GetSpriteByName("cube_violet"),
            Piece.PieceType.Normal
        ));

        pieceQueue.Add(new QueuedPiece(
            gitRevertSprite,
            Piece.PieceType.GitRevert
        ));

        pieceQueue.Add(new QueuedPiece(
            GetSpriteByName("box_orange"),
            Piece.PieceType.Normal
        ));

        pieceQueue.Add(new QueuedPiece(
            GetSpriteByName("cube_green"),
            Piece.PieceType.Normal
        ));

        pieceQueue.Add(new QueuedPiece(
            gitRestoreSprite,
            Piece.PieceType.GitRestore
        ));

        pieceQueue.Add(new QueuedPiece(
            GetSpriteByName("c_pink"),
            Piece.PieceType.Normal
        ));

        pieceQueue.Add(new QueuedPiece(
            GetSpriteByName("box_cyan"),
            Piece.PieceType.Normal
        ));

        pieceQueue.Add(new QueuedPiece(
            GetSpriteByName("p_blue"),
            Piece.PieceType.Normal
        ));

        pieceQueue.Add(new QueuedPiece(
            k8sReplicateSprite,
            Piece.PieceType.K8sReplicate
        ));

        pieceQueue.Add(new QueuedPiece(
            GetSpriteByName("cube_yellow"),
            Piece.PieceType.Normal
        ));

        pieceQueue.Add(new QueuedPiece(
            pytorchTrainSprite,
            Piece.PieceType.PytorchTrain
        ));

        pieceQueue.Add(new QueuedPiece(
            GetSpriteByName("s_red"),
            Piece.PieceType.Normal
        ));

        pieceQueue.Add(new QueuedPiece(
            GetSpriteByName("box_green"),
            Piece.PieceType.Normal
        ));

        pieceQueue.Add(new QueuedPiece(
            GetSpriteByName("cube_orange"),
            Piece.PieceType.Normal
        ));

        pieceQueue.Add(new QueuedPiece(
            GetSpriteByName("o_violet"),
            Piece.PieceType.Normal
        ));

        pieceQueue.Add(new QueuedPiece(
            GetSpriteByName("box_blue"),
            Piece.PieceType.Normal
        ));

        pieceQueue.Add(new QueuedPiece(
            GetSpriteByName("cube_cyan"),
            Piece.PieceType.Normal
        ));

        pieceQueue.Add(new QueuedPiece(
            GetSpriteByName("box_yellow"),
            Piece.PieceType.Normal
        ));

        pieceQueue.Add(new QueuedPiece(
            GetSpriteByName("cube_pink"),
            Piece.PieceType.Normal
        ));

        pieceQueue.Add(new QueuedPiece(
            GetSpriteByName("box_red"),
            Piece.PieceType.Normal
        ));
    }

    private void CreateMedium1()
    {
        pieceQueue.Add(new QueuedPiece(
            GetSpriteByName("box_cyan"),
            Piece.PieceType.Normal
        ));

        pieceQueue.Add(new QueuedPiece(
            gitRestoreSprite,
            Piece.PieceType.GitRestore
        ));

        pieceQueue.Add(new QueuedPiece(
            GetSpriteByName("stair_orange"),
            Piece.PieceType.Normal
        ));

        pieceQueue.Add(new QueuedPiece(
            GetSpriteByName("cube_green"),
            Piece.PieceType.Normal
        ));

        pieceQueue.Add(new QueuedPiece(
            gitRevertSprite,
            Piece.PieceType.GitRevert
        ));

        pieceQueue.Add(new QueuedPiece(
            GetSpriteByName("c_blue"),
            Piece.PieceType.Normal
        ));

        pieceQueue.Add(new QueuedPiece(
            GetSpriteByName("slab_yellow"),
            Piece.PieceType.Normal
        ));

        pieceQueue.Add(new QueuedPiece(
            GetSpriteByName("box_pink"),
            Piece.PieceType.Normal
        ));

        pieceQueue.Add(new QueuedPiece(
            k8sReplicateSprite,
            Piece.PieceType.K8sReplicate
        ));

        pieceQueue.Add(new QueuedPiece(
            GetSpriteByName("p_violet"),
            Piece.PieceType.Normal
        ));

        pieceQueue.Add(new QueuedPiece(
            GetSpriteByName("cube_red"),
            Piece.PieceType.Normal
        ));

        pieceQueue.Add(new QueuedPiece(
            pytorchTrainSprite,
            Piece.PieceType.PytorchTrain
        ));

        pieceQueue.Add(new QueuedPiece(
            GetSpriteByName("s_red"),
            Piece.PieceType.Normal
        ));

        pieceQueue.Add(new QueuedPiece(
            GetSpriteByName("stair_green"),
            Piece.PieceType.Normal
        ));

        pieceQueue.Add(new QueuedPiece(
            GetSpriteByName("o_pink"),
            Piece.PieceType.Normal
        ));

        pieceQueue.Add(new QueuedPiece(
            GetSpriteByName("box_blue"),
            Piece.PieceType.Normal
        ));

        pieceQueue.Add(new QueuedPiece(
            GetSpriteByName("cube_yellow"),
            Piece.PieceType.Normal
        ));

        pieceQueue.Add(new QueuedPiece(
            GetSpriteByName("slab_cyan"),
            Piece.PieceType.Normal
        ));

        pieceQueue.Add(new QueuedPiece(
            GetSpriteByName("box_orange"),
            Piece.PieceType.Normal
        ));

        pieceQueue.Add(new QueuedPiece(
            GetSpriteByName("cube_violet"),
            Piece.PieceType.Normal
        ));
    }

    private void CreateMedium2()
    {
        pieceQueue.Add(new QueuedPiece(
            GetSpriteByName("cube_green"),
            Piece.PieceType.Normal
        ));

        pieceQueue.Add(new QueuedPiece(
            gitRevertSprite,
            Piece.PieceType.GitRevert
        ));

        pieceQueue.Add(new QueuedPiece(
            GetSpriteByName("slab_orange"),
            Piece.PieceType.Normal
        ));

        pieceQueue.Add(new QueuedPiece(
            GetSpriteByName("box_cyan"),
            Piece.PieceType.Normal
        ));

        pieceQueue.Add(new QueuedPiece(
            gitRestoreSprite,
            Piece.PieceType.GitRestore
        ));

        pieceQueue.Add(new QueuedPiece(
            GetSpriteByName("c_violet"),
            Piece.PieceType.Normal
        ));

        pieceQueue.Add(new QueuedPiece(
            GetSpriteByName("stair_yellow"),
            Piece.PieceType.Normal
        ));

        pieceQueue.Add(new QueuedPiece(
            GetSpriteByName("cube_pink"),
            Piece.PieceType.Normal
        ));

        pieceQueue.Add(new QueuedPiece(
            k8sReplicateSprite,
            Piece.PieceType.K8sReplicate
        ));

        pieceQueue.Add(new QueuedPiece(
            GetSpriteByName("p_blue"),
            Piece.PieceType.Normal
        ));

        pieceQueue.Add(new QueuedPiece(
            GetSpriteByName("box_red"),
            Piece.PieceType.Normal
        ));

        pieceQueue.Add(new QueuedPiece(
            pytorchTrainSprite,
            Piece.PieceType.PytorchTrain
        ));

        pieceQueue.Add(new QueuedPiece(
            GetSpriteByName("s_red"),
            Piece.PieceType.Normal
        ));

        pieceQueue.Add(new QueuedPiece(
            GetSpriteByName("cube_orange"),
            Piece.PieceType.Normal
        ));

        pieceQueue.Add(new QueuedPiece(
            GetSpriteByName("o_pink"),
            Piece.PieceType.Normal
        ));

        pieceQueue.Add(new QueuedPiece(
            GetSpriteByName("stair_blue"),
            Piece.PieceType.Normal
        ));

        pieceQueue.Add(new QueuedPiece(
            GetSpriteByName("box_green"),
            Piece.PieceType.Normal
        ));

        pieceQueue.Add(new QueuedPiece(
            GetSpriteByName("cube_yellow"),
            Piece.PieceType.Normal
        ));

        pieceQueue.Add(new QueuedPiece(
            GetSpriteByName("slab_violet"),
            Piece.PieceType.Normal
        ));

        pieceQueue.Add(new QueuedPiece(
            GetSpriteByName("box_cyan"),
            Piece.PieceType.Normal
        ));
    }

    private void CreateHard1()
    {
        pieceQueue.Add(new QueuedPiece(
            GetSpriteByName("stair_orange"),
            Piece.PieceType.Normal
        ));

        pieceQueue.Add(new QueuedPiece(
            gitRestoreSprite,
            Piece.PieceType.GitRestore
        ));

        pieceQueue.Add(new QueuedPiece(
            GetSpriteByName("box_cyan"),
            Piece.PieceType.Normal
        ));

        pieceQueue.Add(new QueuedPiece(
            GetSpriteByName("stair_yellow"),
            Piece.PieceType.Normal
        ));

        pieceQueue.Add(new QueuedPiece(
            gitRevertSprite,
            Piece.PieceType.GitRevert
        ));

        pieceQueue.Add(new QueuedPiece(
            GetSpriteByName("c_red"),
            Piece.PieceType.Normal
        ));

        pieceQueue.Add(new QueuedPiece(
            GetSpriteByName("cube_green"),
            Piece.PieceType.Normal
        ));

        pieceQueue.Add(new QueuedPiece(
            k8sReplicateSprite,
            Piece.PieceType.K8sReplicate
        ));

        pieceQueue.Add(new QueuedPiece(
            GetSpriteByName("p_violet"),
            Piece.PieceType.Normal
        ));

        pieceQueue.Add(new QueuedPiece(
            GetSpriteByName("lab_pink"),
            Piece.PieceType.Normal
        ));

        pieceQueue.Add(new QueuedPiece(
            pytorchTrainSprite,
            Piece.PieceType.PytorchTrain
        ));

        pieceQueue.Add(new QueuedPiece(
            GetSpriteByName("s_blue"),
            Piece.PieceType.Normal
        ));

        pieceQueue.Add(new QueuedPiece(
            GetSpriteByName("box_yellow"),
            Piece.PieceType.Normal
        ));

        pieceQueue.Add(new QueuedPiece(
            GetSpriteByName("stair_green"),
            Piece.PieceType.Normal
        ));

        pieceQueue.Add(new QueuedPiece(
            GetSpriteByName("o_pink"),
            Piece.PieceType.Normal
        ));

        pieceQueue.Add(new QueuedPiece(
            GetSpriteByName("cube_orange"),
            Piece.PieceType.Normal
        ));

        pieceQueue.Add(new QueuedPiece(
            GetSpriteByName("slab_violet"),
            Piece.PieceType.Normal
        ));

        pieceQueue.Add(new QueuedPiece(
            GetSpriteByName("box_red"),
            Piece.PieceType.Normal
        ));

        pieceQueue.Add(new QueuedPiece(
            GetSpriteByName("cube_cyan"),
            Piece.PieceType.Normal
        ));

        pieceQueue.Add(new QueuedPiece(
            GetSpriteByName("stair_blue"),
            Piece.PieceType.Normal
        ));
    }

    private void CreateHard2()
    {
        pieceQueue.Add(new QueuedPiece(
            GetSpriteByName("slab_violet"),
            Piece.PieceType.Normal
        ));

        pieceQueue.Add(new QueuedPiece(
            gitRevertSprite,
            Piece.PieceType.GitRevert
        ));

        pieceQueue.Add(new QueuedPiece(
            GetSpriteByName("box_yellow"),
            Piece.PieceType.Normal
        ));

        pieceQueue.Add(new QueuedPiece(
            GetSpriteByName("stair_cyan"),
            Piece.PieceType.Normal
        ));

        pieceQueue.Add(new QueuedPiece(
            gitRestoreSprite,
            Piece.PieceType.GitRestore
        ));

        pieceQueue.Add(new QueuedPiece(
            GetSpriteByName("c_pink"),
            Piece.PieceType.Normal
        ));

        pieceQueue.Add(new QueuedPiece(
            GetSpriteByName("cube_orange"),
            Piece.PieceType.Normal
        ));

        pieceQueue.Add(new QueuedPiece(
            GetSpriteByName("p_blue"),
            Piece.PieceType.Normal
        ));

        pieceQueue.Add(new QueuedPiece(
            k8sReplicateSprite,
            Piece.PieceType.K8sReplicate
        ));

        pieceQueue.Add(new QueuedPiece(
            GetSpriteByName("slab_green"),
            Piece.PieceType.Normal
        ));

        pieceQueue.Add(new QueuedPiece(
            pytorchTrainSprite,
            Piece.PieceType.PytorchTrain
        ));

        pieceQueue.Add(new QueuedPiece(
            GetSpriteByName("s_violet"),
            Piece.PieceType.Normal
        ));

        pieceQueue.Add(new QueuedPiece(
            GetSpriteByName("box_red"),
            Piece.PieceType.Normal
        ));

        pieceQueue.Add(new QueuedPiece(
            GetSpriteByName("stair_orange"),
            Piece.PieceType.Normal
        ));

        pieceQueue.Add(new QueuedPiece(
            GetSpriteByName("o_red"),
            Piece.PieceType.Normal
        ));

        pieceQueue.Add(new QueuedPiece(
            GetSpriteByName("cube_green"),
            Piece.PieceType.Normal
        ));

        pieceQueue.Add(new QueuedPiece(
            GetSpriteByName("slab_blue"),
            Piece.PieceType.Normal
        ));

        pieceQueue.Add(new QueuedPiece(
            GetSpriteByName("box_pink"),
            Piece.PieceType.Normal
        ));

        pieceQueue.Add(new QueuedPiece(
            GetSpriteByName("stair_yellow"),
            Piece.PieceType.Normal
        ));

        pieceQueue.Add(new QueuedPiece(
            GetSpriteByName("cube_cyan"),
            Piece.PieceType.Normal
        ));
    }

    private Sprite GetSpriteByName(string spriteName)
    {
        Sprite[] allSprites = Resources.FindObjectsOfTypeAll<Sprite>();

        foreach (Sprite sprite in allSprites)
        {
            if (sprite.name == spriteName ||
                sprite.name == spriteName + "_0")
            {
                return sprite;
            }
        }

        Debug.LogError(
            "PieceSpawner: Could not find sprite: " +
            spriteName
        );

        return null;
    }
    /*
    private Sprite GetRandomShapeSprite(Sprite[] sprites, string shapeName)
    {
        if (sprites == null || sprites.Length == 0)
        {
            Debug.LogError(
                "PieceSpawner: No " + shapeName + " sprites assigned!"
            );

            return null;
        }

        return sprites[
            Random.Range(0, sprites.Length)
        ];
    }

    private Sprite GetRandomSprite(Sprite[] sprites)
    {
        if (sprites == null || sprites.Length == 0)
        {
            Debug.LogError("PieceSpawner: Sprite array is empty!");
            return null;
        }

        return sprites[Random.Range(0, sprites.Length)];
    }

    private Sprite GetRandomOtherSprite()
    {
        List<Sprite> otherSprites = new List<Sprite>();

        if (stairSprites != null)
            otherSprites.AddRange(stairSprites);

        if (slabSprites != null)
            otherSprites.AddRange(slabSprites);

        if (otherSprites.Count == 0)
        {
            Debug.LogError("PieceSpawner: No Stair or Slab sprites assigned!");
            return null;
        }

        return otherSprites[
            Random.Range(0, otherSprites.Count)
        ];
    }*/

    public void SpawnPiece()
    {
        if (piecePrefab == null)
        {
            Debug.LogError("PieceSpawner: No piece prefab assigned!");
            return;
        }

        if (pieceQueue.Count == 0)
        {
            Debug.Log("No more pieces in queue.");
            return;
        }

        QueuedPiece nextPiece = pieceQueue[0];
        float highestY = firstSpawnHeight;

        Piece[] pieces = FindObjectsByType<Piece>(
            FindObjectsSortMode.None
        );

        foreach (Piece towerPiece in pieces)
        {
            Collider2D collider =
                towerPiece.GetComponent<Collider2D>();

            if (collider != null &&
                collider.bounds.max.y > highestY)
            {
                highestY = collider.bounds.max.y;
            }
        }

        Vector3 spawnPosition = transform.position;
        spawnPosition.x -= 4f;
        spawnPosition.y = highestY + 11f;
        /*
        Vector3 spawnPosition = transform.position;

        if (Camera.main != null)
        {
            Camera cam = Camera.main;

            spawnPosition.x -= 4f;
        }

        float highestY = transform.position.y;

        Piece[] pieces = FindObjectsByType<Piece>(
            FindObjectsSortMode.None
        );

        foreach (Piece piece in pieces)
        {
            PieceController controller =
                piece.GetComponent<PieceController>();

            if (controller == null || !controller.IsSettled)
                continue;

            Collider2D collider =
                piece.GetComponent<Collider2D>();

            if (collider != null &&
                collider.bounds.max.y > highestY)
            {
                highestY = collider.bounds.max.y;
            }
        }

        spawnPosition.y = highestY + 5f;*/

        currentPiece = Instantiate(
            piecePrefab,
            spawnPosition,
            Quaternion.identity
        );

        Piece piece = currentPiece.GetComponent<Piece>();

        if (piece != null)
        {
            piece.Initialize(
                nextPiece.sprite,
                nextPiece.type
            );
        }

        pieceQueue.RemoveAt(0);
        UpdateQueueUI();

        PieceController pieceController =
            currentPiece.GetComponent<PieceController>();

        if (pieceController != null)
        {
            pieceController.SetSpawner(this);
        }

        Debug.Log(
            "Spawned: " +
            nextPiece.sprite.name +
            " | Type: " +
            nextPiece.type
        );
    }

    public void PieceSettled()
    {
        if (currentPiece != null)
        {
            Piece piece = currentPiece.GetComponent<Piece>();

            if (piece != null)
            {
                QueuedPiece placedPiece = new QueuedPiece(
                    piece.GetComponent<SpriteRenderer>().sprite,
                    piece.Type
                );

                placedHistory.Add(placedPiece);

                if (placedHistory.Count > 3)
                {
                    placedHistory.RemoveAt(0);
                }
            }
        }

        if (pieceQueue.Count == 0)
        {
            currentPiece = null;

            Debug.Log("GAME COMPLETE — No more pieces in queue.");

            if (GameManager.Instance != null)
            {
                GameManager.Instance.ShowRestartButton();
            }

            return;
        }

        Debug.Log("Spawner: Piece settled. Waiting for physics to settle...");
        StartCoroutine(WaitForPhysicsThenCamera());
    }
    /*
    private System.Collections.IEnumerator WaitForPhysicsThenCamera()
    {
        float stableTimer = 0f;
        float maximumWait = 2f;
        float totalWait = 0f;

        while (stableTimer < 0.4f &&
               totalWait < maximumWait)
        {
            bool everythingStable = true;

            Rigidbody2D[] bodies =
                FindObjectsByType<Rigidbody2D>(
                    FindObjectsSortMode.None
                );

            foreach (Rigidbody2D body in bodies)
            {
                if (body.linearVelocity.magnitude > 0.2f ||
                    Mathf.Abs(body.angularVelocity) > 10f)
                {
                    everythingStable = false;
                    break;
                }
            }

            if (everythingStable)
            {
                stableTimer += Time.deltaTime;
            }
            else
            {
                stableTimer = 0f;
            }

            yield return null;
            totalWait += Time.deltaTime;
        }

        Debug.Log("Spawner: Entire tower is stable.");

        // Decide the current tower height ONCE.
        if (GameManager.Instance != null)
        {
            GameManager.Instance.RecalculateCurrentHeight();
        }

        // Now wait for the camera to reach its new position.
        CameraController cameraController = null;

        if (Camera.main != null)
        {
            cameraController =
                Camera.main.GetComponent<CameraController>();
        }

        if (cameraController != null)
        {
            float cameraWait = 0f;
            float maximumCameraWait = 2f;

            while (!cameraController.HasReachedTarget() &&
                   cameraWait < maximumCameraWait)
            {
                cameraWait += Time.deltaTime;
                yield return null;
            }
        }

        Debug.Log("Spawner: Camera ready. Spawning next piece.");

        SpawnPiece();
    }*/

    private System.Collections.IEnumerator WaitForPhysicsThenCamera()
    {
        float stableTimer = 0f;
        float maximumWait = 5f;
        float totalWait = 0f;

        while (stableTimer < 0.4f &&
               totalWait < maximumWait)
        {
            bool everythingStable = true;

            Rigidbody2D[] bodies =
                FindObjectsByType<Rigidbody2D>(
                    FindObjectsSortMode.None
                );

            foreach (Rigidbody2D body in bodies)
            {
                PieceController controller =
                    body.GetComponent<PieceController>();

                // Ignore the currently controlled piece
                if (controller != null &&
                    !controller.IsSettled)
                {
                    continue;
                }

                // Only care about significant movement
                if (body.linearVelocity.magnitude > 0.5f ||
                    Mathf.Abs(body.angularVelocity) > 20f)
                {
                    everythingStable = false;
                    break;
                }
            }

            if (everythingStable)
            {
                stableTimer += Time.deltaTime;
            }
            else
            {
                stableTimer = 0f;
            }

            totalWait += Time.deltaTime;

            yield return null;
        }

        Debug.Log("Spawner: Physics wait finished.");
        
        if (GameManager.Instance != null)
        {
            GameManager.Instance.RecalculateCurrentHeight();
        }

        CameraController cameraController = null;

        if (Camera.main != null)
        {
            cameraController =
                Camera.main.GetComponent<CameraController>();
        }

        if (cameraController != null)
        {
            float cameraWait = 0f;
            float maximumCameraWait = 2f;

            while (!cameraController.HasReachedTarget() &&
                   cameraWait < maximumCameraWait)
            {
                cameraWait += Time.deltaTime;
                yield return null;
            }

            // Give the camera one extra frame to settle
            yield return null;
        }

        Debug.Log("Spawner: Camera ready. Spawning next piece.");

        SpawnPiece();
    }

    public void GitRevert()
    {
        if (placedHistory.Count == 0)
        {
            Debug.Log("Git Revert: Nothing to revert.");
            return;
        }

        Debug.Log("Git Revert activated!");

        // Put the last placed blocks back at the front of the master queue.
        for (int i = placedHistory.Count - 1; i >= 0; i--)
        {
            pieceQueue.Insert(0, placedHistory[i]);
        }

        placedHistory.Clear();

        UpdateQueueUI();
    }

    public void GitRestore()
    {
        Debug.Log("Git Restore activated!");

        if (pieceQueue.Count > 0)
        {
            QueuedPiece removedPiece = pieceQueue[0];

            pieceQueue.RemoveAt(0);

            Debug.Log(
                "Git Restore removed next block: " +
                removedPiece.type
            );

            UpdateQueueUI();
        }
        else
        {
            Debug.Log("Git Restore: Queue is empty.");
        }
    }

    public void K8sReplicate()
    {
        Debug.Log("K8s Replicate activated!");

        int count = Mathf.Min(3, pieceQueue.Count);

        List<QueuedPiece> copies = new List<QueuedPiece>();

        // Copy the next 3 blocks.
        for (int i = 0; i < count; i++)
        {
            copies.Add(
                new QueuedPiece(
                    pieceQueue[i].sprite,
                    pieceQueue[i].type
                )
            );
        }

        // Insert the copies immediately after the original 3.
        for (int i = 0; i < copies.Count; i++)
        {
            pieceQueue.Insert(3 + i, copies[i]);
        }

        UpdateQueueUI();
    }

    public void PytorchTrain()
{
    Debug.Log("Pytorch Train activated!");

    int count = Mathf.Min(3, pieceQueue.Count);

    // Fisher-Yates shuffle for the first 3 pieces
    for (int i = count - 1; i > 0; i--)
    {
        int randomIndex = Random.Range(0, i + 1);

        QueuedPiece temp = pieceQueue[i];
        pieceQueue[i] = pieceQueue[randomIndex];
        pieceQueue[randomIndex] = temp;
    }

    UpdateQueueUI();
}

    public void ActivateSpecial(Piece.PieceType type)
    {
        switch (type)
        {
            case Piece.PieceType.GitRevert:
                GitRevert();
                break;

            case Piece.PieceType.GitRestore:
                GitRestore();
                break;

            case Piece.PieceType.K8sReplicate:
                K8sReplicate();
                break;

            case Piece.PieceType.PytorchTrain:
                PytorchTrain();
                break;
        }

        if (specialPopupUI != null)
        {
            specialPopupUI.Show(type);
        }
    }

    public void PieceFellOut()
    {
        Debug.Log("Piece fell off the base. Waiting for tower physics...");

        currentPiece = null;

        if (pieceQueue.Count == 0)
        {
            Debug.Log("GAME COMPLETE — No more pieces in queue.");

            if (GameManager.Instance != null)
            {
                GameManager.Instance.ShowRestartButton();
            }

            return;
        }

        StartCoroutine(WaitForPhysicsThenCamera());
    }

    public bool IsGameComplete()
    {
        return pieceQueue.Count == 0 && currentPiece == null;
    }
}