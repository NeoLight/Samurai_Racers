using UnityEngine;
using System.Collections;

public class AgentController : MonoBehaviour {
			
	public KeyCode moveUp;
	public KeyCode moveDown;
	public KeyCode moveLeft;
	public KeyCode moveRight;
	public KeyCode rotateLeft;
	public KeyCode rotateRight;

	public float objectSpeed;
	public float turnSpeed;

	float heading = 0;
	Grid grid;

	// Update is called once per frame
	void Update () {
		//Use keyboard input to control the sphere
		if (Input.GetKey (moveUp)) {
			transform.Translate (Vector3.right * objectSpeed * Time.deltaTime);
		} else if (Input.GetKey (moveDown))
		{
			transform.Translate (Vector3.left * objectSpeed * Time.deltaTime);
		}else if (Input.GetKey (moveLeft)) 
		{
			transform.Translate (Vector3.up * objectSpeed * Time.deltaTime);
		} else if (Input.GetKey (moveRight)) 
		{
			transform.Translate (Vector3.down * objectSpeed * Time.deltaTime);
		}  
		if( Input.GetKey(rotateLeft) ) 
		{
			heading = (heading + turnSpeed) % 360;
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler( 0, 0, heading), 15);
		}
		if( Input.GetKey(rotateRight) ) 
		{
			heading = (heading - turnSpeed) % 360;
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler( 0, 0, heading), 15);
		}

		else {
			rigidbody.velocity = Vector3.zero;
			rigidbody.angularVelocity = Vector3.zero;
		}

		if (grid == null) {
			grid = (Grid)GameObject.Find ("MapGrid").GetComponent ("Grid");
		} else {
			grid.UpdateNodeMap();
		}
	}
}
