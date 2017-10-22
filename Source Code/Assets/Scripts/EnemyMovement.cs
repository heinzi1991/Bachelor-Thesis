using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[RequireComponent(typeof(Enemy))]
public class EnemyMovement : MonoBehaviour {

	private Transform target;
	private int wayPointIndex = 0;

	private Enemy enemy;
	private Transform[] wayPoints;

	private GridManager gridManager;
	private StateManager stateManager;

	void Awake() {

		gridManager = GridManager.instance;
		stateManager = StateManager.instance;
		enemy = GetComponent<Enemy>();

		List<GameObject> wayPointsObjects = new List<GameObject>();

		string nameOfRandomGoal = "";

		if (gridManager.getEndNodeList.Count > 1) {

			int randomGoal = Random.Range(0, gridManager.getEndNodeList.Count);

			nameOfRandomGoal = gridManager.getEndNodeList[randomGoal].name;
		} 
		else {

			nameOfRandomGoal = gridManager.getEndNodeList[0].name;
		}

		if (string.Compare(enemy.tag, "LandEnemy") == 0) {

			wayPointsObjects = gridManager.getLandWayPointsFromDict(nameOfRandomGoal);
		}
		else {

			wayPointsObjects = gridManager.getAirWayPointsFromDict(nameOfRandomGoal);
		}

		wayPoints = new Transform[wayPointsObjects.Count];

		for (int i = 0; i < wayPoints.Length; i++) {

			wayPoints[i] = wayPointsObjects[i].transform;
		}
	}

	void Start() {

		target = wayPoints[0];

	}

	void Update() {

		Vector3 dir = target.position - transform.position;
		transform.Translate(dir.normalized * enemy.speed * Time.deltaTime, Space.World);

		if (Vector3.Distance(transform.position, target.position) <= 0.4f) {
			GetNextWayPoint();
		}


		if (string.Compare(enemy.name, "BossEnemy(Clone)") == 0) {
			enemy.speed = stateManager.GetEndBossSpeed;
		}
		else if (string.Compare(enemy.name, "LandEnemy(Clone)") == 0) {
			enemy.speed = stateManager.GetLandEnemySpeed;
		}
		else {
			enemy.speed = stateManager.GetAirEnemySpeed;
		}
	}

	void GetNextWayPoint() {

		if (wayPointIndex >= wayPoints.Length - 1) {
			EndPath();
			return;
		}

		wayPointIndex++;
		target = wayPoints[wayPointIndex];
	}

	void EndPath() {

		if (!gridManager.noHealthPlay) {
			gridManager.playerHealthStatus -= 1;

			if (gridManager.playerHealthStatus == 0) {
				gridManager.isGameOver = true;
			}
		}

		stateManager.AmountOfEnemies += 1;

		Destroy(gameObject);
	}
}
