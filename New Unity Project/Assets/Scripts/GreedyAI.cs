using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreedyAI : MonoBehaviour 
{
	private int[,] boardSituation;
	private int X;
	private int Y;
	private int score;
	private bool isWhiteTurn;
	private List<Vector3> allSurroundingTilesPositions;
	private List<Board> boardList;

	public static GreedyAI greedyAIInstance { set; get; }

	public void Awake()
	{
		greedyAIInstance = this;
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
		
	public Vector3 GetPosition()
	{
		UpdateData ();
		allSurroundingTilesPositions = BoardManager.boardManagerInstance.FindAllSurroundingTilesPositions (boardSituation);
		Board thisBoard = new Board (boardSituation, X, Y, 0, isWhiteTurn);

		boardList = BoardManager.boardManagerInstance.ExpandBoard (thisBoard, allSurroundingTilesPositions);

		foreach (Board b in boardList) {
			b.score = Checker.checkerInstance.Evaluation (b);
			//b.Display (b);
		}
			
		return BoardManager.boardManagerInstance.GetOptimalPosition (boardList);
	}

	public Vector3 GetRandomPosition()
	{
		UpdateData ();
		allSurroundingTilesPositions = BoardManager.boardManagerInstance.FindAllSurroundingTilesPositions (boardSituation);
		int newIndex = Random.Range(0, allSurroundingTilesPositions.Count);
		return allSurroundingTilesPositions [newIndex];
	}
}