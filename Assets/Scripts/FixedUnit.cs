using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FixedUnit : Unit
{
	public LineRenderer forceLine;
	public TMP_Text forceText;
	
	void Start() {
		
	}

	void Update() {

	}

	public override void updateProperty(float m, float e) {
		mass = 0;
		elasticity = e;
	}

	public override void updateForce() {
		forceBuffer = Vector3.zero;
		for (int i=0;i<connections.Count;i++) {
			Vector3 v = connections[i].transform.position - transform.position;
			forceBuffer += v.normalized * (v.magnitude - originalDist[i]) * elasticity;
		}

		forceLine.SetPosition(0, transform.position);
		forceLine.SetPosition(1, transform.position + forceBuffer.normalized * 10);
		forceText.transform.position = transform.position + forceBuffer.normalized * 10;
		forceText.text = $"{Mathf.Round(forceBuffer.magnitude * 100) / 100}";

	}

	public override void updatePosition() {
		return;
	}
}
