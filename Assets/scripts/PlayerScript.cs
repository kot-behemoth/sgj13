using UnityEngine;
using System;

public class PlayerScript : MonoBehaviour {

	[Serializable]
	public class Bullet {
		public Rigidbody rigidbody;
		public float lifetimeLeft;
	}

	public int playerNumber = 1;

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
		bullets = new Bullet[numberOfBullets];
		for(int i = 0; i < numberOfBullets; i++)
		{
			Bullet bullet = new Bullet();
			Rigidbody brigidbody = Instantiate(bulletPrefab, Vector3.zero, Quaternion.identity) as Rigidbody;
			brigidbody.transform.parent = transform;
			brigidbody.gameObject.SetActive(false);
			bullet.rigidbody = brigidbody;
			bullets[i] = bullet;
		}

		GameManager.RegisterPlayer(gameObject);
	}

	void Update () {
		UpdateBullets();

		UpdateControls();

		GUIManager.SetPlayerScore(playerNumber, score);

		// if(playerNumber != 1) {
		// 	Debug.Log(new Vector3(Input.GetAxis(ControlForPlayer("Horizontal")), 0, Input.GetAxis(ControlForPlayer("Vertical"))));
		// }

		// Debug.Log(((GameObject)GameManager.instance.players[0]).transform.position);
		// // Debug.Log(GameManager.instance.players[1].transform.position);
	}

	public void SuccessfulHit() {
		score++;
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
		Bullet b = GetAvailableBullet();
		if(b != null) {
			b.rigidbody.MovePosition(transform.position + direction*bulletOffset);
			b.rigidbody.transform.position = transform.position + direction*bulletOffset;
			b.rigidbody.isKinematic = false;

			b.rigidbody.AddForce(direction * shootPower, ForceMode.Impulse);
			rigidbody.AddForce(-direction * shootPower, ForceMode.Impulse);
		}
	}

	private void UpdateBullets() {
		for(int i = 0; i < numberOfBullets; i++) {
			Bullet bullet = bullets[i];
			if(bullet.lifetimeLeft <= 0f) {
				bullet.rigidbody.gameObject.SetActive(false);
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
