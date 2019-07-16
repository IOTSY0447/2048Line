using UnityEngine;

[RequireComponent (typeof (MeshFilter))]
public class MeshDeformer : MonoBehaviour {

	public float springForce = 20f;
	public float damping = 5f;

	Mesh deformingMesh;
	Vector3[] originalVertices, displacedVertices;
	Vector3[] vertexVelocities;

	float uniformScale = 1f;
	//自己写的代码用来调试
	float testTimeVal = 0; //记录调试间隔
	private bool isAnalogTouch = false; //是否处于模拟触摸
	public float testTouchTime = 0.1f; //模拟触摸时间
	void Start () {
		deformingMesh = GetComponent<MeshFilter> ().mesh;
		originalVertices = deformingMesh.vertices;
		displacedVertices = new Vector3[originalVertices.Length];
		for (int i = 0; i < originalVertices.Length; i++) {
			displacedVertices[i] = originalVertices[i];
		}
		vertexVelocities = new Vector3[originalVertices.Length];
	}

	void Update () {
		uniformScale = transform.localScale.x;
		for (int i = 0; i < displacedVertices.Length; i++) {
			UpdateVertex (i);
		}
		deformingMesh.vertices = displacedVertices;
		deformingMesh.RecalculateNormals ();

		AnalogTouchAnction ();
	}
	//模拟触摸
	public void AnalogTouch (Vector3 touchPoint) {
		isAnalogTouch = true;
	}
	//模拟触摸动画
	void AnalogTouchAnction () {
		if (isAnalogTouch) {
			testTimeVal += Time.deltaTime;
			Vector3 v3 = new Vector3 (0, -0.5f, 0); //只针对正方体
			Vector3 x = transform.TransformPoint (v3);
			AddDeformingForce (x, 10);
			if (testTimeVal > testTouchTime) {
				testTimeVal = 0;
				isAnalogTouch = false;
			}
		}
	}

	void UpdateVertex (int i) {
		Vector3 velocity = vertexVelocities[i];
		Vector3 displacement = displacedVertices[i] - originalVertices[i];
		displacement *= uniformScale;
		velocity -= displacement * springForce * Time.deltaTime;
		velocity *= 1f - damping * Time.deltaTime;
		vertexVelocities[i] = velocity;
		displacedVertices[i] += velocity * (Time.deltaTime / uniformScale);
	}

	public void AddDeformingForce (Vector3 point, float force) {
		point = transform.InverseTransformPoint (point);
		for (int i = 0; i < displacedVertices.Length; i++) {
			AddForceToVertex (i, point, force);
		}
	}

	void AddForceToVertex (int i, Vector3 point, float force) {
		Vector3 pointToVertex = displacedVertices[i] - point;
		pointToVertex *= uniformScale;
		float attenuatedForce = force / (1f + pointToVertex.sqrMagnitude); //这保证当距离为零时，力处于全强度,而不是无限大 Fv=Fd2;
		float velocity = attenuatedForce * Time.deltaTime;
		vertexVelocities[i] += pointToVertex.normalized * velocity;
	}

}