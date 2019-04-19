using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour {

    Rigidbody rb;
    public int Damage = 1;
    public float Power = 5;

    public GameObject TrailParticle;
    public GameObject BoomParticle;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Fire();
    }

    public void Fire()
    {
        Debug.Log("싸버렷");
        //발사 스크립트
        TrailParticle.gameObject.SetActive(true);

        //힘 적용
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().AddForce(transform.forward * Power,ForceMode.Impulse);
        StartCoroutine(FireBoom());
    }
    
    public void Boom()
    {
        //폭발 이펙트
        GameObject newBoom = Instantiate(BoomParticle, transform.position + Vector3.up * 0.5f, Quaternion.identity);
        Destroy(newBoom, 3);
    }

    public void OffBullet()
    {
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Rigidbody>().velocity=Vector3.zero;
        this.gameObject.SetActive(false);
    }

    IEnumerator FireBoom()
    {
        yield return new WaitForSeconds(3f);
        Boom();
        yield return new WaitForSeconds(2f);
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Rigidbody>().velocity=Vector3.zero;
        this.gameObject.SetActive(false);
    }
}
