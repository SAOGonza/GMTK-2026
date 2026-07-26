using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessingSetting : MonoBehaviour
{
    public static PostProcessingSetting Instance;

    public Volume PPVolume;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ApplySettings();
    }

    public void ApplySettings()
    {
        print("Applying Bloom Settings");
        if (PlayerPrefs.HasKey("Bloom"))
        {
            //Bloom bloomEffect;
            //if (PPVolume.profile.TryGet(out bloomEffect))
            //{
            //    print("Bloom is now " + (PlayerPrefs.GetInt("Bloom") == 1));
                //bloomEffect.active = PlayerPrefs.GetInt("Bloom") == 1;
            //}
            PPVolume.enabled = PlayerPrefs.GetInt("Bloom") == 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
