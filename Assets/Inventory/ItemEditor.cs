using UnityEngine;
using UnityEditor;

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
        EditorGUILayout.PropertyField(serializedObject.FindProperty("rarity"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("description"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("descriptionStats"));


        // Hiển thị các biến tùy theo loại attack
        switch (script.itemType)
        {
            case ItemType.Equipment:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("allowedSlot"));
                switch (script.allowedSlot)
                {
                    case EquidmentSlotType.AttackGem:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("damgeBonusGem"));
                        break;
                    case EquidmentSlotType.DefenceGem:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("defBonusGem"));
                        break;
                    case EquidmentSlotType.Ring:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("critRateBonus"));
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("critDamBonus"));
                        break;
                    case EquidmentSlotType.Weapon:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("damgeBonus"));
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("SwordId"));
                        break;
                    case EquidmentSlotType.BaseSkill:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("skillBaseType"));
                        break;
                    case EquidmentSlotType.SpecialSkill:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("skillSpecialType"));
                        break;
                }
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
