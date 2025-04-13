using UnityEngine;
using System.Collections;

public class MakeMeAMaze : MonoBehaviour {


	// Maze vars

	public int mazeHeight = 15;
	public int mazeWidth = 15;

	private int[,]mazeArray;

	public GameObject wallObject;
	public GameObject mazeRunner;
	public GameObject beaconLight;
	public GameObject nodeObject;

	private Vector3 _runnerSpawn;


	static System.Random _random = new System.Random();




	// Use this for initialization
	void Start () 
	{
		//Run maze generator
		mazeArray = makeMaze (mazeHeight, mazeWidth);

		//Spawn floor under the maze
		GameObject floor = Instantiate(wallObject) as GameObject;

		if (floor != null) 
		{
			floor.transform.position = new Vector3 (mazeHeight/2, -1, mazeWidth/2);
			floor.transform.localScale = new Vector3(mazeWidth, 1, mazeHeight);

			BoxCollider floorCollider = floor.GetComponent<Collider>() as BoxCollider;

			if (floorCollider != null)
				floorCollider.size = new Vector3(1,1,1);
		}


		//Show maze in Unity
		for (int i = 0; i < mazeHeight; i++)
		{
			for (int j = 0; j < mazeWidth; j++)
			{
				if (mazeArray[i,j] == 1) // is wall block
				{
					//put block spawn position
					Vector3 position = new Vector3(i, 0 , j);

					//spawn the wall as either a node or a normal wall


					int wallChooser = _random.Next(100)+1;
					GameObject wall = null;

					if(wallChooser < 25 && i!=0 && i!=mazeHeight-1 && j!=0 && j!= mazeWidth-1)//percentage of nodes:normal walls
						wall = Instantiate(nodeObject) as GameObject;
					else
						wall = Instantiate(wallObject) as GameObject;

					//put wall into the position
					if(wall != null)
						wall.transform.position = position;
				}

				if (mazeArray[i,j] == 2)//is end of maze
				{
					Vector3 position = new Vector3(i, .3F , j);

					//spawn beacon
					GameObject beacon = Instantiate(beaconLight) as GameObject;

					//put it at the end!
					if (beacon != null)
						beacon.transform.position = position;
				}
			}
		}



		//Spawn the maze runner
		GameObject runner = Instantiate(mazeRunner) as GameObject;

		if(runner !=  null)
			runner.transform.position = _runnerSpawn;
	
	}


	private int [,] makeMaze(int height, int width)
	{
		//Make temp maze to get passed to mazeArray
		int[,] maze = new int[height, width];

		//init all cells to be cubes
		for (int i = 0; i < height; i++) 
		{
			for (int j = 0; j < width; j++)
			{
				maze[i,j] = 1;
			}
		}

		// new random seed

		System.Random rand = new System.Random();

		//pick random starting cell

		int row = rand.Next(height);
		while (row%2 == 0)
			row = rand.Next(height);

		int col = rand.Next(width);
		while (col%2 == 0)
			col = rand.Next(width);



		//set starting cell to a path with val = 0
		maze[row, col] = 0;

		//set starting cell to be spawn for the runner
		_runnerSpawn= new Vector3(col, 0, row);

		//make the maze with Depth first search
		MazeCarver(maze, row, col);

		carveExit (maze, height, width);

		//return the maze
		return maze;


	}

	private void MazeCarver(int[,] maze, int row, int col)
	{
		//pick north south east or west
		// north = 1
		// east = 2
		// south = 3
		// west = 4

		int[] directions = new int[] {1, 2, 3, 4};

		Shuffle(directions);

		// Look for random direction 2 block ahead
		for (int i = 0; i < directions.Length; i++) 
		{
			switch (directions[i])
			{
				case 1:// NORTH
					//check that we dont go off the grid
					if (row - 2 <= 0)
						continue;
					//carve out north
					if (maze[row - 2, col] != 0)
					{
						maze[row - 2, col] = 0;
						maze[row - 1, col] = 0;
						MazeCarver(maze, row - 2, col);
					}
					break;

				case 2: // EAST
					//check that we dont go off the grid
					if (col + 2 >= mazeWidth - 1)
						continue;
					//carve out east
					if (maze[row , col + 2] != 0)
					{
						maze[row, col + 2] = 0;
						maze[row, col + 1] = 0;
						MazeCarver(maze, row , col + 2);
					}
					break;

				case 3: // SOUTH
					//check that we dont go off the grid
					if (row + 2 >= mazeHeight - 1)
						continue;
					//carve out south
					if (maze[row + 2, col] != 0)
					{
						maze[row + 2, col] = 0;
						maze[row + 1, col] = 0;
						MazeCarver(maze, row + 2, col);
					}
					break;

				case 4: // WEST
					//check that we dont go off the grid
					if (col - 2 <= 0)
						continue;
					//carve out west
					if (maze[row, col - 2] != 0)
					{
						maze[row, col - 2] = 0;
						maze[row, col - 1] = 0;
						MazeCarver(maze, row, col - 2);
					}
					break;

			}
		}
	}

	private void carveExit(int[,] maze, int height, int width)
	{
		//new random seed
		System.Random rand = new System.Random();

		//pick random border cell cardinal direction

		int dir = rand.Next(4)+ 1;

		// north = 1
		// east = 2
		// south = 3
		// west = 4

		if (dir == 1) //north border exit
		{
			//pick random exit along north border 
			int exitCandidate = rand.Next(width);

			//check south of candidate and change if it's a wall
			while(maze[1, exitCandidate] == 1)
				exitCandidate = rand.Next(width);

			maze[0, exitCandidate] = 2;
		}

		else if (dir == 2) //east border exit
		{
			//pick random exit along east border 
			int exitCandidate = rand.Next(height);

			//check west of candidate and change if it's a wall
			while(maze[exitCandidate, width - 2] == 1)
				exitCandidate = rand.Next(height);
			
			maze[exitCandidate, width - 1] = 2;
		}

		else if (dir == 3) //south border exit
		{
			//pick random exit along south border 
			int exitCandidate = rand.Next(width);

			//check north of candidate and change if it's a wall
			while(maze[height - 2, exitCandidate] == 1)
				exitCandidate = rand.Next(width);
			
			maze[height - 1, exitCandidate] = 2;
		}

		else if (dir == 4) //west border exit
		{
			//pick random exit along west border 
			int exitCandidate = rand.Next(height);

			//check east of candidate and change if it's a wall
			while(maze[exitCandidate, 1] == 1)
				exitCandidate = rand.Next(width);
			
			maze[exitCandidate, 0] = 2;
		}
	}

	//fisher-yates shuffle
	public static void Shuffle<T>(T[] array)
	{
		var random = _random;
		for (int i = array.Length; i > 1; i--) 
		{
			//random element to swap
			int j = random.Next(i);
			//swap em
			T tmp = array[j];
			array[j] = array[i-1];
			array[i-1] = tmp;
				
		}


	}































}
