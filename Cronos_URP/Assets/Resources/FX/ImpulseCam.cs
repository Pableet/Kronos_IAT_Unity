using UnityEngine;
using Cinemachine;

public class ImpulseCam : MonoBehaviour
{
    private static ImpulseCam instance;
    public static ImpulseCam Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ImpulseCam>();
                if (instance == null)
                {
                    GameObject ic = new GameObject(typeof(ImpulseCam).Name);
                    instance = ic.AddComponent<ImpulseCam>();

                    DontDestroyOnLoad(ic);
                }
            }
            return instance;
        }
    }

    CinemachineImpulseSource impulse;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        impulse = transform.GetComponent<CinemachineImpulseSource>();
    }

    public void Shake()
    {
        impulse.GenerateImpulse(1f);
    }
    public void Shake(float pow)
    {
        impulse.GenerateImpulse(pow);
    }
}
