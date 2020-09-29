using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    public static TaskManager _taskManagerInst;
    [SerializeField] private int countToCheckWin;
    [SerializeField] private int countToWin;

    public int CountToWin { get => countToWin; set => countToWin = value; }
    public int CountToCheckWin { get => countToCheckWin; set => countToCheckWin = value; }
    public bool CheckToWin { get => checkToWin; set => checkToWin = value; }

    [SerializeField] private bool checkToWin;
    // Start is called before the first frame update
    void Start()
    {
        _taskManagerInst = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (CountToCheckWin == 0 && countToWin == 0)
        {
            checkToWin = true;
            CountToCheckWin = -1;
        }
    }

    public void ChangeLevel()
    {
        StartCoroutine("StartToChangeLevel");
        checkToWin = false;
    }

    IEnumerator StartToChangeLevel()
    {
        yield return new WaitForSeconds(0.7f);
        LevelManager._levelManagerInst.SwipeLevel = true;
    }
}
