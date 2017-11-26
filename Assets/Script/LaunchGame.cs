using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LaunchGame : MonoBehaviour {

	public bool launch;

	void OnMouseOver () {
		GetComponent<Renderer>().material.color = Color.yellow;
	}

	void OnMouseExit () {
		GetComponent<Renderer>().material.color = Color.white;
	}

	void OnMouseDown(){
		if (launch) {
						PlayerPrefs.SetInt ("Score", 0);
						PlayerPrefs.SetInt ("Life", 3);
						PlayerPrefs.Save ();
						SceneManager.LoadScene ("TestScene");
						//Application.LoadLevel ("TestScene");
				} else {
			Application.Quit();
				}
	}
}
