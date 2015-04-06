using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Maze : MonoBehaviour {

	[System.Serializable]
	public class Cell
	{
		public bool visited;
		public GameObject north;//1
		public GameObject east;//2
		public GameObject west;//3
		public GameObject south;//4
		public Vector3 center;
	}
	
	
	
	public GameObject wall;
	public GameObject plane;
	public float wallLength = 1.0f;
	public int xSize;
	public int ySize;
	private int currentCell=0;
	private Vector3 initialPos;
	private GameObject wallHolder;
	private Cell[] cells;
	private int maxNeighbours =4;
	private int totalCell=0;
	private int visitedCells=0;
	private bool startedBuild=false;
	private int curNeighbour=0;
	private List<int> lastCells;
	private int backUp=0;
	private int wallToBreak=0;

	// Use this for initialization
	void Start () 
	{
		GameObject temp;
		CreateWalls ();
		plane.transform.localScale= new Vector3 (xSize*.5f, 1, ySize*.5f);
		temp = Instantiate (plane) as GameObject;
		temp.name = "Floor";
	}
	
	void CreateWalls()
	{
		wallHolder = new GameObject ();
		wallHolder.name = "Maze";
		int temp = 0;
		initialPos= new Vector3((-xSize/2)+wallLength/2,0.0f,(-ySize/2)+wallLength/2); 
		Vector3 myPos = initialPos;
		GameObject tempWall;
		
		// x axis
		for (int i=0; i < ySize; i++)
		{
			for (int j=0; j<=xSize; j++)
			{
				myPos=new Vector3(initialPos.x+(j*wallLength)-wallLength/2,
				                  0.0f,initialPos.z+(i*wallLength)-wallLength/2);
				tempWall=Instantiate(wall,myPos,Quaternion.identity) as GameObject;
				tempWall.name="X wall number: "+temp++;
				tempWall.transform.parent= wallHolder.transform;
			}
		}
		temp = 0;
		//y axis
		for (int i=0; i<=ySize; i++)
		{
			for (int j=0; j<xSize; j++)
			{
				myPos=new Vector3(initialPos.x+(j*wallLength),
				                  0.0f,initialPos.z+(i*wallLength)-wallLength);
				tempWall=Instantiate(wall,myPos,Quaternion.Euler(0.0f,90.0f,0.0f)) as GameObject;
				tempWall.name="Y wall number: "+temp++;
				tempWall.transform.parent= wallHolder.transform;
			}
		}
		CreateCells ();
		
	}
	
	void CreateCells()
	{
		lastCells = new List<int> ();
	//	lastCells.Clear;
		totalCell = xSize * ySize;
		GameObject[] allWalls;
		int children = wallHolder.transform.childCount;
		allWalls = new GameObject[children];
		cells = new Cell[xSize * ySize];
		int eastWestProcess = 0;
		int childProcess = 0;
		int termCount = 0;
		
		//get all the children
		for (int i=0; i<children; i++) 
		{
			allWalls[i]=wallHolder.transform.GetChild(i).gameObject;
		}
		
		//assigns walls to the cells
		for (int cellprocess=0; cellprocess<cells.Length; cellprocess++) 
		{
			cells[cellprocess] =new Cell();
			cells[cellprocess].west = allWalls[eastWestProcess];
			cells[cellprocess].south =allWalls[childProcess+(xSize+1)*ySize];
			cells[cellprocess].center=cells[cellprocess].south.transform.position+new Vector3(0,0,.5f);
			if(termCount == xSize)
			{
				eastWestProcess+=2;
				termCount=0;
			}
			else
				eastWestProcess++;
			termCount++;
			childProcess++;
			cells[cellprocess].east = allWalls[eastWestProcess];
			cells[cellprocess].north =allWalls[(childProcess+(xSize+1)*ySize)+xSize-1];
			//Debug.Log("position of cell :"+cellprocess+ "= "+cells[cellprocess].center);
		}

	
		CreateMaze ();
	}
	
	void CreateMaze()
	{
		//Neighbours ();
		while(visitedCells<totalCell)
		{
			if(startedBuild)
			{
				Neighbours ();
				if(cells[curNeighbour].visited==false &&cells[currentCell].visited==true)
				{
					BreakWall();
					cells[curNeighbour].visited=true;
					visitedCells++;
					lastCells.Add(currentCell);
					currentCell=curNeighbour;
					if(lastCells.Count>0)
					{
						backUp=lastCells.Count-1;
					}
				}
			}
			else
			{
				currentCell=Random.Range(0,totalCell);
				cells[currentCell].visited=true;
				visitedCells++;
				startedBuild=true;

			}

		//us only if ther eis no while at the head of this function
		//Invoke("CreateMaze",0.0f);

		}
		Debug.Log("Finished");
	}

	void BreakWall()
	{
		switch (wallToBreak) {
		case 1: Destroy(cells[currentCell].north);break;
		case 2: Destroy(cells[currentCell].east);break;
		case 3: Destroy(cells[currentCell].west);break;
		case 4: Destroy(cells[currentCell].south);break;

		}
	}
	void Neighbours()
	{
		//Debug.Log ("Checking Neighbours");
		int length = 0;
		int[] neightbours=new int[maxNeighbours];
		int[] connectWall=new int[4];
		int check = 0;
		check = ((currentCell + 1) / xSize);
		check -= 1;
		check *= xSize;
		check += xSize;
		
		//east
		if (currentCell + 1 < totalCell && (currentCell+1)!=check) 
		{
			if(cells[currentCell+1].visited==false)
			{
				neightbours[length]= currentCell+1;
				connectWall[length]=2;
				length++;
			}
		}
		
		//west
		if (currentCell - 1 >= 0&& currentCell!=check) 
		{
			if(cells[currentCell - 1].visited==false)
			{
				neightbours[length]= currentCell-1;
				connectWall[length]=3;
				length++;
			}
		}
		//north
		if (currentCell +xSize<totalCell) 
		{
			if(cells[currentCell+xSize].visited==false)
			{
				neightbours[length]= currentCell+xSize;
				connectWall[length]=1;
				length++;
			}
		}
		//south
		if (currentCell - xSize >= 0) 
		{
			if(cells[currentCell - xSize].visited==false)
			{
				neightbours[length]= currentCell-xSize;
				connectWall[length]=4;
				length++;
			}
		}
		//Debug.Log ("number or neighbors: " + length);
		if (length != 0) {
			int thisOne = Random.Range (0, length);
			curNeighbour = neightbours [thisOne];
			wallToBreak=connectWall[thisOne];
		}
		else 
		{
			if(backUp>0)
			{
				currentCell=lastCells[backUp];
				backUp--;
			}
		}
	}
	// Update is called once per frame
	void Update () 
	{
		
		
	}
}
