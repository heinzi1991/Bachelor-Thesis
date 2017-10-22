using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GridOption : MonoBehaviour {

	public Canvas popUpMenu;


	private bool gridSettingsBool = false;

	public bool getSettingsBool { get { return gridSettingsBool; } set { gridSettingsBool = value; } }



	void Update() {

		if (gridSettingsBool == false) {

			popUpMenu.enabled = false;
		}
		else {

			popUpMenu.enabled = true;
		}
	}

	public void openGridSettings() {

		if (gridSettingsBool == false) {

			gridSettingsBool = true;
		}
		else {

			gridSettingsBool = false;
		}
	}

}
