using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxController : MonoBehaviour
{
    public Material skyboxMaterial;

    public Color[] colours;

    public float Depth
    {
        set
        {
            skyboxMaterial.SetFloat("_Depth", value/15);
        }
    }

    [ContextMenu("Genreate Pallete")]
    void GeneratePallete()
    {
        Texture2D pallete = new Texture2D(colours.Length, 1);

        pallete.filterMode = FilterMode.Point;

        for ( int i = 0; i < colours.Length; i++)
        {
            pallete.SetPixel(i, 1, colours[i]);
        }

        pallete.Apply();
        byte [] data = pallete.EncodeToPNG();
        string path = Application.dataPath + "/pallete.png";
        System.IO.File.WriteAllBytes(path, data);
        Debug.Log(path);
        DestroyImmediate(pallete);
#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif
    }
	
}
