using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtificialIntelligence
{
	//private Random rnd = new Random();
	private List<Vector3> neighborTilesCoordinates = new List<Vector3>();
	private List<Vector3> adjacentAllyTilesCoordinates = new List<Vector3>();
	private List<int> adjacentAllyTilesDirections = new List<int> ();
	private Vector3 neighborTileCoordinate, allyTileCoordinate;
	private int allyTileDirection;
	private int BOARD_SIZE, SHIFTING_OFFSET;
	//private bool hasFourPieceChain, hasThreePieceChain, hasTwoPieceChain;
	//private ChainChecker chainChecker;
	//private const Vector3 zeroVector3 = Vector3.zero;

	// constructor
	public ArtificialIntelligence(int size)
	{
		BOARD_SIZE = size;
	}

	private void findNeighborTilesCoordinates(int X, int Y)
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

	/*public Vector3 place(GomokuPiece[,] gomokuPieces, int X, int Y)
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

	public Vector3 randomPlace(GomokuPiece[,] gomokuPieces, int X, int Y)
	{
		int index, neighborX, neighborY;
		findNeighborTilesCoordinates (X, Y);
		if (neighborTilesCoordinates.Count != 0) 
		{
			index = Random.Range (0, neighborTilesCoordinates.Count);
			neighborX = (int)neighborTilesCoordinates [index].x;
			neighborY = (int)neighborTilesCoordinates [index].z;
			while (gomokuPieces [neighborX, neighborY] != null) 
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
			while (gomokuPieces [neighborX, neighborY] != null) 
			{
				neighborX = Random.Range (0, BOARD_SIZE);
				neighborY = Random.Range (0, BOARD_SIZE);
			}
			neighborTileCoordinate.x = (float)neighborX;
			neighborTileCoordinate.z = (float)neighborY;
			return neighborTileCoordinate;
		}
	}

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
	}*/

	// block for four-peice chain
}