using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeManager : MonoSingleton<MergeManager>
{
    public static event Action<List<SpriteInfo>> OnSpritesMerged;
    public GameObject mergedGroupObject;
    public float moveDuration;
    public float delay;
    public Ease moveEase = Ease.Linear;
    public float red2WhiteDuration = 2f;
    public GameObject matchParticle;
    public GameObject mergeRewardObject;
    List<Tween> redToWhiteTweens = new();
    private void OnEnable()
    {
        ObjectConnector.OnStartConnecting += StopColorTweens;
    }
    private void OnDisable()
    {
        ObjectConnector.OnStartConnecting -= StopColorTweens;
    }
    void StopColorTweens()
    {
        foreach (var item in redToWhiteTweens)
        {
            if (item != null)
            {
                item.Kill();
            }
        }
    }
    public bool GetMergePossibility(List<SpriteInfo> connectedSprites)
    {
        string mainHeader = null;
        int counter = 0;
        while (String.IsNullOrEmpty(mainHeader))
        {
            mainHeader = connectedSprites[counter].header;
            counter++;
        }
        for (int i = 1; i < connectedSprites.Count; i++)
        {
            BackgroundInfo bgInfo = connectedSprites[i] as BackgroundInfo;
            if (bgInfo != null) continue;
            if (connectedSprites[i].header != mainHeader) return false;
        }
        return true;
    }
    public void HandleMergeRequest(ref List<SpriteInfo> connectedSprites)
    {
        GameManager.Instance.UpdateGameState(GameState.hold);
        if (connectedSprites.Count == 0)
        {
            GameManager.Instance.UpdateGameState(GameState.playing);
            return;
        }
        if (connectedSprites.Count == 1)
        {
            connectedSprites[0].UpdateBGColor(connectedSprites[0].bgDefaultColor);
            connectedSprites.Clear();
            GameManager.Instance.UpdateGameState(GameState.playing);
            return;
        }

        List<SpriteInfo> notBlankSprites = new();
        for (int i = 0; i < connectedSprites.Count; i++)
        {
            BackgroundInfo bg = connectedSprites[i] as BackgroundInfo;
            if (bg == null)
            {
                notBlankSprites.Add(connectedSprites[i]);
            }
            else
            {
                bg.UpdateBGColor(bg.bgDefaultColor);
            }
        }

        if (notBlankSprites.Count == 0) return;
        if (notBlankSprites.Count == 1)
        {
            GameManager.Instance.UpdateGameState(GameState.playing);
            connectedSprites[0].UpdateBGColor(connectedSprites[0].bgDefaultColor);
            connectedSprites.Clear();
            return;
        }

        if (GetMergePossibility(connectedSprites))
        {
            //GroupTheConnections(connectedSprites);
            Debug.Log("MERGED: " + connectedSprites.Count);
            StartCoroutine(MergeOneByOne(connectedSprites));
        }
        else
        {
            //HealthManager.Instance.Health -= 1;
            MoveManager.Instance.DecreaseMoveCount();
            GameManager.Instance.UpdateGameState(GameState.playing);
            foreach (SpriteInfo spriteInfo in connectedSprites)
            {
                spriteInfo.whiteBackground.color = Color.red;
                Tween r2w = spriteInfo.whiteBackground.DOColor(spriteInfo.bgDefaultColor, red2WhiteDuration).OnKill(() => spriteInfo.whiteBackground.color = spriteInfo.bgDefaultColor).SetEase(moveEase);
                redToWhiteTweens.Add(r2w);
                r2w.Play();
            }
        }
        connectedSprites.Clear();
    }
    IEnumerator MergeOneByOne(List<SpriteInfo> connectedSprites)
    {
        List<SpriteInfo> sprites = new();
        foreach (SpriteInfo spriteInfo in connectedSprites)
        {
            sprites.Add(spriteInfo);
            spriteInfo.UpdateBGColor(spriteInfo.bgDefaultColor);
        }
        for (int i = sprites.Count - 1; i >= 0; i--)
        {
            // Check if the last object is a BackgroundInfo
            if (sprites[i] is BackgroundInfo)
            {
                // Remove the last object if it is a BackgroundInfo
                sprites.RemoveAt(i);
            }
            else
            {
                // Break the loop if the last object is not a BackgroundInfo
                break;
            }
        }

        ImageSetData groupData = null;
        // Find the data header of the first merged image.
        // That will be the flag for our header of merged sprites.
        // We can assume the first sprite's header is our main header.
        // Any sprite with a different header shows us we made a miss-merge.
        foreach (ImageSetData data in ImageDataManager.Instance.ImageSetDatas)
        {
            if (data.Header == sprites[0].header)
            {
                groupData = data;
                break;
            }
        }
        if (groupData != null)
        {
            GameObject groupHolder = Instantiate(mergedGroupObject, Vector3.zero, Quaternion.identity);
            groupHolder.SetActive(false);
            GroupInfo groupInfo = groupHolder.GetComponent<GroupInfo>();
            List<SpriteInfo> notBlankSprites = new();
            for (int i = 0; i < sprites.Count; i++)
            {
                if (sprites[i] is BackgroundInfo)
                {
                    sprites[i].UpdateBGColor(sprites[i].bgDefaultColor);
                    continue;
                }
                notBlankSprites.Add(sprites[i]);
                AddSpriteToGroup(sprites[i], ref groupInfo);
            }
            Vector3 lastSpritePosition = Vector3.zero;
            Tile lastSpriteTile = null;
            for (int i = 0; i < sprites.Count; i++)
            {
                lastSpritePosition = sprites[i].transform.position;
                lastSpriteTile = sprites[i].myTile;

                if (i < sprites.Count - 1)
                {
                    lastSpritePosition = sprites[i + 1].transform.position;
                    lastSpriteTile = sprites[i + 1].myTile;
                    for (int j = i; j >= 0; j--)
                    {
                        BackgroundInfo bgObject = sprites[j] as BackgroundInfo;
                        if (bgObject == null)
                        {
                            if (j == i)
                            {
                                // There we will spawn gold.
                                // "If" has been inserted just to be
                                // sure we spawn the reward ONE TIME.

                                GameObject reward = Instantiate(mergeRewardObject, sprites[j].transform.position, Quaternion.identity);
                                reward.GetComponent<MergeRewardObject>().SetReward(notBlankSprites.Count);
                                reward.name = "GOLD REWARD";
                            }

                            // Move the sprite towards to the next merged sprite
                            // if it is not a backGround sprite.
                            sprites[j].transform.DOMove(lastSpritePosition, moveDuration)
                        .SetEase(moveEase);
                        }
                        else
                        {
                            Debug.Log("This is a BG.", bgObject.gameObject);

                        }
                    }
                    yield return new WaitForSeconds(moveDuration);
                }
                else
                {
                    for (int j = i; j >= 0; j--)
                    {
                        BackgroundInfo bgObject = sprites[j] as BackgroundInfo;
                        if (bgObject == null)
                        {
                            lastSpritePosition = sprites[j].transform.position;
                            lastSpriteTile = sprites[j].myTile;
                            break;
                        }
                    }
                }

                Instantiate(matchParticle, sprites[i].transform.position, Quaternion.identity);
                yield return new WaitForSeconds(delay);
            }


            int currentCount = groupInfo.connections.Count;
            int maxCount = groupData.maxSpawnAmount;

            groupInfo.counterText.text = $"{currentCount}/{maxCount}";
            groupInfo.header = groupData.Header;
            groupInfo.contentRenderer.sprite = groupData.setSymbol;
            //groupInfo.transform.position = lastSpritePosition;
            //groupHolder.SetActive(true);
            LevelGenerator.Instance.AlignSpriteToTile(groupInfo, lastSpriteTile);
            if (currentCount == maxCount)
            {
                //Player Completed a Set!
                groupInfo.myTile.FillingSprite = null;
                ImageDataManager.Instance.HandleFullSet(groupInfo);
            }
            OnSpritesMerged?.Invoke(notBlankSprites);
            GameManager.Instance.UpdateGameState(GameState.playing);
        }
    }
    public void GroupTheConnections(List<SpriteInfo> connectedSprites)
    {
        OnSpritesMerged?.Invoke(connectedSprites);

        ImageSetData groupData = null;
        foreach (ImageSetData data in ImageDataManager.Instance.ImageSetDatas)
        {
            if (data.Header == connectedSprites[0].header)
            {
                groupData = data;
                break;
            }
        }
        if (groupData != null)
        {
            GameObject groupHolder = Instantiate(mergedGroupObject, Vector3.zero, Quaternion.identity);
            GroupInfo groupInfo = groupHolder.GetComponent<GroupInfo>();
            Vector3 lastSpritePosition = Vector3.zero;
            /*foreach (SpriteInfo spriteInfo in connectedSprites)
            {
                AddSpriteToGroup(spriteInfo, ref groupInfo);
                lastSpritePosition = spriteInfo.transform.position;
            }*/
            for (int i = 0; i < connectedSprites.Count; i++)
            {
                lastSpritePosition = connectedSprites[i].transform.position;
                if (i < connectedSprites.Count - 1)
                {
                    lastSpritePosition = connectedSprites[i + 1].transform.position;
                }
                AddSpriteToGroup(connectedSprites[i], ref groupInfo);
            }
            int currentCount = groupInfo.connections.Count;
            int maxCount = groupData.maxSpawnAmount;

            groupInfo.counterText.text = $"{currentCount}/{maxCount}";
            groupInfo.header = groupData.Header;
            groupInfo.contentRenderer.sprite = groupData.setSymbol;
            groupInfo.transform.position = lastSpritePosition;

            if (currentCount == maxCount)
            {
                //Player Completed a Set!
                ImageDataManager.Instance.HandleFullSet(groupInfo);
            }
        }
    }
    public void AddSpriteToGroup(SpriteInfo spriteInfo, ref GroupInfo groupInfo)
    {
        spriteInfo.myTile.FillingSprite = null;

        GroupInfo previousGroup = spriteInfo as GroupInfo;
        if (previousGroup == null)
        {
            // If this spriteInfo is just a single image:
            groupInfo.connections.Add(spriteInfo);
        }
        else
        {
            // If one of the connections is a group:
            foreach (SpriteInfo groupMember in previousGroup.connections)
            {
                groupInfo.connections.Add(groupMember);
            }
        }
        spriteInfo.gameObject.SetActive(false);
    }
}
