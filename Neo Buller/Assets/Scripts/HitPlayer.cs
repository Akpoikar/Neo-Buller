using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class HitPlayer : MonoBehaviour
{
    GameObject stats;
    public GameObject explosionPrefab;

    public AudioSource AudioClip;

    private void Start()
    {
        stats = GameObject.FindGameObjectWithTag("Player");

    }
    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag.Equals("Player"))
        {
            AudioManager.instance.Play("Explode");
            CameraShaker.Instance.ShakeOnce(2f, 2f, .05f, .35f);
            GameObject explode = Instantiate(explosionPrefab, other.transform.position, Quaternion.identity);
            Destroy(explode, 1f);
            other.GetComponent<PlayerController>().GetHit();
           // CameraShaker.Instance.ShakeOnce(2f, 2f, .05f, .35f);

        }


    }
}
