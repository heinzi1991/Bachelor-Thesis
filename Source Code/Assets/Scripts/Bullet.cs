using UnityEngine;

public class Bullet : MonoBehaviour {

	//public float speed = 70f;
	//public int damage = 50;

	private Transform target;

	private StateManager stateManager;

	private float speed = 0f;
	private int damage = 0;

	void Awake() {

		stateManager = StateManager.instance;
	}
 
	void Update() {

		if (target == null) {
			Destroy(gameObject);
			return;
		}

		Vector3 dir = target.position - transform.position;

		if (string.Compare(this.name, "Bullet(Clone)") == 0) {

			speed = stateManager.GetBulletSpeed;
			damage = stateManager.GetBulletDamage;
		}
		else if (string.Compare(this.name, "Missle(Clone)") == 0) {

			speed = stateManager.GetMissleSpeed;
			damage = stateManager.GetMissleDamage;
		}
		else if (string.Compare(this.name, "CombiBullet(Clone)") == 0) {

			speed = stateManager.GetCombiBulletSpeed;
			damage = stateManager.GetCombiBulletDamage;
		}
		else {

			speed = stateManager.GetCombiMissleSpeed;
			damage = stateManager.GetCombiMissleDamage;
		}

		float distanceThisFrame = speed * Time.deltaTime;

		if (dir.magnitude <= distanceThisFrame) {

			HitTarget();
			return;
		}

		transform.Translate(dir.normalized * distanceThisFrame, Space.World);
		transform.LookAt(target);
	}

	public void Seek(Transform target_) {
		target = target_;
	} 

	void HitTarget() {

		Damage(target);
		Destroy(gameObject);
	}

	void Damage(Transform enemy) {

		Enemy e = enemy.GetComponent<Enemy>();

		if (e != null) {
			e.TakeDamage(damage);
		}
	}
}
