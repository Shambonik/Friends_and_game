﻿using UnitSystem;
using UnityEngine;
#if UNITY_EDITOR
using CreatorKitCode;
using UnityEditor;
#endif

namespace CreatorKitCode 
{
    public abstract class Item : ScriptableObject
    {
        public string ItemName;
        public Sprite ItemSprite;
        public string Description;
        public GameObject WorldObjectPrefab;

        public virtual bool UsedBy(UnitData user)
        {
            return false;
        }
        public virtual string GetDescription()
        {
            return Description;
        }
    }
}
#if UNITY_EDITOR
public class ItemEditor
{
    SerializedObject m_Target;

    SerializedProperty m_NameProperty;
    SerializedProperty m_IconProperty;
    SerializedProperty m_DescriptionProperty;
    SerializedProperty m_WorldObjectPrefabProperty;
    
    public void Init(SerializedObject target)
    {
        m_Target = target;

        m_NameProperty = m_Target.FindProperty(nameof(Item.ItemName));
        m_IconProperty = m_Target.FindProperty(nameof(Item.ItemSprite));
        m_DescriptionProperty = m_Target.FindProperty(nameof(Item.Description));
        m_WorldObjectPrefabProperty = m_Target.FindProperty(nameof(Item.WorldObjectPrefab));
    }

    public void GUI()
    {
        EditorGUILayout.PropertyField(m_IconProperty);
        EditorGUILayout.PropertyField(m_NameProperty);
        EditorGUILayout.PropertyField(m_DescriptionProperty, GUILayout.MinHeight(128));
        EditorGUILayout.PropertyField(m_WorldObjectPrefabProperty);
    }
}
#endif
