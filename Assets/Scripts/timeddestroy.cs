using UnityEngine;

public class timeddestroy : MonoBehaviour
{
	public float lifetime = 0.2f; // Short burst
	void Start() { Destroy(gameObject, lifetime);
	}
}
