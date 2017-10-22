using UnityEngine;
using UnityEngine.UI;

public class BuiltManager : MonoBehaviour {

	public static BuiltManager instance;
	public GameObject misslePrefab;


	private TurretBluePrint turretToBuild;
	private Transform[] misslePosition;
	private GridManager gridManager;

	private int landTurretCounter = 0;
	private int airTurretCounter = 0;
	private int combiTurretCounter = 0;


	void Awake() {

		if (instance != null) {
			Debug.LogError("More than one BuildManager in Scene!");
			return;
		}

		instance = this;
		gridManager = GridManager.instance;
	}

	public void SelectTurretToBuild(TurretBluePrint turret_) {

		turretToBuild = turret_;
	}

	public void BuildTurretOn(Node node) {

		if (!gridManager.noCoinsPlay) {
			if (gridManager.coinsAmount < turretToBuild.cost) {
				Debug.Log("Sorry, you have not enough coins");
				return;
			}
		}
	
		GameObject turret = (GameObject)Instantiate(turretToBuild.turretPrefab, node.GetBuildPosition(), Quaternion.identity);

		if (turretToBuild.turretPrefab.name == "AirTurret" || turretToBuild.turretPrefab.name == "CombiTurret") {

			Transform positionGameObject = turret.transform.Find("PointToRotate/MisslePositions");
			misslePosition = new Transform[positionGameObject.transform.childCount];

			for (int i = 0; i < misslePosition.Length; i++) {

				misslePosition[i] = positionGameObject.transform.GetChild(i);
			}

			foreach (Transform onePosition in misslePosition) {

				GameObject missleObject = (GameObject)Instantiate(misslePrefab, onePosition.position, onePosition.rotation);
				Destroy(onePosition.gameObject);
				missleObject.name = "MissleDummy";
				missleObject.transform.parent = positionGameObject;
			}
		}

		//Turret turretScript = turret.GetComponent<Turret>();


		if (turretToBuild.turretPrefab.name == "LandTurret" || turretToBuild.turretPrefab.name == "AirTurret") {

			Turret turretScript = turret.GetComponent<Turret>();

			if (turretToBuild.turretPrefab.name == "LandTurret") {
				turretScript.counter = landTurretCounter;
				turretScript.turretCost = turretToBuild.cost;
				landTurretCounter++;
			}
			else {
				turretScript.counter = airTurretCounter;
				turretScript.turretCost = turretToBuild.cost;
				airTurretCounter++;
			}
		}
		else {

			CombiTurret combiTurretScript = turret.GetComponent<CombiTurret>();

			combiTurretScript.counter = combiTurretCounter;
			combiTurretScript.turretCost = turretToBuild.cost;
			combiTurretCounter++;
		}


		if (!gridManager.noCoinsPlay) {
			gridManager.coinsAmount -= turretToBuild.cost;
		}


		node.turret = turret;
	}


	public bool CanBuild { get { return turretToBuild != null; } }
}
