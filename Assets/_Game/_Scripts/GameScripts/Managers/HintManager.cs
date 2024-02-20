using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintManager : MonoSingleton<HintManager>
{
    [SerializeField]
    private Color32 hintColor;
    
    [SerializeField] 
    private float hintDuration;
    
    [SerializeField] 
    private Ease hintEase;
    
    Sequence hintRoutine;
    private void OnEnable()
    {
        ObjectConnector.OnStartConnecting += DisableHint;
        GridGenerator.OnGridGenerated += FirstHintCheck;
    }
    private void OnDisable()
    {
        ObjectConnector.OnStartConnecting -= DisableHint;
        GridGenerator.OnGridGenerated -= FirstHintCheck;
    }
    public void FirstHintCheck()
    {
        if (LevelManager.Instance.FirstStart)
        {
            GetFirstPossibleMatch();
            LevelManager.Instance.FirstStart = false;
        }
    }
    [Button]
    public void GetFirstPossibleMatch()
    {
        Dictionary<int, List<Tile>> rows = GridGenerator.Instance.grid.RowInfo;

        for (int i = rows.Count - 1; i >= 0; i--)
        {
            for (int j = 0; j < rows[i].Count; j++)
            {
                SpriteInfo spriteToSeekMatch = rows[i][j].FillingSprite;

                // If this tile has no sprite or it's sprite is inactive
                if (spriteToSeekMatch == null || !spriteToSeekMatch.gameObject.activeInHierarchy)
                {
                    continue;
                }

                // Check top adjacent tile
                if (i < rows.Count - 1)
                {
                    Tile topTile = rows[i + 1][j];
                    if (topTile.FillingSprite != null && topTile.FillingSprite.gameObject.activeInHierarchy)
                    {
                        if(CheckMatch(spriteToSeekMatch, topTile.FillingSprite))
                        {
                            return;
                        }
                    }
                }

                // Check bottom adjacent tile
                if (i > 0)
                {
                    Tile bottomTile = rows[i - 1][j];
                    if (bottomTile.FillingSprite != null && bottomTile.FillingSprite.gameObject.activeInHierarchy)
                    {
                        if (CheckMatch(spriteToSeekMatch, bottomTile.FillingSprite))
                        {
                            return;
                        }
                    }
                }

                // Check left adjacent tile
                if (j > 0)
                {
                    Tile leftTile = rows[i][j - 1];
                    if (leftTile.FillingSprite != null && leftTile.FillingSprite.gameObject.activeInHierarchy)
                    {
                        if (CheckMatch(spriteToSeekMatch, leftTile.FillingSprite))
                        {
                            return;
                        }
                    }
                }

                // Check right adjacent tile
                if (j < rows[i].Count - 1)
                {
                    Tile rightTile = rows[i][j + 1];
                    if (rightTile.FillingSprite != null && rightTile.FillingSprite.gameObject.activeInHierarchy)
                    {
                        if (CheckMatch(spriteToSeekMatch, rightTile.FillingSprite))
                        {
                            return;
                        }
                    }
                }

                // Check top-left corner tile
                if (i < rows.Count - 1 && j > 0)
                {
                    Tile topLeftCornerTile = rows[i + 1][j - 1];
                    if (topLeftCornerTile.FillingSprite != null && topLeftCornerTile.FillingSprite.gameObject.activeInHierarchy)
                    {
                        if (CheckMatch(spriteToSeekMatch, topLeftCornerTile.FillingSprite))
                        {
                            return;
                        }
                    }
                }

                // Check top-right corner tile
                if (i < rows.Count - 1 && j < rows[i].Count - 1)
                {
                    Tile topRightCornerTile = rows[i + 1][j + 1];
                    if (topRightCornerTile.FillingSprite != null && topRightCornerTile.FillingSprite.gameObject.activeInHierarchy)
                    {
                        if (CheckMatch(spriteToSeekMatch, topRightCornerTile.FillingSprite))
                        {
                            return;
                        }
                    }
                }

                // Check bottom-left corner tile
                if (i > 0 && j > 0)
                {
                    Tile bottomLeftCornerTile = rows[i - 1][j - 1];
                    if (bottomLeftCornerTile.FillingSprite != null && bottomLeftCornerTile.FillingSprite.gameObject.activeInHierarchy)
                    {
                        if (CheckMatch(spriteToSeekMatch, bottomLeftCornerTile.FillingSprite))
                        {
                            return;
                        }
                    }
                }

                // Check bottom-right corner tile
                if (i > 0 && j < rows[i].Count - 1)
                {
                    Tile bottomRightCornerTile = rows[i - 1][j + 1];
                    if (bottomRightCornerTile.FillingSprite != null && bottomRightCornerTile.FillingSprite.gameObject.activeInHierarchy)
                    {
                        if (CheckMatch(spriteToSeekMatch, bottomRightCornerTile.FillingSprite))
                        {
                            return;
                        }
                    }
                }
            }
        }
    }

    public bool CheckMatch(SpriteInfo sprite1, SpriteInfo sprite2)
    {
        if(sprite1.header == sprite2.header)
        {
            Debug.Log($"{sprite1.name} has match with {sprite2.name}");
            GiveMatchHint(sprite1, sprite2);
            return true;
        }
        return false;
    }
    public void GiveMatchHint(SpriteInfo sprite1, SpriteInfo sprite2)
    {
        hintRoutine = HintRoutine(sprite1, sprite2);
        if(hintRoutine != null)
        {
            hintRoutine.Play();
        }
    }
    Sequence HintRoutine(SpriteInfo sprite1, SpriteInfo sprite2)
    {
        SpriteRenderer s1R = sprite1.GetComponent<SpriteRenderer>();
        SpriteRenderer s2R = sprite2.GetComponent<SpriteRenderer>();
        Sequence seq = DOTween.Sequence();
        seq.Append(s1R.DOColor(hintColor, hintDuration).SetLoops(-1, LoopType.Yoyo).SetEase(hintEase))
            .Join(s2R.DOColor(hintColor, hintDuration).SetLoops(-1, LoopType.Yoyo).SetEase(hintEase))
            .OnKill(() => ResetColors(s1R, s2R));
        return seq;
    }
    void ResetColors(SpriteRenderer s1R, SpriteRenderer s2R)
    {
        s1R.color = Color.white;
        s2R.color = Color.white;
    }

    [Button]
    public void DisableHint()
    {
        if (hintRoutine != null)
        {
            hintRoutine.Kill();
        }
    }
}
