using System;
using UnityEngine;

public class Gore : MonoBehaviour
{
	private static RaycastHit hit;

	public Gore()
	{
	}

	public void Awake()
	{
		if (!GameSettings.gore)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		else
		{
			for (int i = 0; i < 5; i++)
			{
				Physics.Raycast(base.transform.position, new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)), out Gore.hit, 16f, 34342913);
				if (Gore.hit.collider != null)
				{
					float single = UnityEngine.Random.Range(0.75f, 1.25f);
					GameObject vector3 = (GameObject)UnityEngine.Object.Instantiate(Resources.Load(string.Concat("Effects/blood_", UnityEngine.Random.Range(0, 4))), Gore.hit.point + (Gore.hit.normal * UnityEngine.Random.Range(0.04f, 0.06f)), Quaternion.LookRotation(Gore.hit.normal) * Quaternion.Euler(0f, 0f, (float)UnityEngine.Random.Range(0, 360)));
					vector3.name = "blood";
					vector3.transform.parent = NetworkEffects.model.transform;
					vector3.transform.localScale = new Vector3(single, single, single);
					UnityEngine.Object.Destroy(vector3, 20f);
				}
			}
		}
	}
}