using UnityEngine;
using System.Collections;

public class ChangeMusic : MonoBehaviour {

	public AudioSource Music;
	public AudioSource Player;
	public AudioClip music1;
	public AudioClip music2;
	public AudioClip music3;
	public AudioClip musicBoost;
	public Light light;

	public bool freezeGame = false;
	public bool Boost = false;

	void Start(){
		StartMusic ();
		}

	void StartMusic(){
		int idMusic = PlayerPrefs.GetInt ("Music", 1);
		switch (idMusic) {
		case 1 : Music.clip = music1; break;
		case 2 : Music.clip = music2; break;
		case 3 : Music.clip = music3; break;
		}
		Music.Play ();
		}

	void OnMouseDown(){
		if (Music.clip == music1){
			Music.clip = music2;
			PlayerPrefs.SetInt ("Music", 2);}
				else if (Music.clip == music2){
			Music.clip = music3;
		PlayerPrefs.SetInt ("Music", 3);}
				else{
		Music.clip = music1;
	PlayerPrefs.SetInt ("Music", 1);}
		Music.Play ();
	}

	IEnumerator BoostTime(int time) { 
		Music.clip = musicBoost;
		Music.Play();
		light.enabled = true;
		Boost = true;
		yield return new WaitForSeconds(time); 
		StartMusic ();
		Boost = false;
		light.enabled = false;
	}

	public void ActivateBoost(int boostTime){
		StartCoroutine(BoostTime(boostTime));
		}


	public void PlayerDeath(){
		Music.volume = 0;
		Player.Play();
		}

	public void restartMusic(){
		Music.volume = 0.05f;
		}
}
