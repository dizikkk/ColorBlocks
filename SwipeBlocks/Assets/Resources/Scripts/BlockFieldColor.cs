using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockFieldColor : MonoBehaviour
{

    [SerializeField] private Color colorFieldBlock;
    public bool[] selectColor;
    private bool checkToCount;
    private bool checkToSetColor;

    private Animator Anim;

    // Start is called before the first frame update
    void Start()
    {
        Anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (CubeSwipe._cubeSwipeInst.MainBlock.transform.position.x == transform.position.x && CubeSwipe._cubeSwipeInst.MainBlock.transform.position.y == transform.position.y)
        {
            if (!checkToSetColor && CubeSwipe._cubeSwipeInst.ColorMainBlock.GetComponent<SpriteRenderer>().color != Color.white)
            {
                colorFieldBlock = CubeSwipe._cubeSwipeInst.ColorMainBlock.GetComponent<SpriteRenderer>().color;
                checkToSetColor = true;
            }
            CheckColor();
        }
        else
            checkToCount = false;
    }

    public void CheckColor()
    {
        if (!checkToCount)
        {
            if (colorFieldBlock == ColorManager._colorManagerInst.ColorArray[0] && selectColor[0])
                TaskManager._taskManagerInst.CountToWin--;
            if (colorFieldBlock == ColorManager._colorManagerInst.ColorArray[1] && selectColor[1])
                TaskManager._taskManagerInst.CountToWin--;
            if (colorFieldBlock == ColorManager._colorManagerInst.ColorArray[2] && selectColor[2])
                TaskManager._taskManagerInst.CountToWin--;
            checkToCount = true;
        }
    }

    public void ChangeAnimColor()
    {
        Anim.SetBool("isAnim", true);
    }
}

