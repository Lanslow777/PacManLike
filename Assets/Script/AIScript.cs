using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIScript : MonoBehaviour {

	public int frameBeforeStart;
	public float chaseDpF;
	public float patrolDpF;
	public bool turnLeft;
	public bool teleport;
	public int FramePerTeleport;
	public ChangeMusic changeMusic;
	public ScoreCount scoreCount;

	bool recentTurn = false;
	int frameBeforeTurn = 0;
	int frameBeforeTeleport = 0;
	bool chasePlayer = false;

	// Use this for initialization
	void Start () {
	
	}

	void OnDrawGizmos(){
		var scale = 2.0f;
		
		Gizmos.color = Color.blue;
		Gizmos.DrawLine(transform.position, transform.position + transform.forward * scale);
		
		Gizmos.color = Color.red;
		Gizmos.DrawLine(transform.position, transform.position + transform.right * scale);
		}

	
	IEnumerator WaitTwoSecond(Collider col) { 
		changeMusic.freezeGame = true;
		GameObject score = GameObject.FindGameObjectWithTag("ScoreText");
		ScoreCount scoreCount = (ScoreCount)score.GetComponent("ScoreCount");
		changeMusic.PlayerDeath();
		yield return new WaitForSeconds(2); 
		changeMusic.restartMusic ();
		col.transform.position = new Vector3(0, 0.1f, 1.3f);
		List<GameObject> badList = new List<GameObject>(GameObject.FindGameObjectsWithTag("BadGuy"));
		foreach(GameObject bad in badList){
			AIScript script = (AIScript) bad.GetComponent("AIScript");
			script.frameBeforeStart = 300;
			bad.transform.position = new Vector3(0, 1f, 0.3f);
		}
		scoreCount.updateLife(false);
		changeMusic.freezeGame = false;
	}

	void OnTriggerEnter(Collider col){
		if (col.tag == "Untagged" || col.tag == "MazeComponent") {
						RaycastHit hitForward;
						RaycastHit hitLeft;
						if (Physics.Raycast (transform.position, transform.forward, out hitForward) 
								&& Physics.Raycast (transform.position - 0.5f * transform.right, transform.forward, out hitLeft)) {
								if (hitForward.distance < 1.0f)
										transform.Rotate (0, 180, 0);
								else if (hitLeft.distance > 1.0f)
										transform.position = transform.position - transform.right;
								else
										transform.position = transform.position + transform.right;
						}
				} else if (col.tag.Equals ("Player")) {
			if(!changeMusic.Boost)StartCoroutine(WaitTwoSecond(col));
			else{
				scoreCount.updateScore(100);
				frameBeforeStart = 300;
				transform.position = new Vector3(0, 1f, 0.3f);
			}
				}
	}

	void Teleport(){
		List<GameObject> mazeList = new List<GameObject>(GameObject.FindGameObjectsWithTag("MazeComponent"));
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		bool ok = false;
		while (!ok) {
			GameObject mazeObject = mazeList[Random.Range(0, mazeList.Count - 1)];
			if(Vector3.Distance (mazeObject.transform.position, player.transform.position)>1){
				transform.position = mazeObject.transform.position + Vector3.up;
				ok = true;
			}
				}
		}
	
	// Update is called once per frame
	void Update () {
		if (frameBeforeStart != 0) {
						frameBeforeStart--;
		} else if(!changeMusic.freezeGame){
			if (changeMusic.Boost)
				GetComponent<Renderer>().material.color = Color.white;
			else
				GetComponent<Renderer>().material.color = Color.red;
						if (recentTurn) {
								frameBeforeTurn--;
								if (frameBeforeTurn == 0)
										recentTurn = false;
						}
			if(teleport && !chasePlayer){
				if(frameBeforeTeleport == 0){
					Teleport();
					frameBeforeTeleport = FramePerTeleport;
				}
				else frameBeforeTeleport--;
			}

						List<RaycastHit> hits = new List<RaycastHit> ();
						RaycastHit hitForward;
						RaycastHit hitBack;
						RaycastHit hitRight;
						RaycastHit hitLeft;
						if (Physics.Raycast (transform.position, transform.forward, out hitForward))
								hits.Add (hitForward);
						if (Physics.Raycast (transform.position, -transform.forward, out hitBack))
								hits.Add (hitBack);
						if (Physics.Raycast (transform.position, transform.right, out hitRight))
								hits.Add (hitRight);
			if (Physics.Raycast (transform.position, -transform.right, out hitLeft))
								hits.Add (hitLeft);
						bool moveDone = false;

						//Chase Player
						foreach (RaycastHit ray in hits) {
								if (ray.collider.tag.Equals ("Player")) { //See the player go to the player
										if(changeMusic.Boost){
												if(ray.Equals(hitForward))transform.position += (-transform.forward * chaseDpF);
												else transform.position += (transform.forward * chaseDpF);
										}else{
											if (ray.Equals (hitForward))
													transform.position += (transform.forward * chaseDpF);
											if (ray.Equals (hitBack))
													transform.position += (-transform.forward * chaseDpF);
											if (ray.Equals (hitRight))
													transform.position += (transform.right * chaseDpF);
											if (ray.Equals (hitLeft))
													transform.position += (-transform.right * chaseDpF);
											chasePlayer = true;
											moveDone = true;
										}
										break;
								} 
						}


						if (!moveDone) {
				chasePlayer = false;
								if (turnLeft && !recentTurn && hitLeft.collider != null) {
										if ((hitLeft.collider.tag.Equals ("Untagged") && hitLeft.distance > 1.1f) || hitLeft.collider.tag.Equals ("Item")) {
												transform.Rotate (0, -90, 0);
												recentTurn = true;
												frameBeforeTurn = 120;
						moveDone = true;
										}
								} else if (!turnLeft && !recentTurn && hitRight.collider != null) {
										if ((hitRight.collider.tag.Equals ("Untagged") && hitRight.distance > 1.1f) || hitRight.collider.tag.Equals ("Item")) {
												transform.Rotate (0, 90, 0);
												recentTurn = true;
												frameBeforeTurn = 120;
						moveDone = true;
										}
								}
				if(!moveDone){
				if (hitForward.distance > 1.0f || !hitForward.collider.tag.Equals("Untagged"))
										transform.position += (transform.forward * patrolDpF);
					else if((hitBack.distance > 1.5f || !hitBack.collider.tag.Equals("Untagged")) && !recentTurn){
												transform.Rotate (0, 180, 0);
						recentTurn = true;
						frameBeforeTurn = 120;}
				else transform.Rotate (0, 90, 0);
				}

						}
				}
		}
}
