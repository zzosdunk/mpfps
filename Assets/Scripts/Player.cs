using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour {

    [SyncVar]
    private bool _isDead = false;
    public bool isDead //the same is public bool isDead { get; protected set; }, but it doesnt work with [SyncVar]
    {
        get { return _isDead; }
        protected set { _isDead = value; }
    }

    [SerializeField]
    private int maxHealth = 100;

    [SyncVar]
    private int currentHealth;

    [SerializeField]
    private Behaviour[] disableOnDeath;
    private bool[] wasEnabled;

    public void Setup()
    {
        wasEnabled = new bool[disableOnDeath.Length];
        for (int i = 0; i < wasEnabled.Length; i++)
        {
            wasEnabled[i] = disableOnDeath[i].enabled;
        }

        SetDefaults();
    }
    //void Update()
    //{
    //    if (!isLocalPlayer)
    //        return;
    //    if(Input.GetKeyDown(KeyCode.K))
    //    {
    //        RpcTakeDamage(99999);
    //    }
    //}
    [ClientRpc]
    public void RpcTakeDamage(int _amount)
    {
        if (isDead)
            return;

        currentHealth -= _amount;

        Debug.Log(transform.name + " now has " + currentHealth + " hp ");

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        isDead = true;

        //Disable components (moving, etc.)
        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = false;
        }
        Collider _col = GetComponent<Collider>();
        if (_col != null)
            _col.enabled = false;

        Debug.Log(transform.name + " is DIED!");

        //Call respawn method
        StartCoroutine(Respawn());
    }
    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(3f);

        SetDefaults();
        Transform _spawnPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = _spawnPoint.position;
        transform.rotation = _spawnPoint.rotation;

        Debug.Log(transform.name + " respawned");
    }

    public void SetDefaults()
    {
        isDead = false;

        currentHealth = maxHealth;

        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = wasEnabled[i];

            Collider _col = GetComponent<Collider>();
            if (_col != null)
                _col.enabled = true;
            
        }
    }

}
