using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;
    
    public List<QuestItem> items = new List<QuestItem>();


    private void Start()
    {
        Instance = this;
    }

    public void RemoveFromInventory(ItemData qItem)
    {
        int found = items.FindIndex(q => q.item.Equals(qItem));
        if (found >= 0)
        {
            items.RemoveAt(found);
        }
    }
    public void PickupQuestItem(ItemData questItem)
    {
        int found = items.FindIndex(q => q.item.Equals(questItem));
        if (found < 0)
        {
            items.Add(new QuestItem(questItem));
        }
        else
        {
            items[found].quantity++;
        }
    }

    public bool IsItemFound(ItemData questItem)
    {
        //Empty ID means no necessary key item
        //true if the id is found in the list, false otherwise
        return questItem==null || GetItemIndex(questItem)!= -1;
    }

    public int GetItemIndex(ItemData questItem)
    {
        return items.FindIndex(q => q.item.Equals(questItem));
    }

    public bool HasEveryItem(List<QuestItem> requiredItems)
    {
        foreach (QuestItem item in requiredItems)
        {
            int index = GetItemIndex(item.item);
            if (index == -1 || items[index].quantity < item.quantity)
            {
                return false;
            }
        }

        return true;
    }

    public bool HasEvery(QuestItem requiredItem)
    {
        int index = GetItemIndex(requiredItem.item);
        if (index == -1 || items[index].quantity < requiredItem.quantity)
        {
            return false;
        }

        return true;
    }
}
