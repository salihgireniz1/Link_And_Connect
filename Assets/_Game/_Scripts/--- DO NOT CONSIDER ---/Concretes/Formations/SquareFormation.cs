using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareFormation : IFormation
{
    private List<Point> _formationPoints = new List<Point>();
    public List<Point> FormationPoints => _formationPoints;

    private Transform _originTransform;
    private float _row;
    private float _column;
    private float _gapAmount;

    public SquareFormation(Transform originTransform, float row, float column, float gapAmount)
    {
        _originTransform = originTransform;
        _row = row;
        _column = column;
        _gapAmount = gapAmount;
    }

    public void Create()
    {
        CreateOddSquareFormation();
    }

    private void CreateOddSquareFormation()
    {
        float startPosX = _originTransform.position.x - (Mathf.FloorToInt(_row / 2) * _gapAmount);
        float startPosZ = _originTransform.position.z + (Mathf.FloorToInt(_column / 2) * _gapAmount);
        float startPosY = _originTransform.position.y + 2f;

        Vector3 startPos = new Vector3(startPosX, startPosY, startPosZ);

        for (int i = 0; i < _row; i++)
        {
            for (int j = 0; j < _column; j++)
            {
                Point newPoint = new Point(startPos);
                _formationPoints.Add(newPoint);
                startPos.z -= _gapAmount;
            }

            startPos.x += _gapAmount;
            startPos.z = startPosZ;
        }
    }
}
