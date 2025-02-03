using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Loader<SoundManager>
{

    [SerializeField]
    AudioClip level;
    public AudioClip Level
    {
        get
        {
            return level;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
