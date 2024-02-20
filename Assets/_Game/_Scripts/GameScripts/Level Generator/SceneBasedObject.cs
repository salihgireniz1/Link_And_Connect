using UnityEngine;

public abstract class SceneBasedObject : MonoBehaviour
{
    protected virtual void OnEnable()
    {
        LevelManager.OnLevelLoaded += Clear;
    }
    protected virtual void OnDisable()
    {
        LevelManager.OnLevelLoaded -= Clear;
    }
    protected virtual void Clear(Level level)
    {
        Destroy(this.gameObject);
    }
}
