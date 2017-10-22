using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(LineRenderer))]
public class Turret : MonoBehaviour {

	[Header("Unity Setup Fields")]
	public Transform partToRotate;
	public float turnSpeed = 10f;

	[Header("Turret Bullet")]
	public GameObject bulletPrefab;
	public Transform firePoint;

	[Range(3, 256)]
	public int numSegments = 128;

	public Material rendererMaterial;

	[HideInInspector]
	public int counter = 0;
	public int turretCost = 0;


	private Transform target;
	private float fireCountdown;
	private string airEnemyTag = "AirEnemy";
	private string landEnemyTag = "LandEnemy";
	private int missleCount = 0;

	private Transform missleArray;
	private Transform[] missles;
	private List<Transform> missleList;

	private StateManager stateManager;

	private LineRenderer rangeMarker;
	private bool isMarked = false;

	private float range = 0f;
	private float fireRate;




	void Start() {

		stateManager = StateManager.instance;

		rangeMarker = gameObject.GetComponent<LineRenderer>();
		missleArray = this.transform.Find("PointToRotate/MisslePositions");

		if (missleArray != null) {

			missleList = new List<Transform>();
			missles = missleArray.GetComponentsInChildren<Transform>();

			foreach (Transform oneMissle in missles) {

				if (string.Compare(oneMissle.gameObject.name, "MissleDummy") == 0) {
					missleList.Add(oneMissle);
				}
			}
		}

		InvokeRepeating("UpdateTarget", 0f, 0.5f);
	}

	void Update() {

		if (string.Compare(this.name, "LandTurret(Clone)") == 0) {
			range = stateManager.GetLandRange;
			fireRate = stateManager.GetLandFireRate;
		}
		else {
			range = stateManager.GetAirRange;
			fireRate = stateManager.GetAirFireRate;
		}


		if (isMarked) {
			rangeMarker.enabled = true;
			DoRenderer();
		}
		else {
			rangeMarker.enabled = false;
		}
			
		if (target == null) {
			return;
		}

		LockOnTarget();


		if (fireCountdown <= 0f) {
			Shoot();
			fireCountdown = 1f / fireRate;
		}

		fireCountdown -= Time.deltaTime;
	}

	public bool setMarket { set { isMarked = value; } }

	void UpdateTarget() {

		List<GameObject> enemies = new List<GameObject>();

		if (string.Compare(this.name, "LandTurret(Clone)") == 0) {
			enemies = GameObject.FindGameObjectsWithTag(landEnemyTag).ToList();
		}
		else {
			enemies = GameObject.FindGameObjectsWithTag(airEnemyTag).ToList();
		}
			
		float shortestDistance = Mathf.Infinity;
		GameObject nearestEnemy = null;

		foreach (GameObject enemy in enemies) {

			float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);

			if (distanceToEnemy < shortestDistance) {

				shortestDistance = distanceToEnemy;
				nearestEnemy = enemy;
			}
		}

		if (nearestEnemy != null && shortestDistance <= range) {
			target = nearestEnemy.transform;
		}
		else {
			target = null;
		}
	}

	void LockOnTarget() {

		Vector3 dir = target.position - transform.position;
		Quaternion lookRotation = Quaternion.LookRotation(dir);
		Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
		partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
	}

	void Shoot() {

		GameObject moveBullet = null;

		moveBullet = (GameObject)Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);


		/*if (this.tag == "LandTurret" || this.tag == "AirTurret") {

			moveBullet = (GameObject)Instantiate(bulletPrefab[0], firePoint[0].position, firePoint[0].rotation);
		}
		else {

			if (target.tag == "LandEnemy") {
				moveBullet = (GameObject)Instantiate(bulletPrefab[0], firePoint[0].position, firePoint[0].rotation);
			}
			else {

				if (combiMissleCount < 3) {
					moveBullet = (GameObject)Instantiate(bulletPrefab[1], firePoint[1].position, firePoint[1].rotation);
				}
				else {
					moveBullet = (GameObject)Instantiate(bulletPrefab[1], firePoint[2].position, firePoint[2].rotation);
				}
			}
		}*/

		Bullet bullet = moveBullet.GetComponent<Bullet>();

		if (string.Compare(moveBullet.name, "Missle(Clone)") == 0) {
			missleList[missleCount].gameObject.SetActive(false);
			missleCount++;

			if (missleCount == 16) {
				Reload();
			}
		}


		/*if (string.Compare(moveBullet.name, "Missle(Clone)") == 0 || (string.Compare(moveBullet.name, "CombiMissle(Clone)") == 0)) {

			if (this.tag == "CombiTurret") {

				missleList[combiMissleCount].gameObject.SetActive(false);
				combiMissleCount++;

				if (combiMissleCount == 6) {
					Reload();
				}
			}
			else {

				missleList[missleCount].gameObject.SetActive(false);
				missleCount++;

				if (missleCount == 16) {
					Reload();
				}
			}
		}*/

		if (moveBullet != null) {
			bullet.Seek(target);
		}
	}

	void Reload() {

		foreach (Transform oneMissle in missleList) {
			oneMissle.gameObject.SetActive(true);
		}

		missleCount = 0;

		/*if (this.tag == "CombiTurret") {
			combiMissleCount = 0;
		}
		else {
			missleCount = 0;
		}*/
	}

	void DoRenderer() {

		Color c1 = Color.red;

		rangeMarker.material = new Material(Shader.Find("Particles/Additive"));

		rangeMarker.startColor = c1;
		rangeMarker.endColor = c1;
		rangeMarker.startWidth = 0.25f;
		rangeMarker.endWidth = 0.25f;
		rangeMarker.positionCount = numSegments + 1;
		rangeMarker.useWorldSpace = false;

		float deltaTheta = (float) (2.0f * Mathf.PI) / numSegments;
		float theta = 0f;

		for (int i = 0; i < numSegments + 1; i++) {

			float x = range * Mathf.Cos(theta);
			float z = range * Mathf.Sin(theta);

			Vector3 pos = new Vector3(x, 0.5f, z);
			rangeMarker.SetPosition(i, pos);
			theta += deltaTheta;
		}
	}
}
