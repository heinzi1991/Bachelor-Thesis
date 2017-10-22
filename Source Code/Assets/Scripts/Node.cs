using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class Node : MonoBehaviour {

	public Color hoverColor;
	public Vector3 offset;

	[Header("Optional")]
	public GameObject turret;

	private Renderer rend;
	private Color startColor;

	public List<GameObject> neighbours;

	private GridManager gridManager;
	private BuiltManager builtManager;
	private StateManager stateManager;

	void Awake() {
		gridManager = GridManager.instance;
		builtManager = BuiltManager.instance;
		stateManager = StateManager.instance;
	}

	void Start() {


		rend = GetComponent<Renderer>();
		startColor = rend.material.color;
		neighbours = new List<GameObject>();
	}

	void OnMouseOver() {

		if (gridManager.gameStatusString == "stop") {

			if (Input.GetMouseButtonDown(0)) {

				if (!gridManager.gridStartField) {

					rend.material.color = Color.green;
					gridManager.gridStartField = true;

					neighbours = gridManager.getNeighbours(this.name);

					gridManager.gridStartNode = gridManager.getGameObjectFromNode(this.name);

					GameObject empty = new GameObject("NoTurretBuilt");
					empty.transform.parent = this.transform;
					turret = empty;
				}
				else {

					if (gridManager.gridMaxEndFields != gridManager.gridCountOfEndFields) {

						rend.material.color = Color.red;
						gridManager.gridCountOfEndFields = 1;

						neighbours = gridManager.getNeighbours(this.name);

						gridManager.AddEndNodesToList(gridManager.getGameObjectFromNode(this.name));

						GameObject empty = new GameObject("NoTurretBuilt");
						empty.transform.parent = this.transform;
						turret = empty;
					}
					else {

						Debug.Log("Maximum of End Fields is arrive");
					}
				}
			}

			if (Input.GetMouseButtonDown(1)) {

				if (rend.material.color == hoverColor || rend.material.color == startColor) {

					rend.material.color = Color.black;
					gridManager.AddNodeToPath(gridManager.getGameObjectFromNode(this.name));

					neighbours = gridManager.getNeighbours(this.name);

					GameObject empty = new GameObject("NoTurretBuilt");
					GameObject middlePoint = GameObject.CreatePrimitive(PrimitiveType.Cube);
					middlePoint.transform.position = this.transform.position;
					middlePoint.gameObject.layer = 9;
					middlePoint.transform.localScale = new Vector3(0.25f, 0.5f, 0.25f);
					empty.transform.parent = this.transform;
					middlePoint.transform.parent = this.transform;
					turret = empty;
				}
				else {

					rend.material.color = startColor;
					gridManager.DeleteNodeFromPath(gridManager.getGameObjectFromNode(this.name));

					neighbours = null;
					GameObject empty = this.transform.GetChild(0).gameObject;
					Destroy(empty);
					turret = null;
				}
			}
		}
		else {
			
			if (Input.GetMouseButtonDown(0)) {

				if (turret != null) {
					ActiveVisibleRange(turret.name);
					return;
				}

				if (EventSystem.current.IsPointerOverGameObject()) {
					return;
				}

				if (!builtManager.CanBuild) {
					return;
				}
					
				//DeactiveVisibleRange("LandTurret(Clone)");
				//DeactiveVisibleRange("AirTurret(Clone)");
				//DeactiveVisibleRange("CombiTurret(Clone)");
				builtManager.BuildTurretOn(this);
			}

			if (Input.GetMouseButtonDown(1)) {

				stateManager.ShowBalancingOfTurret(turret);
				stateManager.SetInsideBalancePanels("turret");
			}
		}
	}
		
	void OnMouseEnter() {

		if (rend.material.color == startColor && turret == null) {

			rend.material.color = hoverColor;
		}
	}

	void OnMouseExit() {

		if (rend.material.color == hoverColor) {

			rend.material.color = startColor;
		}
	}

	void ActiveVisibleRange(string turretName) {

		if (string.Compare(turretName, "LandTurret(Clone)") == 0) {

			DeactiveVisibleRange("AirTurret(Clone)");
			DeactiveVisibleRange("CombiTurret(Clone)");

			stateManager.SetActiveTurret = "land";

			GameObject[] landTurrets = GameObject.FindGameObjectsWithTag("LandTurret");

			foreach (GameObject oneTurret in landTurrets) {

				Turret temp = oneTurret.GetComponent<Turret>();
				temp.setMarket = true;
			}
		}
		else if (string.Compare(turretName, "AirTurret(Clone)") == 0) {

			DeactiveVisibleRange("LandTurret(Clone)");
			DeactiveVisibleRange("CombiTurret(Clone)");

			stateManager.SetActiveTurret = "air";

			GameObject[] airTurrets = GameObject.FindGameObjectsWithTag("AirTurret");

			foreach (GameObject oneTurret in airTurrets) {

				Turret temp = oneTurret.GetComponent<Turret>();
				temp.setMarket = true;
			}
		}
		else {

			DeactiveVisibleRange("AirTurret(Clone)");
			DeactiveVisibleRange("LandTurret(Clone)");

			stateManager.SetActiveTurret = "combi";

			GameObject[] combiTurrets = GameObject.FindGameObjectsWithTag("CombiTurret");

			foreach (GameObject oneTurret in combiTurrets) {

				CombiTurret temp = oneTurret.GetComponent<CombiTurret>();
				temp.setMarket = true;
			}
		}
	}

	void DeactiveVisibleRange(string turretName) {

		if (string.Compare(turretName, "LandTurret(Clone)") == 0) {

			GameObject[] landTurrets = GameObject.FindGameObjectsWithTag("LandTurret");

			foreach (GameObject oneTurret in landTurrets) {

				Turret temp = oneTurret.GetComponent<Turret>();
				temp.setMarket = false;
			}
		}
		else if (string.Compare(turretName, "AirTurret(Clone)") == 0) {

			GameObject[] airTurrets = GameObject.FindGameObjectsWithTag("AirTurret");

			foreach (GameObject oneTurret in airTurrets) {

				Turret temp = oneTurret.GetComponent<Turret>();
				temp.setMarket = false;
			}
		}
		else {

			GameObject [] combiTurrets = GameObject.FindGameObjectsWithTag("CombiTurret");

			foreach (GameObject oneTurret in combiTurrets) {

				CombiTurret temp = oneTurret.GetComponent<CombiTurret>();
				temp.setMarket = false;
			}
		}
	}
		
	public Vector3 GetBuildPosition() {

		return transform.position + offset;
	}
}
