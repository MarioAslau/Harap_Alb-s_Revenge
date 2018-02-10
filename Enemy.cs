using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    [System.Serializable]


    public class EnemyStats
    {
        public int maxHealth = 100;

        private int _curHealth;
        public int curHealth
        {
            get { return _curHealth; }
            set { _curHealth = Mathf.Clamp(value, 0, maxHealth); }//ensuring the value will be between an interval, 0 and maxHealth with Clamp( , , )
        }

        public int damage = 40;

        public void Init()
        {
            curHealth = maxHealth;
        }

    }

    public EnemyStats stats = new EnemyStats(); 
    public Transform deathParticles;
    public float shakeAmt = 0.1f;
    public float shakeLength = 0.1f;

    [Header("Optional")]//headers are an atribute that will allow  you to type something in the unity editor
    [SerializeField]
    private StatusIndicator statusIndicator;

    void Start()
    {
        stats.Init();

        if (statusIndicator != null)
        {
            statusIndicator.SetHealth(stats.curHealth, stats.maxHealth);//setting current health to max health
        }

        if (deathParticles == null)
        {
            Debug.LogError("No death particles referenced on Enemy");
        }
    }

    public void DamageEnemy(int damage)
    {
        stats.curHealth -= damage; 

        if (stats.curHealth <= 0)
        {
            GameMaster.KillEnemy(this);
        }

        if (statusIndicator != null)//if it doesn't die
        {
            statusIndicator.SetHealth(stats.curHealth, stats.maxHealth);//setting the health
        }
    }

    void OnCollisionEnter2D(Collision2D _colInfo)
    //unity will make sure that everytime we collide with another object this method will be called. It needs to be called exactly like this. We can collect info about what we collided with, passing a paramater collison2d
    {
        Player _player = _colInfo.collider.GetComponent<Player>();//we grab that player component of the collider info. then we check if the player comp is equal to null, if there is we can call the damage player method
        if (_player != null)
        {
            _player.DamagePlayer(stats.damage);
            DamageEnemy(9999999);
        }
    }
}
