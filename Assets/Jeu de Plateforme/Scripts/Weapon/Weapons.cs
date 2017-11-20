using UnityEngine;
//Classe qui va gérer la gestion des fusils (Importation de Unity)

public class Weapons : MonoBehaviour
{ // Use this for initialization Weapon
    protected bool fire;                    
    protected Animator anim;                  
    public Sprite picture_weapon;            
    public GameObject ammunition;            
    public Vector3 spawn_point;               
    public bool weapon_animation;            
    public bool front;                        
    protected Vector3 Spawn_point             
    {
        get
        {
            Vector3 spawn = transform.parent.position + new Vector3(spawn_point.x * Mathf.Sign(transform.parent.localScale.x), spawn_point.y, spawn_point.z);
            AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("Base Layer.Sit"))
                return spawn -= Vector3.up * 0.6f;
            else
                return spawn;
        }
    }
    public int amount;                    
    protected int Amount                    
    {
        get
        {
            return amount;
        }
        set
        {
            amount = value;
            if(amount==0)
            {
                GetComponent<SpriteRenderer>().sprite = null;
                Destroy(GetComponents<Behaviour>()[GetComponents<Behaviour>().Length - 1]);
            }
        }
    }
    public float fireRate;
    float timeRate;
    public enum FireRate
    {
        simple,fast
    }
    public FireRate TypefireRate;

    protected virtual void Update()
    {
        timeRate += Time.deltaTime;
        switch (TypefireRate)
        {
            case FireRate.simple:
                if (Input.GetButtonDown("Fire1")) 
                {
                    fire = true;
                }
                break;
            case FireRate.fast:
                if (Input.GetButton("Fire1") && timeRate > fireRate)
                {
                    fire = true;
                    timeRate = 0;
                }
                break;
        }
    }

    protected virtual void FixedUpdate()
    {
        if (fire)
        {
            fire = false;
            if (weapon_animation)
                anim.SetTrigger("Shoot");
            PhotonNetwork.Instantiate("Ammo/" + ammunition.name, Spawn_point, Quaternion.LookRotation(new Vector3(0, 0, Mathf.Sign(transform.root.localScale.x))), 0);

            Amount--;
        }
    }
    protected virtual void Start()
    {
        GetComponent<SpriteRenderer>().sortingOrder = front ? 1 : 0;
        anim = transform.root.GetComponent<Animator>();
        GetComponent<SpriteRenderer>().sprite = picture_weapon;
    }
    private void OnDisable()
    {
            GetComponents<Behaviour>()[GetComponents<Behaviour>().Length - 1].enabled = true;
    }
    public void Initialization(Weapons new_, Weapons original)
    {
        new_.picture_weapon = original.picture_weapon;
        new_.ammunition = original.ammunition;
        new_.spawn_point = original.spawn_point;
        new_.weapon_animation = original.weapon_animation;
        new_.front = original.front;
        new_.amount = original.amount;
        new_.fireRate = original.fireRate;
        new_.TypefireRate = original.TypefireRate;
    }
}
