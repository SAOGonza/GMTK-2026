using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FlickeringAlpha : MonoBehaviour
{
    [SerializeField] private Image Image;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(FlickerRoutine());
    }

    IEnumerator FlickerRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(0.5f, 5f));
            float flickerDuration = Random.Range(0.1f, 0.4f);
            while (flickerDuration > 0f)
            {
                Image.color = new Color(Image.color.r, Image.color.g, Image.color.b, Random.Range(0, 0.75f));
                flickerDuration -= Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            Image.color = new Color(Image.color.r, Image.color.g, Image.color.b, 1f);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
