using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtificialIntelligence : MonoBehaviour
{
	private int BOARD_SIZE;
	private List<Vector3> neighborTilesCoordinates = new List<Vector3>();
	private List<Vector3> adjacentAllyTilesCoordinates = new List<Vector3>();
	private List<int> adjacentAllyTilesDirections = new List<int> ();
	private Vector3 neighborTileCoordinate, allyTileCoordinate;
	private int allyTileDirection;

	public int X { set; get; }
	public int Y { set; get; }
	public int[,] boardSituation { set; get; }
	public static ArtificialIntelligence AI_Instance { set; get; }


	private void Start()
	{
		BOARD_SIZE = BoardManager.BOARD_SIZE;
		AI_Instance = this;
	}

	private void Update()
	{
		X = BoardManager.boardManagerInstance.X;
		Y = BoardManager.boardManagerInstance.Y;
		boardSituation = BoardManager.boardManagerInstance.boardSituation;
	}

	private void FindNeighborTilesCoordinates()
	{
		neighborTilesCoordinates.Clear ();
		for (int i = -1; i <=1; i++)
		{
			for (int j = -1; j <= 1; j++) 
			{
				if (((X + i) >= 0) && 
					((Y + j) >= 0) && 
					((X + i) < BOARD_SIZE) && 
					((Y + j) < BOARD_SIZE) &&
					((X + i) != (Y + j))) 
				{
					neighborTileCoordinate.x = X + i;
					neighborTileCoordinate.z = Y + j;
					neighborTilesCoordinates.Add (neighborTileCoordinate);
				}
			}
		}
	}

	public Vector3 RandomPlace()
	{
		int index, neighborX, neighborY;
		FindNeighborTilesCoordinates ();
		if (neighborTilesCoordinates.Count != 0) 
		{
			index = Random.Range (0, neighborTilesCoordinates.Count);
			neighborX = (int)neighborTilesCoordinates [index].x;
			neighborY = (int)neighborTilesCoordinates [index].z;
			while (boardSituation [neighborX, neighborY] != 0) 
			{
				index = Random.Range (0, neighborTilesCoordinates.Count);
				neighborX = (int)neighborTilesCoordinates [index].x;
				neighborY = (int)neighborTilesCoordinates [index].z;
			}
			return neighborTilesCoordinates [index];
		} 
		else 
		{
			neighborX = Random.Range (0, BOARD_SIZE);
			neighborY = Random.Range (0, BOARD_SIZE);
			while (boardSituation [neighborX, neighborY] != 0) 
			{
				neighborX = Random.Range (0, BOARD_SIZE);
				neighborY = Random.Range (0, BOARD_SIZE);
			}
			neighborTileCoordinate.x = (float)neighborX;
			neighborTileCoordinate.z = (float)neighborY;
			return neighborTileCoordinate;
		}
	}

	/*private int alphabeta(node?, int depth, int a, int b, bool isMaximizingPlayer)
	{
		GetData ();
		// base case.
		if ((depth == 0) || terminalNode?)
		{
			return heuristic(boardSituation);
		}

		List<int[,]> childBoardSituations = new List<int[,]> ();
		childBoardSituations = getChildBoardSituations(boardSituation);

		if (isMaximizingPlayer)
		{
			foreach (int[,] childBoardSituation in childBoardSituations)
			{
				a = max(a, alphabeta(childBoardSituation, depth - 1, a, b, false));

				if (a >= b) break; //prune
			}
			return a;
		}
		else if (!isMaximizingPlayer)
		{
			foreach (int[,] childBoardSituation in childBoardSituations)
			{
				b = min(b, alphabeta(childBoardSituation, depth - 1, a, b, true));

				if (a >= b) break; //prune
			}
			return b;
		}
	}

	private bool isWon(int[,] boardSituation)
	{
	}

	private int heuristic(int[,] boardSituation)
	{
	}

	private List<int[,]> getChildBoardSituations(int[,] boardSituation)
	{
	}*/

	private int max(int a, int value)
	{
		if (a >= value) 
		{
			return a;
		} 
		else 
		{
			return value;
		}
	}

	private int min(int b, int value)
	{
		if (b <= value) 
		{
			return b;
		} 
		else 
		{
			return value;
		}
	}
}
	/*public int minimax(LinkedListNode node, int depth, bool isMaximizingPlayer)
	{
		int bestValue, value;

		// base case.
		if ((depth == 0) || (node == Terminal node))
		{
			reutrn heuristicvalue;
		}

		if (isMaximizingPlayer)
		{
			bestValue = -inf;
			foreach (child in node)
			{
				value = minimax(child, depth - 1, false);
				bestValue = max(bestValue, value);
			}
			return bestValue;
		}
		else if (!isMaximizingPlayer)
		{
			bestValue = inf;
			foreach (child in node)
			{
				value = minimax(child, depth - 1, true);
				bestValue = min(bestValue, value);
			}
			return bestValue;
		}
	}*/

	/*
}
/*
	// block for two-piece chain
	//public Vector3 blockTwoPieceChain(){}

	// block for three-peice chain
	/*public Vector3 blockThreePieceChain(GomokuPiece[,] gomokuPieces, int X, int Y)
	{
		SHIFTING_OFFSET = 2;
		DirectionIndexes = chainChecker.isThreePieceChain (gomokuPieces, X, Y);
		if (DirectionIndexes.Count != 0) // has tree piece chain.
		{
			hasFourPieceChain = true;
		}
	}

	// block for four-peice chain


	private void findAdjacentAllyTilesCoordinates(GomokuPiece[,] gomokuPieces, int X, int Y)
	{
		adjacentAllyTilesCoordinates.Clear ();
		for (int i = -1; i <=1; i++)
		{
			for (int j = -1; j <= 1; j++) 
			{
				if (((X + i) >= 0) && 
					((Y + j) >= 0) && 
					((X + i) < BOARD_SIZE) && 
					((Y + j) < BOARD_SIZE) &&
					((X + i) != (Y + j)) &&
					gomokuPieces[X+i,Y+i].isWhite == gomokuPieces[X,Y].isWhite) 
				{
					allyTileCoordinate.x = X + i;
					allyTileCoordinate.z = Y + j;
					adjacentAllyTilesCoordinates.Add (allyTileCoordinate);
					//adjacentAllyTilesDirections.Add ();
				}
			}
		}
	}

	public Vector3 place(GomokuPiece[,] gomokuPieces, int X, int Y)
	{
		findAdjacentAllyTilesCoordinates (gomokuPieces, X, Y);
		if (adjacentAllyTilesCoordinates.Count == 0) // NO adjacent ally tiles.
		{ 
			return randomPlace (gomokuPieces, X, Y);
		} 
		else 
		{
			
		}
	}*/
