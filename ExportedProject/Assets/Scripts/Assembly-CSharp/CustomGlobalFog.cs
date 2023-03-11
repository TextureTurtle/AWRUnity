using System;
using UnityEngine;

public class CustomGlobalFog : MonoBehaviour
{
	public enum FogMode
	{
		AbsoluteYAndDistance = 0,
		AbsoluteY = 1,
		Distance = 2,
		RelativeYAndDistance = 3
	}

	public bool bEnableFog = true;

	public FogMode fogMode;

	private float CAMERA_NEAR = 0.5f;

	private float CAMERA_FAR = 50f;

	private float CAMERA_FOV = 60f;

	private float CAMERA_ASPECT_RATIO = 1.333333f;

	public float startDistance = 200f;

	public float globalDensity = 1f;

	public float heightScale = 100f;

	public float height;

	public Color globalFogColor = Color.grey;

	public Shader fogShader;

	private Material fogMaterial;

	private void Start()
	{
	}

	[ImageEffectOpaque]
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		DrawFog(source, destination);
	}

	private void DrawFog(RenderTexture source, RenderTexture destination)
	{
		if (!bEnableFog)
		{
			Graphics.Blit(source, destination);
			return;
		}
		if (fogMaterial == null)
		{
			fogMaterial = new Material(fogShader);
		}
		CAMERA_NEAR = base.camera.nearClipPlane;
		CAMERA_FAR = base.camera.farClipPlane;
		CAMERA_FOV = base.camera.fieldOfView;
		CAMERA_ASPECT_RATIO = base.camera.aspect;
		Matrix4x4 identity = Matrix4x4.identity;
		float num = CAMERA_FOV * 0.5f;
		Vector3 vector = base.camera.transform.right * CAMERA_NEAR * Mathf.Tan(num * ((float)Math.PI / 180f)) * CAMERA_ASPECT_RATIO;
		Vector3 vector2 = base.camera.transform.up * CAMERA_NEAR * Mathf.Tan(num * ((float)Math.PI / 180f));
		Vector3 vector3 = base.camera.transform.forward * CAMERA_NEAR - vector + vector2;
		float num2 = vector3.magnitude * CAMERA_FAR / CAMERA_NEAR;
		vector3.Normalize();
		vector3 *= num2;
		Vector3 vector4 = base.camera.transform.forward * CAMERA_NEAR + vector + vector2;
		vector4.Normalize();
		vector4 *= num2;
		Vector3 vector5 = base.camera.transform.forward * CAMERA_NEAR + vector - vector2;
		vector5.Normalize();
		vector5 *= num2;
		Vector3 vector6 = base.camera.transform.forward * CAMERA_NEAR - vector - vector2;
		vector6.Normalize();
		vector6 *= num2;
		identity.SetRow(0, vector3);
		identity.SetRow(1, vector4);
		identity.SetRow(2, vector5);
		identity.SetRow(3, vector6);
		fogMaterial.SetMatrix("_FrustumCornersWS", identity);
		fogMaterial.SetVector("_CameraWS", base.camera.transform.position);
		fogMaterial.SetVector("_StartDistance", new Vector4(1f / startDistance, num2 - startDistance));
		fogMaterial.SetVector("_Y", new Vector4(height, 1f / heightScale));
		fogMaterial.SetFloat("_GlobalDensity", globalDensity * 0.01f);
		fogMaterial.SetColor("_FogColor", globalFogColor);
		CustomGraphicsBlit(source, destination, fogMaterial, (int)fogMode);
	}

	private static void CustomGraphicsBlit(RenderTexture source, RenderTexture dest, Material fxMaterial, int passNr)
	{
		Graphics.Blit(source, dest, fxMaterial, passNr);
	}
}
