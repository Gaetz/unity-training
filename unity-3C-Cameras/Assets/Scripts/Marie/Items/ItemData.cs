using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Quest System/Item")]
public class ItemData : ScriptableObject
{
    public Guid UID;
    public string label;
    public Sprite icon;
    
    private void OnValidate()
    {
#if UNITY_EDITOR
        if (UID == Guid.Empty)
        {
            UID = Guid.NewGuid();
            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif
    }

    public bool Equals(ItemData data)
    {
        return UID == data.UID;
    }
}
