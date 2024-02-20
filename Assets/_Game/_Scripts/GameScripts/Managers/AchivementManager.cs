using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AchivementManager : MonoSingleton<AchivementManager>
{
    public RectTransform BallHolder { get => BallHolder; }

    [Header("Set Achivement Settings")]
    public Transform uiParent;
    public AchivementData datas;
    public List<Achivement> activeAchivements = new();

    [Header("General Achivement Settings"), Space]
    public List<Achivement> generalAchivements = new();

    

    private void OnEnable()
    {
        MergeManager.OnSpritesMerged += HandleMatchings;
        ImageDataManager.OnImagesGenerated += InitializeSetAchivements;
    }
    private void OnDisable()
    {
        MergeManager.OnSpritesMerged -= HandleMatchings;
        ImageDataManager.OnImagesGenerated -= InitializeSetAchivements;
    }
    private void Awake()
    {
        
        InitializeGeneralAchivements();
    }
    public Achivement GetNextInactiveAchivement()
    {
        Achivement next = null;
        foreach (Achivement achivement in datas.setAchivements)
        {
            if (achivement.IsCompleted) continue;
            if (!achivement.IsActive)
            {
                next = achivement;
                Debug.Log("Random activated Achivement: " + next.AchivementHeader);
                return achivement;
            }
        }
        return next;
    }
    public void InitializeSetAchivements(List<SpriteInfo> activeImages)
    {
        if (datas == null) return;

        activeAchivements = new();
        List<string> headers = new();
        List<Achivement> achivementsToActivate = new();

        foreach (SpriteInfo image in activeImages)
        {
            string header = image.header;
            if (!headers.Contains(header)) headers.Add(header);
            
        }
        foreach (Achivement achivement in datas.setAchivements)
        {
            SetAchivement setAch = achivement as SetAchivement; 
            if (setAch != null)
            {
                if (headers.Contains(setAch.achivementDataSet.Header))
                {
                    achivementsToActivate.Add(achivement);
                }
            }
            
        }
        foreach (Achivement ach in achivementsToActivate)
        {
            ach.IsActive = true;
            GameObject achivementObj = Instantiate(ach.gameObject, uiParent);
            Achivement spawnedAchivement = achivementObj.GetComponent<Achivement>();
            if (spawnedAchivement.IsCompleted)
            {
                spawnedAchivement.ResetAchievement();
            }
            spawnedAchivement.InitializeAchivement();
            activeAchivements.Add(spawnedAchivement);
        }
    }
    public void ProgressAchivement(Achivement ach, float progressAmount)
    {
        ach.EarnScore(progressAmount);
    }
    public void HandleMatchings(List<SpriteInfo> matches)
    {
        string header = matches[0].header;
        foreach (Achivement ach in activeAchivements)
        {
            if (!ach.IsActive) continue;
            if(ach.AchivementHeader == header)
            {
                ComboManager.Instance.SpawnComboText(matches);
                BallSpawner.Instance.SpawnBallPieceToAchivement(matches, ach);
            }
        }
        foreach (Achivement ach in generalAchivements)
        {
            if (!ach.IsActive) continue;
            if (ach.AchivementHeader == matches.Count.ToString() + "x")
            {
                ProgressAchivement(ach, 1);
            }
        }
    }
    
    public void InitializeGeneralAchivements()
    {
        foreach (Achivement achivement in generalAchivements)
        {
            achivement.InitializeAchivement();
        }
    }
}