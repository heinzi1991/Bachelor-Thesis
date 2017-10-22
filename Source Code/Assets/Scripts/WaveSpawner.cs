using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour {

	public Transform landEnemyPrefab;
	public Transform airEnemyPrefab;
	public Transform bossEnemyPrefab;
	//public float timeBetweenWaves = 5.0f;
	public GameObject playerInfoPanel;
	public GameObject builtManagerPanel;

	public Text countDownText;
	public Text waveCounterText;

	public BarChart arriveBarChart;
	public BarChart killBarChart;

	//private float countDown = 2.0f;
	private GridManager gridManager;
	private StateManager stateManager;
	private float countDown = 0f;
	private float timeBetweenWaves = 0f;
	private int enemyPerWave = 0;
	private int waveCount = 0;
	private string timeStamp = "";


	void Awake() {
		gridManager = GridManager.instance;
		stateManager = GetComponent<StateManager>();
	}

	void Start() {

		builtManagerPanel.SetActive(true);
		playerInfoPanel.SetActive(true);

		countDown = gridManager.firstWaveTime;
		timeBetweenWaves = gridManager.timeWaves;
		enemyPerWave = gridManager.enemyAmountValue;
	}

	void Update() {

		if (waveCount == 0) {
			waveCounterText.text = "Wave 0";
		}

		if (waveCount >= 1) {
			stateManager.afterFirstWave = true;
		}

		if (countDown <= 0.0f) {

			if (waveCount >= 1) {
				SendData();
			}
				
			waveCount++;
			StartCoroutine(SpawnWave());
			waveCounterText.text = "Wave " + waveCount.ToString();
			countDown = timeBetweenWaves;
		}
			
		countDown -= Time.deltaTime;
		countDown = Mathf.Clamp(countDown, 0f, Mathf.Infinity);
		timeStamp = string.Format("{0:00.00}", countDown);

		countDownText.text = "Next Wave: " + timeStamp;
	}

	IEnumerator SpawnWave() {

		if (waveCount % 10 == 0) {
			SpawnBoss();
		}

		enemyPerWave = stateManager.GetSpawnRate;
			
		for (int i = 0; i < enemyPerWave; i++) {

			SpawnEnemy();
			yield return new WaitForSeconds(0.5f);
		}
	}

	void SpawnEnemy() {

		int randomNumber = Random.Range(0, 1000);

		if (randomNumber % 2 == 0) {
			Instantiate(landEnemyPrefab, gridManager.gridStartNode.transform.position + new Vector3(0, 1.3f, 0), gridManager.gridStartNode.transform.rotation);
		}
		else {
			Instantiate(airEnemyPrefab, gridManager.gridStartNode.transform.position + new Vector3(0, 5f, 0), Quaternion.identity);
		}
	}

	void SpawnBoss() {

		Instantiate(bossEnemyPrefab, gridManager.gridStartNode.transform.position + new Vector3(0, 2.5f, 0), gridManager.gridStartNode.transform.rotation);
	}

	void SendData() {

		arriveBarChart.AddValuesToLists(stateManager.AmountOfEnemies, "arrive");
		arriveBarChart.DisplayGraph("arrive");

		killBarChart.AddValuesToLists(stateManager.AmountOfKillEnemies, "kill");
		killBarChart.DisplayGraph("kill");

		stateManager.AmountOfEnemies = 0;
		stateManager.AmountOfKillEnemies = 0;
	}

	public void OpenCloseBuiltManager() {

		if (builtManagerPanel.activeSelf) {
			builtManagerPanel.SetActive(false);
		}
		else {
			builtManagerPanel.SetActive(true);
		}
	}
}
