using komal.puremvc;
using UnityEngine;

public class MeshDeformerInput : ComponentEx, INotificationHandler {

	public float force = 10f;
	public float forceOffset = 0.1f;
	GameContrProxy GameContrProxy;
	private void Start () {
		GameContrProxy = this.facade.RetrieveProxy (ProxyNameEnum.GameContrProxy) as GameContrProxy;
	}
	void Update () {
		if (Input.GetMouseButton (0)) {
			HandleInput ();
		}
	}

	void HandleInput () {
		Ray inputRay = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;

		if (Physics.Raycast (inputRay, out hit)) {
			MeshDeformer deformer = hit.collider.GetComponent<MeshDeformer> ();
			if (deformer) {
				Vector3 point = hit.point;
				point += hit.normal * forceOffset;
				deformer.AddDeformingForce (point, force);
				GameContrProxy.onTouchCub (hit.collider.transform.position, hit.collider.transform);
			}
			if (hit.collider.transform.name == "RayPlan") {
				Vector3 point = hit.point;
				GameObject linePrefab = GameContrProxy.linePrefabNow;
				if (linePrefab != null) {
					linePrefab.transform.GetComponent<DrawLineTool> ().drag (point);
				}
			}
		}
	}
}