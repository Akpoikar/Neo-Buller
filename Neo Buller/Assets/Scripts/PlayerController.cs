using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
	public float moveSpeed = 5f;

	Vector2 move;
	Vector2 aim;
	public Camera cam;
	Rigidbody rb;
	PlayerWeapon weapon;
	bool fire = false;
	private void Start()
	{
		rb = GetComponent<Rigidbody>();
		weapon = GetComponent<PlayerWeapon>();
	}

	private void FixedUpdate()
	{
		Vector3 moveDir = new Vector3(-move.y, 0f, move.x);
		rb.MovePosition(rb.position + moveDir * moveSpeed * Time.fixedDeltaTime);

		transform.position = new Vector3(transform.position.x, GameObject.FindGameObjectWithTag("SpawnPointA").transform.position.y, transform.position.z);

/**/
		Vector3 mousePos = Input.mousePosition;
		mousePos.z = 5.23f;

		Vector3 objectPos = UnityEngine.Camera.main.WorldToScreenPoint(transform.position);

		mousePos.x = mousePos.x - objectPos.x;
		mousePos.y = mousePos.y - objectPos.y;

		float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler(new Vector3(0, -angle + 90, 0));
	}

	void OnMove(InputValue value)
	{
		if (GameManager.Instance.waitingForPlayers)
		{
			move = Vector2.zero;
			return;
		}

		move = value.Get<Vector2>();
	}

	void OnAim(InputValue value)
	{
		aim = value.Get<Vector2>();
	}

	void OnFire()
	{
		/*if (GameManager.Instance.waitingForPlayers)
			return;
*/
		weapon.Shoot();
	}

    private void Update()
    {
		if (Input.GetKeyDown(KeyCode.Mouse0))
		{
			fire = !fire;
		}

		if (fire == true)
			OnFire();
		

	}
}
