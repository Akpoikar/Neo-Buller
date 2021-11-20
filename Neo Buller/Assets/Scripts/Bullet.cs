using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public enum Abilities { Def, Split, Circle, Ricochet };

public class Bullet : MonoBehaviour
{
	public Abilities ability = Abilities.Def;
	Elementals element;
//	public GameObject explosionPrefab;
	public int bounce = 2;
	bool hitPlayer = false;
	float lastBounceTime = 0f;
	public GameObject explosionPrefab;
	int damage = 100;
	private void Awake()
    {
		Destroy(gameObject, 3f);
	}

	public void HitPlayer()
    {
		hitPlayer = true;
    }

	public void setElement(Elementals elementals)
    {
		element = elementals;
    }

    public void setAbility(Abilities setAbility)
    {
		ability = setAbility;
    }  
	
	public void setDamage(int dmg)
    {
		damage = dmg;
    }
	
	private void OnCollisionEnter(Collision col)
	{
		if(col.collider.CompareTag("Bullet") && !hitPlayer 
			&& col.transform.GetComponent<Bullet>().hitPlayer == true
			) {
			GameObject explode = Instantiate(explosionPrefab, col.transform.position, Quaternion.identity);
			Destroy(explode, 1f);
			Explode();
		}
		else if(col.collider.CompareTag("Player") && hitPlayer)
        {
			AudioManager.instance.Play("Explode");
			CameraShaker.Instance.ShakeOnce(2f, 2f, .05f, .35f);
			GameObject explode = Instantiate(explosionPrefab, col.transform.position, Quaternion.identity);
			Destroy(explode, 1f);
			col.transform.GetComponent<PlayerController>().GetHit();
			Explode();
		}
		else if (col.collider.CompareTag("Enemy") && !hitPlayer)
		{
			col.transform.GetComponent<EnemyAI>().Hit(element, damage);
			GameObject explode = Instantiate(explosionPrefab, col.transform.position, Quaternion.identity);
			Destroy(explode, 1f);
			Explode();
		} 
		else
		{
			if (hitPlayer == true)
			{
				Explode();
				return;
			}
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
		//AudioManager.instance.Play("Explode");

		/*GameObject explode = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
		Destroy(explode, 10f);*/
		


		Destroy(gameObject);

		CameraShaker.Instance.ShakeOnce(1f, 1f, .05f, .35f);
	}

}
