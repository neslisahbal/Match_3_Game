using UnityEngine;

public class RoundManager : MonoBehaviour
{
    public float roundTime = 60f;
    private UIManager uiManager;

    private bool endingRound = false;

    private Board board;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        uiManager = FindObjectOfType<UIManager>();
        board = FindObjectOfType<Board>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(roundTime > 0)
        {
            roundTime -= Time.deltaTime;

            if(roundTime <= 0)
            {
                roundTime = 0;

                endingRound = true;
            }
        }

        if(endingRound && board.currentState == Board.BoardState.move)
        {
            WinCheck();
            endingRound = false;
        }

        uiManager.timeText.text = roundTime.ToString("0.0") + "s";
        
    }

    private void WinCheck()
    {
        uiManager.roundOverScreen.SetActive(true);
    }
}
