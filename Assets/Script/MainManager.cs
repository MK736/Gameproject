using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    static public MainManager instance;

    // Player
    public int m_Php = 500;
    public int m_MaxPhp = 500;
    public int m_AtackPower = 1;
    //public Vector3 m_PlayerPosi = Vector3.zero;
    public bool isDead = false;
    private GameObject m_weapon;
    private BoxCollider m_Weaponcol;
    //public bool m_deth = false;


    // Enemy
    public int m_Ehp = 2;
    public int m_MaxEhp = 2;
    public int m_EAtackPower = 10;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

    }
    private void Start()
    {
        //Player.instance.transform.position = m_PlayerPosi;
        //m_weapon = GameObject.FindWithTag("weapon");
        //m_Weaponcol = m_weapon.GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        //m_PlayerPosi = Player.instance.transform.position;

    }

    // Player
    //public void WeaponColOn()
    //{
    //    m_Weaponcol.enabled = true;
    //}
    //public void WeaponColOff()
    //{
    //    m_Weaponcol.enabled = false;
    //}

    // 変数（データ）は行けるが関数実行（コライダーオンオフ）はできない模様
    public void TakeDamage(Enemy m_Enemy)
    {

        //m_PlayerAnimmator.SetTrigger("Hit");
        PlayerGage.instance.GaugeReduction(m_EAtackPower);


        m_Php -= m_EAtackPower;//m_BattleManager.HpDown(g_PlayerHP, m_Enemy.atackPower);


        m_Enemy.AtackEnd();
        if (m_Php == 0)
        {
            isDead = true;
            //m_deth = true;

        }
    }
}
