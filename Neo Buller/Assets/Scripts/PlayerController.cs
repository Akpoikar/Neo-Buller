using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
	public float moveSpeed = 5f;

	Vector2 move;
	public Camera cam;
	Rigidbody rb;
	PlayerWeapon weapon;
	public bool autoFire = false;
	public Joystick moveJoystick;
	public Joystick aimJoystick;
	public bool PhoneDebug = false;
	public GameObject Score;
	public GameObject Hp;
	public GameObject HpUI;
	int score = 0;
	int HighestScore;
	int hp = 5;
	private void Start()
	{
		if (Application.platform.Equals(RuntimePlatform.Android) || Application.platform.Equals(RuntimePlatform.IPhonePlayer) || PhoneDebug)
		{
			moveJoystick.gameObject.SetActive(true);
			aimJoystick.gameObject.SetActive(true);
		}

		rb = GetComponent<Rigidbody>();
		weapon = GetComponent<PlayerWeapon>();
	}

	private void FixedUpdate()
	{
		if (!Application.platform.Equals(RuntimePlatform.Android) && !Application.platform.Equals(RuntimePlatform.IPhonePlayer)
			&& !PhoneDebug)
		{
			Vector3 moveDir = new Vector3(-move.y, 0f, move.x);

			rb.MovePosition(rb.position + moveDir * moveSpeed * Time.fixedDeltaTime);

			transform.position = new Vector3(transform.position.x, GameObject.FindGameObjectWithTag("SpawnPointA").transform.position.y, transform.position.z);

			Vector3 mousePos = Input.mousePosition;
			mousePos.z = 5.23f;

			Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);

			mousePos.x = mousePos.x - objectPos.x;
			mousePos.y = mousePos.y - objectPos.y;

			float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.Euler(new Vector3(0, -angle + 90, 0));
		}
		if (Application.platform.Equals(RuntimePlatform.Android) || Application.platform.Equals(RuntimePlatform.IPhonePlayer) || PhoneDebug)
		{
			var horizontal = moveJoystick.Horizontal;
			var vertical = moveJoystick.Vertical;

			var movement = new Vector3(-vertical, 0, horizontal);
			rb.MovePosition(rb.position + movement * moveSpeed * Time.deltaTime);

			Vector3 joy;

			joy.x = Vector3.zero.x - aimJoystick.Horizontal;
			joy.y = Vector3.zero.y - aimJoystick.Vertical;

			if (Mathf.Abs(aimJoystick.Horizontal) > .01 || Mathf.Abs(aimJoystick.Vertical) > .01)
			{
				float angle = Mathf.Atan2(joy.y, -joy.x) * Mathf.Rad2Deg;
				transform.rotation = Quaternion.Euler(new Vector3(0, angle + 90, 0));
				autoFire = true;
			}
			else
				autoFire = false;

		}
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

    public void Restart()
    {
		hp = 5;
		score = 0;
    }

    void OnFire()
	{
		/*if (GameManager.Instance.waitingForPlayers)
			return;
*/
		weapon.Shoot();
	}

	public void SetAutoFire()
    {
		autoFire = !autoFire;
    }
	public void AddScore()
    {
		score += 1;
		Score.GetComponent<Animator>().SetTrigger("Scored");
    }

	public void GetHit()
    {
		hp--;
		if (HighestScore < score) HighestScore = score;

		if (hp <= 0)
		{
			Hp.GetComponent<Slider>().value = hp;
			GameManager.Instance.GameEnded(HighestScore);
		}
		score = 0;

	}
	private void Update()
    {
		Hp.GetComponent<Slider>().value = hp;
		var scoreText = score > 0 ? $"COMBO: x{score}" : "";
		
		Score.GetComponent<TMPro.TextMeshProUGUI>().text = scoreText;

		if (autoFire == true)
		{
			OnFire();
		} 
		
		if(Input.GetButton("Fire1") && !PhoneDebug)
		{
			OnFire(); 
			//autoFire = !autoFire;
		}

	}
}
