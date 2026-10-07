using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QueueUI : MonoBehaviour
{
    [SerializeField] private Image[] queueSlots;
    [SerializeField] private TMP_Text queueCountText;

    public void UpdateQueue(PieceSpawner.QueuedPiece[] queue)
    {
        for (int i = 0; i < queueSlots.Length; i++)
        {
            if (i < queue.Length &&
                queue[i] != null &&
                queue[i].sprite != null)
            {
                Image slot = queueSlots[i];

                slot.sprite = queue[i].sprite;
                slot.enabled = true;
                slot.preserveAspect = true;
            }
            else
            {
                queueSlots[i].enabled = false;
            }
        }
    }

    public void UpdateQueueCount(int count)
    {
        if (queueCountText != null)
        {
            queueCountText.text = "Blocks left: " + count;
        }
    }
}