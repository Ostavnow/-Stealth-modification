using System.Collections.Generic;
using UnityEngine;

public class ListEnemy : MonoBehaviour
{
	// Token: 0x060002BE RID: 702 RVA: 0x0000EBF4 File Offset: 0x0000CDF4
	private void Awake()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("Bot");
		FieldOfView [] flashLight = FindObjectsOfType<FieldOfView>();
		for (int j = 0; j < array.Length; j++)
		{
			_listTransform.Add(array[j].gameObject.transform);
		}
		for (int i = 0; i < flashLight.Length; i++)
		{
			list.Add(flashLight[i]);
		}
		ListEnemy.instance = this;
	}
	public void FindAll()
	{
		for (int i = 0; i < list.Count; i++)
		{
			if(list[i] != null)
			{
				list[i].StartFindPlayer();
			}
		}
	}
	public void DefaultSun()
	{
	}
	public List<FieldOfView> list = new List<FieldOfView>();
	public static ListEnemy instance;

	[HideInInspector]
	public List<Transform> _listTransform = new List<Transform>();
}

