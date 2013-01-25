using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {
	public AudioClip _expand;
	public AudioClip _contract;
	private GameObject[] audioContainers;
	private int currentContainer = 0;
	private int numContainers = 12;
	// Use this for initialization

	void Start () {
		audioContainers = new GameObject[numContainers];
		for (int i = 0; i < numContainers; i++) {
			GameObject g = audioContainers[i] = new GameObject();
			g.transform.position = transform.position;
			g.transform.parent = transform;
	        g.AddComponent<AudioSource>();			
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void contract(){
		Play(_contract, transform, 1.0f, 1.0f);

	}
	public void expand(){
		Play(_expand, transform, 1.0f, 1.0f);

	}
	
    public AudioSource Play(AudioClip clip, Transform emitter, float volume, float pitch)
    {
		GameObject g = audioContainers[currentContainer];
		currentContainer = (currentContainer+1)%numContainers;
        AudioSource source = g.audio;
        source.clip = clip;
        source.volume = volume;
        source.pitch = pitch;
        source.Play ();
        return source;
    }

}