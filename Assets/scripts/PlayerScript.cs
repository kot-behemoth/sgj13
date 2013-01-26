using UnityEngine;
using System;

public class PlayerScript : MonoBehaviour {

	[Serializable]
	private class Bullet {
		public Rigidbody rigidbody;
		public float lifetimeLeft;
	}

	public Rigidbody bulletPrefab;
	public float bulletLife = 3f;
	public float bulletOffset = 1f;
	public int numberOfBullets = 10;
	public float shootPower = 2.0f;
	private Bullet[] bullets;

	private Vector3 direction = Vector3.zero;
	public Transform indicator;
	public float indicatorOffset = 0.6f;

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
	}

	void Update () {
		// Update direction
		Vector3 mousePlanePosition = ScreenToWorld(Input.mousePosition);
		direction = (transform.position - mousePlanePosition).normalized;
		indicator.localPosition = direction * indicatorOffset;

		UpdateBullets();

		// Now you can simply pass the mouse cursor’s current position to get its equivalent world-space position like this:
		if(Input.GetButtonDown("Fire1"))
		{
			Bullet b = GetAvailableBullet();
			if(b != null) {
				// Rigidbody b = Instantiate(bullet, transform.position + direction*bulletOffset, Quaternion.identity) as Rigidbody;
				b.rigidbody.position = transform.position + direction*bulletOffset;

				b.rigidbody.AddForce(direction * shootPower, ForceMode.Impulse);
				rigidbody.AddForce(-direction * shootPower, ForceMode.Impulse);
			}
		}
	}

	private void UpdateBullets() {
		for(int i = 0; i < numberOfBullets; i++) {
			Bullet bullet = bullets[i];
			bullet.lifetimeLeft -= Time.deltaTime;
			if(bullet.lifetimeLeft <= 0f) {
				bullet.rigidbody.gameObject.SetActive(false);
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
				bullet.rigidbody.gameObject.SetActive(true);
				bullet.lifetimeLeft = bulletLife;
				return bullet;
			}
		}

		return null;
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
