using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GridManager : MonoBehaviour {

	public static GridManager instance;

	public int offset;
	public Transform wayPointsParent;
	public GameObject errorPanel;
	public Text errorMessageText;
	public Text coinsAmountText;
	public Text playerHealthText;


	private Dictionary<string, GameObject> grid;
	private List<GameObject> path;
	private List<GameObject> endNodes;
	private GameObject startNode;
	private bool isStartField = false;
	private int countOfEndFields = 0;
	private int maxEndFields = 0;
	private string gameStatus = "stop";

	private List<GameObject> visitedList;

	private Dictionary<string, List<GameObject>> landWayPointsDict;
	private Dictionary<string, List<GameObject>> airWayPointsDict;

	private int enemyAmount = 0;
	private float timeBetweenWaves = 0f;
	private float waitForFirstWave = 0f;

	private int coinAmount = 0;
	private int playerHealth = 0;

	private bool gameOver = false;

	private bool noCoinGame = false;
	private bool noHealthGame = false;

	private StateManager stateManager;

	void Awake() {

		if (instance != null) {
			Debug.LogError("More than one GridManager in Scene!");
			return;
		}

		instance = this;

		grid = new Dictionary<string, GameObject>();
		path = new List<GameObject>();
		endNodes = new List<GameObject>();

		visitedList = new List<GameObject>();

		landWayPointsDict = new Dictionary<string, List<GameObject>>();
		airWayPointsDict = new Dictionary<string, List<GameObject>>();

		stateManager = GetComponent<StateManager>();
	}

	void Update() {

		if (noCoinGame) {
			coinsAmountText.text = "---";
		}
		else {
			coinsAmountText.text = coinAmount.ToString();
		}


		if (noHealthGame) {
			playerHealthText.text = "---";
		}
		else {
			playerHealthText.text = playerHealth.ToString();
		}

		if (gameOver) {
			Time.timeScale = 0f;
			showErrorMessagePanel("Sorry you have no lives!\n Please restart the simulation!");
		}

		if (Input.GetKeyDown(KeyCode.Space)) {
			DeactiveAllRanges();
		}
	}

	public bool gridStartField { get { return isStartField; } set { isStartField = value; } }
	public int gridCountOfEndFields { get { return countOfEndFields; } set {countOfEndFields += value; } }
	public int gridMaxEndFields { get { return maxEndFields; } set { maxEndFields = value; } }
	public int gridOffset { get { return offset; } }
	public GameObject gridStartNode { get { return startNode; } set { startNode = value; } }
	public string gameStatusString { get { return gameStatus; } set { gameStatus = value; } }
	public List<GameObject> getEndNodeList { get { return endNodes; } }
	public int enemyAmountValue { get { return enemyAmount; } set { enemyAmount = value; } }
	public float timeWaves { get { return timeBetweenWaves; } set { timeBetweenWaves = value; } }
	public float firstWaveTime { get { return waitForFirstWave; } set { waitForFirstWave = value; } }
	public List<GameObject> getPathList { get { return path; } }
	public int coinsAmount { get { return coinAmount; } set { coinAmount = value; } }
	public int playerHealthStatus { get { return playerHealth; } set { playerHealth = value; } }
	public bool noCoinsPlay {get { return noCoinGame; } set { noCoinGame = value; } }
	public bool noHealthPlay { get { return noHealthGame; } set { noHealthGame = value; } }
	public bool isGameOver { set { gameOver = value; } }


	public void AddToDictionary(string key, GameObject value) {
		grid.Add(key, value);
	}

	public void AddNodeToPath(GameObject pathObject) {
		path.Add(pathObject);
	}

	public void DeleteNodeFromPath(GameObject pathObject) {
		path.Remove(pathObject);
	}

	public void AddEndNodesToList(GameObject endNode) {
		endNodes.Add(endNode);
	}

	public GameObject getGameObjectFromNode(string nodeName) {
		return grid[nodeName];
	}

	public List<GameObject> getLandWayPointsFromDict(string endNodeName) {
		return landWayPointsDict[endNodeName];
	}

	public List<GameObject> getAirWayPointsFromDict(string endNodeName) {
		return airWayPointsDict[endNodeName];
	}
		

	public List<GameObject> getNeighbours(string key) {

		List<GameObject> neighbours = new List<GameObject>();

		string[] digits = Regex.Split(key, @"\D+");

		int firstNumber = int.Parse(digits[1]);
		int secondNumber = int.Parse(digits[2]);

		int plusFirstNumber = firstNumber + 5;
		int minusFirstNumber = firstNumber - 5;
		int plusSecondNumber = secondNumber + 5;
		int minusSecondNumber = secondNumber - 5;

		string neighbourUp =  "Node (" + firstNumber + "," + plusSecondNumber + ")";
		string neighbourDown =  "Node (" + firstNumber + "," + minusSecondNumber + ")";
		string neighbourRight =  "Node (" + plusFirstNumber + "," + secondNumber + ")";
		string neighbourLeft =  "Node (" + minusFirstNumber + "," + secondNumber + ")";

		if (grid.ContainsKey(neighbourUp)) {

			neighbours.Add(grid[neighbourUp]);
		}

		if (grid.ContainsKey(neighbourDown)) {

			neighbours.Add(grid[neighbourDown]);
		}

		if (grid.ContainsKey(neighbourLeft)) {

			neighbours.Add(grid[neighbourLeft]);
		}

		if (grid.ContainsKey(neighbourRight)) {

			neighbours.Add(grid[neighbourRight]);
		}

		return neighbours;
	}



	public bool pathConnect(Node check) {

		int breakCounter = 0;

		Node currentNode = check;

		if (currentNode == null) {

			if (path.Count == 0) {

				return false;
			}

			currentNode = startNode.GetComponent<Node>();
		}
			
		while(true) {

			breakCounter++;

			if (breakCounter == 100) {
				return false;
			}
				
			List<int> neighbourNumber = new List<int>();
		
			//Get neighbours from current node

			for (int i = 0; i < currentNode.neighbours.Count; i++) {

				if (path.Contains(currentNode.neighbours[i])) {

					neighbourNumber.Add(i);
				}
			}
				
			if (neighbourNumber.Count == 1) {

				foreach (GameObject o in endNodes) {

					if (currentNode.neighbours.Contains(o)) {

						return true;
					}
				}

				visitedList.Add(currentNode.gameObject);
										
				currentNode = currentNode.neighbours[neighbourNumber[0]].GetComponent<Node>();

				if (visitedList.Contains(currentNode.gameObject)) {

					return false;
				}
			}
			else if (neighbourNumber.Count == 2) {

				int visitedCount = 0;

				foreach (int v in neighbourNumber) {

					if (!visitedList.Contains(currentNode.neighbours[v])) {

						visitedList.Add(currentNode.gameObject);

						currentNode = currentNode.neighbours[v].GetComponent<Node>();
						break;
					}
					else {
						visitedCount++;
					}
				}

				if (visitedCount == 2) {
					visitedList.Add(currentNode.gameObject);
					return true;
				}
			}
			else if (neighbourNumber.Count >= 3) {

				Node tempNode = currentNode;
				int moreWays = 0;

				foreach (int v in neighbourNumber) {

					if (!visitedList.Contains(tempNode.neighbours[v])) {

						visitedList.Add(currentNode.gameObject);

						currentNode = tempNode.neighbours[v].GetComponent<Node>();

						if (pathConnect(currentNode)) {
							moreWays++;
						}
						else {
							return false;
						}
					}
				}

				if (moreWays == 2 || moreWays == 3) {

					return true;
				}
			}
			else {

				return false;
			}
		}
	}
		
	public void calcAllPossibleWays() {

		foreach (GameObject endNode in endNodes) {

			List<PathFindingClass> mainList = new List<PathFindingClass>();

			mainList.Add(new PathFindingClass(endNode.transform.position.x, endNode.transform.position.z, 0));
				
			for (int i = 0; i < mainList.Count; i++) {

				if (mainList.Count == (path.Count + 1)) {
					break;
				}

				List<GameObject> neighbourObjectList = new List<GameObject>();
				List<PathFindingClass> neighbourArrayList = new List<PathFindingClass>();
				string neighbourKey = "Node (" + mainList[i].x_coord + "," + mainList[i].z_coord + ")";

				neighbourObjectList = getNeighbours(neighbourKey);

				for (int j = 0; j < neighbourObjectList.Count; j++) {

					neighbourArrayList.Add(new PathFindingClass(neighbourObjectList[j].transform.position.x, neighbourObjectList[j].transform.position.z, mainList[i].counter_value + 1));
				}

				for (int j = neighbourObjectList.Count - 1; j >= 0; j--) {

					if (!path.Contains(neighbourObjectList[j])) {
						neighbourArrayList.RemoveAt(j);
					}
				}

				for (int j = neighbourArrayList.Count - 1; j >= 0; j--) {

					for (int k = 0; k < mainList.Count; k++) {

						if ((neighbourArrayList[j].x_coord == mainList[k].x_coord) && (neighbourArrayList[j].z_coord == mainList[k].z_coord)) {

							if  (neighbourArrayList[j].counter_value >= mainList[k].counter_value) {

								neighbourArrayList.RemoveAt(j);
								break;
							}
						}
					}
				}
					
				for (int j = 0; j < neighbourArrayList.Count; j++) {
					mainList.Add(neighbourArrayList[j]);
				}
			}

			finishWayCalculation(endNode.name, mainList);

		}
	}

	void finishWayCalculation(string endNodeName, List<PathFindingClass> listOfAllWays) {

		List<GameObject> startNodeNeighbours = startNode.GetComponent<Node>().neighbours;
		int startPathIndex = 0;


		List<PathFindingClass> shortPathWayPoints = new List<PathFindingClass>();

		for (int i = 0; i < listOfAllWays.Count; i++) {

			for (int j = 0; j < startNodeNeighbours.Count; j++) {

				if (listOfAllWays[i].x_coord == startNodeNeighbours[j].transform.position.x && listOfAllWays[i].z_coord == startNodeNeighbours[j].transform.position.z) {

					shortPathWayPoints.Add(listOfAllWays[i]);
					startPathIndex = i;
				}
			}
		}

		for (int i = listOfAllWays[startPathIndex].counter_value - 1; i >= 0; i--) {

			List<PathFindingClass> matches = listOfAllWays.Where(x => x.counter_value == i).ToList();

			foreach (PathFindingClass oneElement in matches) {

				if (oneElement.x_coord == shortPathWayPoints.Last().x_coord) {

					if (shortPathWayPoints.Last().z_coord - 5 == oneElement.z_coord || shortPathWayPoints.Last().z_coord + 5 == oneElement.z_coord) {

						shortPathWayPoints.Add(oneElement);
					}
				}

				if (oneElement.z_coord == shortPathWayPoints.Last().z_coord) {

					if (shortPathWayPoints.Last().x_coord - 5 == oneElement.x_coord || shortPathWayPoints.Last().x_coord + 5 == oneElement.x_coord) {

						shortPathWayPoints.Add(oneElement);
					}
				}
			}
		}

		List<GameObject> landWayPointList = new List<GameObject>();
		List<GameObject> airWayPointList = new List<GameObject>();

		foreach (PathFindingClass oneElement in shortPathWayPoints) {

			string findingGameObject = "Node (" + oneElement.x_coord + "," + oneElement.z_coord + ")";

			GameObject node = getGameObjectFromNode(findingGameObject);

			GameObject newLandWayPoint = new GameObject();
			GameObject newAirWayPoint = new GameObject();

			newLandWayPoint.transform.position = node.transform.position + new Vector3(0, 1.3f, 0);
			newAirWayPoint.transform.position = node.transform.position + new Vector3(0, 5f, 0);

			newLandWayPoint.transform.parent = wayPointsParent;
			newAirWayPoint.transform.parent = wayPointsParent;

			landWayPointList.Add(newLandWayPoint);
			airWayPointList.Add(newAirWayPoint);
		}

		landWayPointsDict.Add(endNodeName, landWayPointList);
		airWayPointsDict.Add(endNodeName, airWayPointList);

	}

	void DeactiveAllRanges() {

		GameObject[] landTurrets = GameObject.FindGameObjectsWithTag("LandTurret");
		GameObject[] airTurrets = GameObject.FindGameObjectsWithTag("AirTurret");
		GameObject[] combiTurrets = GameObject.FindGameObjectsWithTag("CombiTurret");

		foreach (GameObject oneTurret in landTurrets) {
			Turret temp = oneTurret.GetComponent<Turret>();
			temp.setMarket = false;
		}

		foreach (GameObject oneTurret in airTurrets) {
			Turret temp = oneTurret.GetComponent<Turret>();
			temp.setMarket = false;
		}

		foreach (GameObject oneTurret in combiTurrets) {
			CombiTurret temp = oneTurret.GetComponent<CombiTurret>();
			temp.setMarket = false;
		}
			
		stateManager.SetActiveTurret = "";
	}


	public void showErrorMessagePanel(string errorMessage) {
		errorMessageText.text = errorMessage;
		errorPanel.SetActive(true);
	}

	public void errorOkFunction() {
		errorPanel.SetActive(false);
	}
		
	public void Restart() {

		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
}