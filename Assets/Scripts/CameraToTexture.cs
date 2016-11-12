using UnityEngine;
using System.Collections;

public class CameraToTexture : MonoBehaviour
{
	[Range(1,8)] public int levelOfDetails = 1;
	public string textureName = "_CameraTexture";
	RenderTexture renderTexture;
	
	void Awake ()
	{
		renderTexture = new RenderTexture(Screen.width / levelOfDetails, Screen.height / levelOfDetails, 24);
		renderTexture.Create();
		GetComponent<Camera>().targetTexture = renderTexture;
	}

	void Update ()
	{
		Shader.SetGlobalTexture(textureName, renderTexture);
	}
}