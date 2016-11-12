using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(RadialBlur))]
public class RadialBlurEditor : Editor {
    SerializedProperty _blurAmount, _offsetX, _offsetY;

    void OnEnable() {
        _blurAmount = serializedObject.FindProperty("_blurAmount");
        _offsetX = serializedObject.FindProperty("x");
        _offsetY = serializedObject.FindProperty("y");
    }

    public override void OnInspectorGUI() {
        serializedObject.Update();

        EditorGUILayout.PropertyField(_blurAmount);
        EditorGUILayout.LabelField("Offset");
        EditorGUI.indentLevel++;
        EditorGUILayout.PropertyField(_offsetX);
        EditorGUILayout.PropertyField(_offsetY);
        EditorGUI.indentLevel--;

        serializedObject.ApplyModifiedProperties();
    }
}