using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour 
{
	private const int MAX_CAP = 5000;
	
	public Dropdown dropdown1, dropdown2;
	public Toggle toggle1, toggle2, toggle3;
	public Text text;

	public int gameMode { set; get; }
	public int pc1MethodIndex { set; get; }
	public int pc2MethodIndex { set; get; }
	public int cap { set; get; }
	public static MenuManager MenuManager_Instance { set; get; }

	void Awake()
	{
		gameMode = 1;
		pc1MethodIndex = 0;
		pc2MethodIndex = 0;
		cap = 1;
		MenuManager_Instance = this;
	}

	public void PlayGame(string name)
	{
		SceneManager.LoadScene (name);
	}

	public void QuitGame()
	{
		Debug.Log ("Exit!");
		Application.Quit ();
	}

	public void ChooseGameMode()
	{
		if (toggle1.isOn)
			gameMode = 1;
		else if (toggle2.isOn)
			gameMode = 2;
		else if (toggle3.isOn)
			gameMode = 3;
	}

	public void SetCap(float value)
	{
		cap = Mathf.RoundToInt (value * MAX_CAP);
		text.text = cap.ToString ();
	}

	public void SelectMethod1(int value)
	{
		pc1MethodIndex = value;
	}

	public void SelectMethod2(int value)
	{
		pc2MethodIndex = value;
	}

	public void OnSubmit()
	{
		ChooseGameMode ();
	}
}