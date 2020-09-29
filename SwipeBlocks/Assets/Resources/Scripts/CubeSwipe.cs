using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CubeSwipe : MonoBehaviour
{
    public static CubeSwipe _cubeSwipeInst;
    [SerializeField] private GameObject colorMainBlock;
    [SerializeField] private GameObject[] tmp_getColorField;
    [SerializeField] private List<GameObject> tmp_blockField;
    private List<GameObject> toColorFieldBlocks;
    private List<GameObject> tmp_toColorFieldBlocks;

    private Vector2 blockPos;
    private Vector2 fingerDown;
    private Vector2 fingerUp;

    [SerializeField] private int colorPointCount;

    private float SWIPE_THRESHOLD = 200f;
    private float distBetweenFieldBlock = 1.1f;
    private float speed = 200f;
    private float lastTime;

    private Rigidbody2D _rb;

    private Color blockFieldColor;

    [SerializeField] private bool leftMove;
    [SerializeField] private bool rightMove;
    [SerializeField] private bool upMove;
    [SerializeField] private bool downMove;
    private readonly bool detectSwipeOnlyAfterRelease;
    private bool moveAllowed;
    private bool checkToColor;
    private bool addToColorArray;

    public GameObject MainBlock { get; set; }
    public GameObject ColorMainBlock { get => colorMainBlock; set => colorMainBlock = value; }

    Vector2 startPos;

    private void Awake()
    {
        _cubeSwipeInst = this;
    }

    void Start()
    {
        toColorFieldBlocks = new List<GameObject>();
        tmp_toColorFieldBlocks = new List<GameObject>();
        startPos = transform.position;
        MainBlock = gameObject;
        blockPos = transform.position;
        _rb = gameObject.GetComponent<Rigidbody2D>();
        CheckToMove();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CheckToColor();
        Swipe();
    }

    Vector3[] GetBlockPos(Vector3 _neighBlockPos)
    {
        Vector3[] _posBlockPoint = new Vector3[4];
        _posBlockPoint[0] = _neighBlockPos; _posBlockPoint[0].x += distBetweenFieldBlock;
        _posBlockPoint[1] = _neighBlockPos; _posBlockPoint[1].x -= distBetweenFieldBlock;
        _posBlockPoint[2] = _neighBlockPos; _posBlockPoint[2].y += distBetweenFieldBlock;
        _posBlockPoint[3] = _neighBlockPos; _posBlockPoint[3].y -= distBetweenFieldBlock;
        return _posBlockPoint;
    }

    public void Swipe()
    {
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                fingerUp = touch.position;
                fingerDown = touch.position;
            }

            if (touch.phase == TouchPhase.Moved)
            {
                if (!detectSwipeOnlyAfterRelease)
                {
                    moveAllowed = true;
                    fingerDown = touch.position;
                }
            }

            if (touch.phase == TouchPhase.Ended && moveAllowed)
            {
                fingerDown = touch.position;

                if (verticalMove() > SWIPE_THRESHOLD && verticalMove() > horizontalValMove())
                {
                    if (fingerDown.y - fingerUp.y > 0 && upMove)
                    {
                        startPos = transform.position;
                        _rb.velocity = new Vector2(0f, speed * Time.fixedDeltaTime);
                        blockPos = transform.position;
                    }
                    else if (fingerDown.y - fingerUp.y < 0 && downMove)
                    {
                        startPos = transform.position;
                        _rb.velocity = new Vector2(0f, -speed * Time.fixedDeltaTime);
                        blockPos = transform.position;
                    }
                    fingerUp = fingerDown;
                }

                else if (horizontalValMove() > SWIPE_THRESHOLD && horizontalValMove() > verticalMove())
                {
                    if (fingerDown.x - fingerUp.x > 0 && rightMove)
                    {
                        startPos = transform.position;
                        _rb.velocity = new Vector2(speed * Time.fixedDeltaTime, 0f);
                        blockPos = transform.position;
                    }
                    else if (fingerDown.x - fingerUp.x < 0 && leftMove)
                    {
                        startPos = transform.position;
                        _rb.velocity = new Vector2(-speed * Time.fixedDeltaTime, 0f);
                        blockPos = transform.position;
                    }
                    fingerUp = fingerDown;
                }
                moveAllowed = false;
            }

            float verticalMove()
            {
                return Mathf.Abs(fingerDown.y - fingerUp.y);
            }
            float horizontalValMove()
            {
                return Mathf.Abs(fingerDown.x - fingerUp.x);
            }
        }
    }

    private void CheckToMove()
    {
        Color mainBlockColor = ColorMainBlock.GetComponent<SpriteRenderer>().color;
        Vector3[] _neighPos = GetBlockPos(blockPos);

        Transform tmp_blockFieldChild;
        foreach (var blockField in tmp_blockField)
        {
            tmp_blockFieldChild = blockField.transform.Find("FieldBlockColor");
            Vector3 pos_blockField = blockField.transform.position;
            foreach (Vector3 __neighPos in _neighPos)
            {
                if (__neighPos == pos_blockField && tmp_blockFieldChild.GetComponent<SpriteRenderer>().color == Color.white)
                {
                    if (__neighPos.x > blockPos.x)
                        rightMove = true;
                    if (__neighPos.x < blockPos.x)
                        leftMove = true;
                    if (__neighPos.y > blockPos.y)
                        upMove = true;
                    if (__neighPos.y < blockPos.y)
                        downMove = true;
                    break;
                }
            }

            if (blockField.transform.position.x == transform.position.x && blockField.transform.position.y == transform.position.y)
            {
                if (addToColorArray)
                    toColorFieldBlocks.Add(blockField); // запись окрашиваемых блоков в массив начата

                if (addToColorArray && colorPointCount == 2)
                    addToColorArray = false; // запись окрашиваемых блоков в массив завершена

                if (tmp_blockFieldChild)
                    tmp_blockFieldChild.GetComponent<SpriteRenderer>().color = mainBlockColor;
            }

            if (colorPointCount == 2)
                StartCoroutine("ChangeColorAnim");// старт анимации окрашивания
        }
    }

    private void GetColorField()
    {
        foreach (var colorField in tmp_getColorField)
        {
            if (colorField.transform.position.x == transform.position.x && colorField.transform.position.y == transform.position.y)
            {
                addToColorArray = true; // запись окрашиваемых блоков в массив начата

                if (colorPointCount == 0)
                    colorPointCount = 1;
                else if (colorField.GetComponent<SpriteRenderer>().color == ColorMainBlock.GetComponent<SpriteRenderer>().color)
                    colorPointCount = 2;

                if (colorPointCount == 1 || colorPointCount == 2)
                {
                    if (colorField.GetComponent<SpriteRenderer>().color != ColorMainBlock.GetComponent<SpriteRenderer>().color)
                    {
                        toColorFieldBlocks.RemoveRange(0, toColorFieldBlocks.Count);
                        colorPointCount = 1;
                    }
                }

                if (colorPointCount == 2)
                    TaskManager._taskManagerInst.CountToCheckWin--;

                ColorMainBlock.GetComponent<SpriteRenderer>().color = colorField.GetComponent<SpriteRenderer>().color;
            }
        }
    }

    public void CheckToColor()
    {
        if (transform.position.y - startPos.y > 1.1f)
        {
            transform.position = new Vector3(transform.position.x, startPos.y + 1.1f, -2f);
            checkToColor = true;
        }

        if (transform.position.y - startPos.y < -1.1f)
        {
            transform.position = new Vector3(transform.position.x, startPos.y - 1.1f, -2f);
            checkToColor = true;
        }

        if (transform.position.x - startPos.x > 1.1f)
        {
            transform.position = new Vector3(startPos.x + 1.1f, transform.position.y, -2f);
            checkToColor = true;
        }

        if (transform.position.x - startPos.x < -1.1f)
        {
            transform.position = new Vector3(startPos.x - 1.1f, transform.position.y, -2f);
            checkToColor = true;
        }

        if (checkToColor)
        {
            _rb.velocity = new Vector2(0f, 0f);
            blockPos = transform.position;
            DoColor();
            checkToColor = false;
        }
    }

    private void DoColor()
    {
        ResetPos();
        GetColorField();
        CheckToMove();
    }

    private void ResetPos()
    {
        leftMove = false;
        rightMove = false;
        upMove = false;
        downMove = false;
    }

    IEnumerator ChangeColorAnim()
    {
        yield return new WaitForSeconds(0.01f);
        ColorMainBlock.GetComponent<SpriteRenderer>().color = Color.white;

        for (int i = 0; i < toColorFieldBlocks.Count; i++)
        {
            tmp_toColorFieldBlocks.Add(toColorFieldBlocks[i]);// передача окрашиваемых блоков в tmp массив
        }
        toColorFieldBlocks.RemoveRange(0, toColorFieldBlocks.Count);// очистка массива с окрашиваемыми блоками

        for (int i = 0; i < tmp_toColorFieldBlocks.Count; i++)
        {
            BlockFieldColor BFC = tmp_toColorFieldBlocks[i].GetComponent<BlockFieldColor>();// получение скрипта на блоке
            Transform childBlockField = tmp_toColorFieldBlocks[i].transform.Find("FieldBlockColor");// получение child'a блока
            yield return new WaitForSeconds(0.15f);
            tmp_toColorFieldBlocks[i].GetComponent<SpriteRenderer>().color = childBlockField.GetComponent<SpriteRenderer>().color;// окрашивание блока
            BFC.ChangeAnimColor();// анимация блока

            if(tmp_toColorFieldBlocks.Count - i == 1 && TaskManager._taskManagerInst.CheckToWin)
            {
                TaskManager._taskManagerInst.ChangeLevel();
            }
        }
        tmp_toColorFieldBlocks.RemoveRange(0, tmp_toColorFieldBlocks.Count);// очистка tmp массива с окрашенными блоками
    }
}

