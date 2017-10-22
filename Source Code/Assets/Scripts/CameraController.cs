using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public float panSpeed = 30f;
	public float panBorderThickness = 10f;
	public float scrollSpeed = 5f;
	public float minY = 10f;
	public float maxY = 80f;

	private bool doMovement = false;

	void Update() {

		if (Input.GetKeyDown("c")) {
			doMovement = !doMovement;
		}

		if (!doMovement) {
			return;
		}

		if (Input.GetKeyDown("w") || Input.mousePosition.y >= Screen.height - panBorderThickness) {
			transform.Translate(Vector3.forward * panSpeed * Time.deltaTime, Space.World);
		}

		if (Input.GetKeyDown("s") || Input.mousePosition.y <= panBorderThickness) {
			transform.Translate(Vector3.back * panSpeed * Time.deltaTime, Space.World);
		}

		if (Input.GetKeyDown("d") || Input.mousePosition.x >= Screen.width - panBorderThickness) {
			transform.Translate(Vector3.right * panSpeed * Time.deltaTime, Space.World);
		}

		if (Input.GetKeyDown("a") || Input.mousePosition.x <= panBorderThickness) {
			transform.Translate(Vector3.left * panSpeed * Time.deltaTime, Space.World);
		}

		float scroll = Input.GetAxis("Mouse ScrollWheel");

		Vector3 pos = transform.position;

		pos.y -= scroll * 1000 * scrollSpeed * Time.deltaTime;
		pos.y = Mathf.Clamp(pos.y, minY, maxY);

		transform.position = pos;
	}
}
