using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class DoTweenManager : MonoBehaviour
{
    public static DoTweenManager _DoTweenManagerInst;

    [SerializeField] private RectTransform menu;
    [SerializeField] private RectTransform slideBtn;
    [SerializeField] private RectTransform retryBtn;
    [SerializeField] private RectTransform freeTrialTextRect;

    [SerializeField] private GameObject freeTrialTextGO;

    private float lastTime;

    private bool isMenuOpen;

    public GameObject FreeTrialTextGO { get => freeTrialTextGO; set => freeTrialTextGO = value; }

    // Start is called before the first frame update

    private void Awake()
    {
        _DoTweenManagerInst = this;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SlideMenu()
    {
        if (!isMenuOpen)
        {
            OpenMenu();
        }
        else
        {
            CloseMenu();
        }
    }

    public void Retry()
    {
        StartCoroutine("RetryCorrutine");
    }

    public void OpenMenu()
    {
        menu.DOAnchorPos(new Vector2(840f, -1000f), 0.5f);
        slideBtn.DORotate(new Vector3(0f, 0f, -90f), 0.5f);
        isMenuOpen = true;
    }

    public void CloseMenu()
    {
        menu.DOAnchorPos(new Vector2(540f, -1000f), 0.5f);
        slideBtn.DORotate(new Vector3(0f, 0f, 90f), 0.5f);
        isMenuOpen = false;
    }

    public void ShowFreeTrialText()
    {
        freeTrialTextGO.SetActive(true);
        freeTrialTextRect.DOAnchorPos(new Vector2(-200f, 0f), 1f);
    }

    public void HideFreeTrialText()
    {
        freeTrialTextGO.SetActive(false);
        freeTrialTextRect.DOAnchorPos(new Vector2(-200f, -1200f), 0f);
    }

    public void ToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    IEnumerator RetryCorrutine()
    {
        retryBtn.DORotate(new Vector3(0f, 0f, -720f), 1f);
        yield return new WaitForSeconds(0.7f);
        LevelManager._levelManagerInst.RestartLevel();
        CloseMenu();
    }
}
