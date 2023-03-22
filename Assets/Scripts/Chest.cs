using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
	[SerializeField] Animator anim;
	[SerializeField] GameObject coinPrefab;
	[SerializeField] float openTime = 2f;
	[SerializeField] float splitSpeed;
	[SerializeField] bool spawn;

	
    // Start is called before the first frame update
    void Start()
    {
		if (!spawn) return;
		StartCoroutine(OpenCo());
		InvokeRepeating("DropCoin", openTime + 0.5f, splitSpeed);
    }

	IEnumerator OpenCo()
	{
		yield return new WaitForSeconds(openTime);
		anim.SetTrigger("open");
	}

    // Update is called once per frame
	private void DropCoin()
	{
		// Please, learn about DOTS / Object Pooling
		Instantiate(coinPrefab, transform.position, Quaternion.identity, transform);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.CompareTag("Player") && !spawn)
		{
			spawn = true;
			StartCoroutine(OpenCo());
			InvokeRepeating("DropCoin", openTime + 0.5f, splitSpeed);
		}
	}
}
