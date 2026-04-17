using UnityEngine;

public class SimpleFollowCamera : MonoBehaviour
{
	public Transform target;        // This will swap between Player and Arena Center
	public float smoothSpeed = 5f;  
	public Vector3 offset = new Vector3(0, 0, -10); 

	void LateUpdate()
	{
		if (target == null) return;

		Vector3 desiredPosition = target.position + offset;
		// The camera will now always move toward the current target
		transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
	}
}