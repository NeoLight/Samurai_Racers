using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {
	public Vector2 Smoothing;
	public Transform Player;
	public bool isFollowing { get; set; }

	private float rightBound;
	private float leftBound;
	private float topBound;
	private float bottomBound;
	private MeshRenderer spriteBounds;

	// Use this for initialization
	void Start () {
		float vertExtent = Camera.main.camera.orthographicSize;  
		float horzExtent = vertExtent * Screen.width / Screen.height;
		spriteBounds = GameObject.Find("Background").GetComponentInChildren<MeshRenderer>();
		isFollowing = true;
		leftBound = (float)(horzExtent - spriteBounds.bounds.size.x / 2.0f);
		rightBound = (float)(spriteBounds.bounds.size.x / 2.0f - horzExtent);
		bottomBound = (float)(vertExtent - spriteBounds.bounds.size.y / 2.0f);
		topBound = (float)(spriteBounds.bounds.size.y  / 2.0f - vertExtent);
	}
	
	// Update is called once per frame
	void Update () {
		var pos = new Vector3(Player.position.x, Player.position.y, transform.position.z);
		pos.x = Mathf.Clamp(pos.x, leftBound, rightBound);
		pos.y = Mathf.Clamp(pos.y, bottomBound, topBound);
		transform.position = pos;
		CameraMovement ();
	}

	void CameraMovement()
	{
		var x = transform.position.x;
		var y = transform.position.y;

		if (isFollowing) 
		{
			x = Mathf.Lerp(x, Player.position.x, Smoothing.x * Time.deltaTime);
			y = Mathf.Lerp(y, Player.position.y, Smoothing.y * Time.deltaTime);
		}

		transform.position = new Vector3 (x, y, transform.position.z);
	}
}
