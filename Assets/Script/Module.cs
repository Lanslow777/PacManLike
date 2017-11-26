using UnityEngine;

public class Module : MonoBehaviour
{
	public string[] Tags;
	public string Name;

	public ModuleConnector[] GetExits()
	{
		return GetComponentsInChildren<ModuleConnector>();
	}
}
