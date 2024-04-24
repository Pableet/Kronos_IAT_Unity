using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    static EffectManager instance;
    static EffectManager Instance {  get { Initialize(); return instance; } }

    static GameObject trail;

    // 이펙트를 로드하는 단계?
    private void Awake()
    {
        LoadEffect();
    }

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        instance = null;
    }

    static void Initialize()
    {
        if (instance == null)
        {
            GameObject gameObject = GameObject.Find("EffectManager");
            if (gameObject == null)
            {
                gameObject = new GameObject { name = "EffectManager" };
                gameObject.AddComponent<EffectManager>();
            }

            DontDestroyOnLoad(gameObject);
            instance = gameObject.GetComponent<EffectManager>();
        }
    }

    static void LoadEffect()
    {
        GameObject trailFX = Resources.Load<GameObject>("FX/Trail_Prac");
        trail = Instantiate(trailFX);
        trail.SetActive(false);
    }


    public void PlayerSlash()
    {
        Debug.Log("FX 매니저의 훅 슬래시");

        GameObject player = GameObject.Find("unitychan");
        GameObject playerArm = FindChild(player, "Character1_RightArm");
        trail.transform.position = playerArm.transform.TransformPoint(Vector3.zero);
        trail.transform.rotation = Quaternion.Euler(0f, playerArm.transform.rotation.eulerAngles.y + 40f, 0f);
        trail.SetActive(true);
        Invoke("DestroyFX", 1.0f);
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

    void DestroyFX()
    {
        trail.SetActive(false);
    }
}
