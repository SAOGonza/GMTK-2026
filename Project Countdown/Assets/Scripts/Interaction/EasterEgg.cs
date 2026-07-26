using UnityEngine;

public class EasterEgg : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Game.Eggs++;
    }

    public void OnInteract()
    {
        Game.Eggs--;
        if (Game.Eggs <= 0)
        {
            Goober.Instance.gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
