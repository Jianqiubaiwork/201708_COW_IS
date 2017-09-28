using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour 
{
	// private default variables.
	private const int BOARD_SIZE = 15;
	private const int GAME_MODE = 1; // 1 for human vs pc, 2 for pc vs pc
	private const float TILE_SIZE = 1.0f;
	private const float TILE_OFFSET = 0.5f;

	private bool isWhiteTurn;
	private bool won;
	private Vector3 position = Vector3.zero;

	// construct new children class.
	private ChainChecker chainChecker = new ChainChecker (BOARD_SIZE);
	private ArtificialIntelligence AI = new ArtificialIntelligence (BOARD_SIZE);
	private GomokuPiece currentGomokuPiece; //abstract

	// public variables.
	public GomokuPiece[,] GomokuPieces{ set; get; }
	public int X{ set; get; }
	public int Y{ set; get; }
	public List<GameObject> gomokuPiecesPrefabs; // holds the black and white gomoku peice prefab.

	// start.
	private void Start()
	{
		won = false;
		isWhiteTurn = false;
		GomokuPieces = new GomokuPiece[(BOARD_SIZE), (BOARD_SIZE)];

		if (GAME_MODE == 2) 
		{
			X = BOARD_SIZE / 2;
			Y = BOARD_SIZE / 2;
			AddGomokuPiece (X, Y);
		}
	}

	// update the game frame.
	private void Update()
	{
		DrawBoard ();
		if (!won) 
		{
			switch (GAME_MODE) 
			{
			case 1:
				HumanvsPC ();
				break;
			case 2:
				PCvsPC ();
				break;
			default:
				break;
			}
		} 
		else return;
	}

	// update the selection when the mouse moves.
	private void HumanvsPC()
	{
		if (!Camera.main)
			return;

		if (!isWhiteTurn) 
		{
			RaycastHit hit; 
			if (Physics.Raycast (
				Camera.main.ScreenPointToRay (Input.mousePosition), 
				out hit, 100.0f, LayerMask.GetMask ("GomokuPlane"))) 
			{
				X = (int)hit.point.x;
				Y = (int)hit.point.z;
				if (GomokuPieces [X, Y] == null && Input.GetMouseButtonDown (0)) 
				{
					AddGomokuPiece (X, Y);
				}

			} 
			else 
			{
				X = -1;
				Y = -1;
			}
		} 
		else 
		{
			position = AI.randomPlace (GomokuPieces, X, Y);
			AddGomokuPiece ((int)position.x, (int)position.z);
		}
	}

	private void PCvsPC()
	{
		position = AI.randomPlace (GomokuPieces, X, Y);
		X = (int)position.x;
		Y = (int)position.z;
		AddGomokuPiece (X, Y);
	}

	// draw the board.
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

	private void AddGomokuPiece(int X, int Y)
	{
		// figure out whether a white or a black piece needed to be placed based on the turn.
		int index;
		if (isWhiteTurn) 
		{
			index = 0;
		}
		else index = 1;

		// clone a gomoku piece from the prefab list and place in the scene.
		GameObject gomokuPiece = Instantiate 
			(
				gomokuPiecesPrefabs [index], 
				GetTileCenter(X, Y), 
				Quaternion.identity
			) as GameObject;

		// record the GameObject into the implicit 2D GomokuPieces matrix.
		GomokuPieces [X, Y] = gomokuPiece.GetComponent<GomokuPiece> ();

		// check winning
		if (chainChecker.isWon (GomokuPieces, X, Y)) 
		{
			// display winning massage
			Debug.Log ("You won!");
			won = true;
			return;
		} 
		else 
		{
			isWhiteTurn = !isWhiteTurn; // switch the turn.
		}
	}

	private Vector3 GetTileCenter(int x, int y)
	{
		position = Vector3.zero;
		position.x += (TILE_SIZE * x) + TILE_OFFSET;
		position.z += (TILE_SIZE * y) + TILE_OFFSET;
		return position;
	}
}