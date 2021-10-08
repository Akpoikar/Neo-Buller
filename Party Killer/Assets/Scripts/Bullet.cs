using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public enum Abilities { Def, Split, Circle, Ricochet };

public class Bullet : MonoBehaviour
{
	Abilities ability = Abilities.Def;
	public GameObject explosionPrefab;

	int bounce = 2;

	float lastBounceTime = 0f;

	public void setAbility(Abilities setAbility)
    {
		ability = setAbility;
    }
	
	private void OnCollisionEnter(Collision col)
	{
		/*if (col.collider.CompareTag("Player"))
		{
			GameManager.Instance.KillPlayer(col.collider.gameObject);
			Explode();
		} else*/ if (col.collider.CompareTag("Bullet"))
		{
			Explode();
		} else
		{
            switch (ability)
            {
				case Abilities.Ricochet:
					if (Time.time >= lastBounceTime + .05f)
					{
						if (bounce == 0)
						{
							Explode();
						}
						else
						{
							bounce--;
							lastBounceTime = Time.time;

							AudioManager.instance.Play("Bounce");
						}
					}
					break;
				case Abilities.Split:

					break;
				case Abilities.Circle:
					break;
				default:
					break;
            }

            
		}
		
	}

	void Explode()
	{
		AudioManager.instance.Play("Explode");

		GameObject explode = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
		Destroy(explode, 10f);
		Destroy(gameObject);

		CameraShaker.Instance.ShakeOnce(2f, 2f, .05f, .35f);
	}

}
