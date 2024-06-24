using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitStop : MonoBehaviour
{
    public float hitStopTime = 0.0f;
    public float hitStopScale = 0.0f;
    Animator animator;
    
	public bool isHit;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        isHit = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H) && isHit == false)
        {
            isHit = true;
            Debug.Log("Testing HitStop");
            StartCoroutine(HitStopCoroutine());
        }
    }

	// 진짜 미안한데 퍼블릭으로 바꿈
    public IEnumerator HitStopCoroutine()
    {
        animator.speed = hitStopScale;
        yield return new WaitForSecondsRealtime(hitStopTime);
        animator.speed = 1.0f;
        isHit = false;
    }
}
