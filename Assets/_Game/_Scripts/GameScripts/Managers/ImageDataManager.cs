using System;
using System.Collections.Generic;
using UnityEngine;
public class ImageDataManager : MonoSingleton<ImageDataManager>
{
    public static event Action<List<SpriteInfo>> OnImagesGenerated;
    public List<ImageSetData> ImageSetDatas { get => imageSetDatas; }
    public List<SpriteInfo> ActiveImages
    {
        get => activeImages;
        private set => activeImages = value;
    }
    [SerializeField]
    private GameObject cellBase;

    [SerializeField]
    private List<ImageSetData> imageSetDatas;

    [SerializeField]
    private List<SpriteInfo> activeImages = new();

    private int maxSpawnCount = 4;

    List<ImageSetData> activeSets = new();
    private void OnEnable()
    {
        //GameManager.OnGameStateChanged += InitializeActiveSets;
        GridGenerator.OnGridGenerated += InitializeActiveSets;
    }
    private void OnDisable()
    {
        //GameManager.OnGameStateChanged -= InitializeActiveSets;
        GridGenerator.OnGridGenerated -= InitializeActiveSets;
    }
    public void InitializeActiveSets()
    {
        maxSpawnCount = LevelManager.Instance.latestLevel.gridData.width;
        activeImages = new();
        activeSets = new();
        List<ImageSetData> deactives = new();

        if (LevelManager.Instance.FirstStart)
        {
            for (int i = 0; i < maxSpawnCount; i++)
            {
                ActivateImageSet(imageSetDatas[i]);
            }
        }

        foreach (ImageSetData setData in imageSetDatas)
        {
            if (GetActivityResult(setData))
            {
                activeSets.Add(setData);
            }
            else
            {
                deactives.Add(setData);
            }
        }

        int missingSets = maxSpawnCount - activeSets.Count;
        for (int i = 0; i < missingSets; i++)
        {
            int randomSetIndex = UnityEngine.Random.Range(0, deactives.Count);
            ImageSetData randomSet = deactives[randomSetIndex];
            deactives.Remove(randomSet);
            ActivateImageSet(randomSet);
            activeSets.Add(randomSet);
        }
        foreach (ImageSetData set in activeSets)
        {
            SpawnSet(set);
        }
        OnImagesGenerated?.Invoke(activeImages);
    }
    public void InitializeActiveSets(GameState state)
    {
        if (state != GameState.loaded) return;
        activeImages = new();
        activeSets = new();
        List<ImageSetData> deactives = new();

        if (LevelManager.Instance.FirstStart)
        {
            for (int i = 0; i < maxSpawnCount; i++)
            {
                ActivateImageSet(imageSetDatas[i]);
            }
        }

        foreach (ImageSetData setData in imageSetDatas)
        {
            if (GetActivityResult(setData))
            {
                activeSets.Add(setData);
            }
            else
            {
                deactives.Add(setData);
            }
        }

        int missingSets = maxSpawnCount - activeSets.Count;
        for (int i = 0; i < missingSets; i++)
        {
            int randomSetIndex = UnityEngine.Random.Range(0, deactives.Count);
            ImageSetData randomSet = deactives[randomSetIndex];
            deactives.Remove(randomSet);
            Debug.Log(randomSet.name + " is selected between " + deactives.Count);
            ActivateImageSet(randomSet);
            activeSets.Add(randomSet);
        }
        foreach (ImageSetData set in activeSets)
        {
            SpawnSet(set);
        }
        OnImagesGenerated?.Invoke(activeImages);
    }
    public void SpawnSet(ImageSetData setData)
    {
        int nameIndex = 0;
        List<Sprite> spawnableSprites = new();
        foreach (Sprite sprite in setData.Content)
        {
            spawnableSprites.Add(sprite);
        }

        for (int i = 0; i < setData.maxSpawnAmount; i++)
        {
            if (spawnableSprites.Count == 0) break;

            int randomSpriteIndex = UnityEngine.Random.Range(0, spawnableSprites.Count);
            Sprite randomSprite = spawnableSprites[randomSpriteIndex];
            spawnableSprites.Remove(randomSprite);
            GameObject imageObj = Instantiate(cellBase, Vector3.zero, Quaternion.identity, this.transform);
            SpriteInfo info = imageObj.GetComponent<SpriteInfo>();
            info.header = setData.Header;
            info.contentRenderer.sprite = randomSprite;

            imageObj.name = $"{setData.Header}-{nameIndex}";
            imageObj.transform.position = new Vector3(0f, 0f, 0f);

            if (!activeImages.Contains(info))
                activeImages.Add(info);

            nameIndex += 1;
        }
    }
    public void ActivateImageSet(ImageSetData set)
    {
        ES3.Save(set.Header, true);
    }
    public void DeactivateImageSet(ImageSetData set)
    {
        ES3.Save(set.Header, false);
    }
    public bool GetActivityResult(ImageSetData set)
    {
        return ES3.Load(set.Header, false);
    }

    public void HandleFullSet(GroupInfo group)
    {
        foreach (SpriteInfo spriteInfo in group.connections)
        {
            activeImages.Remove(spriteInfo);
            Destroy(spriteInfo.gameObject);
            if (activeImages.Count == 0)
            {
                GameManager.Instance.UpdateGameState(GameState.completed);
            }
        }
        Destroy(group.gameObject);
    }
}