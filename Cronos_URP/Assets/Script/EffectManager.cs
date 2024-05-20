using System;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    private static EffectManager instance;

    public static EffectManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<EffectManager>();
                if (instance == null)
                {
                    GameObject effectManager = new GameObject(typeof(EffectManager).Name);
                    instance = effectManager.AddComponent<EffectManager>();

                    DontDestroyOnLoad(effectManager);
                }
            }
            return instance;
        }
    }

    GameObject player;
    GameObject trail;
    GameObject burst;
    GameObject smoke;
    GameObject skull;
    static List<GameObject> effects;

    Vector3 EdestPos;

    // 이펙트를 로드하는 단계
    protected void Awake()
    {
        // 이미 인스턴스가 존재한다면
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad (gameObject);
        }

        Debug.Log("Effect Manager 활성화");
        LoadEffect();
    }

    // 로드한 이펙트에 게임 오브젝트 할당
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
//         UpdateWpos();
//         UpdateEpos();
    }

    void Initialize()
    {
        trail = FindName("Trail_Prac");
        burst = FindName("Flame_Circle");
        smoke = FindName("Explosion");
        skull = FindName("ProjectileE");
        player = GameObject.Find("Player");
    }

    void LoadEffect()
    {
        GameObject[] prefabs = Resources.LoadAll<GameObject>("FX");
        effects = new List<GameObject>();
        foreach (GameObject effect in prefabs)
        {
            GameObject effectInstance = Instantiate(effect);
            effectInstance.name = effect.name;
            effects.Add(effectInstance);
            effectInstance.SetActive(false);
        }
    }


    public void PlayerSlash()
    {
        Debug.Log("FX 매니저의 훅 슬래시");
        GameObject playerArm = FindChild(player, "Character1_RightArm");
        trail.transform.position = playerArm.transform.TransformPoint(Vector3.zero);
        trail.transform.rotation = Quaternion.Euler(0f, playerArm.transform.rotation.eulerAngles.y - 50.0f, 0f);
        trail.SetActive(true);
        Invoke("DestroyQ", 1.0f);
    }

    public void PlayerW()
    {
        Debug.Log("FX 매니저의 W 공격");
        burst.SetActive(true);
    }

    public void PlayerW2()
    {
        smoke.transform.position = player.transform.position;
        smoke.SetActive(true);
        Invoke("DestroyW2", 1.0f);
    }

    public void PlayerE()
    {
        Debug.Log("FX 매니저의 E 공격");
        skull.SetActive(true);
        skull.transform.position = player.transform.position + Vector3.up * 2.0f;
    }

    public void TurnE()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Debug.DrawRay(Camera.main.transform.position, ray.direction * 100.0f, Color.red, 1.0f);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100.0f))
        {
            EdestPos = new Vector3(hit.point.x, transform.position.y, hit.point.z);
            Vector3 dir = EdestPos - player.transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(dir.x, 0f, dir.z));
            player.transform.rotation = Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f);
            skull.transform.rotation = Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f);
        }
    }

//     void UpdateWpos()
//     {
//         burst.transform.position = player.transform.position;
//     }
// 
// 
//     void UpdateEpos()
//     {
//         if (skull.activeSelf == false)
//         {
//             skull.transform.position = player.transform.position + Vector3.up * 2.0f;
//             return;
//         }
// 
//         Vector3 dir = EdestPos - skull.transform.position;
//         dir.y = 0f;
//         float speed = 15.0f;
// 
//         float moveDist = Mathf.Clamp(speed * Time.deltaTime, 0, dir.magnitude);
// 
//         if (dir.magnitude < 0.001f)
//         {
//             skull.SetActive(false);
//             skull.transform.position = player.transform.position + Vector3.up * 2.0f;
//         }
//         else
//         {
//             Vector3 newPosition = skull.transform.position + new Vector3(dir.x, 0f, dir.z).normalized * moveDist;
//             skull.transform.position = newPosition;
//         }
//     }

    GameObject FindName(string name)
    {
        foreach(GameObject effect in effects)
        {
            if (effect.name == name)
                return effect;
        }
        return null;
    }

    GameObject FindChild(GameObject parent, string name)
    {
        GameObject result = GameObject.Find(name);
        if (result != null)
            return result;
        foreach(GameObject gameObject in parent.GetComponentsInChildren<GameObject>())
        {
            result = FindChild(gameObject, name);
            if (result != null)
                return result;
        }
        return null;
    }

    void DestroyQ()
    {
        trail.SetActive(false);
    }

    public void DestroyW()
    {
        burst.SetActive(false);
    }

    void DestroyW2()
    {
        smoke.SetActive(false);
    }

    void DestroyE()
    {
        skull.SetActive(false);
    }
}
