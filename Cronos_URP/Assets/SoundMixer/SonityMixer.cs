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


    void Start()
    {
        //masterSlider.onValueChanged.AddListener(x => audioMixer.SetFloat("MasterVolume", AdjustVolume(x)));
        //bgmSlider.onValueChanged.AddListener(x => audioMixer.SetFloat("BgmVolume", AdjustVolume(x)));
        //sfxSlider.onValueChanged.AddListener(x => audioMixer.SetFloat("SfxVolume", AdjustVolume(x)));
    }

    // 빌드 중에는 업데이트에서 리소스를 부르는 것도 괜찮다
    void Update()
    {
        
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

}
