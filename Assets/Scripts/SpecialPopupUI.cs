using System.Collections;
using TMPro;
using UnityEngine;

public class SpecialPopupUI : MonoBehaviour
{
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text descriptionText;

    [SerializeField] private float displayTime = 3f;

    private Coroutine hideCoroutine;

    public void Show(Piece.PieceType type)
    {
        switch (type)
        {
            case Piece.PieceType.GitRevert:
                titleText.text = "GIT REVERT";
                descriptionText.text =
                    "The last 3 placed blocks are returned to the queue.";
                break;

            case Piece.PieceType.GitRestore:
                titleText.text = "GIT RESTORE";
                descriptionText.text =
                    "The next block in the queue is removed.";
                break;

            case Piece.PieceType.K8sReplicate:
                titleText.text = "K8S REPLICATE";
                descriptionText.text =
                    "The next 3 blocks are duplicated.";
                break;

            case Piece.PieceType.PytorchTrain:
                titleText.text = "PYTORCH TRAIN";
                descriptionText.text =
                    "The next 3 blocks are shuffled.";
                break;

            default:
                return;
        }

        gameObject.SetActive(true);

        if (hideCoroutine != null)
        {
            StopCoroutine(hideCoroutine);
        }

        hideCoroutine = StartCoroutine(HideAfterDelay());
    }

    private IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(displayTime);

        gameObject.SetActive(false);
        hideCoroutine = null;
    }
}