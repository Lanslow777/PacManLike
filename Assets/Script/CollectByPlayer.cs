using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class CollectByPlayer : MonoBehaviour {

	public enum TypeObject{
		Collectable,
		Bonus,
		Boost
	};

	public int score;
	public TypeObject type;
	ScoreCount scoreCount;
	ChangeMusic changeMusic;

	void Start(){
		GameObject gameObject = GameObject.FindGameObjectWithTag ("ScoreText");
		changeMusic = (ChangeMusic)GameObject.FindGameObjectWithTag("Smelly").GetComponent("ChangeMusic");
		if (gameObject != null) {
			scoreCount = gameObject.GetComponent<ScoreCount>();
		}
	}

	public void OnTriggerEnter(Collider col)
	{
		if (col.tag == "Player") 
		{
			scoreCount.updateScore(score);
			Destroy(gameObject);
			if(type == TypeObject.Boost && !changeMusic.Boost){
				changeMusic.ActivateBoost(20);
			}
			if (type != TypeObject.Bonus && IsLastCollectable ()) {
				PlayerPrefs.Save ();
				SceneManager.LoadScene ("TestScene");
				//Application.LoadLevel ("TestScene");
			}
		}
	}

	bool IsLastCollectable(){
		List<GameObject> itemList = new List<GameObject>(GameObject.FindGameObjectsWithTag("Item"));
		bool lastItem = false;
		foreach (GameObject item in itemList) {
			if(item.name.Contains("CollectObject") || item.name.Contains("CollectBoost")){
				if(lastItem) return false;
				lastItem = true;
			}
		}
		return true;
	}
}
