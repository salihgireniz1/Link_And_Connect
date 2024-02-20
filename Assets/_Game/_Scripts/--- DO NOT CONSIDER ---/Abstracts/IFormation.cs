using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFormation
{
    public List<Point> FormationPoints { get; }
    public void Create();
}
