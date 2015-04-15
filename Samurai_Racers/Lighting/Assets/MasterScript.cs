using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MasterScript : MonoBehaviour {

	public GameObject maze;
	//public GameObject grid;

	// Use this for initialization
	void Start () {
		Instantiate (maze);
		//Instantiate (grid);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
