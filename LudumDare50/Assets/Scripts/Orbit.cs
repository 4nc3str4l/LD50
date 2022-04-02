using UnityEngine;

public class Orbit : MonoBehaviour
{

	public float xSpread = 3.0f;
	public float zSpread = 3.0f;
	public float yOffset = 0f;
	public Vector3 centerPoint;

	public float rotSpeed;
	public bool rotateClockwise;

	float timer = 0;

	// Update is called once per frame
	void Update()
	{
		timer += Time.deltaTime * rotSpeed;
		Rotate();
	}

	void Rotate()
	{
		if (rotateClockwise)
		{
			float x = -Mathf.Cos(timer) * xSpread;
			float z = Mathf.Sin(timer) * zSpread;
			Vector3 pos = new Vector3(x, yOffset, z);
			transform.position = pos + centerPoint;
		}
		else
		{
			float x = Mathf.Cos(timer) * xSpread;
			float z = Mathf.Sin(timer) * zSpread;
			Vector3 pos = new Vector3(x, yOffset, z);
			transform.position = pos + centerPoint;
		}
	}
}

