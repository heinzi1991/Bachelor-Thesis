using UnityEngine;
using UnityEngine.UI;

public class TurretChoice : MonoBehaviour {

	public TurretBluePrint landTurret;
	public TurretBluePrint airTurret;
	public TurretBluePrint combiTurret;

	public Sprite normalLandTurret;
	public Sprite normalAirTurret;
	public Sprite normalCombiTurret;

	public Sprite selectedLandTurret;
	public Sprite selectedAirTurret;
	public Sprite selectedCombiTurret;

	BuiltManager buildManager;

	void Awake() {
		buildManager = BuiltManager.instance;
	}

	public void SelectLandTurret() {

		Debug.Log("Land Turret Selected");

		Transform[] ts = GetComponentsInChildren<Transform>();

		foreach (Transform one in ts) {
			if (one.gameObject.name == "LandTurret") {
				Image buttonImage = one.gameObject.GetComponent<Image>();
				buttonImage.sprite = selectedLandTurret;
			}

			if (one.gameObject.name == "AirTurret") {
				Image buttonImage = one.gameObject.GetComponent<Image>();
				buttonImage.sprite = normalAirTurret;
			}

			if (one.gameObject.name == "CombiTurret") {
				Image buttonImage = one.gameObject.GetComponent<Image>();
				buttonImage.sprite = normalCombiTurret;
			}
		}

		buildManager.SelectTurretToBuild(landTurret);
	}

	public void SelectAirTurret() {

		Debug.Log("Air Turret Selected");

		Transform[] ts = GetComponentsInChildren<Transform>();

		foreach (Transform one in ts) {
			if (one.gameObject.name == "AirTurret") {
				Image buttonImage = one.gameObject.GetComponent<Image>();
				buttonImage.sprite = selectedAirTurret;
			}

			if (one.gameObject.name == "LandTurret") {
				Image buttonImage = one.gameObject.GetComponent<Image>();
				buttonImage.sprite = normalLandTurret;
			}

			if (one.gameObject.name == "CombiTurret") {
				Image buttonImage = one.gameObject.GetComponent<Image>();
				buttonImage.sprite = normalCombiTurret;
			}
		}
			
		buildManager.SelectTurretToBuild(airTurret);
	}

	public void SelectCombiTurret() {

		Debug.Log("Combi Turret Selected");

		Transform[] ts = GetComponentsInChildren<Transform>();

		foreach (Transform one in ts) {
			if (one.gameObject.name == "CombiTurret") {
				Image buttonImage = one.gameObject.GetComponent<Image>();
				buttonImage.sprite = selectedCombiTurret;
			}

			if (one.gameObject.name == "AirTurret") {
				Image buttonImage = one.gameObject.GetComponent<Image>();
				buttonImage.sprite = normalAirTurret;
			}

			if (one.gameObject.name == "LandTurret") {
				Image buttonImage = one.gameObject.GetComponent<Image>();
				buttonImage.sprite = normalLandTurret;
			}
		}
			
		buildManager.SelectTurretToBuild(combiTurret);
	}

	void Update() {

		if (Input.GetKeyDown(KeyCode.Q)) {
			Debug.Log ("No Turret Selected");

			Transform[] ts = GetComponentsInChildren<Transform>();

			foreach (Transform one in ts) {

				if (one.gameObject.name == "LandTurret") {
					Image buttonImage = one.gameObject.GetComponent<Image>();
					buttonImage.sprite = normalLandTurret;
				}

				if (one.gameObject.name == "AirTurret") {
					Image buttonImage = one.gameObject.GetComponent<Image>();
					buttonImage.sprite = normalAirTurret;
				}

				if (one.gameObject.name == "CombiTurret") {
					Image buttonImage = one.gameObject.GetComponent<Image>();
					buttonImage.sprite = normalCombiTurret;
				}
			}

			buildManager.SelectTurretToBuild(null);
		}
	}
}
