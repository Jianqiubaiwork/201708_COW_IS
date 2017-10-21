using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct node
{
	public int[,] currentBoardSituation;
	public int currentX;
	public int currentY;
}

public class BoardManager : MonoBehaviour 
{
	// variables
	private const int GAME_MODE = 3; // 1 for Human vs. PC; 2 for PC vs. Human; 3 for PC vs. PC
	private const float TILE_SIZE = 1.0f;
	private const float TILE_OFFSET = 0.5f;
	private Vector3 position;
	private List<GameObject> stoneList;

	public const int TEST_CAP = 1000;
	public const int BOARD_SIZE = 15;
	public int[,] boardSituation{ set; get; }
	public int X{ set; get; }
	public int Y{ set; get; }
	public bool isWhiteTurn;
	public bool isWon { set; get; }

	public List<GameObject> stonesPrefabs; // holds the black and white gomoku peice prefab
	public static BoardManager boardManagerInstance { set; get; }

	private void Awake()
	{
		boardManagerInstance = this;
	}

	// start the game
	private void Start()
	{
		// initilizations
		isWhiteTurn = false;
		isWon = false;
		boardSituation = new int[BOARD_SIZE, BOARD_SIZE];
		stoneList = new List<GameObject> ();
	}

	private void FixedUpdate()
	{
		DrawBoard ();
		if (Summerizor.summerizorInstance.roundsNum <= TEST_CAP) 
		{
			if (!isWon)
				PlaceStoneByPC ();
			if (isWon)
				StartOver ();
		}
		/*if (!isWon) 
		{
			switch (GAME_MODE) 
			{
			case 1:
				if (!isWhiteTurn)
					PlaceStoneByHuman (!isWhiteTurn);
				else
					PlaceStoneByPC ();
				break;
			case 2:
				if (!isWhiteTurn)
					PlaceStoneByPC ();
				else
					PlaceStoneByHuman (isWhiteTurn);
				break;
			case 3:
				PlaceStoneByPC ();
				break;
			case 4:
				if (!isWhiteTurn)
					PlaceStoneByHuman (!isWhiteTurn);
				else
					test ();
				break;
			default:
				break;
			}
		}*/
	}

	private void DrawBoard()
	{
		// set the width and the heigth of the board.
		Vector3 widthLine = Vector3.right * BOARD_SIZE;
		Vector3 heigthLine = Vector3.forward * BOARD_SIZE;

		// a double-forloop to draw horizontal lines (X) first,
		// and then draw vertical lines (Y) to make the board.
		for (int i = 0; i <= BOARD_SIZE; i++) 
		{
			Vector3 startX = Vector3.forward * i;
			Debug.DrawLine (startX, startX + widthLine);
			for (int j = 0; j <= BOARD_SIZE; j++) 
			{
				Vector3 startY = Vector3.right * j;
				Debug.DrawLine (startY, startY + heigthLine);
			}
		}

		// draw the selection
		if (X >= 0 && Y >= 0 && X <= BOARD_SIZE && Y <= BOARD_SIZE) 
		{
			Debug.DrawLine
			(
				Vector3.forward * Y + Vector3.right * X,
				Vector3.forward * (Y + 1) + Vector3.right * (X + 1)
			);
			Debug.DrawLine
			(
				Vector3.forward * Y + Vector3.right * (X + 1),
				Vector3.forward * (Y + 1) + Vector3.right * X
			);
		}
	}

	private void PlaceStoneByHuman(bool currentTurn)
	{
		if (!Camera.main)
			return;

		RaycastHit hit; 
		if (Physics.Raycast (
			Camera.main.ScreenPointToRay (Input.mousePosition), 
			out hit, 100.0f, LayerMask.GetMask ("GomokuPlane")) && currentTurn) 
		{
			X = (int)hit.point.x;
			Y = (int)hit.point.z;
			if (Input.GetMouseButtonDown (0) && boardSituation[X,Y] == 0) 
				PlaceStone (X, Y);
		} 
		else 
		{
			X = -1;
			Y = -1;
		}
	}

