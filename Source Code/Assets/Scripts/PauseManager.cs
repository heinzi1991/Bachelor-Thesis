using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour {

	private GridManager gridManager;

	void Awake() {

		gridManager = GridManager.instance;
	}
		
	public void Toggle(Text buttonText) {

		if (string.Compare(gridManager.gameStatusString, "run") == 0) {
			gridManager.gameStatusString = "pause";
			buttonText.text = "Continue";
			Time.timeScale = 0f;
		}
		else {
			gridManager.gameStatusString = "run";
			buttonText.text = "Pause";
			Time.timeScale = 1f;
		}
	}
}
