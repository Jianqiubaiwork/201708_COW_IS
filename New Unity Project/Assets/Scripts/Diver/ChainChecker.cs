using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainChecker : MonoBehaviour
{
	private int BOARD_SIZE;
	private int SHIFTING_OFFSET;
	private int Mask;
	private int sum;
	private int X;
	private int Y;
	private int[,] boardSituation;
	private bool isNInChain;

	public static ChainChecker chainCheckerInstance { set; get; }
		
	private void Awake()
	{
		BOARD_SIZE = BoardManager.BOARD_SIZE;
		chainCheckerInstance = this;
	}

	private void UpdateData()
	{
		X = BoardManager.boardManagerInstance.X;
		Y = BoardManager.boardManagerInstance.Y;
		boardSituation = BoardManager.boardManagerInstance.boardSituation;
	}

	public bool IsWon()
	{
		UpdateData ();
		return IsNInChain (4, 31);
	}

	public bool IsEnd(int currentX, int currentY, int[,] currentBoardSituation)
	{
		X = currentX;
		Y = currentY;
		boardSituation = currentBoardSituation;
		return IsNInChain (4, 31);
	}

	private bool IsNInChain(int _SHIFTING_OFFSET, int _Mask)
	{
		SHIFTING_OFFSET = _SHIFTING_OFFSET;
		Mask = _Mask;
		isNInChain = false;
		if (CheckLeftToRightDiagonal ()) 
		{
			isNInChain = true;
		}
		if (CheckVertical ()) 
		{
			isNInChain = true;
		}
		if (CheckRightToLeftDiagonal ()) 
		{
			isNInChain = true;
		}
		if (CheckHorizontal ()) 
		{
			isNInChain = true;
		}
		return isNInChain;
	}

	private bool CheckLeftToRightDiagonal()
	{
		sum = 0;
		for (int i = -SHIFTING_OFFSET; i <= SHIFTING_OFFSET; i++) 
		{
			if (((X + i) >= 0) && ((Y - i) >= 0) && ((X + i) < BOARD_SIZE) && ((Y - i) < BOARD_SIZE))
			{
				if (boardSituation [X + i, Y - i] == boardSituation [X, Y]) 
				{
					sum = sum | (1 << (SHIFTING_OFFSET - i));
				}
			}
		}
		return CheckSum (sum);
	}

	private bool CheckVertical()
	{
		sum = 0;
		for (int i = -SHIFTING_OFFSET; i <= SHIFTING_OFFSET; i++) 
		{
			if (((Y + i) >= 0) && ((Y + i) < BOARD_SIZE))
			{
				if (boardSituation [X, Y + i] == boardSituation [X, Y]) 
				{
					sum = sum | (1 << (SHIFTING_OFFSET - i));
				}
			}
		}
		return CheckSum (sum);
	}

	private bool CheckRightToLeftDiagonal()
	{
		sum = 0;
		for (int i = -SHIFTING_OFFSET; i <= SHIFTING_OFFSET; i++) 
		{
			if (((X - i) >= 0) && ((Y - i) >= 0) && ((X - i) < BOARD_SIZE) && ((Y - i) < BOARD_SIZE)) 
			{
				if (boardSituation [X - i, Y - i] == boardSituation [X, Y]) 
				{
					sum = sum | (1 << (SHIFTING_OFFSET - i));
				}
			}
		}
		return CheckSum (sum);
	}

	private bool CheckHorizontal()
	{
		sum = 0;
		for (int i = -SHIFTING_OFFSET; i <= SHIFTING_OFFSET; i++) 
		{
			if (((X + i) >= 0) && ((X + i) < BOARD_SIZE)) 
			{
				if (boardSituation [X + i, Y] == boardSituation [X, Y]) 
				{
					sum = sum | (1 << (SHIFTING_OFFSET - i));
				}
			}
		}
		return CheckSum (sum);
	}

	private bool CheckSum(int sum)
	{
		int shiftedSum = 0;
		for (int i = 0; i <= SHIFTING_OFFSET; i++) 
		{
			shiftedSum = sum >> i;
			if ((shiftedSum & Mask) == Mask) {
				return true;
			}
		}
		return false;
	}

	private void Display(int[,] boardSituation)
	{
		string content = "";
		for (int i = 0; i < BOARD_SIZE; i++) 
		{
			for (int j = 0; j < BOARD_SIZE; j++)
			{
				content += boardSituation [j, i].ToString() + " ";
			}
			content += "\n";
		}	
		Debug.Log (content);
	}
}