using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager _levelManagerInst;

    private GameObject curObjLevel;

    [SerializeField] private GameObject[] levels;

    public bool SwipeLevel { get; set; }
    [SerializeField] private int curLvl;

    // Start is called before the first frame update
    void Start()
    {
        _levelManagerInst = this;
        curObjLevel = Instantiate(levels[curLvl], levels[curLvl].transform.position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {

        if (SwipeLevel)
        {
            curObjLevel.SetActive(false);
            Destroy(curObjLevel);
            curLvl++;
            if (curLvl < levels.Length)
                curObjLevel = Instantiate(levels[curLvl], levels[curLvl].transform.position, Quaternion.identity);
            SwipeLevel = false;
        }

       if (curLvl < levels.Length)
            DoTweenManager._DoTweenManagerInst.HideFreeTrialText();
        else
            DoTweenManager._DoTweenManagerInst.ShowFreeTrialText();
    }

    public void RestartLevel()
    {
        Destroy(curObjLevel);
        curObjLevel = Instantiate(levels[curLvl], levels[curLvl].transform.position, Quaternion.identity);
    }
}
