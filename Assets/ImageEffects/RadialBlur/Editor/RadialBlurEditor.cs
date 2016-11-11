using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(RadialBlur))]
public class RadialBlurEditor : Editor {
    SerializedProperty _blurAmount;

    void OnEnable() {
        _blurAmount = serializedObject.FindProperty("_blurAmount");
    }

    public override void OnInspectorGUI() {
        serializedObject.Update();

        EditorGUILayout.PropertyField(_blurAmount);

        serializedObject.ApplyModifiedProperties();
    }
}