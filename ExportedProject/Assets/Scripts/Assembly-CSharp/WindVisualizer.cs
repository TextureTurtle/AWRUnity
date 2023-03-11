using UnityEngine;

public class WindVisualizer : MonoBehaviour
{
	[SerializeField]
	private int _horizontalSteps = 16;

	[SerializeField]
	private int _verticalSteps = 16;

	[SerializeField]
	private float _horizontalSize = 128f;

	[SerializeField]
	private float _verticalSize = 128f;

	[SerializeField]
	private float _drawScale = 10f;

	private AeroManager _aeroManager;

	private void Start()
	{
		_aeroManager = SingletonComponent<AeroManager>.Instance;
	}

	private void OnDrawGizmos()
	{
		if (!Application.isPlaying)
		{
			return;
		}
		Gizmos.color = Color.white;
		float num = _horizontalSize / (float)_horizontalSteps;
		float num2 = _verticalSize / (float)_verticalSteps;
		float num3 = _horizontalSize * 0.5f;
		float y = _verticalSize * 0.5f;
		Vector3 vector = base.transform.position - new Vector3(num3, y, num3);
		Debug.DrawLine(base.transform.position, vector);
		for (int i = 0; i < _horizontalSteps; i++)
		{
			float x = (float)i * num;
			for (int j = 0; j < _verticalSteps; j++)
			{
				float y2 = (float)j * num2;
				for (int k = 0; k < _horizontalSteps; k++)
				{
					float z = (float)k * num;
					Vector3 vector2 = vector + new Vector3(x, y2, z);
					Vector3 direction = _aeroManager.GetWindVelocity(vector2, true) * _drawScale;
					Gizmos.DrawRay(vector2, direction);
				}
			}
		}
	}
}
