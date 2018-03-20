using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor;
using System.IO;
using System.Linq;

public class NNAI : MonoBehaviour 
{
	private List<List<double>> IWs;
	private List<List<double>> W2; 
	private List<List<double>> W3; 
	private List<double> b1;
	private List<double> b2;
	private List<double> b3;
	private List<double> input;
	private List<double> output;
	private int optimalNumericalPositioin;

	public static NNAI nnAIInstance { set; get; }

	void Awake()
	{
		nnAIInstance = this;
	}

	void Start()
	{
		IWs = new List<List<double>> ();
		W2 = new List<List<double>> ();
		W3 = new List<List<double>> ();
		b1 = new List<double> ();
		b2 = new List<double> ();
		b3 = new List<double> ();
		input = new List<double> ();
		output = new List<double> ();
		PrepareData ();
	}

	public Vector3 NN()
	{
		Board newBoard = new Board (BoardManager.boardManagerInstance.boardSituation,
			                 BoardManager.boardManagerInstance.X,
			                 BoardManager.boardManagerInstance.Y,
			                 0,
			                 BoardManager.boardManagerInstance.isWhiteTurn);
		input = ConvertToList (newBoard);
		optimalNumericalPositioin = -1;
		optimalNumericalPositioin = MatrixManipulation ();
		int newX = (int)optimalNumericalPositioin % BoardManager.BOARD_SIZE;
		int newY = (int)optimalNumericalPositioin / BoardManager.BOARD_SIZE;
		if (newBoard.boardSituation [newX, newY] == 0) 
			return new Vector3 (newX, 0, newY);
		else
			return NextOptimalPosition (newBoard.boardSituation);
	}

	private int MatrixManipulation()
	{
		output.Clear ();
		output = Time (IWs, input);
		output = Add (b1, output);
		output = Sigmoid (output);
		output = Time (W2, output);
		output = Add (b2, output);
		output = Sigmoid (output);
		output = Time (W3, output);
		output = Add (b3, output);
		output = Softmax (output);

		return FindMaxIndex ();
	}

	private int FindMaxIndex()
	{
		double max = 0;
		int index = 0;
		for (int i = 0; i < output.Count; i++) 
		{
			if (output [i] > max) 
			{
				max = output [i];
				index = i;
			}
		}
		return index;
	}

	private void PrepareData()
	{
		b1 = ReadBias ("Assets/Sources/b1.txt");
		b2 = ReadBias ("Assets/Sources/b2.txt");
		b3 = ReadBias ("Assets/Sources/b3.txt");
		ReadInitialWeights ();
		ReadSecondLayer ();
		ReadThirdLayer ();
	}

	private List<double> ReadBias(string currentPath)
	{	
		List<double> newBias = new List<double> ();
		StreamReader reader = new StreamReader(currentPath);
		while (!reader.EndOfStream) 
		{
			double b = double.Parse(reader.ReadLine());
			//b = System.Math.Round (b, 4);
			newBias.Add (b);
		}
		reader.Close ();
		return newBias;
	}

	private void ReadInitialWeights()
	{
		string newPath = "Assets/Sources/IW.txt";
		StreamReader reader = new StreamReader(newPath);
		while (!reader.EndOfStream) 
		{
			List<double> newWeights = new List<double> ();
			for (int i = 0; i < 225; i++) 
			{
				double newWeight = double.Parse(reader.ReadLine());
				//newWeight = System.Math.Round (newWeight, 4);
				newWeights.Add (newWeight);
			}
			IWs.Add (newWeights);
		}
		reader.Close();
	}

	private void ReadSecondLayer()
	{
		string newPath = "Assets/Sources/W2.txt";
		StreamReader reader = new StreamReader(newPath);
		while (!reader.EndOfStream) 
		{
			List<double> newWeights = new List<double> ();
			for (int i = 0; i < 100; i++) 
			{
				double newWeight = double.Parse(reader.ReadLine());
				//newWeight = System.Math.Round (newWeight, 4);
				newWeights.Add (newWeight);
			}
			W2.Add (newWeights);
		}
		reader.Close();
	}

	private void ReadThirdLayer()
	{
		string newPath = "Assets/Sources/W3.txt";
		StreamReader reader = new StreamReader(newPath);
		while (!reader.EndOfStream) 
		{
			List<double> newWeights = new List<double> ();
			for (int i = 0; i < 50; i++) 
			{
				double newWeight = double.Parse(reader.ReadLine());
				//newWeight = System.Math.Round (newWeight, 4);
				newWeights.Add (newWeight);
			}
			W3.Add (newWeights);
		}
		reader.Close();
	}

	private List<double> Add(List<double> A, List<double> B)
	{
		if (A.Count != B.Count) 
			Debug.Log ("SizeException");

		for (int i = 0; i < A.Count; i++) 
		{	
			B [i] += A [i];
		}
		return B;
	}

	private List<double> Time(List<List<double>> A, List<double> B)
	{
		List<double> answer = new List<double> ();
		for (int i = 0; i < A.Count; i++) 
		{	
			double value = VectorTime (A[i], B);
			answer.Add (value);
		}
		return answer;
	}

	private double VectorTime(List<double> A, List<double> B)
	{
		double v = 0;
		if (A.Count != B.Count) 
		{
			Debug.Log ("SizeException");
			return v;
		}
		for (int i = 0; i < A.Count; i++) 
		{
			//v += System.Math.Round( A [i] * B [i], 4);
			v += A[i] * B[i];
		}
		return v;
	}

	private List<double> Sigmoid(List<double> A)
	{
		for(int i = 0; i < A.Count; i++)
		{
			A[i] = 1 / (1 + System.Math.Exp (-A[i]));
			//A[i] = System.Math.Round (A[i], 4);
		}
		return A;
	}

	private List<double> Softmax(List<double> A)
	{
		double sum = 0;
		for (int i = 0; i < A.Count; i++) 
		{
			sum += System.Math.Exp(A [i]);
			//sum = System.Math.Round (sum, 4);
		}
		for (int i = 0; i < A.Count; i++) 
		{
			A [i] = System.Math.Exp(A [i]) / sum;
			//A[i] = System.Math.Round (A[i], 4);
		}
		return A;
	}

	private List<double> ConvertToList(Board currentBoard)
	{
		input.Clear ();
		for (int y = 0; y < BoardManager.BOARD_SIZE; y++) 
		{
			for (int x = 0; x < BoardManager.BOARD_SIZE; x++) 
			{
				//if (currentBoard.boardSituation [x, y] == 2)
				//	input.Add (-1);
				//else
				input.Add (currentBoard.boardSituation [x, y]);
			}
		}
		return input;
	}

	private Vector3 NextOptimalPosition(int[,] current_board_situation)
	{
		output [optimalNumericalPositioin] = 0;
		optimalNumericalPositioin = FindMaxIndex ();
		//Debug.Log ("NN is saying the next optimal is at " + optimalNumericalPositioin);
		int newX = (int)optimalNumericalPositioin % BoardManager.BOARD_SIZE;
		int newY = (int)optimalNumericalPositioin / BoardManager.BOARD_SIZE;
		if (current_board_situation [newX, newY] == 0)
			return new Vector3 (newX, 0, newY);
		else
			return NextOptimalPosition (current_board_situation);
	}
}