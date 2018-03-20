using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checker : MonoBehaviour
{
	private const string FIVE_BLACK = "22222";
	private const string FIVE_WHITE = "11111";

	private const string OPEN_FOUR_BLACK_1 = "022220";
	private const string OPEN_FOUR_BLACK_2 = "0202220";
	private const string OPEN_FOUR_BLACK_3 = "0220220";
	private const string OPEN_FOUR_BLACK_4 = "0222020";
	private const string OPEN_FOUR_WHITE_1 = "011110";
	private const string OPEN_FOUR_WHITE_2 = "0101110";
	private const string OPEN_FOUR_WHITE_3 = "0110110";
	private const string OPEN_FOUR_WHITE_4 = "0111010";

	private const string SEMI_FOUR_BLACK_1 = "122220";
	private const string SEMI_FOUR_BLACK_2 = "022221";
	private const string SEMI_FOUR_BLACK_3 = "1222020";
	private const string SEMI_FOUR_BLACK_4 = "1220220";
	private const string SEMI_FOUR_BLACK_5 = "1202220";
	private const string SEMI_FOUR_BLACK_6 = "0222021";
	private const string SEMI_FOUR_BLACK_7 = "0220221";
	private const string SEMI_FOUR_BLACK_8 = "0202221";
	private const string SEMI_FOUR_WHITE_1 = "211110";
	private const string SEMI_FOUR_WHITE_2 = "011112";
	private const string SEMI_FOUR_WHITE_3 = "2111010";
	private const string SEMI_FOUR_WHITE_4 = "2110110";
	private const string SEMI_FOUR_WHITE_5 = "2101110";
	private const string SEMI_FOUR_WHITE_6 = "0111012";
	private const string SEMI_FOUR_WHITE_7 = "0110112";
	private const string SEMI_FOUR_WHITE_8 = "0101112";

	private const string OPEN_THREE_BLACK_1 = "02220";
	private const string OPEN_THREE_BLACK_2 = "022020";
	private const string OPEN_THREE_BLACK_3 = "020220";
	private const string OPEN_THREE_WHITE_1 = "01110";
	private const string OPEN_THREE_WHITE_2 = "011010";
	private const string OPEN_THREE_WHITE_3 = "010110";

	private const string SEMI_THREE_BLACK_1 = "12220";
	private const string SEMI_THREE_BLACK_2 = "02221";
	private const string SEMI_THREE_BLACK_3 = "122020";
	private const string SEMI_THREE_BLACK_4 = "120220";
	private const string SEMI_THREE_BLACK_5 = "022021";
	private const string SEMI_THREE_BLACK_6 = "020221";
	private const string SEMI_THREE_WHITE_1 = "21110";
	private const string SEMI_THREE_WHITE_2 = "01112";
	private const string SEMI_THREE_WHITE_3 = "211010";
	private const string SEMI_THREE_WHITE_4 = "210110";
	private const string SEMI_THREE_WHITE_5 = "011012";
	private const string SEMI_THREE_WHITE_6 = "010112";

	private const string OPEN_TWO_BLACK_1 = "0220";
	private const string OPEN_TWO_BLACK_2 = "02020";
	private const string OPEN_TWO_WHITE_1 = "0110";
	private const string OPEN_TWO_WHITE_2 = "01010";

	private const string SEMI_TWO_BLACK_1 = "1220";
	private const string SEMI_TWO_BLACK_2 = "0221";
	private const string SEMI_TWO_WHITE_1 = "2110";
	private const string SEMI_TWO_WHITE_2 = "0112";

	private int[,] boardSituation;
	private int X;
	private int Y;
	private int score;
	private bool isWhiteTurn;
	private int[] winning_window;
	private int winning_window_1;
	private int winning_window_2;
	private int winning_window_3;
	private int winning_window_4;
	private int winning_window_5;

	public static Checker checkerInstance { set; get; }

	void Awake()
	{
		checkerInstance = this;
	}

	private void UpdateData()
	{
		X = BoardManager.boardManagerInstance.X;
		Y = BoardManager.boardManagerInstance.Y;
		boardSituation = BoardManager.boardManagerInstance.boardSituation;
		isWhiteTurn = BoardManager.boardManagerInstance.isWhiteTurn;
	}

	public bool IsWon()
	{
		UpdateData ();
		if (IsNInChain (4, FIVE_BLACK) | IsNInChain (4, FIVE_WHITE))
			return true;
		return false;
	}

	public bool IsEnd(Board currentBoard)
	{
		X = currentBoard.X;
		Y = currentBoard.Y;
		boardSituation = currentBoard.boardSituation;
		isWhiteTurn = currentBoard.isWhiteTurn;
		if (IsNInChain (4, FIVE_BLACK) | IsNInChain (4, FIVE_WHITE))
			return true;
		return false;
	}

	private bool IsNInChain(int currentOffset, string currentMask)
	{
		if (checkDirection (1, currentOffset, currentMask)) // left-to-right diagonal
			return true;
		else if (checkDirection (2, currentOffset, currentMask)) // vertical
			return true;
		else if (checkDirection (3, currentOffset, currentMask)) // right-to-left diagonal
			return true;
		else if (checkDirection (4, currentOffset, currentMask)) // horizontal
			return true;
		else
			return false;
	}
		
	private bool checkDirection(int currentDirectionIndex, int currentOffset, string currentMask)
	{
		List<int> newSequence = new List<int>();
		switch (currentDirectionIndex) 
		{
		case 1:
			for (int i = -currentOffset; i <= currentOffset; i++) {
				if (((X + i) >= 0) && ((Y - i) >= 0) && ((X + i) < BoardManager.BOARD_SIZE) && ((Y - i) < BoardManager.BOARD_SIZE)) {
					newSequence.Add (boardSituation [X + i, Y - i]);
				}
			}

			break;
		case 2:
			for (int i = -currentOffset; i <= currentOffset; i++) 
			{
				if (((Y + i) >= 0) && ((Y + i) < BoardManager.BOARD_SIZE))
				{
					newSequence.Add (boardSituation [X, Y + i]);
				}
			}
			break;
		case 3:
			for (int i = -currentOffset; i <= currentOffset; i++) 
			{
				if (((X - i) >= 0) && ((Y - i) >= 0) && ((X - i) < BoardManager.BOARD_SIZE) && ((Y - i) < BoardManager.BOARD_SIZE))
				{
					newSequence.Add (boardSituation [X - i, Y - i]);
				}
			}
			break;
		case 4:
			for (int i = -currentOffset; i <= currentOffset; i++) 
			{
				if (((X + i) >= 0) && ((X + i) < BoardManager.BOARD_SIZE)) 
				{
					newSequence.Add (boardSituation [X + i, Y]);
				}
			}
			break;
		}
		string newString = ConvertToString (newSequence);

		if (newString.IndexOf (currentMask) != -1)
			return true;
		return false;
	}

	private string ConvertToString (List<int> curretSequence)
	{
		string s = "";
		foreach (int i in curretSequence) 
		{
			s += i.ToString ();
		}
		return s;
	}

	public int Evaluation(Board currentBoard)
	{
		X = currentBoard.X;
		Y = currentBoard.Y;
		boardSituation = currentBoard.boardSituation;
		isWhiteTurn = currentBoard.isWhiteTurn;

		int newScore, whiteScore, blackScore;
		whiteScore = 0;
		blackScore = 0;

		newScore = 0;

		boardSituation [X, Y] = BoardManager.BLACK;
		for (int i = 1; i <= 4; i++) 
		{
			if (checkDirection (i, 4, FIVE_BLACK))
				newScore += 8530;
			else if (checkDirection (i, 5, OPEN_FOUR_BLACK_1))
				newScore += 2130;
			else if (checkDirection (i, 5, SEMI_FOUR_BLACK_1) | checkDirection (i, 5, SEMI_FOUR_BLACK_2))
				newScore += 530;
			else if (checkDirection (i, 4, OPEN_THREE_BLACK_1))
				newScore += 130;
			else if (checkDirection (i, 4, SEMI_THREE_BLACK_1) | checkDirection (i, 4, SEMI_THREE_BLACK_2))
				newScore += 5;
			else if (checkDirection (i, 3, OPEN_TWO_BLACK_1))
				newScore += 30;
		}
			
		boardSituation [X, Y] = BoardManager.WHITE;
		for (int i = 1; i <= 4; i++) 
		{
			if (checkDirection (i, 4, FIVE_WHITE))
				newScore += 8530;
			else if (checkDirection (i, 5, OPEN_FOUR_WHITE_1))
				newScore += 2130;
			else if (checkDirection (i, 5, SEMI_FOUR_WHITE_1) | checkDirection (i, 5, SEMI_FOUR_WHITE_2))
				newScore += 530;
			else if (checkDirection (i, 4, OPEN_THREE_WHITE_1))
				newScore += 130;
			else if (checkDirection (i, 4, SEMI_THREE_WHITE_1) | checkDirection (i, 4, SEMI_THREE_WHITE_2))
				newScore += 5;
			else if (checkDirection (i, 3, OPEN_TWO_WHITE_1))
				newScore += 30;
		}

		if (isWhiteTurn)
			newScore = -newScore;

		return newScore;
	}

	public int Heuristic(Board current_board)
	{
		score = 0;
		int white_score = ComputeScore (current_board, 1);
		int black_score = ComputeScore (current_board, 2);
		score = white_score - black_score;
		return score;
	}

	private int ComputeScore(Board current_board, int current_search_value)
	{
		winning_window_1 = 0;
		winning_window_2 = 0;
		winning_window_3 = 0;
		winning_window_4 = 0;
		winning_window_5 = 0;

		CheckHorizontal (current_board.boardSituation, current_search_value);
		CheckVertival (current_board.boardSituation, current_search_value);
		CheckLeftToRight (current_board.boardSituation, current_search_value);
		CheckRightToLeft (current_board.boardSituation, current_search_value);

		if (!current_board.isWhiteTurn && current_search_value == BoardManager.BLACK) 
		{
			if (winning_window_3 > 1)
				winning_window_3 = winning_window_3 * 2;
			if (winning_window_4 > 0)
				winning_window_4 = winning_window_4 * 2;
			if (winning_window_5 > 0)
				winning_window_5 = winning_window_5 * 2;
		} 
		else if (current_board.isWhiteTurn && current_search_value == BoardManager.WHITE) 
		{
			if (winning_window_3 > 1)
				winning_window_3 = winning_window_3 * 2;
			if (winning_window_4 > 0)
				winning_window_4 = winning_window_4 * 2;
			if (winning_window_5 > 0)
				winning_window_5 = winning_window_5 * 2;
		}

		return (winning_window_1 * 1 + winning_window_2 * 10 + winning_window_3 * 100 + winning_window_4 * 1000 + winning_window_5 * 10000);
	}

	private void CheckHorizontal (int[,] current_board_situation, int current_search_value)
	{
		for (int y = 0; y < BoardManager.BOARD_SIZE; y++) 
		{
			for (int x = 0; x <= (BoardManager.BOARD_SIZE - 5); x++) 
			{
				winning_window = new int[5];
				winning_window [0] = current_board_situation [x, y];
				winning_window [1] = current_board_situation [x + 1, y];
				winning_window [2] = current_board_situation [x + 2, y];
				winning_window [3] = current_board_situation [x + 3, y];
				winning_window [4] = current_board_situation [x + 4, y];
				CountWinningWindow (current_search_value);
			}
		}
	}

	private void CheckVertival (int[,] current_board_situation, int current_search_value)
	{
		for (int x = 0; x < BoardManager.BOARD_SIZE; x++) 
		{
			for (int y = 0; y <= (BoardManager.BOARD_SIZE - 5); y++) 
			{
				winning_window = new int[5];
				winning_window [0] = current_board_situation [x, y];
				winning_window [1] = current_board_situation [x, y + 1];
				winning_window [2] = current_board_situation [x, y + 2];
				winning_window [3] = current_board_situation [x, y + 3];
				winning_window [4] = current_board_situation [x, y + 4];
				CountWinningWindow (current_search_value);
			}
		}
	}

	private void CheckLeftToRight (int[,] current_board_situation, int current_search_value)
	{
		for (int y = 4; y < BoardManager.BOARD_SIZE; y++) 
		{
			for (int x = 0; x <= (BoardManager.BOARD_SIZE - 5); x++) 
			{
				winning_window = new int[5];
				winning_window [0] = current_board_situation [x, y];
				winning_window [1] = current_board_situation [x + 1, y - 1];
				winning_window [2] = current_board_situation [x + 2, y - 2];
				winning_window [3] = current_board_situation [x + 3, y - 3];
				winning_window [4] = current_board_situation [x + 4, y - 4];
				CountWinningWindow (current_search_value);
			}
		}
	}

	private void CheckRightToLeft (int[,] current_board_situation, int current_search_value)
	{
		for (int y = (BoardManager.BOARD_SIZE - 5); y >= 0; y--) 
		{
			for (int x = 0; x <= (BoardManager.BOARD_SIZE - 5); x++) 
			{
				winning_window = new int[5];
				winning_window [0] = current_board_situation [x, y];
				winning_window [1] = current_board_situation [x + 1, y + 1];
				winning_window [2] = current_board_situation [x + 2, y + 2];
				winning_window [3] = current_board_situation [x + 3, y + 3];
				winning_window [4] = current_board_situation [x + 4, y + 4];
				CountWinningWindow (current_search_value);
			}
		}
	}

	private void CountWinningWindow(int current_search_value)
	{
		int n = 0;
		foreach (int i in winning_window) 
		{
			if (i != current_search_value && i != 0)
				return;

			if (i == current_search_value)
				n++;
		}

		if (n == 1)
			winning_window_1++;
		else if (n == 2)
			winning_window_2++;
		else if (n == 3)
			winning_window_3++;
		else if (n == 4)
			winning_window_4++;
		else if (n == 5)
			winning_window_5++;
	}
}