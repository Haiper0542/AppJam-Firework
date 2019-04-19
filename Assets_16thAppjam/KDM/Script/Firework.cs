using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FireworkType { Stick, Speaker, Fountain,Sparkle }
public enum FireworkState { Item, Equiped, FireReady, Off}

public class Firework : MonoBehaviour {
    #region Variables

    public FireworkType Type;
    public FireworkState State;

    [SerializeField] private GameObject FireBall;
    
    [SerializeField] private float ShootSpeed; //분수형일 경우, 적을 때리는 간격
    [SerializeField] private int ShootAmount; //분수형일 경우, 지속 시간
    [SerializeField] private int Damage;

    [SerializeField] private Transform FireTr;
    [SerializeField] private Transform TableTr;
    [SerializeField] private GameObject SmokeEffect;
    [SerializeField] private ParticleSystem FireEffect;

    public bool InFire = false;
    private int amount;
    private BulletCtrl[] fireballs;

    #endregion

    #region LifeCycle Methods

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fire") && State == FireworkState.Equiped)
        {
            InFire = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Fire"))
        {
            InFire = false;
        }
    }

    private void Start()
    {
        fireballs = new BulletCtrl[ShootAmount];
        for (int i = 0; i < ShootAmount; i++)
        {
            GameObject fb = Instantiate(FireBall, FireTr);
            fireballs[i] = fb.GetComponent<BulletCtrl>();
            fireballs[i].gameObject.SetActive(false);
            fireballs[i].Damage = Damage;
        }
    }

    IEnumerator Fire()
    {
        yield return new WaitForSeconds(2f);
        FireEffect.gameObject.SetActive(true);
        while (State == FireworkState.FireReady)
        {
            amount--;
            if (amount <= 0)
            {
                yield return new WaitForSeconds(1f);
                OffFirework();
            }

            if (FireEffect.isPlaying)
                FireEffect.Stop();
            FireEffect.Play();

            fireballs[(ShootAmount - 1) - amount].gameObject.SetActive(true);
            fireballs[(ShootAmount - 1) - amount].transform.SetParent(null);
            fireballs[(ShootAmount - 1) - amount].Fire();
            
            yield return new WaitForSeconds(ShootSpeed);
        }
    }
    
    #endregion

    #region Other Methods

    public void EquipFirework()
    {
        State = FireworkState.Equiped;
    }

    public void BurnFirework()
    {

        amount = ShootAmount;
        State = FireworkState.FireReady;
        SmokeEffect.SetActive(true);
        StartCoroutine(Fire());
    }

    public void OffFirework()
    {
        FireEffect.gameObject.SetActive(false);
        SmokeEffect.SetActive(false);
        StopCoroutine(Fire());
        State = FireworkState.Off;
    }

    public void Reset()
    {
        for (int i = 0; i < fireballs.Length;i++)
        {
            fireballs[i].transform.SetParent(FireTr);
            fireballs[i].transform.localPosition = Vector3.zero;
            fireballs[i].transform.localRotation = Quaternion.Euler(Vector3.zero);
            fireballs[i].transform.localScale= Vector3.one;
        }
        
        transform.SetParent(TableTr);
        transform.localPosition=Vector3.zero;
        transform.localRotation = Quaternion.Euler(0, 0, 0);
        State = FireworkState.Item;
    }

    #endregion
}
