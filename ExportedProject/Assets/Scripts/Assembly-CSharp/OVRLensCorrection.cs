using UnityEngine;

[AddComponentMenu("Image Effects/OVRLensCorrection")]
public class OVRLensCorrection : OVRImageEffectBase
{
	[HideInInspector]
	public Vector2 _Center = new Vector2(0.5f, 0.5f);

	[HideInInspector]
	public Vector2 _ScaleIn = new Vector2(1f, 1f);

	[HideInInspector]
	public Vector2 _Scale = new Vector2(1f, 1f);

	[HideInInspector]
	public Vector4 _HmdWarpParam = new Vector4(1f, 0f, 0f, 0f);

	public Material material_CA;

	[HideInInspector]
	public Vector4 _ChromaticAberration = new Vector4(0.996f, 0.992f, 1.014f, 1.014f);

	public Material GetMaterial()
	{
		material.SetVector("_Center", _Center);
		material.SetVector("_Scale", _Scale);
		material.SetVector("_ScaleIn", _ScaleIn);
		material.SetVector("_HmdWarpParam", _HmdWarpParam);
		return material;
	}

	public Material GetMaterial_CA()
	{
		material_CA.SetVector("_Center", _Center);
		material_CA.SetVector("_Scale", _Scale);
		material_CA.SetVector("_ScaleIn", _ScaleIn);
		material_CA.SetVector("_HmdWarpParam", _HmdWarpParam);
		Vector4 chromaticAberration = _ChromaticAberration;
		float value = chromaticAberration[1] - chromaticAberration[0];
		float value2 = chromaticAberration[3] - chromaticAberration[2];
		chromaticAberration[1] = value;
		chromaticAberration[3] = value2;
		material_CA.SetVector("_ChromaticAberration", chromaticAberration);
		return material_CA;
	}
}
