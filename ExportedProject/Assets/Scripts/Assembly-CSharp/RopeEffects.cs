using UnityEngine;

[ExecuteInEditMode]
public class RopeEffects : MonoBehaviour
{
	[SerializeField]
	private BalloonJoint _balloonJoint;

	[SerializeField]
	private LineRenderer _lineRenderer;

	[SerializeField]
	private float _startRopeWidth = 0.05f;

	private float _ropeWidth;

	private void Start()
	{
		_ropeWidth = _startRopeWidth;
		_lineRenderer.SetVertexCount(2);
		RenderRope();
	}

	private void Update()
	{
		RenderRope();
	}

	private void RenderRope()
	{
		_lineRenderer.SetWidth(_ropeWidth, _ropeWidth);
		_lineRenderer.SetPosition(0, _balloonJoint.AttachPointA);
		Vector3 position = ((!_balloonJoint.BodyB) ? _balloonJoint.AttachPointB : _balloonJoint.BodyB.transform.TransformPoint(_balloonJoint.AttachPointB));
		_lineRenderer.SetPosition(1, position);
	}
}
