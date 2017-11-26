using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class RestartLevel : MonoBehaviour {

	void OnMouseDown(){
				SceneManager.LoadScene ("TestScene");
		//Application.LoadLevel ("TestScene");
	}
	
}
