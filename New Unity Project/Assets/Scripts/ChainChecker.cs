using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainChecker
{
	private bool isFiveInChain;
	private int sum;
	private int BOARD_SIZE;
	private int SHIFTING_OFFSET;
	private int Mask;

	public Stack<int> DirectionIndexes { set; get; }

	// constructor.
	public ChainChecker(int size)
	{
		isFiveInChain = false;
		//isFourInChian = false;
		//isThreeInChain = false;
		//isTwoInChain = false;
		//isThreeInChain = false;
		BOARD_SIZE = size;
	}

	public bool isWon(GomokuPiece[,] gomokuPieces, int X, int Y)
	{
		SHIFTING_OFFSET = 4;
		Mask = 31; // 11111
		if (checkLeftToRightDiagonal (gomokuPieces, X, Y)) 
		{
			isFiveInChain = true;
		}
		if (checkVertical (gomokuPieces, X, Y)) 
		{
			isFiveInChain = true;
		}
		if (checkRightToLeftDiagonal (gomokuPieces, X, Y)) 
		{
			isFiveInChain = true;
		}
		if (checkHorizontal (gomokuPieces, X, Y)) 
		{
			isFiveInChain = true;
		}
		return isFiveInChain;
	}
		
	private bool checkLeftToRightDiagonal(GomokuPiece[,] gomokuPieces, int X, int Y)
	{
		sum = 0;
		for (int i = -SHIFTING_OFFSET; i <= SHIFTING_OFFSET; i++) 
		{
			if (((X + i) >= 0) && ((Y - i) >= 0) && ((X + i) < BOARD_SIZE) && ((Y - i) < BOARD_SIZE)) {
				if (gomokuPieces [X + i, Y - i] != null) {
					if (gomokuPieces [X + i, Y - i].isWhite == gomokuPieces [X, Y].isWhite) {
						sum = sum | (1 << (4 - i));
					}
				}
			}
		}
		return checkSum (sum);
	}

	private bool checkVertical(GomokuPiece[,] gomokuPieces, int X, int Y)
	{
		sum = 0;
		for (int i = -SHIFTING_OFFSET; i <= SHIFTING_OFFSET; i++) 
		{
			if (((Y + i) >= 0) && ((Y + i) < BOARD_SIZE)) {
				if (gomokuPieces [X, Y + i] != null) {
					if (gomokuPieces [X, Y + i].isWhite == gomokuPieces [X, Y].isWhite) {
						sum = sum | (1 << (4 - i));
					}
				}
			}
		}
		return checkSum (sum);
	}

	private bool checkRightToLeftDiagonal(GomokuPiece[,] gomokuPieces, int X, int Y)
	{
		sum = 0;
		for (int i = -SHIFTING_OFFSET; i <= SHIFTING_OFFSET; i++) 
		{
			if (((X - i) >= 0) && ((Y - i) >= 0) && ((X - i) < BOARD_SIZE) && ((Y - i) < BOARD_SIZE)) {
				if (gomokuPieces [X - i, Y - i] != null) {
					if (gomokuPieces [X - i, Y - i].isWhite == gomokuPieces [X, Y].isWhite) {
						sum = sum | (1 << (4 - i));
					}
				}
			}
		}
		return checkSum (sum);
	}

	private bool checkHorizontal(GomokuPiece[,] gomokuPieces, int X, int Y)
	{
		sum = 0;
		for (int i = -SHIFTING_OFFSET; i <= SHIFTING_OFFSET; i++) 
		{
			if (((X + i) >= 0) && ((X + i) < BOARD_SIZE)) {
				if (gomokuPieces [X + i, Y] != null) {
					if (gomokuPieces [X + i, Y].isWhite == gomokuPieces [X, Y].isWhite) {
						sum = sum | (1 << (4 - i));
					}
				}
			}
		}
		return checkSum (sum);
	}

	private bool checkSum(int sum)
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
}