	private void PlaceStoneByPC()
	{
		if (Summerizor.summerizorInstance.movesNum == 0) 
		{
			X = (int)BOARD_SIZE / 2;
			Y = (int)BOARD_SIZE / 2;
		}
		if (Summerizor.summerizorInstance.movesNum > 0) 
		{
			position = ArtificialIntelligence.AI_Instance.RandomPlace ();
			X = (int)position.x;
			Y = (int)position.z;
		}
		PlaceStone (X, Y);
	}
		
	private void test()
	{
		float v = 0f;
		node theNode;
		node theOptimalNode;
		if (Summerizor.summerizorInstance.movesNum > 0) 
		{
			
			theNode.currentX = X;
			theNode.currentY = Y;
			theNode.currentBoardSituation = boardSituation;

			List<node> nodes = new List<node> ();
			nodes = ArtificialIntelligence.AI_Instance.ExpandNodes (theNode, isWhiteTurn);
			theOptimalNode = nodes [0];
			foreach(node childrenNode in nodes)
			{
				if (ArtificialIntelligence.AI_Instance.AlphaBeta (childrenNode, 20, -Mathf.Infinity, Mathf.Infinity, !isWhiteTurn) > v)
					theOptimalNode = childrenNode;
			}
			X = theOptimalNode.currentX;
			Y = theOptimalNode.currentY;
		}
		PlaceStone (X, Y);
	}

	private void PlaceStone(int X, int Y)
	{
		// figure out whether a white or a black piece needed to be placed based on the turn.
		int index;
		if (isWhiteTurn) 
			index = 0;
		else 
			index = 1;

		// clone a gomoku piece from the prefab list and place in the scene.
		GameObject stone = Instantiate (
			stonesPrefabs [index], 
			GetTileCenter(X, Y), 
			Quaternion.identity) 
			as GameObject;
		stoneList.Add (stone);

		UpdateData ();
		CheckWin ();
	}

	private void UpdateData()
	{
		// update moves in summerizor
		Summerizor.summerizorInstance.movesNum++;

		// record the GameObject into the implicit 2D GomokuPieces matrix.
		if (isWhiteTurn) 
			boardSituation [X, Y] = 1;
		else 
			boardSituation [X, Y] = -1;
	}

	private void CheckWin()
	{
		if (ChainChecker.chainCheckerInstance.IsWon ()) 
		{
			isWon = true;
			Summerizor.summerizorInstance.DataPreperation ();
			EndGame ();
			return;
		} 
		else 
		{
			isWhiteTurn = !isWhiteTurn;
		}
	}

	private void EndGame()
	{
		if (isWhiteTurn) 
			Summerizor.summerizorInstance.winningText.text = "The white wins!";
		else
			Summerizor.summerizorInstance.winningText.text = "The black wins!";
	}

	private void DestroyAllStones()
	{
		for (int i = 0; i < stoneList.Count; i++) 
		{
			Destroy (stoneList [i]);
		}
		stoneList.Clear ();

		for (int i = 0; i < BOARD_SIZE; i++) 
		{
			for (int j = 0; j < BOARD_SIZE; j++) 
			{
				boardSituation [i, j] = 0;
			}
		}
	}

	private Vector3 GetTileCenter(int X, int Y)
	{
		position = Vector3.zero;
		position.x += (TILE_SIZE * X) + TILE_OFFSET;
		position.z += (TILE_SIZE * Y) + TILE_OFFSET;
		return position;
	}

	public void StartOver()
	{
		if (Summerizor.summerizorInstance.roundsNum == TEST_CAP)
			return;
		
		DestroyAllStones ();
		isWon = false;
		isWhiteTurn = false;
		Summerizor.summerizorInstance.winningText.text = "";
		Summerizor.summerizorInstance.movesNum = 0;
		Summerizor.summerizorInstance.roundsNum++;
	}
}