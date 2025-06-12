using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ItemIn))]
public class ItemSettingEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        ItemIn script = (ItemIn)target;

        EditorGUILayout.PropertyField(serializedObject.FindProperty("itemType"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("image"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("Name"));

        // Hiển thị các biến tùy theo loại attack
        switch (script.itemType)
        {
            case ItemIn.ItemType.Consumbles:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("Consumbles_Id"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("Consumbles_Value"));
                break;

            case ItemIn.ItemType.Weapon:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("Weapon_Id"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("Weapon_Range"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("Weapon_Damge"));
                break;

            case ItemIn.ItemType.SkillBase:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("SkillBase_Id"));
                break;
            case ItemIn.ItemType.SkillBoss:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("SkillBoss_Id"));
                break;
        }

        serializedObject.ApplyModifiedProperties();
    }
}
