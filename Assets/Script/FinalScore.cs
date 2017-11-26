using UnityEngine;
using System.Collections;

public class FinalScore : MonoBehaviour {

	public TextMesh textMesh;

	// Use this for initialization
	void Start () {
		textMesh.text = "Score : " + PlayerPrefs.GetInt ("Score", 0);
	}
}
