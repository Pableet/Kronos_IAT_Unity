using System;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    // 싱글턴
    private static EffectManager instance;
    // Get하는 프로퍼티
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

    [SerializeField]
    GameObject player;

    // 사용할 이펙트 리스트
    static List<GameObject> effects;


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
            DontDestroyOnLoad(gameObject);
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



    GameObject FindName(string name)
    {
        foreach (GameObject effect in effects)
        {
            if (effect.name == name)
                return effect;
        }
        return null;
    }

    // 부모 오브젝트에서 이름을 가진 자식 오브젝트를 리턴
    // 이펙트가 나올 자식 오브젝트 위치 찾는 데 사용하는 중
    GameObject FindChild(GameObject parent, string name)
    {
        GameObject result = GameObject.Find(name);
        if (result != null)
            return result;
        foreach (GameObject gameObject in parent.GetComponentsInChildren<GameObject>())
        {
            result = FindChild(gameObject, name);
            if (result != null)
                return result;
        }
        return null;
    }

    // 오브젝트의 SetActive를 false로 하는 것
    void TurnOffObject(GameObject obj)
    {
        obj.SetActive(false);
    }

    // 이건 씬에서 없애는 것 인스턴싱하고 지울 때
    void DestroyObject(GameObject obj)
    {
        Destroy(obj);
    }
}
