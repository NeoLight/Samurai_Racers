﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Pathfinding : MonoBehaviour {
	
	public Transform seeker, target;
	public float objectSpeed;
	Vector3 newPosition;
	Grid grid;
	
	void Update()
	{
		FindPath (seeker.position, target.position);
		
		if(grid.path != null)
		{
			var first =  grid.path.FirstOrDefault();
			if (first != null){
				newPosition = (first.worldPosition);
				seeker.position = Vector3.MoveTowards(seeker.position, newPosition, objectSpeed * Time.deltaTime);
			}
		}
	}
	
	void Awake()
	{
		grid = GetComponent<Grid> ();
	}
	
	void FindPath(Vector3 startPos, Vector3 targetPos)
	{
		Node startNode = grid.NodeFromWorldPoint (startPos);
		Node targetNode = grid.NodeFromWorldPoint (targetPos);
		
		List<Node> openSet = new List<Node>();
		HashSet<Node> closedSet = new HashSet<Node>();
		openSet.Add(startNode);
		
		while(openSet.Count > 0)
		{
			Node currentNode = openSet[0];
			for(int i = 1; i<openSet.Count; i++)
			{
				if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
				{
					currentNode = openSet[i];
				}
			}
			
			openSet.Remove(currentNode);
			closedSet.Add(currentNode);
			
			if(currentNode == targetNode)
			{
				RetracePath (startNode, targetNode);
				return;
			}
			
			foreach (Node neighbour in grid.GetNeighbours(currentNode))
			{
				if(!neighbour.walkable || closedSet.Contains(neighbour))
				{
					continue;
				}
				
				int newMovemetnCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
				if(newMovemetnCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
				{
					neighbour.gCost = newMovemetnCostToNeighbour;
					neighbour.hCost = GetDistance(neighbour, targetNode);
					neighbour.parent = currentNode;
					
					if(!openSet.Contains(neighbour))
					{
						openSet.Add(neighbour);
					}
				}
			}
		}
	}
	
	void RetracePath (Node startNode, Node endNode)
	{
		List<Node> path = new List<Node> ();
		Node currentNode = endNode;
		
		while(currentNode != startNode)
		{
			path.Add(currentNode);
			currentNode = currentNode.parent;
		}
		path.Reverse ();
		
		grid.path = path;
	}
	
	int GetDistance(Node nodeA, Node nodeB)
	{
		int dstX = Mathf.Abs (nodeA.gridX - nodeB.gridX);
		int dstY = Mathf.Abs (nodeA.gridY - nodeB.gridY);
		
		if (dstX > dstY) 
		{
			return (14 * dstY + 10 * (dstX - dstY));
		}
		return (14 * dstX + 10 * (dstY - dstX));
	}
}
