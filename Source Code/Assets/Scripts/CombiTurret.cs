using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(LineRenderer))]
public class CombiTurret : MonoBehaviour {

	[Header("Unity Setup Fields")]
	public Transform partToRotate;
	public float turnSpeed = 10f;

	[Header("Turret Bullet")]
	//public GameObject[] bulletPrefab;
	//public Transform[] firePoint;
	public GameObject bulletPrefab;
	public GameObject misslePrefab;

	public Transform bulletFirePoint;
	public Transform leftMissleFirePoint;
	public Transform rightMissleFirePoint;

	[Range(3, 256)]
	public int numSegments = 128;

	public Material rendererMaterial;

	[HideInInspector]
	public int counter = 0;
	public int turretCost = 0;


	private Transform target;
	private Transform missleArray;

	private List<Transform> missleList;
	private Transform[] missles;

	private LineRenderer rangeMarker;

	private bool isMarked = false;

	private float range = 0f;
	private float bulletFireRate = 0f;
	private float bulletFireCountDown = 0f;
	private float missleFireRate = 0f;
	private float missleFireCountDown = 0f;

	private int missleCount = 0;

	private StateManager stateManager;

	// Use this for initialization
	void Start () {

		stateManager = StateManager.instance;

		rangeMarker = gameObject.GetComponent<LineRenderer>();
		missleArray = this.transform.Find("PointToRotate/MisslePositions");

		missleList = new List<Transform>();
		missles = missleArray.GetComponentsInChildren<Transform>();

		foreach (Transform oneMissle in missles) {
			
			if (string.Compare(oneMissle.gameObject.name, "MissleDummy") == 0) {
				missleList.Add(oneMissle);
			}
		}

		InvokeRepeating("UpdateTarget", 0f, 0.5f);
	}
	
	// Update is called once per frame
	void Update () {

		range = stateManager.GetCombiRange;

		bulletFireRate = stateManager.GetCombiLandFireRate;
		missleFireRate = stateManager.GetCombiAirFireRate;

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

		LookOnTarget();

		if (bulletFireCountDown <= 0f) {
			BulletShoot();
			bulletFireCountDown = 1.0f / bulletFireRate;
		}

		if (missleFireCountDown <= 0f) {
			MissleShoot();
			missleFireCountDown = 1.0f / missleFireRate;
		}

		bulletFireCountDown -= Time.deltaTime;
		missleFireCountDown -= Time.deltaTime;
		
	}

	public bool setMarket { set { isMarked = value; } }

	void UpdateTarget() {

		List<GameObject> enemies = new List<GameObject>();
		List<GameObject> landEnemies = new List<GameObject>();
		List<GameObject> airEnemies = new List<GameObject>();

		landEnemies = GameObject.FindGameObjectsWithTag("LandEnemy").ToList();
		airEnemies = GameObject.FindGameObjectsWithTag("AirEnemy").ToList();
		enemies = landEnemies.Concat(airEnemies).ToList();

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


	void LookOnTarget() {

		Vector3 dir = target.position - transform.position;
		Quaternion lookRotation = Quaternion.LookRotation(dir);
		Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
		partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
	}

	void BulletShoot() {

		GameObject moveBullet = null;

		moveBullet = (GameObject)Instantiate(bulletPrefab, bulletFirePoint.position, bulletFirePoint.rotation);

		Bullet bullet = moveBullet.GetComponent<Bullet>();

		if (moveBullet != null) {
			bullet.Seek(target);
		}
	}

	void MissleShoot() {

		GameObject moveMissle = null;

		if (missleCount < 3) {
			moveMissle = (GameObject)Instantiate(misslePrefab, rightMissleFirePoint.position, rightMissleFirePoint.rotation);
		}
		else {
			moveMissle = (GameObject)Instantiate(misslePrefab, leftMissleFirePoint.position, leftMissleFirePoint.rotation);
		}

		Bullet missle = moveMissle.GetComponent<Bullet>();

		missleList[missleCount].gameObject.SetActive(false);
		missleCount++;

		if (missleCount == 6) {
			Reload();
		}

		if (moveMissle != null) {
			missle.Seek(target);
		}
	}

	void Reload() {

		foreach (Transform oneMissle in missleList) {
			oneMissle.gameObject.SetActive(true);
		}

		missleCount = 0;
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

		//rangeMarker.SetColors(c1, c1);
		//rangeMarker.SetWidth(0.25f, 0.25f);
		//rangeMarker.SetVertexCount(numSegments + 1);
	}
}
