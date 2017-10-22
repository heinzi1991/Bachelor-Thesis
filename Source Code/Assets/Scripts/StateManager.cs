using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class StateManager : MonoBehaviour {

	public static StateManager instance;

	[Header("StateManagerPanels")]
	public GameObject turretPanel;
	public GameObject enemyPanel;
	public GameObject balanceStatsPanel;

	[Header("Info Text Panel")]
	public Text activeTurretStateText;
	public Text activeEnemyStateText;

	[Header("Turret States Panels")]
	public GameObject[] statePanels;

	[Header("Turret Range")]
	public Slider[] rangeSliders;
	public Text[] rangeSliderValues;

	[Header("Turret Fire Rate")]
	public Slider[] fireRateSliders;
	public Text[] fireRateSliderValues;

	[Header("Bullet / Missle Speed")]
	public Slider[] bulletSpeedSliders;
	public Text[] bulletSpeedValues;

	[Header("Bullet / Missle Damage")]
	public Slider[] bulletDamageSliders;
	public Text[] bulletDamageValues;

	[Header("Enemy Stats Panels")]
	public GameObject landEnemyStatsPanel;
	public GameObject airEnemyStatsPanel;
	public GameObject bossEnemyStatsPanel;

	[Header("Enemy Spawn Rate")]
	public Slider spawnRateSlider;
	public Text spawnRateText;

	[Header("Enemy Speed Stats")]
	public Slider[] enemySpeedSliders;
	public Text[] enemySpeedValues;

	[Header("Enemy Gain Stats")]
	public Slider[] enemyGainSliders;
	public Text[] enemyGainValues;

	[Header("Balancing Things")]
	public Text infoText;
	public GameObject turretStatsPanel;
	public GameObject diagramsPanel;
	public Text turretNameText;
	public Text turretTypeText;
	public Text turretCostText;
	public Text turretRangeText;
	public Text turretBulletFireRateText;
	public Text turretMissleFireRateText;
	public Text bulletDamageText;
	public Text missleDamageText;
	public Text shootsInRangeText;
	public Text damageOnEnemyText;
	public LayerMask mask;


	private GridManager gridManager;

	private string activeTurretType = "";

	private float landRange = 15f;
	private float airRange = 25f;
	private float combiRange = 20f;

	private float landFireRate = 1f;
	private float airFireRate = 0.25f;
	private float combiLandFireRate = 1f;
	private float combiAirFireRate = 0.25f;

	private float bulletSpeed = 70f;
	private float missleSpeed = 40f;
	private float combiBulletSpeed = 70f;
	private float combiMissleSpeed = 40f;

	private int bulletDamage = 20;
	private int missleDamage = 30;
	private int combiBulletDamage = 20;
	private int combiMissleDamage = 30;

	private int spawnRateValue = 0;

	private float landEnemySpeed = 10f;
	private float airEnemySpeed = 10f;
	private float endBossSpeed = 10f;

	private int landEnemyGain = 50;
	private int airEnemyGain = 50;
	private int endBossGain = 250;

	private bool finishFirstWave = false;

	private int enemiesArrive = 0;
	private int killEnemies = 0;



	// Use this for initialization
	void Awake () {

		if (instance != null) {
			Debug.LogError("More than one StateManager in Scene!");
			return;
		}

		instance = this;
		gridManager = GridManager.instance;

		turretPanel.SetActive(true);
		enemyPanel.SetActive(false);
		balanceStatsPanel.SetActive(false);

		landEnemyStatsPanel.SetActive(false);
		airEnemyStatsPanel.SetActive(false);
		bossEnemyStatsPanel.SetActive(false);

		turretStatsPanel.SetActive(false);
		diagramsPanel.SetActive(false);

		//Land Turret
		rangeSliders[0].value = landRange;
		fireRateSliders[0].value = landFireRate;
		bulletSpeedSliders[0].value = bulletSpeed;
		bulletDamageSliders[0].value = bulletDamage;

		//Air Turret
		rangeSliders[1].value = airRange;
		fireRateSliders[1].value = airFireRate;
		bulletSpeedSliders[1].value = missleSpeed;
		bulletDamageSliders[1].value = missleDamage;

		//Combi Turret
		rangeSliders[2].value = combiRange;
		fireRateSliders[2].value = combiLandFireRate;
		bulletSpeedSliders[2].value = combiBulletSpeed;
		bulletDamageSliders[2].value = combiBulletDamage;
		fireRateSliders[3].value = combiAirFireRate;
		bulletSpeedSliders[3].value = combiMissleSpeed;
		bulletDamageSliders[3].value = combiMissleDamage;

		//Spawn Rate
		spawnRateSlider.value = spawnRateValue;

		//Enemy Speed
		enemySpeedSliders[0].value = landEnemySpeed;
		enemySpeedSliders[1].value = airEnemySpeed;
		enemySpeedSliders[2].value = endBossSpeed;

		//Enemy Gain
		enemyGainSliders[0].value = landEnemyGain;
		enemyGainSliders[1].value = airEnemyGain;
		enemyGainSliders[2].value = endBossGain;
	}
		
	// Update is called once per frame
	void Update () {

		ActivatePanels();

		//Land Turret
		landRange = rangeSliders[0].value;
		rangeSliderValues[0].text = landRange.ToString("F2");

		landFireRate = fireRateSliders[0].value;
		fireRateSliderValues[0].text = landFireRate.ToString("F2");

		bulletSpeed = bulletSpeedSliders[0].value;
		bulletSpeedValues[0].text = bulletSpeed.ToString("F2");

		bulletDamage = (int)bulletDamageSliders[0].value;
		bulletDamageValues[0].text = bulletDamage.ToString();

		//Air Turret 
		airRange = rangeSliders[1].value;
		rangeSliderValues[1].text = airRange.ToString("F2");

		airFireRate = fireRateSliders[1].value;
		fireRateSliderValues[1].text = airFireRate.ToString("F2");

		missleSpeed = bulletSpeedSliders[1].value;
		bulletSpeedValues[1].text = missleSpeed.ToString("F2");

		missleDamage = (int)bulletDamageSliders[1].value;
		bulletDamageValues[1].text = missleDamage.ToString();

		//Combi Turret
		combiRange = rangeSliders[2].value;
		rangeSliderValues[2].text = combiRange.ToString("F2");

		combiLandFireRate = fireRateSliders[2].value;
		fireRateSliderValues[2].text = combiLandFireRate.ToString("F2");

		combiBulletSpeed = bulletSpeedSliders[2].value;
		bulletSpeedValues[2].text = combiBulletSpeed.ToString("F2");

		combiBulletDamage = (int)bulletDamageSliders[2].value;
		bulletDamageValues[2].text = combiBulletDamage.ToString();

		combiAirFireRate = fireRateSliders[3].value;
		fireRateSliderValues[3].text = combiAirFireRate.ToString("F2");

		combiMissleSpeed = bulletSpeedSliders[3].value;
		bulletSpeedValues[3].text = combiMissleSpeed.ToString("F2");

		combiMissleDamage = (int)bulletSpeedSliders[3].value;
		bulletDamageValues[3].text = combiMissleDamage.ToString();

		//Spawn Rate

		if (!finishFirstWave) {
			spawnRateSlider.value = spawnRateValue;
			spawnRateText.text = spawnRateValue.ToString();
		}
		else {
			spawnRateValue = (int)spawnRateSlider.value;
			spawnRateText.text = spawnRateValue.ToString();
		}

		//Enemy Speed
		landEnemySpeed = enemySpeedSliders[0].value;
		enemySpeedValues[0].text = landEnemySpeed.ToString("F2");

		airEnemySpeed = enemySpeedSliders[1].value;
		enemySpeedValues[1].text = airEnemySpeed.ToString("F2");

		endBossSpeed = enemySpeedSliders[2].value;
		enemySpeedValues[2].text = endBossSpeed.ToString("F2");

		//Enemy Gain
		landEnemyGain = (int)enemyGainSliders[0].value;
		enemyGainValues[0].text = landEnemyGain.ToString();

		airEnemyGain = (int)enemyGainSliders[1].value;
		enemyGainValues[1].text = airEnemyGain.ToString();

		endBossGain = (int)enemyGainSliders[2].value;
		enemyGainValues[2].text = endBossGain.ToString();
	}

	public string SetActiveTurret { set { activeTurretType = value; } }

	public float GetLandRange { get { return landRange; } }
	public float GetAirRange { get { return airRange; } }
	public float GetCombiRange { get { return combiRange; } }

	public float GetLandFireRate { get { return landFireRate; } }
	public float GetAirFireRate { get { return airFireRate; } }
	public float GetCombiLandFireRate { get { return combiLandFireRate; } }
	public float GetCombiAirFireRate { get { return combiAirFireRate; } }

	public float GetBulletSpeed { get { return bulletSpeed; } }
	public float GetMissleSpeed { get { return missleSpeed; } }
	public float GetCombiBulletSpeed { get { return combiBulletSpeed; } }
	public float GetCombiMissleSpeed { get { return combiMissleSpeed; } }

	public int GetBulletDamage { get { return bulletDamage; } }
	public int GetMissleDamage { get { return missleDamage; } }
	public int GetCombiBulletDamage { get { return combiBulletDamage; } }
	public int GetCombiMissleDamage { get { return combiMissleDamage; } }

	public int GetSpawnRate { get { return spawnRateValue; } set { spawnRateValue = value; } }
	public bool afterFirstWave {get { return finishFirstWave; } set { finishFirstWave = value; } }

	public float GetLandEnemySpeed { get { return landEnemySpeed; } }
	public float GetAirEnemySpeed { get { return airEnemySpeed; } }
	public float GetEndBossSpeed { get { return endBossSpeed; } }

	public int GetLandEnemyGain { get { return landEnemyGain; } }
	public int GetAirEnemyGain { get { return airEnemyGain; } }
	public int GetEndBossGain { get { return endBossGain; } }

	public int AmountOfEnemies { get { return enemiesArrive; } set { enemiesArrive = value; } }
	public int AmountOfKillEnemies { get { return killEnemies; } set { killEnemies = value; } }

	//Panel Button Functions
	public void SetTurretPanelActive() {

		turretPanel.SetActive(true);
		enemyPanel.SetActive(false);
		balanceStatsPanel.SetActive(false);
	}

	public void SetEnemyPanelActive() {

		turretPanel.SetActive(false);
		enemyPanel.SetActive(true);
		balanceStatsPanel.SetActive(false);
	}

	public void SetBalanceStatsPanelActive() {

		SetInsideBalancePanels("button");
	}

	//Enemy Type Button Functions
	public void SetLandEnemyStatsActive() {

		activeEnemyStateText.text = "Land Enemy";

		landEnemyStatsPanel.SetActive(true);
		airEnemyStatsPanel.SetActive(false);
		bossEnemyStatsPanel.SetActive(false);
	}

	public void SetAirEnemyStatsActive() {

		activeEnemyStateText.text = "Air Enemy";

		landEnemyStatsPanel.SetActive(false);
		airEnemyStatsPanel.SetActive(true);
		bossEnemyStatsPanel.SetActive(false);
	}

	public void SetBossEnemyStatsActive() {

		activeEnemyStateText.text = "Boss Enemy";

		landEnemyStatsPanel.SetActive(false);
		airEnemyStatsPanel.SetActive(false);
		bossEnemyStatsPanel.SetActive(true);
	}

	public void SetInsideBalancePanels(string type) {

		turretPanel.SetActive(false);
		enemyPanel.SetActive(false);

		if (type == "button") {
			diagramsPanel.SetActive(true);
			infoText.text = "You can see how many enemies arrive their goal and how many enemies are killed.";
			turretStatsPanel.SetActive(false);
		}
		else {
			turretStatsPanel.SetActive(true);
			infoText.text = "You can see all information about the tower";
			diagramsPanel.SetActive(false);
		}

		balanceStatsPanel.SetActive(true);
	}
		
	public void ShowBalancingOfTurret(GameObject turret) {

		float calculationRange = 0.0f;

		if (turret.name == "LandTurret(Clone)") {
			calculationRange = landRange;
		}
		else if (turret.name == "AirTurret(Clone)") {
			calculationRange = airRange;
		}
		else {
			calculationRange = combiRange;
		}


		Collider[] hitColliders = Physics.OverlapSphere(turret.transform.position, calculationRange);
		List<GameObject> pathInRange = new List<GameObject>();

		int i = 0;

		while(i < hitColliders.Length) {

			if (gridManager.getPathList.Contains(hitColliders[i].gameObject)) {

				Vector3 dir = hitColliders[i].gameObject.transform.position - turret.transform.position;

				if (Physics.Raycast(turret.transform.position, dir, calculationRange, mask.value)) {
					pathInRange.Add(hitColliders[i].gameObject);
				}
			}
				
			i++;
		}

		GameObject oneNode = gridManager.getGameObjectFromNode("Node (0,0)");
		float sizeOfNode = oneNode.GetComponent<Renderer>().bounds.size.x;
		float amountOfPixels = (pathInRange.Count * sizeOfNode) + ((pathInRange.Count - 1) * 0.5f);

		WriteTextValues(turret, amountOfPixels);
	}

	void ActivatePanels() {

		if (string.Compare(activeTurretType, "land") == 0) {
			activeTurretStateText.text = "Land Turret States";
			statePanels[0].SetActive(true);
			statePanels[1].SetActive(false);
			statePanels[2].SetActive(false);
		}
		else if (string.Compare(activeTurretType, "air") == 0) {
			activeTurretStateText.text = "Air Turret States";
			statePanels[0].SetActive(false);
			statePanels[1].SetActive(true);
			statePanels[2].SetActive(false);
		}
		else if (string.Compare(activeTurretType, "combi") == 0) {
			activeTurretStateText.text = "Combi Turret States";
			statePanels[0].SetActive(false);
			statePanels[1].SetActive(false);
			statePanels[2].SetActive(true);
		}
		else {
			activeTurretStateText.text = "";
			statePanels[0].SetActive(false);
			statePanels[1].SetActive(false);
			statePanels[2].SetActive(false);
		}
	}

	void WriteTextValues(GameObject turret, float rangeInPixel) {

		if (turret.name == "LandTurret(Clone)") {

			Turret turretScript = turret.GetComponent<Turret>();

			float enemyTime = rangeInPixel / landEnemySpeed;
			float shootsPerSeconds = 1.0f / landFireRate;

			if (shootsPerSeconds < 1) {
				shootsPerSeconds = 1.0f / shootsPerSeconds;
			}

			int shootsOnEnemy = (int)(enemyTime / shootsPerSeconds);
				
			turretNameText.text = "Name: LandTurret" + " " + turretScript.counter.ToString();
			turretTypeText.text = "Type: Land";
			turretCostText.text = "Cost: " + turretScript.turretCost.ToString();
			turretRangeText.text = "Range: " + landRange.ToString();
			turretBulletFireRateText.text = "Bullet Fire Rate: " + landFireRate.ToString();
			turretMissleFireRateText.text = "Missile Fire Rate: ----";
			bulletDamageText.text = "Bullet Damage: " + bulletDamage.ToString();
			missleDamageText.text = "Missile Damage: ----";
			shootsInRangeText.text = "Shoots in Range: " + shootsOnEnemy.ToString();
			damageOnEnemyText.text = "Damage on Enemey: " + (shootsOnEnemy * bulletDamage).ToString();
		}
		else if (turret.name == "AirTurret(Clone)") {

			Turret turretScript = turret.GetComponent<Turret>();

			float enemyTime = rangeInPixel / airEnemySpeed;
			float shootsPerSeconds = 1.0f / airFireRate;

			if (shootsPerSeconds < 1) {
				shootsPerSeconds = 1.0f / shootsPerSeconds;
			}

			int shootsOnEnemy = (int)(enemyTime / shootsPerSeconds);
				
			turretNameText.text = "Name: AirTurret" + " " + turretScript.counter.ToString();
			turretTypeText.text = "Type: Air";
			turretCostText.text = "Cost: " + turretScript.turretCost.ToString();
			turretRangeText.text = "Range: " + airRange.ToString();
			turretBulletFireRateText.text = "Bullet Fire Rate: ----";
			turretMissleFireRateText.text = "Missile Fire Rate: " + airFireRate.ToString();
			bulletDamageText.text = "Bullet Damage: ----";
			missleDamageText.text = "Missile Damage: " + missleDamage.ToString();
			shootsInRangeText.text = "Shoots in Range: " + shootsOnEnemy.ToString();
			damageOnEnemyText.text = "Damage on Enemey: " + (shootsOnEnemy * missleDamage).ToString();
		}
		else {

			CombiTurret turretScript = turret.GetComponent<CombiTurret>();

			float landEnemyTime = rangeInPixel / landEnemySpeed;
			float airEnemyTime = rangeInPixel / airEnemySpeed;

			float landShootsPerSeconds = 1.0f / landFireRate;
			float airShootsPerSeconds = 1.0f / airFireRate;

			if (landShootsPerSeconds < 1) {
				landShootsPerSeconds = 1.0f / landShootsPerSeconds;
			}

			if (airShootsPerSeconds < 1) {
				airShootsPerSeconds = 1.0f / airShootsPerSeconds;
			}

			int landShootsOnEnemy = (int)(landEnemyTime / landShootsPerSeconds);
			int airShootsOnEnemy = (int)(airEnemyTime / airShootsPerSeconds);

			turretNameText.text = "Name: CombiTurret" + " " + turretScript.counter.ToString();
			turretTypeText.text = "Type: Combi";
			turretCostText.text = "Cost: " + turretScript.turretCost.ToString();
			turretRangeText.text = "Range: " + combiRange.ToString();
			turretBulletFireRateText.text = "Bullet Fire Rate: " + combiLandFireRate.ToString();
			turretMissleFireRateText.text = "Missile Fire Rate: " + combiAirFireRate.ToString();
			bulletDamageText.text = "Bullet Damage: " + combiBulletDamage.ToString();
			missleDamageText.text = "Missile Damage: " + combiMissleDamage.ToString();
			shootsInRangeText.text = "Shoots in Range: Bullet: " + landShootsOnEnemy.ToString() + " / Missile: " + airShootsOnEnemy.ToString();
			damageOnEnemyText.text = "Damage on Enemey: Land: " + (landShootsOnEnemy * bulletDamage).ToString() + " / Air: " + (airShootsOnEnemy * missleDamage).ToString();
		}
	}
}
