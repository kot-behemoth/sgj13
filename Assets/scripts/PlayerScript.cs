using UnityEngine;

public class PlayerScript : MonoBehaviour {

	public float speed = 3.0F;
	public Rigidbody bullet;
	public float shootPower = 2.0f;

	private CharacterController controller;
	private Rigidbody rigidbody;

	private Vector3 direction = Vector3.zero;
	public Transform indicator;
	public float indicatorOffset = 0.6f;

	void Start () {
		controller = GetComponent<CharacterController>();
		rigidbody = GetComponent<Rigidbody>();
	}

	void Update () {
		// Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
		// moveDirection = transform.TransformDirection(moveDirection);
		// moveDirection *= speed;
		// controller.Move(moveDirection * Time.deltaTime);

		// Update direction
		Vector3 mousePlanePosition = ScreenToWorld(Input.mousePosition);
		direction = (transform.position - mousePlanePosition).normalized;
		indicator.localPosition = direction * indicatorOffset;

		// Now you can simply pass the mouse cursor’s current position to get its equivalent world-space position like this:
		if(Input.GetButtonDown("Fire1"))
		{
			Rigidbody b = Instantiate(bullet, transform.position, Quaternion.identity) as Rigidbody;
			b.transform.parent = transform;

			b.AddForce(direction * shootPower, ForceMode.Impulse);
			rigidbody.AddForce(-direction * shootPower, ForceMode.Impulse);
		}
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
