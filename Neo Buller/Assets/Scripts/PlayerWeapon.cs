using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;
using UnityEngine.UI;

public enum BulletNumber { Single, Double, Tripple, Quadro }

public class PlayerWeapon : MonoBehaviour
{
	public BulletNumber bulletNumber = BulletNumber.Single;
	public GameObject bulletPrefab;
	public Transform[] firePointsSingle;
	public Transform[] firePointsTripple;
	public Transform[] firePointsDouble;
	public Transform[] firePointsQuadro;
	public Color[] bulletColors;
	public Material bulletMaterial;
	public float bulletSpeed = 10f;

	public float bulletRefillRate = 1f;
	int bulletsReady = 0;
	public bool ifPlayer = true;

	public bool SuperShoot = false;
	public float SuperShootTimer = 0f;
	public float SuperShootDefault = 5f;
	bool superShotEnded = true;
	bool UltimateUIShow = true;
	float nextBulletTime = 0f;
	public GameObject UltimateUI;
	public GameObject UltimateUIBar;
	public GameObject UltimateUIButton;
	public int Damage = 100;
    private void Awake()
    {
		SuperShootTimer = SuperShootDefault;

	}

    public void Reset()
	{
		bulletsReady = 1;
		nextBulletTime = Time.time + 1f / bulletRefillRate;
		nextBulletTime =0 ;
		SuperShootTimer = 0;

	}

	public void Shooting()
    {
		UltimateUIBar.GetComponent<Slider>().value = 0;
		var shooter = EnumeratorSuperShoot();
		StartCoroutine(shooter);
		UltimateUIShow = true;
		UltimateUIButton.SetActive(false);
	}

	private void Update()
	{
		if (SuperShootTimer > 0)
		{
			SuperShootTimer -= Time.deltaTime;
			UltimateUIBar.GetComponent<Slider>().value = 5-SuperShootTimer;
		}

		

		if(SuperShootTimer < 0 && superShotEnded)
        {

			if (UltimateUIShow)
			{
				UltimateUI.GetComponent<Animator>().SetTrigger("Ulti");
				UltimateUIShow = false;
			}
			UltimateUIButton.SetActive(true);

			if(Input.GetKeyDown(KeyCode.Space))
            {
				Shooting();
			}
        }


		if (GameManager.Instance.waitingForPlayers || bulletsReady >= 1)
			return;

		if (Time.time >= nextBulletTime)
		{
			bulletsReady++;

			if(!ifPlayer)
				nextBulletTime = Time.time + 3f / bulletRefillRate;
			else 
				nextBulletTime = Time.time + 1f / bulletRefillRate;
		}
		if(!ifPlayer)
			Shoot();
	}
	public void SuperShooter()
    {
		SuperShoot = !SuperShoot;

	}
	public void Shoot()
	{
		if (bulletsReady > 0 || SuperShoot)
		{
			Light light = bulletPrefab.GetComponentInChildren<Light>();

			light.color = GetComponent<Elements>().GetColorOnElement();
			//light.color = bulletColors[ Random.Range(0,bulletColors.Length)];

			var firePoints =GetPoints();
            for (int i = 0; i < firePoints.Length; i++)
            {
				GameObject ball = Instantiate(bulletPrefab, firePoints[i].position, firePoints[i].rotation);
				ball.GetComponent<Bullet>().setElement( GetComponent<Elements>().element );
				ball.GetComponent<Bullet>().setDamage(Damage);
				Rigidbody ballRB = ball.GetComponent<Rigidbody>();
				ballRB.AddForce(firePoints[i].forward * bulletSpeed, ForceMode.VelocityChange);
			}
			
			AudioManager.instance.Play("Shoot");
			CameraShaker.Instance.ShakeOnce(1.3f, 1.3f, .05f, .25f);

			if (SuperShoot == false)
			{
				bulletsReady--;
				nextBulletTime = Time.time + 1f / bulletRefillRate;
			}
		}
	}

	IEnumerator EnumeratorSuperShoot()
    {
		superShotEnded = false; 
		SuperShooter();
		yield return new WaitForSeconds(3f);
		SuperShooter();
		SuperShootTimer = SuperShootDefault;
		superShotEnded = true;
	}

	public Transform[] GetPoints()
    {
        return bulletNumber switch
        {
            BulletNumber.Single => firePointsSingle,
            BulletNumber.Double => firePointsDouble,
            BulletNumber.Tripple => firePointsTripple,
            BulletNumber.Quadro => firePointsQuadro,
            _ => firePointsSingle,
        };
    }

}
