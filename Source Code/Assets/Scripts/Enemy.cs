using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {

	//public float startSpeed = 10f;

	[HideInInspector]
	public float speed;

	public float startHealth = 100f;
	//public int gain = 50;

	public Image healthBar;

	private float health;
	private int gain;
	private StateManager stateManager;
	private GridManager gridManager;

	void Awake() {

		stateManager = StateManager.instance;
		gridManager = GridManager.instance;
	}

	void Start() {

		if (string.Compare(this.name, "BossEnemy(Clone)") == 0) {
			int randomNumber = Random.Range(0, 1000);

			if (randomNumber % 2 == 0) {
				this.tag = "LandEnemy";
			}
			else {
				this.tag = "AirEnemy";
			}

			speed = stateManager.GetEndBossSpeed;
		}
		else if (string.Compare(this.name, "LandEnemy(Clone)") == 0) {
			speed = stateManager.GetLandEnemySpeed;
		}
		else {
			speed = stateManager.GetAirEnemySpeed;
		}
			
		health = startHealth;
	}

	void Die() {

		if (!gridManager.noCoinsPlay) {

			if (this.name == "LandEnemy(Clone)") {
				gain = stateManager.GetLandEnemyGain;
			}
			else if (this.name == "AirEnemy(Clone)") {
				gain = stateManager.GetAirEnemyGain;
			}
			else {
				gain = stateManager.GetEndBossGain;
			}

			gridManager.coinsAmount += gain;
		}
			
		stateManager.AmountOfKillEnemies += 1;

		Destroy(gameObject);
	}

	public void TakeDamage(int amount) {

		health -= amount;

		healthBar.fillAmount = health / startHealth;

		if (health <= 0) {

			Die();
		}
	}
}
