using UnityEngine;
using UnityEditor;
using static ItemIn;

[CustomEditor(typeof(Item))]
public class ItemEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        Item script = (Item)target;

        EditorGUILayout.PropertyField(serializedObject.FindProperty("ItemID"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("itemType"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("itemName"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("icon"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("prefab"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("rarity"));


        // Hiển thị các biến tùy theo loại attack
        switch (script.itemType)
        {
            case ItemType.Equipment:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("allowedSlot"));
                break;

            case ItemType.Consumable:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("consumableType"));
                switch (script.consumableType)
                {
                    case ConsumableType.Health:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("addHeath"));
                        break;
                    case ConsumableType.Point:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("addPoint"));
                        break;
                }
                break;
        }

        //switch (script.consumableType)
        //{
        //    case ConsumableType.Health:
        //        EditorGUILayout.PropertyField(serializedObject.FindProperty("addHeath"));
        //        break;
        //    case ConsumableType.Point:
        //        EditorGUILayout.PropertyField(serializedObject.FindProperty("addPoint"));
        //        break;
        //}
        serializedObject.ApplyModifiedProperties();
    }
}
