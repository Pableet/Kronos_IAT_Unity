using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class MonsterSelector : MonoBehaviour
{

    public Transform Monsetr0;
    public Transform Monsetr1;
    public Transform Monsetr2;
    public Transform Monsetr3;

    List<Transform> monsters;

    // 오토 타겟에 그걸 해주자
    public AutoTargetting autoTargetor;

    // Start is called before the first frame update
    void Start()
    {
        // 일단 담아
        monsters.Add(Monsetr0);
        monsters.Add(Monsetr1);
        monsters.Add(Monsetr2);
        monsters.Add(Monsetr3);
    }

    // Update is called once per frame
    void Update()
    {
        Transform _temp;

        // 제일 가까운 애를 찾는다
        for (int i = 0; i < monsters.Count; i++) 
        {
            Vector3 min = autoTargetor.PlayerObject.position - monsters[i].position;
            // 몬스터사이의 벡터를 구하고 크기를 비교해서 제일 작은 벡터를 찾아서 넣어주자
            
            // 시야범위를 제한 하면 좋을 지도 지금은 불가능 머리가 안돌아서
        }

        // 오토타겟에 넣어준다
        //autoTargetor.Target = _temp;


    }
}
