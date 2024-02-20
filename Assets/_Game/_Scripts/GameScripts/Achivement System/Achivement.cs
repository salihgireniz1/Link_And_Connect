using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Achivement : SceneBasedObject
{
    public abstract string AchivementHeader { get; protected set; }
    public virtual float Score
    { 
        get => ES3.Load(AchivementHeader + "_Achivement", 0f);
        set
        {
            ES3.Save(AchivementHeader + "_Achivement", value);
            if(value >= Target)
            {
                IsCompleted = true;
            }
        }
    }
    public abstract int Target { get; }
    public bool IsActive 
    {
        get
        {
            return ES3.Load(AchivementHeader + "_IsActive", false);
        }
        set
        {
            ES3.Save(AchivementHeader + "_IsActive", value);
        }
    }
    public bool IsCompleted
    {
        get
        {
            return ES3.Load(AchivementHeader + "_IsCompleted", false);
        }
        set
        {
            ES3.Save(AchivementHeader + "_IsCompleted", value);
        }
    }
    public virtual void InitializeAchivement()
    {
        float currentScore = Score;
        Score = currentScore;
    }
    public virtual void EarnScore(float score)
    {
        Score += score;
    }
    public virtual void ResetAchievement()
    {
        Score = 0f;
        IsActive = true;
        IsCompleted = false;
    }
}
