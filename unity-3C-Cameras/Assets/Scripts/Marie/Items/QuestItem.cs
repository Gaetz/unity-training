using System;

[Serializable]
public class QuestItem
{
    public ItemData item;
    public int quantity = 0;

    public QuestItem(ItemData data, int qtt = 1)
    {
        item = data;
        quantity = qtt;
    }
}