using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Material sunsetSkyMaterial;

    void Start()
    {
        RenderSettings.skybox = sunsetSkyMaterial;
    }
    // Update is called once per frame
    void Update()
    {

    }
}
