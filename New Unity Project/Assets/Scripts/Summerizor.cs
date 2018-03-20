using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class Summerizor : MonoBehaviour
{
	private int BOARD_SIZE;
	private int[,] boardSituation;
	private int X;
	private int Y;
	private bool isWhiteTurn;

	private List<int> sampleData;
	private int target;

	public int movesNum { set; get; }
	public int roundsNum { set; get; }
	public Text movesText;
	public Text roundsText;
	public Text winningText;

	public static Summerizor summerizorInstance { set; get; }

	private void Awake()
	{
		summerizorInstance = this;
	}

	private void Start()
	{
		movesNum = 0;
		roundsNum = 1;
		winningText.text = "";
		movesText.text = "# of moves: 0";
		roundsText.text = "# of rounds: 0";
	}

	private void UpdateData()
	{
		BOARD_SIZE = BoardManager.BOARD_SIZE;
		X = BoardManager.boardManagerInstance.X;
		Y = BoardManager.boardManagerInstance.Y;
		boardSituation = BoardManager.boardManagerInstance.boardSituation;
		isWhiteTurn = BoardManager.boardManagerInstance.isWhiteTurn;
		sampleData = new List<int>();
	}
		
	public void DataPreperation(bool isEven)
	{
		UpdateData ();
		sampleData.Clear ();
		for (int y = 0; y < BOARD_SIZE; y++) 
		{
			for (int x = 0; x < BOARD_SIZE; x++)
			{
				sampleData.Add(boardSituation [x, y]);
			}
		}
			
		sampleData[Y * BOARD_SIZE + X] = 0; //remove the last stone

		if (isEven) 
		{
			sampleData.Add (0); // add 0 indicating the game is tied
			sampleData.Add (0); // add target value as 0/null
		}
		else 
		{
			if (isWhiteTurn)
				sampleData.Add (BoardManager.WHITE);
			else
				sampleData.Add (BoardManager.BLACK);

			sampleData.Add(Y * BOARD_SIZE + X); // add target values
		}

		Write ();
	}
		
	private void Write()
	{
		string path = "Assets/Sources/RawData.txt";
		string line = "";

		//Write some text to the test.txt file
		StreamWriter writer = new StreamWriter(path, true);

		for (int i = 0; i < sampleData.Count; i++)
		{
			line += sampleData[i].ToString () + ",";
		}
		writer.WriteLine (line);
		writer.Close();
	}

	private void FixedUpdate()
	{
		SetText ();
	}

	private void SetText()
	{
		movesText.text = "# of moves: " + movesNum.ToString ();
		roundsText.text = "# of rounds: " + roundsNum.ToString ();
	}
}