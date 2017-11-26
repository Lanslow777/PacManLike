using System.Linq;
using UnityEngine;
using System.Collections.Generic;

public class ModularWorldGenerator : MonoBehaviour
{
	public Module[] Modules;
	public Module[] StartModules;

	public int Iterations = 5;
	public int CollectablePerBoost;
	public GameObject BoostPrefab;
	public GameObject BonusPrefab;


	void Start()
	{
		Iterations = PlayerPrefs.GetInt("Size", 20);
		int nbBad = PlayerPrefs.GetInt("Ennemies", 5);
		for (int i = 0; i < nbBad-1; i++) {
			var newModulePrefab = GameObject.FindGameObjectWithTag("BadGuy");
			GameObject newModule =(GameObject) Instantiate(newModulePrefab);
			newModule.transform.position = new Vector3(0, 1f, 0.3f);
			AIScript script = (AIScript)newModule.GetComponent("AIScript");
			script.turnLeft = (Random.Range(0, 100) % 2 == 0);
			script.teleport = (Random.Range(0, 100) % 2 == 0);
				}

		var startModule = (Module) Instantiate(StartModules[Random.Range(0, StartModules.Count())], transform.position, transform.rotation);
		var pendingExits = new List<ModuleConnector>(startModule.GetExits());

		//Generate Maze
		for (int iteration = 0; iteration < Iterations; iteration++)
		{
			var newExits = new List<ModuleConnector>();

			foreach (var pendingExit in pendingExits)
			{
				List<GameObject> mazeList = new List<GameObject>(GameObject.FindGameObjectsWithTag("MazeComponent"));
				bool addComponent = true;
				foreach(GameObject mazeComponent in mazeList)
				{
						if(Vector3.Distance(pendingExit.transform.position + pendingExit.transform.forward * 2, mazeComponent.transform.position)<0.5f){
							addComponent = false;
							break;
						}
				}
				if(addComponent){
					var newTag = GetRandom(pendingExit.Tags);
					var newModulePrefab = GetRandomWithTag(Modules, newTag);
					var newModule = (Module) Instantiate(newModulePrefab);
					var newModuleExits = newModule.GetExits();
					var exitToMatch = newModuleExits.FirstOrDefault(x => x.IsDefault) ?? GetRandom(newModuleExits);
					MatchExits(pendingExit, exitToMatch);
					newExits.AddRange(newModuleExits.Where(e => e != exitToMatch));
				}
			}

			pendingExits = newExits;
		}

		//Close Maze
		foreach (var pendingExit in pendingExits)
		{
			List<GameObject> mazeList = new List<GameObject>(GameObject.FindGameObjectsWithTag("MazeComponent"));
			bool addComponent = true;
			foreach(GameObject mazeComponent in mazeList)
			{
				if(pendingExit.transform.position + pendingExit.transform.forward * 2 == mazeComponent.transform.position){
					addComponent = false;
					break;
				}
			}
			if(addComponent){
				var newModulePrefab = StartModules.First(x => x.Tags.Contains("Couloir") && x.Name.Equals("End"));
				var newModule = (Module) Instantiate(newModulePrefab);
				var newModuleExits = newModule.GetExits();
				var exitToMatch = newModuleExits.FirstOrDefault(x => x.IsDefault) ?? GetRandom(newModuleExits);
				MatchExits(pendingExit, exitToMatch);
			}
		}

		//Generate Boost
		List<GameObject> itemList = new List<GameObject>(GameObject.FindGameObjectsWithTag("Item"));
		int nbBoost = itemList.Count / CollectablePerBoost;
		for (int i = 0; i<nbBoost; i++) {
			GameObject item = itemList[Random.Range(0, itemList.Count - 1)];
			Vector3 itemPosition = item.transform.position;
			Destroy(item);
			itemList.Remove(item);
			GameObject boost = (GameObject)Instantiate(BoostPrefab);
			boost.transform.position = itemPosition;
		}
	}


	private void MatchExits(ModuleConnector oldExit, ModuleConnector newExit)
	{
		var newModule = newExit.transform.parent;
		var forwardVectorToMatch = -oldExit.transform.forward;
		var correctiveRotation = Azimuth(forwardVectorToMatch) - Azimuth(newExit.transform.forward);
		newModule.RotateAround(newExit.transform.position, Vector3.up, correctiveRotation);
		var correctiveTranslation = oldExit.transform.position - newExit.transform.position;
		newModule.transform.position += correctiveTranslation;
	}


	private static TItem GetRandom<TItem>(TItem[] array)
	{
		return array[Random.Range(0, array.Length)];
	}


	private static Module GetRandomWithTag(IEnumerable<Module> modules, string tagToMatch)
	{
		var matchingModules = modules.Where(m => m.Tags.Contains(tagToMatch)).ToArray();
		return GetRandom(matchingModules);
	}


	private static float Azimuth(Vector3 vector)
	{
		return Vector3.Angle(Vector3.forward, vector) * Mathf.Sign(vector.x);
	}

	int bonusPop = 0;
	void Update(){
		if (bonusPop > 3000) {
			List<GameObject> mazeList = new List<GameObject>(GameObject.FindGameObjectsWithTag("MazeComponent"));
			List<GameObject> itemList = new List<GameObject>(GameObject.FindGameObjectsWithTag("Item"));
			GameObject bonus = (GameObject)Instantiate(BonusPrefab);
			bonus.transform.position = mazeList[Random.Range(0, mazeList.Count)].transform.position + Vector3.up;
			foreach(GameObject obj in itemList){
				if(obj.transform.position == bonus.transform.position){
					GameObject.Destroy(bonus);
					break;
				}
			}
			bonusPop = 0;
				} else
						bonusPop++;
		}
}
