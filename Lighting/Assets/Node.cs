using UnityEngine;
using System.Collections;

public class Node {
	
	public bool walkable;
	public Vector3 worldPosition;
	
	//The costs are used for the A* algorithm
	public int gCost; //Current Cost from the Target Node
	public int hCost; //Estimated Cost from the Target Node
	public int fCost{
		get{
			return gCost + hCost;
		}
	}
	
	public int gridX;
	public int gridY;
	
	public Node parent;
	
	public Node(bool walkable, Vector3 worldPosition, int gridX, int gridY){
		this.walkable = walkable;
		this.worldPosition = worldPosition;
		this.gridX = gridX;
		this.gridY = gridY;
		
		parent = null;
	}
}