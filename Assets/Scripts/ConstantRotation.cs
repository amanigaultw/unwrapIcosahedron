using UnityEngine;

public class ConstantRotation : MonoBehaviour {


	public float speed = 1;
	public float RotAngleY = 45;

	void Update() {
		float rY = Mathf.SmoothStep(-RotAngleY, RotAngleY, Mathf.PingPong(Time.time * speed, 1));
		transform.rotation = Quaternion.Euler(0, rY, 0);
	}
}
