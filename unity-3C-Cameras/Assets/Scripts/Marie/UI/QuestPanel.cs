using TMPro;
using UnityEngine;

public class QuestPanel : MonoBehaviour
{
    public TextMeshProUGUI title, progress;
    public Sprite completeQuest;
    private QuestData trackedQuest;
    private int max = 0;

    public void SetupQuest(QuestData quest)
    {
        trackedQuest = quest;
        title.text = quest.title;
        SetTotalRequirements();
        progress.text = " 0/" + max;
    }

    public void Notify()
    {
        int amount = 0;
        foreach (QuestItem item in trackedQuest.requirements)
        {
            int index = Inventory.Instance.items.FindIndex(i=> i.item.Equals(item.item));
            if (index != -1)
            {
                amount += Inventory.Instance.items[index].quantity;
                Debug.Log(amount+" ITEMS FOUND");
            }
            else
            {
                Debug.Log("NO ITEM FOUND");
            }
        }
        progress.text = amount +" /" + max;
    }

    public void SetTotalRequirements()
    {
        foreach (QuestItem item in trackedQuest.requirements)
        {
            max += item.quantity;
        }
    }
}
