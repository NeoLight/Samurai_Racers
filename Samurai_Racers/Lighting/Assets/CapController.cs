using UnityEngine;
using System.Collections;
public class CapController : MonoBehaviour
{
	public float moveSpeed;
	private Vector3 moveDirection;

	void Update()
	{
		Movement ();
	}
	
	void Movement()
	{
		moveDirection = new Vector3 (0,0,0);
		
		if( Input.GetKey(KeyCode.D) ) 
		{
			transform.Translate (Vector3.right * moveSpeed * Time.deltaTime);
			//heading = 0;
			//transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler( 0, 0, heading), 15);
		}
		if( Input.GetKey(KeyCode.W) ) 
		{
			transform.Translate (Vector3.forward * moveSpeed * Time.deltaTime);
			//heading = 90;
			//transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler( 0, 0, heading), 15);
		}
		if( Input.GetKey(KeyCode.A) ) 
		{
			transform.Translate (Vector3.left * moveSpeed * Time.deltaTime);
			//heading = 180;
			//transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler( 0, 0, heading), 15);
		}
		if( Input.GetKey(KeyCode.S) ) 
		{
			transform.Translate (Vector3.back * moveSpeed * Time.deltaTime);
			//heading = 270;
			//transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler( 0, 0, heading), 15);
		}
		//newPosition = transform.position;
	}
}