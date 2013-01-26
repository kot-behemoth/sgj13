using UnityEngine;
using System.Collections;

public class EnemyBaseScript : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	void OnCollisionEnter(Collision collision) {
        foreach (ContactPoint contact in collision.contacts) {
            if(contact.otherCollider.gameObject.tag == "bullet") {
				Destroy(gameObject);
            	contact.otherCollider.gameObject.transform.parent.gameObject.GetComponent<PlayerScript>().SuccessfulHit();
			}
        }
    }

}
