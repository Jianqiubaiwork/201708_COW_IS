using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board
{
	public int[,] boardSituation { set; get; }
	public int X { set; get; }
	public int Y { set; get; }
	public int score { set; get; }
	public bool isWhiteTurn { set; get; }

	public Board(int[,] currentBoardSituation, int currentX, int currentY, int currentScore, bool currentIsWhiteTurn)
	{
		boardSituation = currentBoardSituation;
		X = currentX;
		Y = currentY;
		score = currentScore;
		isWhiteTurn = currentIsWhiteTurn;
	}

	public void Display(Board currentBoard)
	{
		string newString = "";
		for (int y = BoardManager.BOARD_SIZE - 1; y >= 0; y--) 
		{
			for (int x = 0; x < BoardManager.BOARD_SIZE; x++) 
			{
				newString += currentBoard.boardSituation [x, y].ToString () + " ";
			}
			newString += "\n";
		}
		Debug.Log ("====================");
		Debug.Log ("The board situation is");
		Debug.Log (newString);
		Debug.Log ("The position is " + currentBoard.X + " , " + currentBoard.Y);
		Debug.Log ("The score is :" + currentBoard.score);
		if (currentBoard.isWhiteTurn)
			Debug.Log ("WHITE stone is about to be placed, but it is currently Minimizor");
		else
			Debug.Log ("BLACK stone is about to be placed, but it is currently Maximizor");
	}
}