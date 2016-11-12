using UnityEngine;
using System.Collections;

public class ImageFilter : MonoBehaviour 
{
	public Material material;
	private Camera cam;

	void Start ()
	{
		cam = GetComponent<Camera>();
	}
	
	void OnRenderImage (RenderTexture source, RenderTexture destination) 
	{
		material.SetVector("_CameraForward", cam.transform.forward);
		material.SetVector("_CameraUp", cam.transform.up);
		material.SetVector("_CameraRight", cam.transform.right);
		Graphics.Blit (source, destination, material);
	}
}