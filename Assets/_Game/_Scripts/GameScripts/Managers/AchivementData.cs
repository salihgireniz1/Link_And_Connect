using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Achivement Data Holder", menuName = "ScriptableObjects/AchivementData", order = 2)]
public class AchivementData : ScriptableObject
{
    public List<Achivement> setAchivements;
    public Transform uIParent;
    public int defaultActiveCount = 1;
}
