using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimaxAI : MonoBehaviour 
{
	private int[,] boardSituation;
	private int X;
	private int Y;
	private bool isWhiteTurn;
	private List<Vector3> allSurroundingTilesPositions;
	private List<Board> boardList;

	public static MinimaxAI minimaxAI { set; get; }

	void Awake()
	{
		minimaxAI = this;
	}

	void Start()
	{
		allSurroundingTilesPositions = new List<Vector3> ();
		boardList = new List<Board> ();
	}

	private void UpdateData()
	{
		X = BoardManager.boardManagerInstance.X;
		Y = BoardManager.boardManagerInstance.Y;
		boardSituation = BoardManager.boardManagerInstance.boardSituation;
		isWhiteTurn = BoardManager.boardManagerInstance.isWhiteTurn;
	}

	public Vector3 TreeSearch(int futureMoves)
	{
		UpdateData ();
		allSurroundingTilesPositions = 
			BoardManager.boardManagerInstance.FindAllSurroundingTilesPositions (boardSituation);
		Board thisBoard = new Board (boardSituation, X, Y, 0, isWhiteTurn);
		boardList = BoardManager.boardManagerInstance.ExpandBoard (thisBoard, allSurroundingTilesPositions);

		foreach (Board b in boardList) 
		{
			b.score = MiniMax (b, futureMoves, -100000000, 10000000, !b.isWhiteTurn);
		}

		return BoardManager.boardManagerInstance.GetOptimalPosition (boardList);
	}

	private int MiniMax(Board currentBoard, int futureMoves, int a, int b, bool isMaximizingPlayer)
	{
		// base case.
		if ((futureMoves == 0) || 
			(Checker.checkerInstance.IsEnd(currentBoard)))
		{
			int newScore = Checker.checkerInstance.Heuristic(currentBoard);
			return newScore;
		}

		List<Vector3> newAllSurroundingTilesPositions = new List<Vector3> ();
		List<Board> newBoardList = new List<Board> ();
		newAllSurroundingTilesPositions = 
			BoardManager.boardManagerInstance.FindAllSurroundingTilesPositions (currentBoard.boardSituation);
		newBoardList = 
			BoardManager.boardManagerInstance.ExpandBoard (currentBoard, newAllSurroundingTilesPositions);

		if (isMaximizingPlayer)
		{
			foreach (Board maximizor in newBoardList)
			{
				maximizor.score = MiniMax (maximizor, futureMoves - 1, a , b, false);
				a = Max (a, maximizor.score);
				if (a >= b) {
					break;
				}
			}
			return a;
		}
		else
		{
			foreach (Board minimizor in newBoardList)
			{  
				minimizor.score = MiniMax (minimizor, futureMoves - 1, a, b, true);
				b = Min (b, minimizor.score);
				if (a >= b) {
					break;
				}
			}
			return b;
		}
	}

	private int Max(int a, int value)
	{
		if (a >= value) 
			return a;
		else 
			return value;
	}

	private int Min(int b, int value)
	{
		if (b <= value) 
			return b;
		else 
			return value;
	}
}