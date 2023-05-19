using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Unit : MonoBehaviour {

	public LineRenderer mesh;

	protected static float gravity = 9.8f;
	protected static float speed = 0.2f;

	protected float mass = 0.6f;
	protected float elasticity = 1f;

	protected List<Unit> connections = new List<Unit>();
	protected List<float> originalDist = new List<float>();

	protected Vector3 forceBuffer = Vector3.zero;
	
	void Start() {
		mesh.SetWidth(0.1f, 0.1f);
	}

	void Update() {
		mesh.positionCount = connections.Count * 2 + 1;
		mesh.SetPosition(connections.Count * 2, transform.position);
		for (int i=0;i<connections.Count;i++) {
			mesh.SetPosition(i * 2, transform.position);
			mesh.SetPosition(i * 2 + 1, connections[i].transform.position);
		}
	}

	public virtual void updateProperty(float m, float e) {
		mass = m;
		elasticity = e;
	}

	public void addConnection(Unit other, float dist) {
		connections.Add(other);
		originalDist.Add(dist);
	}

	public virtual void updateForce() {
		forceBuffer = Vector3.zero;
		for (int i=0;i<connections.Count;i++) {
			Vector3 v = connections[i].transform.position - transform.position;
			forceBuffer += v.normalized * (v.magnitude - originalDist[i]) * elasticity;
		}
		forceBuffer += Vector3.down * gravity * mass;
	}

	public virtual void updatePosition() {
		forceBuffer = damp(forceBuffer);
		transform.position = Vector3.Lerp(transform.position, transform.position + forceBuffer * Time.deltaTime * speed / mass, Time.deltaTime * 0.2f);
		forceBuffer = Vector3.zero;
	}

	protected virtual Vector3 damp(Vector3 f) {
		return f.normalized * (1 / (1 + Mathf.Exp(-f.magnitude)) - 0.5f) * 2;
	}
}
