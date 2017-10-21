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


	private void Awake()
	{
		BOARD_SIZE = BoardManager.BOARD_SIZE;
		AI_Instance = this;
	}

	private void Start()
	{
		boardSituation = new int[BOARD_SIZE, BOARD_SIZE];
	}

	private void UpdateData()
	{
		X = BoardManager.boardManagerInstance.X;
		Y = BoardManager.boardManagerInstance.Y;
		boardSituation = BoardManager.boardManagerInstance.boardSituation;
	}

	private void FindNeighborTilesCoordinates()
	{
		neighborTilesCoordinates.Clear ();
		for (int i = -1; i <= 1; i++)
		{
			for (int j = -1; j <= 1; j++) 
			{
				if (((X + i) >= 0) && 
					((Y + j) >= 0) && 
					((X + i) < BOARD_SIZE) && 
					((Y + j) < BOARD_SIZE) &&
					((X + i) != (Y + j))) 
				{
					if (boardSituation [X + i, Y + j] == 0) 
					{
						neighborTileCoordinate.x = X + i;
						neighborTileCoordinate.z = Y + j;
						neighborTilesCoordinates.Add (neighborTileCoordinate);
					}
				}
			}
		}
	}

	public Vector3 RandomPlace()
	{
		UpdateData ();
		int index, neighborX, neighborY;
		FindNeighborTilesCoordinates ();
		if (neighborTilesCoordinates.Count != 0) 
		{
			index = Random.Range (0, neighborTilesCoordinates.Count);
			neighborX = (int)neighborTilesCoordinates[index].x;
			neighborY = (int)neighborTilesCoordinates[index].z;
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

	public float AlphaBeta(node currentNode, int futureMoves, float a, float b, bool isMaximizingPlayer)
	{
		// base case.
		if ((futureMoves == 0) || 
			(ChainChecker.chainCheckerInstance.IsEnd(currentNode.currentX, 
				currentNode.currentY, 
				currentNode.currentBoardSituation)))
		{
			return Heuristic(currentNode.currentBoardSituation);
		}

		if (isMaximizingPlayer)
		{
			List<node> childrenNodes = new List<node>();
			childrenNodes = ExpandNodes (currentNode, true);
			foreach (node whiteNode in childrenNodes)
			{
				a = Max(a, AlphaBeta(whiteNode, futureMoves - 1, a, b, false));
				Debug.Log ("maxing working");

				if (a >= b) break; //prune
			}
			return a;
		}
		else
		{
			List<node> childrenNodes = new List<node>();
			childrenNodes = ExpandNodes (currentNode, false);
			foreach (node blackNode in childrenNodes)
			{
				b = Min(b, AlphaBeta(blackNode, futureMoves - 1, a, b, true));
				Debug.Log ("min working");

				if (a >= b) break; //prune
			}
			return b;
		}
	}

	public List<node> ExpandNodes(node currentNode, bool isWhiteTurn)
	{
		List<node> childrenNodes = new List<node> ();
		node childrenNode;

		for (int i = currentNode.currentY - 4; i <= (currentNode.currentY + 4); i++) 
		{
			for (int j = currentNode.currentX - 4; j <= (currentNode.currentX + 4); j++) 
			{
				if ((i >= 0) &&
				    (j >= 0) &&
				    (i < BOARD_SIZE) &&
				    (j < BOARD_SIZE) &&
				    (currentNode.currentBoardSituation [i, j] == 0)) 
				{
					childrenNode.currentX = j;
					childrenNode.currentY = i;
					childrenNode.currentBoardSituation = currentNode.currentBoardSituation;
					if (isWhiteTurn) 
					{
						childrenNode.currentBoardSituation [i, j] = 1;
					} 
					else 
					{
						childrenNode.currentBoardSituation [i, j] = -1;
					}
					childrenNodes.Add (childrenNode);
				}
			}
		}
		return childrenNodes;
	}

	private float Heuristic(int[,] boardSituation)
	{
		int center = (int)BOARD_SIZE / 2;
		float maximazorSum = 0;
		float minimizorSum = 0;
		int maximazorNum = 0;
		int minimizorNum = 0;

		for (int i = 0; i < BOARD_SIZE; i++) {
			for (int j = 0; j < BOARD_SIZE; j++) {
				if (boardSituation [j, i] == 1) {
					minimizorSum -= Mathf.Sqrt ((float)((j - center) * (j - center) + (i - center) * (i - center)));
					minimizorNum++;
				}
				if (boardSituation [j, i] == -1) {
					maximazorSum += Mathf.Sqrt ((float)((j - center) * (j - center) + (i - center) * (i - center)));
					maximazorNum++;
				}
			}
		}

		if ((minimizorNum + maximazorNum) % 2 == 0) 
		{
			return (minimizorSum / minimizorNum);
		} 
		else 
		{
			return (maximazorSum / maximazorNum);
		}
	}

	private float Max(float a, float value)
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

	private float Min(float b, float value)
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