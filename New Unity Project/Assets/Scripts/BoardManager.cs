using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BoardManager : MonoBehaviour 
{
	private int gameMode;
	private int cap;
	private int pc1MethodIndex, pc2MethodIndex;
	private bool isWon;
	private Vector3 position;

	public const int BLACK = 2;
	public const int WHITE = 1;
	public const int BOARD_SIZE = 15;
	public const float TILE_SIZE = 1.0f;
	public const float TILE_OFFSET = 0.5f;

	public List<GameObject> stonePrefabs;
	public List<GameObject> stoneList { set; get; }
	public int[,] boardSituation { set; get; }
	public int X { set; get; }
	public int Y { set; get; }
	public bool isWhiteTurn { set; get; }

	public static BoardManager boardManagerInstance { set; get; }

	void Awake()
	{
		boardManagerInstance = this;
	}

	void Start()
	{
		isWhiteTurn = false;
		isWon = false;
		boardSituation = new int[BOARD_SIZE, BOARD_SIZE];
		stoneList = new List<GameObject> ();

		gameMode = MenuManager.MenuManager_Instance.gameMode;
		pc1MethodIndex = MenuManager.MenuManager_Instance.pc1MethodIndex;
		pc2MethodIndex = MenuManager.MenuManager_Instance.pc2MethodIndex;
		cap = MenuManager.MenuManager_Instance.cap;
	}

	void FixedUpdate()
	{
		switch (gameMode) 
		{
		case 1: // human vs pc
			HumanVsPC();
			break;
		case 2: // pc vs pc
			PCVsPC();
			break;
		case 3: // human vs human
			HumanVsHuman();
			break;
		default:
			break;
		}
	}
		
	private void HumanVsPC()
	{
		if (Summerizor.summerizorInstance.roundsNum % 2 != 0 && !isWon) // Human goes first
		{
			if (!isWhiteTurn)
				PlaceStoneByHuman ();
			else
				PlaceStoneByPC (pc1MethodIndex);
		}
		else if (Summerizor.summerizorInstance.roundsNum % 2 == 0 && !isWon)
		{
			if (!isWhiteTurn)
				PlaceStoneByPC (pc1MethodIndex);
			else
				PlaceStoneByHuman ();
		}
	}

	private void PCVsPC()
	{
		if (Summerizor.summerizorInstance.roundsNum <= cap) 
		{
			//if (Input.GetMouseButtonDown (0)) 
			{
				if (Summerizor.summerizorInstance.roundsNum % 2 != 0 && !isWon) // Human goes first
				{
					if (!isWhiteTurn)
						PlaceStoneByPC (pc1MethodIndex);
					else
						PlaceStoneByPC (pc2MethodIndex);
				}
				else if (Summerizor.summerizorInstance.roundsNum % 2 == 0 && !isWon)
				{
					if (!isWhiteTurn)
						PlaceStoneByPC (pc2MethodIndex);
					else
						PlaceStoneByPC (pc1MethodIndex);
				}
				if (isWon)
					StartOver ();
			}
		}
	}

	private void HumanVsHuman()
	{
		if (!isWhiteTurn)
			PlaceStoneByHuman ();
		else
			PlaceStoneByHuman ();
	}
		
	private void PlaceStoneByHuman()
	{
		if (!Camera.main)
			return;

		RaycastHit hit; 
		if (Physics.Raycast (
			Camera.main.ScreenPointToRay (Input.mousePosition), 
			out hit, 100.0f, LayerMask.GetMask ("GomokuPlane"))) 
		{
			X = (int)hit.point.x;
			Y = (int)hit.point.z;
			if (Input.GetMouseButtonDown (0) && 
				X >= 0 && X < BOARD_SIZE &&
				Y >= 0 && Y < BOARD_SIZE &&
				boardSituation [X, Y] == 0) 
				PlaceStone ();
		} 
		else 
		{
			X = -1;
			Y = -1;
		}
	}

	private void PlaceStoneByPC(int currentMethodIndex)
	{
		if (Summerizor.summerizorInstance.movesNum == 0) 
		{
			X = (int)BOARD_SIZE / 2;
			Y = (int)BOARD_SIZE / 2;
		}
		if (Summerizor.summerizorInstance.movesNum > 0) 
		{
			if (currentMethodIndex == 0)
				position = GreedyAI.greedyAIInstance.GetPosition ();
			else if (currentMethodIndex == 1)
				position = MinimaxAI.minimaxAI.TreeSearch (2);
			else if (currentMethodIndex == 2)
				position = NNAI.nnAIInstance.NN ();
			else if (currentMethodIndex == 3)
				position = GreedyAI.greedyAIInstance.GetRandomPosition ();

			X = (int)position.x;
			Y = (int)position.z;
		}
		if (X == -1 && Y == -1)
			PlaceStoneByHuman ();
		else
			PlaceStone ();
	}

	private void PlaceStone()
	{
		int index;
		if (isWhiteTurn) 
			index = 0;
		else 
			index = 1;

		GameObject stone = Instantiate (
			stonePrefabs [index], 
			GetTileCenter(X, Y), 
			Quaternion.identity) 
			as GameObject;

		stoneList.Add (stone);
		UpdateData ();
		CheckWin ();
	}

	private Vector3 GetTileCenter(int X, int Y)
	{
		Vector3 thisVector3 = Vector3.zero;
		thisVector3.x += (BoardManager.TILE_SIZE * X) + BoardManager.TILE_OFFSET;
		thisVector3.z += (BoardManager.TILE_SIZE * Y) + BoardManager.TILE_OFFSET;
		return thisVector3;
	}

	private void DestroyAllStones()
	{
		for (int i = 0; i < stoneList.Count; i++) 
		{
			Destroy (stoneList [i]);
		}
		stoneList.Clear ();
	}

	private void UpdateData()
	{
		Summerizor.summerizorInstance.movesNum++;

		if (isWhiteTurn) 
			boardSituation [X, Y] = WHITE;
		else 
			boardSituation [X, Y] = BLACK;
	}

	private void CheckWin()
	{
		if (stoneList.Count == BOARD_SIZE * BOARD_SIZE)
			IsEven ();
		
		if (Checker.checkerInstance.IsWon ()) 
		{
			isWon = true;
			if (gameMode == 2)
				Summerizor.summerizorInstance.DataPreperation (false);
			EndGame ();
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
		return;
	}

	private void IsEven()
	{
		isWon = true;
		Summerizor.summerizorInstance.winningText.text = "The game is tied!";

		StartOver ();
	}

	public void StartOver()
	{
		if (gameMode == 2 && Summerizor.summerizorInstance.roundsNum == cap)
			return;

		DestroyAllStones ();
		boardSituation = new int[BOARD_SIZE, BOARD_SIZE];
		isWon = false;
		isWhiteTurn = false;
		Summerizor.summerizorInstance.winningText.text = "";
		Summerizor.summerizorInstance.movesNum = 0;
		Summerizor.summerizorInstance.roundsNum++;
		//if (Summerizor.summerizorInstance.roundsNum % 2 != 0)
		//	isWhiteTurn = false;
		//else
		//	isWhiteTurn = true;
		//Debug.Log (isWhiteTurn);
	}
		
	public void BackToMenu(string name)
	{
		SceneManager.LoadScene (name);
	}

	public List<Vector3> FindAllSurroundingTilesPositions(int[,] currentBoardSituation)
	{
		List<Vector3> newListVec = new List<Vector3> ();
		for (int i = 0; i < BOARD_SIZE; i++) 
		{
			for (int j = 0; j < BOARD_SIZE; j++) 
			{
				if (currentBoardSituation [i, j] != 0) 
				{
					AddSurroundingTilesPosition(newListVec, currentBoardSituation, i, j);
				}
			}
		}
		return newListVec;
	}

	private void AddSurroundingTilesPosition(List<Vector3> currentList, int[,] currentBoardSituation, int newX, int newY)
	{
		for (int i = -1; i <= 1; i++)
		{
			for (int j = -1; j <= 1; j++) 
			{
				if (((newX + i) >= 0) && 
					((newY + j) >= 0) && 
					((newX + i) < BOARD_SIZE) && 
					((newY + j) < BOARD_SIZE)) 
				{
					if ((currentBoardSituation [newX + i, newY + j] == 0) &&
						!IsDuplicatedPosition(currentList, newX + i, newY + j))
					{
						Vector3 myNewPostion = new Vector3 {
							x = newX + i,
							y = 0,
							z = newY + j
						};
						currentList.Add (myNewPostion);
					}
				}
			}
		}
	}

	private bool IsDuplicatedPosition(List<Vector3> myList, int newX, int newY)
	{
		if (myList.Exists 
			(vec => ((vec.x == newX)
				&& (vec.z == newY)))) {
			return true;
		}
		else
			return false;
	}

	public List<Board> ExpandBoard(Board currentBoard, List<Vector3> currentListVec)
	{
		List<Board> newBoardList = new List<Board> ();
		foreach (Vector3 v in currentListVec) 
		{
			int newX = (int)v.x;
			int newY = (int)v.z;
			int[,] newBoardSituation = new int[BOARD_SIZE, BOARD_SIZE];
			newBoardSituation = (int[,]) currentBoard.boardSituation.Clone ();
			if (currentBoard.isWhiteTurn)
				newBoardSituation [newX, newY] = WHITE;
			else
				newBoardSituation [newX, newY] = BLACK;
			Board newBoard = new Board (newBoardSituation, newX, newY, 0, !currentBoard.isWhiteTurn);
			newBoardList.Add (newBoard);
		}
		return newBoardList;
	}
		
	public Vector3 GetOptimalPosition(List<Board> currentBoardList)
	{
		if (isWhiteTurn) 
		{
			int maxScore = FindMaxScore (currentBoardList);
			currentBoardList.RemoveAll (b => b.score < maxScore);
		} 
		else 
		{
			int minScore = FindMinScore (currentBoardList);
			currentBoardList.RemoveAll (b => b.score > minScore);
		}

		int index = Random.Range (0, currentBoardList.Count);
		Vector3 newVec = new Vector3 
		{
			x = currentBoardList [index].X,
			y = 0,
			z = currentBoardList [index].Y
		};
		//Debug.Log (currentBoardList [index].score + " is at " + currentBoardList [index].X + " , " + currentBoardList [index].Y);
		return newVec;
	}

	private int FindMaxScore(List<Board> currentBoardList)
	{
		int newScore = -1000000;
		foreach (Board b in currentBoardList) 
		{
			if (b.score > newScore)
				newScore = b.score;
		}
		return newScore;
	}

	private int FindMinScore(List<Board> currentBoardList)
	{
		int newScore = 1000000;
		foreach (Board b in currentBoardList) 
		{
			if (b.score < newScore)
				newScore = b.score;
		}
		return newScore;
	}
}