/*public Stack<int> isFourPieceChain(GomokuPiece[,] gomokuPieces, int X, int Y)
	{
		SHIFTING_OFFSET = 3;
		Mask = 15; // 1111
		DirectionIndexes.Clear();
		if (checkLeftToRightDiagonal (gomokuPieces, X, Y)) 
		{
			isThreeInChain = true;
			DirectionIndexes.Push (2);
		}
		if (checkVertical (gomokuPieces, X, Y)) 
		{
			isThreeInChain = true;
			DirectionIndexes.Push (3);
		}
		if (checkRightToLeftDiagonal (gomokuPieces, X, Y)) 
		{
			isThreeInChain = true;
			DirectionIndexes.Push (4);
		}
		if (checkHorizontal (gomokuPieces, X, Y)) 
		{
			isThreeInChain = true;
			DirectionIndexes.Push (1);
		}
		return DirectionIndexes;
	}

	public Stack<int> isThreePieceChain(GomokuPiece[,] gomokuPieces, int X, int Y)
	{
		SHIFTING_OFFSET = 2;
		Mask = 7; // 111
		DirectionIndexes.Clear();
		if (checkLeftToRightDiagonal (gomokuPieces, X, Y)) 
		{
			isThreeInChain = true;
			DirectionIndexes.Push (2);
		}
		if (checkVertical (gomokuPieces, X, Y)) 
		{
			isThreeInChain = true;
			DirectionIndexes.Push (3);
		}
		if (checkRightToLeftDiagonal (gomokuPieces, X, Y)) 
		{
			isThreeInChain = true;
			DirectionIndexes.Push (4);
		}
		if (checkHorizontal (gomokuPieces, X, Y)) 
		{
			isThreeInChain = true;
			DirectionIndexes.Push (1);
		}
		return DirectionIndexes;
	}

	public Stack<int> isTwoPieceChain(GomokuPiece[,] gomokuPieces, int X, int Y)
	{
		SHIFTING_OFFSET = 1;
		Mask = 3; // 11
		DirectionIndexes.Clear();
		if (checkLeftToRightDiagonal (gomokuPieces, X, Y)) 
		{
			isThreeInChain = true;
			DirectionIndexes.Push (2);
		}
		if (checkVertical (gomokuPieces, X, Y)) 
		{
			isThreeInChain = true;
			DirectionIndexes.Push (3);
		}
		if (checkRightToLeftDiagonal (gomokuPieces, X, Y)) 
		{
			isThreeInChain = true;
			DirectionIndexes.Push (4);
		}
		if (checkHorizontal (gomokuPieces, X, Y)) 
		{
			isThreeInChain = true;
			DirectionIndexes.Push (1);
		}
		return DirectionIndexes;
	}
*/

/*public Stack<int> isThreePieceChain(GomokuPiece[,] gomokuPieces, int X, int Y)
{
	SHIFTING_OFFSET = 2;
	Mask = 7; // 111
	DirectionIndexes.Clear();
	if (checkLeftToRightDiagonal (gomokuPieces, X, Y)) 
	{
		isThreeInChain = true;
		DirectionIndexes.Push (2);
	}
	if (checkVertical (gomokuPieces, X, Y)) 
	{
		isThreeInChain = true;
		DirectionIndexes.Push (3);
	}
	if (checkRightToLeftDiagonal (gomokuPieces, X, Y)) 
	{
		isThreeInChain = true;
		DirectionIndexes.Push (4);
	}
	if (checkHorizontal (gomokuPieces, X, Y)) 
	{
		isThreeInChain = true;
		DirectionIndexes.Push (1);
	}
	return DirectionIndexes;
}*/

/*public bool hasToBlock(GomokuPiece[,] gomokuPieces, int X, int Y)
{
	SHIFTING_OFFSET = 4;
	int [] Masks = new int[]{15, 23, 27, 29}; //1111, 10111, 11011, 11101
	if (checkLeftToRightDiagonal (gomokuPieces, X, Y)) 
	{
		won = true;
	}
	if (checkVertical (gomokuPieces, X, Y)) 
	{
		won = true;
	}
	if (checkRightToLeftDiagonal (gomokuPieces, X, Y)) 
	{
		won = true;
	}
	if (checkHorizontal (gomokuPieces, X, Y)) 
	{
		won = true;
	}

	return won;
}*/