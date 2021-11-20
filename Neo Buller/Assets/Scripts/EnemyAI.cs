using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyAI : MonoBehaviour
{
    public GameObject player;
    public GameObject SpawnPrefab;
    private NavMeshAgent agent;
    bool dead = false;
    public bool range = false;
    public int hp = 100;
    public Elementals element;

    // HIT MELEE
    float AttackCD;
    public float defAttackCD = 1f;
    IEnumerator HitEffect;
    public Animator animator;
    public GameObject hitter;
    public float HitTimer = .3f;
    // HIT MELEE END

    // HIT RANGE
    public GameObject bulletPrefab;
    public Color bulletColor;
    public float bulletRefillRate = 1f;
    int bulletsReady = 1;
    float nextBulletTime = 0f;
    public Transform firePoint;
    public float bulletSpeed = 10f;
    // HIT RANGE END

    public GameObject DamagePrefab;
    public GameObject HpBarPrefab;
    int maxHp;
    bool firstHit = false;
    // Start is called before the first frame update
    void Start()
    {
        HpBarPrefab.SetActive(false);
        agent = GetComponent<NavMeshAgent>();
        AttackCD = 0;
        maxHp = hp;
    }

    // Update is called once per frame
    void Update()
    {
        if (firstHit)
        {
            HpBarPrefab.GetComponentInChildren<Slider>().value = (float)((float)hp / (float)maxHp);

            HpBarPrefab.transform.rotation = Quaternion.Euler(HpBarPrefab.transform.rotation.x * -1.0f, HpBarPrefab.transform.rotation.y * -1.0f, HpBarPrefab.transform.rotation.z * -1.0f);
        }
        
        if (dead == true) {
            agent.ResetPath();
            return; 
        }
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
            return;

        var dis = Vector3.Distance(player.transform.position, transform.position);

        if(dis < 3f)
            agent.ResetPath();
        else
            agent.SetDestination(player.transform.position);

        AttackCD -= Time.deltaTime;
        
        if(dis < 20f && range && dead == false)
        {
            Vector3 dirFromAtoB = (player.transform.position - transform.position).normalized;
            float dotProd = Vector3.Dot(dirFromAtoB, transform.forward);

            if (dotProd > 0.9)
            {
                if (Time.time >= nextBulletTime)
                {
                    bulletsReady++;
                    nextBulletTime = Time.time + 1f / bulletRefillRate;
                }
                Shoot();
            }
        }
        else if (dis < 5f && dead == false)
        {
            if (AttackCD < 0)
            {
                HitPlayer();
                AttackCD = defAttackCD;
            }
        }
    }

    private void Shoot()
    {
        if (bulletsReady > 0)
        {
            Light light = bulletPrefab.GetComponentInChildren<Light>();

            light.color = bulletColor;

            GameObject ball = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Rigidbody ballRB = ball.GetComponent<Rigidbody>();
            ball.GetComponent<Bullet>().HitPlayer();
            ballRB.AddForce(firePoint.forward * bulletSpeed, ForceMode.VelocityChange);

            AudioManager.instance.Play("Shoot");

            bulletsReady--;
            nextBulletTime = Time.time + 1f / bulletRefillRate;

        }
    }

    public void Hit(Elementals elementals, int dmg)
    {
        if (!firstHit)
        {
            HpBarPrefab.SetActive(true);
        }

        firstHit = true;

        int d = getDamageAfterElements(elementals, dmg);
        hp  -= d;
        
        GameObject dmgPopUp = Instantiate(DamagePrefab, transform.position, Quaternion.identity);
        dmgPopUp.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = $"-{d}";

        if (d < dmg)
            dmgPopUp.GetComponentInChildren<TMPro.TextMeshProUGUI>().color = Color.cyan;
        else if (d > dmg)
            dmgPopUp.GetComponentInChildren<TMPro.TextMeshProUGUI>().color = Color.red;


        Destroy(dmgPopUp, 2f);

        if (hp <= 0) Die();
    }

    int getDamageAfterElements( Elementals elementals, int dmg )
    {
        double damage = 1;
        switch (element)
        {
            case Elementals.Fire:
                if (elementals == Elementals.Water || elementals == Elementals.Electro)
                    damage *= 1.2;
                break;
            case Elementals.Water:
                if (elementals == Elementals.Cryo || elementals == Elementals.Electro)
                    damage *= 1.2;
                break;
            case Elementals.Electro:
                if (elementals == Elementals.Water || elementals == Elementals.Fire || elementals == Elementals.Cryo)
                    damage *= 1.2;
                break;
            case Elementals.Cryo:
                if (elementals == Elementals.Fire || elementals == Elementals.Electro)
                    damage *= 1.2;
                break;
            case Elementals.Crystal:
                damage *= .8;
                break;
            default:
                break;
        }
        return (int)(dmg * damage);
    }


    void Die()
    {
        if (GameManager.Instance.gameIsEnded)
            return;
        player.GetComponent<PlayerController>().AddScore();
        GetComponent<CapsuleCollider>().enabled = false;
        dead = true;
        var deadAlready = Death();
        StartCoroutine(deadAlready);
    }


    IEnumerator Death()
    {
        var point = Instantiate(SpawnPrefab);
        point.transform.position = transform.position;

        point.GetComponent<Animator>().SetTrigger("Big");
        yield return new WaitForSeconds(1);
        GetComponent<Animator>().SetTrigger("Small");

        
        point.GetComponent<Animator>().SetTrigger("Small");
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
        Destroy(point);
    }


    public void HitPlayer()
    {
        //animator.SetTrigger("Hit");
        HitEffect = hitEff();
        StartCoroutine(HitEffect);
    }

    public IEnumerator hitEff()
    {
        if (dead) yield break;
        animator.SetTrigger("Hit");
        GetComponentInChildren<TrailRenderer>().enabled = true;
        yield return new WaitForSeconds(.3f);
        if (dead)
        {
            GetComponentInChildren<TrailRenderer>().enabled = false;
            yield break;
        }
        hitter.transform.gameObject.SetActive(true);
        yield return new WaitForSeconds(.4f);
        GetComponentInChildren<TrailRenderer>().enabled = false;
        hitter.transform.gameObject.SetActive(false);
    }

}
