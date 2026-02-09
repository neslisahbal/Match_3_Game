using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class MatchFinder : MonoBehaviour
{
    private Board board;
    public List<Gem> currentMatches = new List<Gem>();


    private void Awake()
    {
        board = FindObjectOfType<Board>();
    }

    public void FindAllMatches()
    {
        //currentMatches.Clear();
        
        for(int x = 0; x < board.width; x++)
        {
            for(int y = 0; y < board.height; y++)
            {
                Gem currentGem = board.allGems[x,y];
                if(currentGem != null)
                {
                    // Horizontal match kontrolü
                    if(x>0 && x<board.width-1)
                    {
                        Gem leftGem = board.allGems[x-1,y];
                        Gem rightGem = board.allGems[x+1,y];

                        if(leftGem != null && rightGem != null)
                        {
                            if(leftGem.type == currentGem.type && rightGem.type == currentGem.type)
                            {
                                currentGem.isMatched = true;
                                leftGem.isMatched = true;
                                rightGem.isMatched = true;

                                currentMatches.Add(currentGem);
                                currentMatches.Add(leftGem);
                                currentMatches.Add(rightGem);
                            }
                        }
                    }

                    // Vertical match kontrolü
                    if(y>0 && y<board.height-1)
                    {
                        Gem downGem = board.allGems[x,y-1];
                        Gem upGem = board.allGems[x,y+1];

                        if(downGem != null && upGem != null)
                        {
                            if(downGem.type == currentGem.type && upGem.type == currentGem.type)
                            {
                                currentGem.isMatched = true;
                                downGem.isMatched = true;
                                upGem.isMatched = true;

                                currentMatches.Add(currentGem);
                                currentMatches.Add(downGem);
                                currentMatches.Add(upGem);
                            }
                        }
                    }
                }

            }
        }

        if(currentMatches.Count > 0)
        {
            currentMatches = currentMatches.Distinct().ToList();
        }
    }
   
}
