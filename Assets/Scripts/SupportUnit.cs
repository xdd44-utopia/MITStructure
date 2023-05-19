using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SupportUnit : Unit
{
	public LineRenderer forceLine;
	public LineRenderer columnLine;
	public LineRenderer cableLine;
	public TMP_Text forceText;

	private float tensionDist = 16;
	private Vector3 baseP;
	
	void Start() {
		
	}

	void Update() {

	}

	public void updateBase(Vector3 b) {
		baseP = b;
	}

	public override void updateProperty(float m, float e) {
		mass = 1;
		elasticity = e;
	}

	public override void updateForce() {
		forceBuffer = Vector3.zero;
		for (int i=0;i<connections.Count;i++) {
			Vector3 v = connections[i].transform.position - transform.position;
			forceBuffer += v.normalized * (v.magnitude - originalDist[i]) * elasticity;
		}

		Vector3 forceDir = forceBuffer - new Vector3(0, forceBuffer.y, 0);
		Vector3 tensionP = baseP - forceDir.normalized * tensionDist;
		Vector3 tension = (tensionP - transform.position).normalized * forceBuffer.magnitude;

		Vector3 combined = forceBuffer + tension;

		forceLine.SetPosition(0, transform.position);
		forceLine.SetPosition(1, transform.position + combined.normalized * 10);
		forceText.transform.position = transform.position + combined.normalized * 10;
		forceText.text = $"{Mathf.Round(combined.magnitude * 100) / 100}";
		columnLine.SetPosition(0, transform.position);
		columnLine.SetPosition(1, baseP);
		cableLine.SetPosition(0, transform.position);
		cableLine.SetPosition(1, tensionP);

		Vector3 supportDir = (baseP - transform.position).normalized;
		Vector3 supportForce = supportDir * Vector3.Dot(combined, supportDir);
		forceBuffer = supportForce - combined;

	}

	protected override Vector3 damp(Vector3 f) {
		return f.normalized * (1 / (1 + Mathf.Exp(-f.magnitude)) - 0.5f) * 2;
	}

}
