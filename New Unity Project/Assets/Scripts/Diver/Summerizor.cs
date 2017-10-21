using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.IO;

public class Summerizor : MonoBehaviour
{
	private int BOARD_SIZE;
	private int X;
	private int Y;
	private int[,] boardSituation;
	private List<int> sampleData;
	private int target;
	private bool isWhiteTurn;

	public int movesNum { set; get; }
	public int roundsNum { set; get; }
	public Text movesText;
	public Text roundsText;
	public Text winningText;
	public TextAsset data;
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
		BOARD_SIZE = BoardManager.BOARD_SIZE;
	}

	private void UpdataData()
	{
		X = BoardManager.boardManagerInstance.X;
		Y = BoardManager.boardManagerInstance.Y;
		boardSituation = BoardManager.boardManagerInstance.boardSituation;
		isWhiteTurn = BoardManager.boardManagerInstance.isWhiteTurn;
		sampleData = new List<int>();
	}
		
	public void DataPreperation()
	{
		UpdataData ();

		for (int i = 0; i < BOARD_SIZE; i++) 
		{
			for (int j = 0; j < BOARD_SIZE; j++)
			{
				sampleData.Add(boardSituation [j, i]);
			}
		}
			
		sampleData[Y * BOARD_SIZE + X] = 0; //remove the last stone

		if (isWhiteTurn)
			sampleData.Add (1);
		else
			sampleData.Add (-1);

		sampleData.Add(Y * BOARD_SIZE + X);

		Write ();
	}
		
	private void Write()
	{
		string path = "Assets/Sources/Data.txt";
		string line = "";

		//Write some text to the test.txt file
		StreamWriter writer = new StreamWriter(path, true);

		Debug.Log (sampleData.Count);
		foreach (int i in sampleData) 
		{
			line += i.ToString ();
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