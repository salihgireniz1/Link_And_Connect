using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICollectible
{
    public int Bonus { get; set; }

    public void GetBonus();

    public void MakeInActive();
}
