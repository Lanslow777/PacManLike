using UnityEngine;
using System.Collections;

public class ChangeSizeEnnemies : MonoBehaviour {

	public bool size;
	public bool up;
	public TextMesh textSize;
	public TextMesh textEnnemies;

	void Start(){
		textEnnemies.text = PlayerPrefs.GetInt("Ennemies", 5).ToString();
		textSize.text = PlayerPrefs.GetInt("Size", 20).ToString();
		}

	void OnMouseOver () {
		GetComponent<Renderer>().material.color = Color.yellow;
	}
	
	void OnMouseExit () {
		GetComponent<Renderer>().material.color = Color.white;
	}
	
	void OnMouseDown(){
		if(size){
			if(up){
				int s = PlayerPrefs.GetInt("Size", 20);
				s++;
				if(s>30) s = 1;
				PlayerPrefs.SetInt("Size", s);
			}else{
				int s = PlayerPrefs.GetInt("Size", 20);
				s--;
				if(s<1) s = 30;
				PlayerPrefs.SetInt("Size", s);
			}
			textSize.text = PlayerPrefs.GetInt("Size", 20).ToString();
		}else{
			if(up){
				int e = PlayerPrefs.GetInt("Ennemies", 5);
				e++;
				if(e>15) e= 1;
				PlayerPrefs.SetInt("Ennemies", e);
			}else{
				int e = PlayerPrefs.GetInt("Ennemies", 5);
				e--;
				if(e<1) e = 15;
				PlayerPrefs.SetInt("Ennemies", e);
			}
			textEnnemies.text = PlayerPrefs.GetInt("Ennemies", 5).ToString();
		}
	}
}
