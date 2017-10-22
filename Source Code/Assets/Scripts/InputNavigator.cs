using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InputNavigator : MonoBehaviour {

	EventSystem system;

	void Start() {

		system = EventSystem.current; //EventSystemManager.currentSystem;
	}

	void Update() {

		if (Input.GetKeyDown(KeyCode.Tab)) {

			Selectable next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();

			if (next != null) {

				InputField inputField = next.GetComponent<InputField>();

				if (inputField != null) {

					inputField.OnPointerClick(new PointerEventData(system));
				}

				system.SetSelectedGameObject(next.gameObject, new BaseEventData(system));
			}
			else {

				Debug.Log("next navigation element not found");
			}


		}
	}
}
