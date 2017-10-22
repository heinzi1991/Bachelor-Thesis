using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CreateField : MonoBehaviour {

	public GameObject nodePrefab;
	public Transform overNode;

	public InputField gridHeight;
	public InputField gridWidth;
	public InputField gridEndFields;

	public InputField enemyAmountField;
	public InputField timeBetweenWavesField;
	public InputField timeBeforeFirstWave;

	public InputField coinAmountField;
	public InputField playerHealthField;

	public Text startPauseButtonText;

	private GridManager gridManager;
	private StateManager stateManager;
	private GridOption gridOption;
	private WaveSpawner waveSpawner;
	private PauseManager pauseManager;

	private int gridHeightValue;
	private int gridWidthValue;
	private int gridEndFieldsValue;

	private int enemyAmount;
	private float timeBetweenWaves;
	private float timeBeforeWave;

	private int coinAmount;
	private int playerHealth;

	void Awake() {
		gridManager = GridManager.instance;
	}

	void Start() {

		stateManager = GetComponent<StateManager>();
		gridOption = GetComponent<GridOption>();
		waveSpawner = GetComponent<WaveSpawner>();
		pauseManager = GetComponent<PauseManager>();

		waveSpawner.enabled = false;
	}

	public void CheckInputFields() {

		if (int.TryParse(gridHeight.text, out gridHeightValue) && int.TryParse(gridWidth.text, out gridWidthValue) && 
			int.TryParse(gridEndFields.text, out gridEndFieldsValue) && int.TryParse(enemyAmountField.text, out enemyAmount) && 
			float.TryParse(timeBetweenWavesField.text, out timeBetweenWaves) && float.TryParse(timeBeforeFirstWave.text, out timeBeforeWave)) {

			if (!(gridHeightValue > 0 && gridHeightValue <= 20) || !(gridWidthValue > 0 && gridWidthValue <= 20) || !(gridEndFieldsValue > 0 && gridEndFieldsValue <= 10)) {

				gridManager.showErrorMessagePanel("Sorry but the Grid Height or the Grid Width or the value of the Endfields is greater as the maximum!");
			}
			else {

				if (!(enemyAmount > 0 && enemyAmount <= 50)) {
					gridManager.showErrorMessagePanel("Sorry but the enemy amount is greater then 50!");
					return;
				}
			
				if ((enemyAmount * 0.5f) >= timeBetweenWaves) {
					gridManager.showErrorMessagePanel("Sorry but the time between waves is greater as the enemy spawn time!\n You must choose a time greater then "
						+ Mathf.Ceil((enemyAmount * 0.5f)));
					return;
				}

				if (coinAmount > 9999) {
					gridManager.showErrorMessagePanel("Sorry but the amount of coins is greater then 9999!");
					return;
				}


				gridOption.getSettingsBool = false;
				gridManager.gridMaxEndFields = gridEndFieldsValue;
				gridManager.enemyAmountValue = enemyAmount;
				gridManager.timeWaves = timeBetweenWaves;
				gridManager.firstWaveTime = timeBeforeWave;

				if (coinAmountField.text == "") {
					gridManager.noCoinsPlay = true;
				}
				else {
					int.TryParse(coinAmountField.text, out coinAmount);
					gridManager.coinsAmount = coinAmount;
				}

				if (playerHealthField.text == "") {
					gridManager.noHealthPlay = true;
				}
				else {
					int.TryParse(playerHealthField.text, out playerHealth);
					gridManager.playerHealthStatus = playerHealth;
				}

				stateManager.GetSpawnRate = enemyAmount;

				CreateGrid();
			}
		}
		else {

			gridManager.showErrorMessagePanel("Sorry but one or more input field(s) has not only integers!");
		}
	}


	public void CreateGrid() {

		for (int i = 0; i < gridWidthValue * gridManager.gridOffset; i += gridManager.gridOffset) {

			for (int j = 0; j < gridHeightValue * gridManager.gridOffset; j += gridManager.gridOffset) {
				
				GameObject node = (GameObject)Instantiate(nodePrefab, new Vector3(i, 0, j), Quaternion.identity);
				node.name = "Node (" + i + "," + j + ")";
				node.transform.parent = overNode;

				gridManager.AddToDictionary(node.name, node);
			}
		}
	}

	public void StartAndPauseGame() {

		if (string.Compare(gridManager.gameStatusString, "stop") == 0) {

			bool connect = gridManager.pathConnect(null);

			if (connect) {

				gridManager.gameStatusString = "run";
				startPauseButtonText.text = "Pause";
				gridManager.calcAllPossibleWays();
				waveSpawner.enabled = true;
			}
		}
		else {

			pauseManager.Toggle (startPauseButtonText);
		}
	}
}
