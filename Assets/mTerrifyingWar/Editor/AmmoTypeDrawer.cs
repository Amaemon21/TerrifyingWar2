#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(EAmmoType))]
public class AmmoTypeDrawer : PropertyDrawer
{
    private readonly string[] _displayNames = { "5,45x39", "5,56x45", "7.62x39" };

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        
        property.enumValueIndex = EditorGUI.Popup(position, label.text, property.enumValueIndex, _displayNames);
        
        EditorGUI.EndProperty();
    }
}
#endif