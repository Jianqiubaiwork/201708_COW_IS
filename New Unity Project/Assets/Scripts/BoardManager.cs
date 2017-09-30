using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour 
{
	// variables
	private const int GAME_MODE = 2; // 1 for Human vs. PC; 2 for PC vs. PC
	private const float TILE_SIZE = 1.0f;
	private const float TILE_OFFSET = 0.5f;
	private bool isWhiteTurn;
	private bool isWon;
	private Vector3 position;
	private ArtificialIntelligence AI;
	private ChainChecker chainChecker;
	private Summerizor summerizor;

	public const int BOARD_SIZE = 15;
	public int[,] boardSituation{ set; get; }
	public int X{ set; get; }
	public int Y{ set; get; }

	public List<GameObject> stonesPrefabs; // holds the black and white gomoku peice prefab
	public static BoardManager instance { set; get; }

	// start the game
	private void Start()
	{
		// initilizations
		isWhiteTurn = false;
		isWon = false;
		X = -1;
		Y = -1;
		boardSituation = new int[BOARD_SIZE, BOARD_SIZE];
		instance = this;
		AI = new ArtificialIntelligence ();
		chainChecker = new ChainChecker ();
		summerizor = new Summerizor ();

		if (GAME_MODE == 2) 
		{
			X = (int)BOARD_SIZE / 2;
			Y = (int)BOARD_SIZE / 2;
			PlaceStone (X, Y);
		}
	}

	private void Update()
	{
		DrawBoard ();
		if (!isWon) 
		{
			switch (GAME_MODE) 
			{
			case 1:
				PlaceStoneByHuman ();
				if (isWhiteTurn) 
				{
					PlaceStoneByPC ();
				}
				break;
			case 2:
				PlaceStoneByPC ();
				break;
			default:
				break;
			}
		}
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

	private void PlaceStoneByHuman()
	{
		if (!Camera.main)
			return;

		RaycastHit hit; 
		if (Physics.Raycast (
			Camera.main.ScreenPointToRay (Input.mousePosition), 
			out hit, 100.0f, LayerMask.GetMask ("GomokuPlane")) && !isWhiteTurn) 
		{
			X = (int)hit.point.x;
			Y = (int)hit.point.z;
			if (Input.GetMouseButtonDown (0) && boardSituation[X,Y] == 0) 
			{			
				PlaceStone (X, Y);
			}

		} 
		else 
		{
			X = -1;
			Y = -1;
		}
	}

	private void PlaceStoneByPC()
	{
		position = AI.RandomPlace ();
		X = (int)position.x;
		Y = (int)position.z;
		PlaceStone (X, Y);
	}

	private void PlaceStone(int X, int Y)
	{
		// figure out whether a white or a black piece needed to be placed based on the turn.
		int index;
		if (isWhiteTurn) 
		{
			index = 0;
		} 
		else 
		{
			index = 1;
		}

		// clone a gomoku piece from the prefab list and place in the scene.
		GameObject stone = Instantiate 
			(
				stonesPrefabs [index], 
				GetTileCenter(X, Y), 
				Quaternion.identity
			) as GameObject;

		// record the GameObject into the implicit 2D GomokuPieces matrix.
		if (isWhiteTurn)
		{
			boardSituation [X, Y] = 1;
		}
		else
		{
			boardSituation [X, Y] = -1;
		}
			
		if (chainChecker.IsFiveInChain()) 
		{
			Debug.Log ("You Win!");
			isWon = true;
			return;
		} 
		else 
		{
			isWhiteTurn = !isWhiteTurn; // switch the turn.
		}
	}

	private Vector3 GetTileCenter(int X, int Y)
	{
		position = Vector3.zero;
		position.x += (TILE_SIZE * X) + TILE_OFFSET;
		position.z += (TILE_SIZE * Y) + TILE_OFFSET;
		return position;
	}
}