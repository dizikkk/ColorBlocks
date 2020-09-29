using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorManager : MonoBehaviour
{
    public static ColorManager _colorManagerInst;
    [SerializeField] private Color[] colorArray;

    public Color[] ColorArray { get => colorArray; set => colorArray = value; }

    // Start is called before the first frame update
    void Start()
    {
        _colorManagerInst = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
