using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    public class Ref<T>
    {
        private T backup;
        public T Value { get { return backup; } set { backup = value; } }
        public Ref(T referance)
        {
            backup = referance;
        }
    }


    [SerializeField]
    private float setFireRate = 0.25f;
    [SerializeField]
    private float reloadTime = 0.5f;
    private float fireRateTimer;
    [SerializeField]
    private Animator weaponAnimator;

    public Ref<bool> canShoot;
    // public bool isShoot = true;
    public Ref<bool> reloadDone;

    public float SetFireRate { get => setFireRate; }
    public float ReloadTime { get => reloadTime; }


    // Start is called before the first frame update
    private void Awake()
    {
        fireRateTimer = SetFireRate;
        canShoot = new Ref<bool>(true);
        reloadDone = new Ref<bool>(true);  

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void coolDown(float time, Ref<bool> value)
    {
        StartCoroutine(coolDownCo(time, value));
    }


    public void ReloadAnimUpdate()
    {
        weaponAnimator.SetTrigger("Reload");
    }

    IEnumerator coolDownCo(float time, Ref<bool> value)
    {
        value.Value = !value.Value;
        yield return new WaitForSeconds(time);
        value.Value = !value.Value;
    }

    public void fireAnimUpdate()
    {
        weaponAnimator.SetTrigger("Shoot");
    }

}
