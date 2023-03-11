using UnityEngine;

[RequireComponent(typeof(Camera))]
public class OVRCamera : MonoBehaviour
{
	private Material _blitMaterial;

	private Material _colorOnlyMaterial;

	private Color _quadColor = Color.red;

	private float _cameraTextureScale = 1f;

	[SerializeField]
	private OVRCameraController _cameraController;

	private RenderTexture _cameraTexture;

	public bool LensCorrection = true;

	public bool Chromatic = true;

	private void Awake()
	{
		if (_blitMaterial == null)
		{
			_blitMaterial = new Material("Shader \"BlitCopy\" {\n\tSubShader { Pass {\n \t\tZTest Off Cull Off ZWrite Off Fog { Mode Off }\n\t\tSetTexture [_MainTex] { combine texture}\t}}\nFallback Off }");
		}
		if (_colorOnlyMaterial == null)
		{
			_colorOnlyMaterial = new Material("Shader \"Solid Color\" {\nProperties {\n_Color (\"Color\", Color) = (1,1,1)\n}\nSubShader {\nColor [_Color]\nPass {}\n}\n}");
		}
	}

	private void Start()
	{
		if (_cameraController == null)
		{
			Debug.LogWarning("OVRCameraController not set in inspector");
		}
		if (!_cameraTexture && _cameraTextureScale > 1f)
		{
			int width = (int)((float)Screen.width / 2f * _cameraTextureScale);
			int height = (int)((float)Screen.height * _cameraTextureScale);
			_cameraTexture = new RenderTexture(width, height, 24);
		}
	}

	private void OnPreCull()
	{
		if (!_cameraController.UpdateOrientationInPreRender)
		{
			SetCameraOrientation();
		}
	}

	private void OnPreRender()
	{
		if (_cameraController.UpdateOrientationInPreRender)
		{
			SetCameraOrientation();
		}
		if (_cameraController.WireMode)
		{
			GL.wireframe = true;
		}
		if (_cameraTexture != null)
		{
			Graphics.SetRenderTarget(_cameraTexture);
			GL.Clear(true, true, base.gameObject.camera.backgroundColor);
		}
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		bool flip = Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer;
		RenderTexture renderTexture = source;
		if (_cameraTexture != null)
		{
			renderTexture = _cameraTexture;
			flip = false;
		}
		else if (QualitySettings.antiAliasing == 0 || base.camera.actualRenderingPath == RenderingPath.DeferredLighting)
		{
			flip = false;
		}
		RenderPreLensCorrection(base.camera, renderTexture);
		Material material = _blitMaterial;
		if (LensCorrection)
		{
			material = ((!Chromatic) ? GetComponent<OVRLensCorrection>().GetMaterial() : GetComponent<OVRLensCorrection>().GetMaterial_CA());
		}
		Blit(renderTexture, destination, material, flip);
	}

	private void OnPostRender()
	{
		if (_cameraController.WireMode)
		{
			GL.wireframe = false;
		}
	}

	private void SetCameraOrientation()
	{
		if (base.gameObject.camera.depth == 0f)
		{
			_cameraController.UpdateOrientation();
		}
	}

	private void Blit(RenderTexture source, RenderTexture destination, Material material, bool flip)
	{
		RenderTexture.active = destination;
		source.SetGlobalShaderProperty("_MainTex");
		GL.PushMatrix();
		GL.LoadOrtho();
		for (int i = 0; i < material.passCount; i++)
		{
			material.SetPass(i);
			DrawQuad(flip);
		}
		GL.PopMatrix();
	}

	private void DrawQuad(bool flip)
	{
		GL.Begin(7);
		if (flip)
		{
			GL.TexCoord2(0f, 1f);
			GL.Vertex3(0f, 0f, 0.1f);
			GL.TexCoord2(1f, 1f);
			GL.Vertex3(1f, 0f, 0.1f);
			GL.TexCoord2(1f, 0f);
			GL.Vertex3(1f, 1f, 0.1f);
			GL.TexCoord2(0f, 0f);
			GL.Vertex3(0f, 1f, 0.1f);
		}
		else
		{
			GL.TexCoord2(0f, 0f);
			GL.Vertex3(0f, 0f, 0.1f);
			GL.TexCoord2(1f, 0f);
			GL.Vertex3(1f, 0f, 0.1f);
			GL.TexCoord2(1f, 1f);
			GL.Vertex3(1f, 1f, 0.1f);
			GL.TexCoord2(0f, 1f);
			GL.Vertex3(0f, 1f, 0.1f);
		}
		GL.End();
	}

	public virtual void RenderPreLensCorrection(Camera camera, RenderTexture target)
	{
	}

	public void SetPerspectiveOffset(Vector3 offset)
	{
		base.gameObject.camera.ResetProjectionMatrix();
		Matrix4x4 identity = Matrix4x4.identity;
		identity.SetColumn(3, new Vector4(offset.x, offset.y, 0f, 1f));
		base.gameObject.camera.projectionMatrix = identity * base.gameObject.camera.projectionMatrix;
	}
}
