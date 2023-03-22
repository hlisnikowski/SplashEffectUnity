using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

	[SerializeField] float speed = 3;
	public Rigidbody2D rg;

	[Header("Press P to instantiate Apple")]
	[SerializeField] GameObject apple;
	Vector3 move;
	// Update is called once per frame
	void FixedUpdate()
    {
		move = Vector3.zero;
		move.x = Input.GetAxisRaw("Horizontal");
		move.y = Input.GetAxisRaw("Vertical");
		if (move != Vector3.zero)
		{
			move.x = Mathf.Round(move.x);
			move.y = Mathf.Round(move.y);
			move = move.normalized;
			rg.MovePosition(transform.position + move * speed * Time.deltaTime);
		}
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.P))
			Instantiate(apple, transform.position, Quaternion.identity);
	}
}
