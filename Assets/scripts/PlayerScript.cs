using UnityEngine;
using System;

public class PlayerScript : MonoBehaviour {

	[Serializable]
	public class Bullet {
		public Rigidbody rigidbody;
		public float lifetimeLeft;
		public Sprite sprite;
	}

	public int playerNumber = 1;

	public GameObject swarmObject;
	private SwarmScript swarm;

	public GameObject spriteManagerObject;
	private SpriteManager spriteManager;

	public int maxBees = 10;
	public float beeRespawnCooldown = 1;
	private float beeRespawn;

	public Rigidbody bulletPrefab;
	public float bulletLife = 3f;
	public float bulletOffset = 1f;
	public int numberOfBullets = 10;
	public float shootPower = 2.0f;
	private Bullet[] bullets;

	private Vector3 direction = Vector3.zero;
	public Transform indicator;
	public float indicatorOffset = 0.6f;

	private int score = 0;

	void Start () {
		spriteManager = spriteManagerObject.GetComponent<SpriteManager>();

		bullets = new Bullet[numberOfBullets];
		for(int i = 0; i < numberOfBullets; i++)
		{
			Bullet bullet = new Bullet();
			Rigidbody brigidbody = Instantiate(bulletPrefab, Vector3.zero, Quaternion.identity) as Rigidbody;
			brigidbody.transform.parent = transform;
			brigidbody.gameObject.SetActive(false);
			bullet.rigidbody = brigidbody;

			bullet.sprite = spriteManager.AddSprite(bullet.rigidbody.gameObject, 2f, 2f, 0, 512, 512, 512, true);

			UVAnimation animation = new UVAnimation();
			Vector2 randomFacingUV = (UnityEngine.Random.value >= 0.5f) ? new Vector2(0, 0.5f) : new Vector2(0.5f, 0);
			animation.BuildUVAnim(randomFacingUV, new Vector2(0.5f, 0.5f), 2, 1, 2, 8);
			animation.loopCycles = 3000;
			bullet.sprite.PlayAnim(animation);
			spriteManager.AnimateSprite(bullet.sprite);

			bullets[i] = bullet;
		}

		GameManager.RegisterPlayer(gameObject);

		swarm = swarmObject.GetComponent<SwarmScript>();
	}

	void Update () {
		UpdateBullets();

		UpdateControls();

		RespawnBees();

		GUIManager.SetPlayerScore(playerNumber, score);
	}

	public void SuccessfulHit() {
		score++;
	}

	private void RespawnBees() {
		beeRespawn -= Time.deltaTime;
		if(beeRespawn <= 0) {
			swarm.amountOfBees += 1;
			beeRespawn = beeRespawnCooldown;
			if(swarm.amountOfBees > maxBees) {
				swarm.amountOfBees = maxBees;
			}
		}
	}

	public void GotHit() {
		swarm.amountOfBees -= 3;
		IsDead();
	}

	private void IsDead() {
		if(swarm.amountOfBees <= 0) {
			GameEventManager.TriggerGameOver();
		}
	}

	private void UpdateControls() {
		// Update direction
		if(playerNumber == 1) {
			Vector3 mousePlanePosition = ScreenToWorld(Input.mousePosition);
			direction = (transform.position - mousePlanePosition).normalized;
		} else {
			direction = new Vector3(Input.GetAxis(ControlForPlayer("Horizontal")), 0, Input.GetAxis(ControlForPlayer("Vertical")));
		}
		indicator.localPosition = direction * indicatorOffset;

		if(Input.GetButtonDown(ControlForPlayer("Fire1"))) {
			Fire();
		}
	}

	private void Fire() {
		if(swarm.amountOfBees <= 1) return;

		Bullet b = GetAvailableBullet();
		if(b != null) {
			b.rigidbody.MovePosition(transform.position + direction*bulletOffset);
			b.rigidbody.transform.position = transform.position + direction*bulletOffset;
			b.rigidbody.isKinematic = false;
			b.sprite.hidden = false;

			b.rigidbody.AddForce(direction * shootPower, ForceMode.Impulse);
			rigidbody.AddForce(-direction * shootPower, ForceMode.Impulse);

			swarm.amountOfBees--;
		}
	}

	private void UpdateBullets() {
		for(int i = 0; i < numberOfBullets; i++) {
			Bullet bullet = bullets[i];
			if(bullet.lifetimeLeft <= 0f) {
				bullet.rigidbody.gameObject.SetActive(false);
				bullet.sprite.hidden = true;
			} else {
				bullet.lifetimeLeft -= Time.deltaTime;
			}
		}
	}

	private Bullet GetAvailableBullet() {
		for(int i = 0; i < numberOfBullets; i++)
		{
			Bullet bullet = bullets[i];
			// We're looking for dead bullets
			if(!bullet.rigidbody.gameObject.activeSelf)
			{
				bullet.rigidbody.isKinematic = true;
				bullet.rigidbody.gameObject.SetActive(true);
				bullet.lifetimeLeft = bulletLife;
				return bullet;
			}
		}

		return null;
	}

	private String ControlForPlayer(String controlName) {
		return String.Format(controlName + playerNumber.ToString());
	}

	// Borrowed from http://www.fatalfrog.com/?p=7
	private Vector3 ScreenToWorld(Vector2 screenPos) {
	    // Create a ray going into the scene starting
	    // from the screen position provided
	    Ray ray = Camera.main.ScreenPointToRay(screenPos);

	    // ray hit an object, return intersection point
	    // HOWEVER we're not looking for a collision
	    // RaycastHit hit;
	    // if( Physics.Raycast(ray, out hit ))
	    //    return hit.point;

	    // ray didn't hit any solid object, so return the
	    // intersection point between the ray and
	    // the Y=0 plane (horizontal plane)
	    float t = -ray.origin.y / ray.direction.y;
	    return ray.GetPoint(t);
	}

}
