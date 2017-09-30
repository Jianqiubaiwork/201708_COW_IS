using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Summerizor : MonoBehaviour
{
	public int movesNum { set; get; }
	public int roundsNum { set; get; }
	public Text movesText;
	public Text roundsText;
	public Text winningText;
	public static Summerizor summerizorInstance { set; get; }

	private void Start()
	{
		movesNum = 0;
		roundsNum = 0;
		summerizorInstance = this;
		winningText.text = "";
		movesText.text = "# of moves: 0";
		roundsText.text = "# of rounds: 0";
	}

	private void Update()
	{
		SetText ();
	}

	private void SetText()
	{
		movesText.text = "# of moves: " + movesNum.ToString ();
		roundsText.text = "# of rounds: " + roundsNum.ToString ();
	}
}