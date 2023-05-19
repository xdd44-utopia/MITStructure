using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Controller : MonoBehaviour
{
	public GameObject unitPrefab;
	public GameObject fixedPrefab;
	public GameObject supportPrefab;
	public TMP_Text fps;

	private List<List<Unit>> units;

	private int iteration = 250;
	
	private float width = 25;
	private float height = 100;
	private int widthCount = 6;
	private int heightCount = 21;

	private float unitMass = 0.4f;
	private float unitElasticity = 10000f;

	void Start() {
		units = new List<List<Unit>>();
		for (int i=0;i<widthCount;i++) {
			units.Add(new List<Unit>());
			for (int j=0;j<heightCount;j++) {
				Vector3 pos = new Vector3(
					i * width / (widthCount - 1),
					i * width / (widthCount - 1) + (25 - 2 * i * width / (widthCount - 1)) * j / (heightCount - 1),
					j * height / (heightCount - 1)
				);
				GameObject newUnitObject;
				if ((i == 0 && j == 0) || (i == widthCount - 1 && j == heightCount - 1)) {
					newUnitObject = Instantiate(fixedPrefab, pos, Quaternion.identity);
				}
				else if ((i == 0 && j == heightCount - 1) || (i == widthCount - 1 && j == 0)) {
					newUnitObject = Instantiate(supportPrefab, pos, Quaternion.identity);
					newUnitObject.GetComponent<SupportUnit>().updateBase(new Vector3(i * width / (widthCount - 1), 0, j * height / (heightCount - 1)));
				}
				else {
					newUnitObject = Instantiate(unitPrefab, pos, Quaternion.identity);
				}
				Unit newUnit = newUnitObject.GetComponent<Unit>();
				newUnit.updateProperty(unitMass, unitElasticity);
				if (i > 0) {
					newUnit.addConnection(units[i - 1][j], Mathf.Sqrt(2) * width / (widthCount - 1));
					units[i - 1][j].addConnection(newUnit, Mathf.Sqrt(2) * width / (widthCount - 1));
				}
				if (j > 0) {
					newUnit.addConnection(units[i][j - 1], height / (heightCount - 1));
					units[i][j - 1].addConnection(newUnit, height / (heightCount - 1));
				}
				units[i].Add(newUnit);
			}
		}

	}

	void Update() {
		fps.text = $"FPS = {Mathf.Round(1 / Time.deltaTime * 100f) / 100f}";
		for (int t=0;t<iteration;t++) {
			for (int i=0;i<widthCount;i++) {
				for (int j=0;j<heightCount;j++) {
					units[i][j].updateForce();
				}
			}
			for (int i=0;i<widthCount;i++) {
				for (int j=0;j<heightCount;j++) {
					units[i][j].updatePosition();
				}
			}
		}
	}
}
