using UnityEngine;

public class PlayerScript : MonoBehaviour {

	public float speed = 3.0F;

	private CharacterController controller;

	void Start () {
		controller = GetComponent<CharacterController>();
	}

	void Update () {
		Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
		moveDirection = transform.TransformDirection(moveDirection);
		moveDirection *= speed;
		controller.Move(moveDirection * Time.deltaTime);
	}

}
