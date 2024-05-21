using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Door : MonoBehaviour
{
	bool isOpening = false;
	float angle = 0f;
	float speed = -20;
	// Start is called before the first frame update

	public CombatZone _combatZone;

	void Start()
	{
	}

	// Update is called once per frame
	void Update()
	{
		if (_combatZone.isCLear)
		{
			isOpening = true;
		}

		OpenTheDoor(isOpening);

		if(Mathf.Abs(angle) >= 80f)
		{
			isOpening=false;
		}

	}
	void OpenTheDoor(bool value)
	{
		if (isOpening)
		{
			angle += Time.deltaTime * speed ;
			GetComponent<Transform>().Rotate(new Vector3(0f, 1f, 0f), Time.deltaTime*speed);
		}
	}
}
