using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyController : MonoBehaviour {
	
	public int Speed;
	Vector3 moveTo;
	
	Grid grid;
	Node[,] nodeMap;
	int gridSizeX;
	int gridSizeY;
	List<Node> path;

	void Start (){
		grid = null;
		path = null;
	}

	// Update is called once per frame
	void Update () {
		if(grid == null && nodeMap == null){
			grid = (Grid) GameObject.Find("MapGrid").GetComponent("Grid");
			nodeMap = grid.GetNodeMap();

			SetNewInitialPosition();
			SetNewFinalPosition();
			FindPath(transform.position, moveTo);
		}

		if (path != null) {
			if(path[0].walkable){
				transform.position = Vector3.MoveTowards (transform.position, path[0].worldPosition, Speed * Time.deltaTime);
			}else{
				FindPath(transform.position, moveTo);
			}

			if(transform.position == path[0].worldPosition){
				if(path.Count == 1){
					SetNewInitialPosition();
					SetNewFinalPosition();
					FindPath(transform.position, moveTo);
				}
				else{
					path.RemoveAt(0);
				}
			}
		}
	}

	void SetNewInitialPosition(){
		if (gridSizeX == 0 && gridSizeY == 0) {
			gridSizeX = grid.GetGridSizeX();
			gridSizeY = grid.GetGridSizeY();
		}

		bool isBlocked = true;
		while (isBlocked) {
			int newX = Random.Range (0, gridSizeX);
			int newY = Random.Range (0, gridSizeY);

			if(nodeMap[newX, newY].walkable){
				transform.position = nodeMap[newX, newY].worldPosition; 
				isBlocked = false;
			}
		}
	}

	void SetNewFinalPosition(){
		bool isBlocked = true;
		while (isBlocked) {
			int newX = Random.Range (0, gridSizeX);
			int newY = Random.Range (0, gridSizeY);
			
			if(nodeMap[newX, newY].walkable){
				moveTo = nodeMap[newX, newY].worldPosition; 
				isBlocked = false;
			}
		}
	}

	void FindPath(Vector3 startPosition, Vector3 endPosition){
		//Get the start and target nodes for the path
		Node startNode = grid.NodeFromWorldPoint (startPosition);
		Node endNode = grid.NodeFromWorldPoint (endPosition);
		
		List<Node> openSet = new List<Node> ();
		HashSet<Node> closedSet = new HashSet<Node> ();
		
		openSet.Add (startNode);
		
		while (openSet.Count > 0) {
			Node currentNode = openSet[0];
			
			for(int i = 1; i< openSet.Count; i++){
				if(openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost){
					currentNode = openSet[i];
				}
			}
			openSet.Remove(currentNode);
			closedSet.Add(currentNode);
			
			if(currentNode == endNode){
				RetracePath(startNode, endNode);
				return;
			}
			
			//Cycle through each node next to the neighbor
			foreach(Node neighbor in grid.GetNeighbors(currentNode)){
				if(!neighbor.walkable || closedSet.Contains(neighbor)){
					continue;
				}
				
				//This portion updates the node's gCost, hCost, and parent node if the node is
				//either not in the openSet or if its gCost is less than the new gCost
				int newMovementCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbor);
				if(newMovementCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor)){
					neighbor.gCost = newMovementCostToNeighbor;
					neighbor.hCost = GetDistance(neighbor, endNode);
					neighbor.parent = currentNode;
					
					if(!openSet.Contains(neighbor)){
						openSet.Add(neighbor);
					}
				}
			}
		}
	}
	
	//This function retrieves the nodes in the generated path
	void RetracePath(Node startNode, Node endNode){
		List<Node> path = new List<Node> ();
		Node currentNode = endNode;
		
		while (currentNode != startNode) {
			path.Add(currentNode);
			currentNode = currentNode.parent;
		}
		
		path.Reverse ();
		
		this.path = path;
	}
	
	//This function calculates the distance between two nodes
	//For the nodes that are side by side the distance is 10,
	//for diagonal, the distance is 14 which is about sqrt(20)
	int GetDistance(Node nodeA, Node nodeB){
		int distanceX = Mathf.Abs (nodeA.gridX - nodeB.gridX);
		int distanceY = Mathf.Abs (nodeA.gridY - nodeB.gridY);
		
		//1 diagonal = 1 X Distance and 1 Y Distance
		if(distanceX > distanceY){
			return 14 * distanceY + 10 * (distanceX - distanceY);
		}
		
		return 14 * distanceX + 10 * (distanceY - distanceX);
	}
}
