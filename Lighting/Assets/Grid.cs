using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour {
	
	public Vector3 gridWorldSize;
	public float nodeRadius;
	public LayerMask unwalkableMask;
	
	Node[,] grid;
	
	float nodeDiameter;
	int gridSizeX, gridSizeY;
	
	public List<Node> path;

	//This portion sets each node's diameter and the size of the grid
	void Start(){
		nodeDiameter = nodeRadius * 2;
		gridSizeX = Mathf.RoundToInt (gridWorldSize.x / nodeDiameter);
		gridSizeY = Mathf.RoundToInt (gridWorldSize.y / nodeDiameter);
		CreateGrid ();
	}
	
	//This function creates the grid and marks if any node is not walkable
	void CreateGrid(){
		grid = new Node[gridSizeX, gridSizeY];
		Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.up * gridWorldSize.y / 2;
		
		for(int x = 0; x < gridSizeX; x++){
			for(int y = 0; y < gridSizeY; y++){
				Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.up * (y * nodeDiameter + nodeRadius);
				bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));
				grid[x, y] = new Node(walkable, worldPoint, x, y);
			}
		}
	}
	
	//This function returns a list of nodes that are surrounding a certain node
	public List<Node> GetNeighbors(Node node){
		List<Node> neighbors = new List<Node> ();
		
		for (int x = -1; x <= 1; x++) {
			for (int y = -1; y <= 1; y++) {
				if(x==0 && y==0){
					continue;
				}
				
				int checkX = node.gridX + x;
				int checkY = node.gridY + y;
				
				if(checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY){
					neighbors.Add(grid[checkX, checkY]);
				}
			}
		}
		
		return neighbors;
	}
	
	//This function converts a point on the map to a node in the grid
	public Node NodeFromWorldPoint(Vector3 worldPosition){
		//Due to the center of the map being 0,0 the worldPosition will have to be
		//offset by half the world size
		float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
		float percentY = (worldPosition.y + gridWorldSize.y / 2) / gridWorldSize.y;
		
		int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
		int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
		
		return grid [x, y];
	}

	public void UpdateNodeMap(){
		for(int x = 0; x < gridSizeX; x++){
			for(int y = 0; y < gridSizeY; y++){
				grid[x, y].walkable = !(Physics.CheckSphere(grid[x, y].worldPosition, nodeRadius, unwalkableMask));
			}
		}
	}

	public Node[,] GetNodeMap(){
		return grid;
	}

	public int GetGridSizeX(){
		return gridSizeX;
	}

	public int GetGridSizeY(){
		return gridSizeY;
	}

	//This function uses gizmos to draw the grid to the screen
	void OnDrawGizmos(){
		Gizmos.DrawWireCube (transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y, 0.5f));
		
		if (grid != null) {
			foreach(Node node in grid){
				if(node.walkable){
					Gizmos.color = Color.black;
				}
				else{
					Gizmos.color = Color.red;
				}
				
				if(path != null){
					if(path.Contains(node)){
						Gizmos.color = Color.cyan;
					}
				}
				
				Gizmos.DrawCube(node.worldPosition, Vector3.one * (nodeDiameter - .05f));
			}
		}
	}	
}