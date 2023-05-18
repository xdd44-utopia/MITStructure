using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Unit : MonoBehaviour
{
	private static float gravity = 9.8f;
	private static float speed = 0.1f;

	private float mass = 0.6f;
	private float elasticity = 1f;

	private bool fixPosition = false;

	private List<Unit> connections = new List<Unit>();
	private List<float> originalDist = new List<float>();

	private Vector3 forceBuffer = Vector3.zero;

	public LineRenderer forceLine;
	public TMP_Text forceText;
	public LineRenderer gravityLine;
	public TMP_Text gravityText;
	
	void Start() {
		
	}

	void Update() {

	}

	public void fix() {
		fixPosition = true;
	}

	public void updateProperty(float m, float e) {
		mass = m;
		elasticity = e;
	}

	public void addConnection(Unit other, float dist) {
		connections.Add(other);
		originalDist.Add(dist);
	}

	public void updateForce() {
		forceBuffer = Vector3.zero;
		for (int i=0;i<connections.Count;i++) {
			Vector3 v = connections[i].transform.position - transform.position;
			forceBuffer += v.normalized * (v.magnitude - originalDist[i]) * elasticity;
		}

		forceLine.SetPosition(0, transform.position);
		forceLine.SetPosition(1, transform.position + forceBuffer);
		forceText.transform.position = transform.position + forceBuffer;
		forceText.text = $"{Mathf.Round(forceBuffer.magnitude * 100) / 100}";

		forceBuffer += Vector3.down * gravity * mass;

		gravityLine.SetPosition(0, transform.position);
		gravityLine.SetPosition(1, transform.position + Vector3.down * gravity * mass);
		gravityText.transform.position = transform.position + Vector3.down * gravity * mass;
		gravityText.text = $"{Mathf.Round(gravity * mass * 100) / 100}";
	}

	public void updatePosition() {
		if (fixPosition) {
			return;
		}
		forceBuffer = damp(forceBuffer);
		transform.position = Vector3.Lerp(transform.position, transform.position + forceBuffer * Time.deltaTime * speed / mass, Time.deltaTime * 0.2f);
		forceBuffer = Vector3.zero;
	}

	public Vector3 damp(Vector3 f) {
		return f.normalized * (1 / (1 + Mathf.Exp(-f.magnitude)) - 0.5f) * 2;
	}
}
