using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Controller : MonoBehaviour
{
	public GameObject prefab;
	public TMP_Text fps;

	private List<List<Unit>> units;

	private int iteration = 100;
	
	private float width = 25;
	private float height = 100;
	private int widthCount = 6;
	private int heightCount = 16;

	private float unitMass = 0.25f;
	private float unitElasticity = 500f;

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
				GameObject newUnitObject = Instantiate(prefab, pos, Quaternion.identity);
				Unit newUnit = newUnitObject.GetComponent<Unit>();
				newUnit.updateProperty(unitMass, unitElasticity);
				if (i > 0) {
					newUnit.addConnection(units[i - 1][j], (units[i - 1][j].transform.position - newUnit.transform.position).magnitude);
					units[i - 1][j].addConnection(newUnit, (units[i - 1][j].transform.position - newUnit.transform.position).magnitude);
				}
				if (j > 0) {
					newUnit.addConnection(units[i][j - 1], (units[i][j - 1].transform.position - newUnit.transform.position).magnitude);
					units[i][j - 1].addConnection(newUnit, (units[i][j - 1].transform.position - newUnit.transform.position).magnitude);
				}
				units[i].Add(newUnit);
			}
		}
		units[0][0].fix();
		units[widthCount - 1][0].fix();
		units[0][heightCount - 1].fix();
		units[widthCount - 1][heightCount - 1].fix();
	}

	void Update() {
		// if (Input.GetKeyDown("space")) {
		if (true) {
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
}
