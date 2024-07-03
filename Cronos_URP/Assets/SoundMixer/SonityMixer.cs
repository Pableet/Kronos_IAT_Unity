using UnityEngine;
using Sonity;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.Collections.Generic;

public class SonityMixer : MonoBehaviour
{
    // 볼륨 조절 관련
    [SerializeField]
    AudioMixer audioMixer;
    [SerializeField]
    Slider masterSlider;
    [SerializeField]
    Slider bgmSlider;
    [SerializeField]
    Slider sfxSlider;

    [SerializeField]
    Player player;
    [SerializeField]
    Transform camera;


    // 일단 BGM 리스트
    List<SoundEvent> BgmList = new List<SoundEvent>();
    List<SoundEvent> SfxList = new List<SoundEvent>();
    // Play를 id를 통해서 함수로 호출할거라면 딕셔너리가 괜찮을 것 같다
    Dictionary<int, SoundEvent> soundDictionary;

    // 캐싱한다면 여기서
    private void Awake()
    {
        LoadSE();
    }


    void Start()
    {
        //masterSlider.onValueChanged.AddListener(x => audioMixer.SetFloat("MasterVolume", AdjustVolume(x)));
        //bgmSlider.onValueChanged.AddListener(x => audioMixer.SetFloat("BgmVolume", AdjustVolume(x)));
        //sfxSlider.onValueChanged.AddListener(x => audioMixer.SetFloat("SfxVolume", AdjustVolume(x)));
    }

    // 빌드 중에는 업데이트에서 리소스를 부르는 것도 괜찮다
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            var se = Resources.Load<SoundEvent>("Sound/Bgm/Demo_SE");
            se.PlayMusic();
            Debug.Log("음악 재생함");
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            PlaySound("Demo_SE");
        }
    }

    // 리소스 폴더 내부의 SE를 한번에 로드
    void LoadSE()
    {
        SoundEvent[] bgms = Resources.LoadAll<SoundEvent>("Sound/Bgm");
        foreach (SoundEvent soundEvent in bgms)
        {
            // 리스트나 딕셔너리에 넣자
            BgmList.Add(soundEvent);
        }
        Debug.Log("리소스 폴더의 사운드 로드");
    }

    // 선형 슬라이더에 로그 페이더 값을 변형시키려면...
    public float AdjustVolume(float value)
    {
        // 최대값은 1, 최소값은 0.0001 -> 0이면 계산이 고장나고 볼륨이 올라감 
        float clampVal = Mathf.Clamp(value, 0.0001f, 1f);
        // 선형 보간값을 감쇠 보간값으로 변환
        float logVal = Mathf.Log10(clampVal) * 20f;
        return logVal;
    }

    void PlaySound(string name)
    {
        foreach (SoundEvent se in BgmList)
        {
            if (se.name == name)
                se.PlayMusic();
        }
    }
}
