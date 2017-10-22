using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BarChart : MonoBehaviour {

	public Bar barPrefab;

	private List<int> arriveValues = new List<int>();
	private List<int> killValues = new List<int>();
	private float chartHeight;


	void Awake () {
		chartHeight = GetComponent<RectTransform>().rect.height;
	}


	public void DisplayGraph(string type) {

		DeleteAllChildren();

		if (type == "arrive") {

			for (int i = 0; i < arriveValues.Count; i++) {

				Bar newBar = Instantiate(barPrefab) as Bar;
				newBar.transform.SetParent(transform);
				newBar.transform.localScale = new Vector3(0.5f, 1.0f, 0.5f);
				RectTransform rt = newBar.bar.GetComponent<RectTransform>();

				float normalizedValue = (float)arriveValues[i] / 50.0f;

				rt.sizeDelta = new Vector2(rt.sizeDelta.x, chartHeight * normalizedValue);

				GameObject barValueText = new GameObject("barValue");
				barValueText.transform.SetParent(newBar.transform);
				barValueText.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
				barValueText.transform.position = new Vector3(newBar.transform.position.x, (newBar.transform.position.y - (chartHeight / 2)) + 7.0f, newBar.transform.position.z);

				Text value = barValueText.AddComponent<Text>();
				RectTransform textRt = barValueText.GetComponent<RectTransform>();

				textRt.sizeDelta = new Vector2(25, 20.0f);

				Font arialFont = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");

				value.text = arriveValues[i].ToString();
				value.font = arialFont;
				value.material = arialFont.material;
				value.color = new Color(0, 0, 0);
			}
		}
		else {

			for (int i = 0; i < killValues.Count; i++) {

				Bar newBar = Instantiate(barPrefab) as Bar;
				newBar.transform.SetParent(transform);
				newBar.transform.localScale = new Vector3(0.5f, 1.0f, 0.5f);
				RectTransform rt = newBar.bar.GetComponent<RectTransform>();

				float normalizedValue = (float)killValues[i] / 50.0f;

				rt.sizeDelta = new Vector2(rt.sizeDelta.x, chartHeight * normalizedValue);

				GameObject barValueText = new GameObject("barValue");
				barValueText.transform.SetParent(newBar.transform);
				barValueText.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
				barValueText.transform.position = new Vector3(newBar.transform.position.x, (newBar.transform.position.y - (chartHeight / 2)) + 7.0f, newBar.transform.position.z);

				Text value = barValueText.AddComponent<Text>();
				RectTransform textRt = barValueText.GetComponent<RectTransform>();

				textRt.sizeDelta = new Vector2(25, 20.0f);

				Font arialFont = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");

				value.text = killValues[i].ToString();
				value.font = arialFont;
				value.material = arialFont.material;
				value.color = new Color(0, 0, 0);
			}
		}
	}

	public void AddValuesToLists(int barValue, string type) {

		if (type == "arrive") {
			arriveValues.Add(barValue);
		}
		else {
			killValues.Add(barValue);
		}
	}
		
	void DeleteAllChildren() {

		foreach(Transform child in this.transform) {
			Destroy(child.gameObject);
		}
	}
}
