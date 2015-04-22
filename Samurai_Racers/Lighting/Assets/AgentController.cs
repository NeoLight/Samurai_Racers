using UnityEngine;
using System.Collections;

public class AgentController : MonoBehaviour {
			
	public KeyCode moveUp;
	public KeyCode moveDown;
	public KeyCode moveLeft;
	public KeyCode moveRight;

	public float objectSpeed;
	
	// Update is called once per frame
	void Update () {
		//Use keyboard input to control the sphere
		if (Input.GetKey (moveUp)) {
			transform.Translate (Vector3.forward * objectSpeed * Time.deltaTime);
		} 
		else if (Input.GetKey (moveDown))
		{
			transform.Translate (Vector3.back * objectSpeed * Time.deltaTime);
		}
		else if (Input.GetKey (moveLeft)) 
		{
			transform.Translate (Vector3.left * objectSpeed * Time.deltaTime);
		} 
		else if (Input.GetKey (moveRight)) 
		{
			transform.Translate (Vector3.right * objectSpeed * Time.deltaTime);
		}
	}
}
