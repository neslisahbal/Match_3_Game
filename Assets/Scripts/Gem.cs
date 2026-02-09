using UnityEngine;
using System.Collections;

public class Gem : MonoBehaviour
{
    [HideInInspector]
    public Vector2Int posIndex;
    [HideInInspector]
    public Board board;

    private Vector2Int firstTouchPosition;
    private Vector2Int finalTouchPosition;

    private bool mousePressed;
    private float swipeAngle = 0;

    private Gem otherGem;

    public enum GemType { blue, green, red, yellow, purple}
    public GemType type;

    public bool isMatched;

    private Vector2Int previousPos;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector2.Distance(transform.position, posIndex) > 0.01f)
        {
            transform.position = Vector2.Lerp(transform.position, posIndex, board.gemSpeed*Time.deltaTime);

        }
        else{
            transform.position = new Vector3(posIndex.x, posIndex.y, 0f);
            board.allGems[posIndex.x, posIndex.y] = this;
        }
        


        if (mousePressed && Input.GetMouseButtonUp(0))
        {
            mousePressed = false;
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            finalTouchPosition = new Vector2Int(Mathf.RoundToInt(mousePos.x), Mathf.RoundToInt(mousePos.y));
            CalculateAngle();
            
        }
        
    }

    private void OnMouseDown()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        firstTouchPosition = new Vector2Int(Mathf.RoundToInt(mousePos.x), Mathf.RoundToInt(mousePos.y));
        mousePressed = true;
        
    }

    public void SetupGem(Vector2Int pos, Board theBoard)
    {
        posIndex = pos;
        board = theBoard;
    }

    private void CalculateAngle()
    {
        swipeAngle = Mathf.Atan2(finalTouchPosition.y - firstTouchPosition.y, finalTouchPosition.x - firstTouchPosition.x);
        swipeAngle = swipeAngle * 180 / Mathf.PI;
        Debug.Log(swipeAngle);

        if(Vector2.Distance((Vector2)firstTouchPosition, (Vector2)finalTouchPosition) > 0.5f)
        {
            MovePieces();
        }
    }

    private void MovePieces()
    {
        previousPos = posIndex;
        // Sağa swipe
        if(swipeAngle < 45 && swipeAngle > -45 && posIndex.x < board.width - 1)
        {
            otherGem = board.allGems[posIndex.x + 1, posIndex.y];
            otherGem.posIndex.x--;
            posIndex.x++;
        }
        // Yukarı swipe
        else if(swipeAngle >= 45 && swipeAngle < 135 && posIndex.y < board.height - 1)
        {
            otherGem = board.allGems[posIndex.x, posIndex.y + 1];
            otherGem.posIndex.y--;
            posIndex.y++;
        }
        // Sola swipe
        else if((swipeAngle >= 135 || swipeAngle < -135) && posIndex.x > 0)
        {
            otherGem = board.allGems[posIndex.x - 1, posIndex.y];
            otherGem.posIndex.x++;
            posIndex.x--;
        }
        // Aşağı swipe
        else if(swipeAngle >= -135 && swipeAngle < -45 && posIndex.y > 0)
        {
            otherGem = board.allGems[posIndex.x, posIndex.y - 1];
            otherGem.posIndex.y++;
            posIndex.y--;
        }

        board.allGems[posIndex.x, posIndex.y] = this;
        board.allGems[otherGem.posIndex.x, otherGem.posIndex.y] = otherGem;

        StartCoroutine(CheckMoveCo());

    }

    public IEnumerator CheckMoveCo()
    {
        yield return new WaitForSeconds(.75f);
        board.matchFind.FindAllMatches();

        if(!isMatched && !otherGem.isMatched)
        {
            otherGem.posIndex = posIndex;
            posIndex = previousPos;

            board.allGems[posIndex.x, posIndex.y] = this;
            board.allGems[otherGem.posIndex.x, otherGem.posIndex.y] = otherGem;

        }

    }
}
