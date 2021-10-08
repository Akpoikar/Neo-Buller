using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Riser : MonoBehaviour
{
	private float randomOffset;

	private void Start()
	{
		randomOffset = Random.Range(0f, 5f);
	}

	// Update is called once per frame
	void Update()
    {
		float perlin = Mathf.PerlinNoise(transform.position.x / 5f + Time.time * 1f, transform.position.z / 5f + Time.time * 1f);
		transform.localScale = new Vector3(transform.localScale.x, perlin * 3.5f + 1f, transform.localScale.z);
    }
}
