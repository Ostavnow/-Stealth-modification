using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
public class FieldOfView : MonoBehaviour
{
	public float viewRadius;
	[Range(0f, 360f)]
	public float viewAngle;
	public LayerMask targetMask;
	public LayerMask obstacleMask;
	[HideInInspector]
	public static Transform target;
	private Vector3 dirToTarget;
	private RaycastHit hit;
	public float meshResolution = 1.3f;
	public int edgeResolveIterations;
	public float edgeDstThreshold;
	public float maskCutawayDst = 0.1f;
	public MeshFilter viewMeshFilter;
	private Mesh viewMesh;
	private Bots _bots;
	[HideInInspector]
	public bool visibleCamera;
	private bool viewEnemyDestroyed = true;
	private List<Vector3> viewPoints = new List<Vector3>();
	private Vector3[] vertices;
	private int[] triangles;
	private FieldOfView.ViewCastInfo newViewCast;
	private float defaultResolution;
	private int timeFrameRate = 10;
	public struct ViewCastInfo
	{
		public ViewCastInfo(bool _hit, Vector3 _point, float _dst, float _angle)
		{
			hit = _hit;
			point = _point;
			dst = _dst;
			angle = _angle;
		}

		public bool hit;
		public Vector3 point;
		public float dst;
		public float angle;
	}
	public struct EdgeInfo
	{
		public EdgeInfo(Vector3 _pointA, Vector3 _pointB)
		{
			pointA = _pointA;
			pointB = _pointB;
		}
		public Vector3 pointA;
		public Vector3 pointB;

}
    private void Start()    
	{
		viewMesh = new Mesh();
		viewMesh.name = "View Mesh";
		viewMeshFilter.mesh = viewMesh;
		_bots = GetComponent<Bots>();
		defaultResolution = meshResolution;
		DrawFieldOfView();
	}
    private void Update() {
		if(_bots.botStand != true)
		{
		DrawFieldOfView();	
		}
        if(Time.frameCount % timeFrameRate == 0)
		{
		FindVisibleTargets();	
		}
    }
	private void FindVisibleTargets()
	{
		// this.target = null;
		Collider[] array = Physics.OverlapSphere(base.transform.position, viewRadius, targetMask);
		for (int i = 0; i < array.Length; i++)
		{
			dirToTarget = (array[i].transform.position - base.transform.position).normalized;
			if (Vector3.Angle(base.transform.forward, dirToTarget) < viewAngle / 2f)
			{
				float maxDistance = Vector3.Distance(base.transform.position, array[i].transform.position);
				if (Physics.Raycast(base.transform.position, dirToTarget, out hit, maxDistance, targetMask) && !Physics.Raycast(base.transform.position, dirToTarget, maxDistance, obstacleMask))
				{
					if (hit.collider.gameObject.CompareTag("Player"))
					{
						target = array[i].transform;
						ListEnemy.instance.FindAll();
						timeFrameRate = 5;
					}
					if (viewEnemyDestroyed)
					{
						if (hit.collider.gameObject.CompareTag("corpse"))
						{
						target =array[i].transform;
						ListEnemy.instance.FindAll();
						}
					}
				}
			}
		}
	}
    private void DrawFieldOfView()
	{
		int num = Mathf.RoundToInt(viewAngle * meshResolution);
		float num2 = viewAngle / (float)num;
		viewPoints.Clear();
		for (int i = 0; i <= num; i++)
		{
			newViewCast = ViewCast(transform.eulerAngles.y - viewAngle / 2f + num2 * (float)i);
			viewPoints.Add(newViewCast.point);
		}
		int num3 = viewPoints.Count + 1;
		vertices = new Vector3[num3];
		triangles = new int[(num3 - 2) * 3];
		vertices[0] = Vector3.zero;
		for (int j = 0; j < num3 - 1; j++)
		{
			vertices[j + 1] = transform.InverseTransformPoint(viewPoints[j]) + Vector3.forward * maskCutawayDst;
			if (j < num3 - 2)
			{
				triangles[j * 3] = 0;
				triangles[j * 3 + 1] = j + 1;
				triangles[j * 3 + 2] = j + 2;
			}
		}
		viewMesh.Clear();
		viewMesh.vertices = vertices;
		viewMesh.triangles = triangles;
		viewMesh.RecalculateNormals();
	}
	public void StartFindPlayer()
	{
		Profiler.BeginSample("StartFindPlayer()");
		_bots.IsInView(target.position);
		Profiler.EndSample();
	}
    private FieldOfView.ViewCastInfo ViewCast(float globalAngle)
	{
		Vector3 vector = DirFromAngle(globalAngle, true);
		RaycastHit raycastHit;
		if (Physics.Raycast(transform.position, vector, out raycastHit, viewRadius, obstacleMask))
		{
			return new FieldOfView.ViewCastInfo(true, raycastHit.point, raycastHit.distance, globalAngle);
		}
		return new FieldOfView.ViewCastInfo(false, transform.position + vector * viewRadius, viewRadius, globalAngle);
	}
    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
	{
		if (!angleIsGlobal)
		{
			angleInDegrees += transform.eulerAngles.y;
		}
		return new Vector3(Mathf.Sin(angleInDegrees * 0.017453292f), 0f, Mathf.Cos(angleInDegrees * 0.017453292f));
	}
}
