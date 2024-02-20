using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Image Set", menuName = "ScriptableObjects/ImageSetData", order = 1)]
public class ImageSetData : ScriptableObject
{
    public string Header;
    public List<Sprite> Content;
    public int maxSpawnAmount;
    public Sprite setSymbol;
    [Button]
    public void ActivateManually()
    {
        ImageDataManager.Instance.ActivateImageSet(this);
        //Debug.Log("Activated..");
    }
    public void DeactivateManually()
    {
        ImageDataManager.Instance.DeactivateImageSet(this);
        //Debug.Log("Deactivated..");
    }
}
