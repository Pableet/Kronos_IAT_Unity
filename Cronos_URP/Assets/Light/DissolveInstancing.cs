using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class DissolveInstancing : MonoBehaviour
{
    public float delaySec = 2.0f;

    [SerializeField]
    private float dissolveTime = 0f;

    public Material mat;
    int dissolveAmount = Shader.PropertyToID("_DissolveAmount");

    private void Start()
    {
        mat = gameObject.GetComponent<Renderer>().material;
        DoVanish();
    }

    private void Update()
    {

    }

    public void DoVanish()
    {
        StartCoroutine(Vanish(delaySec));
    }

    IEnumerator Vanish(float delay)
    {
        yield return new WaitForSeconds(delay);

        float elapsedTime = 0.0f;
        while (elapsedTime < dissolveTime)
        {
            elapsedTime += Time.deltaTime;

            float lerpDissolve = Mathf.Lerp(0, 1.1f, (elapsedTime / dissolveTime));
            mat.SetFloat(dissolveAmount, lerpDissolve);
            Debug.Log("마테리얼 디졻므");

            yield return null;
        }

        //gameObject.SetActive(false);
    }
}
