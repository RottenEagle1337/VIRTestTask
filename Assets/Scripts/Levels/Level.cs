using UnityEngine;

public class Level : MonoBehaviour
{
    public Level(int _levelIndex, bool _isPasseed)
    {
        levelIndex = _levelIndex;
    }

    [SerializeField] private Transform completedLevelStar;

    public int levelIndex;

    public void ShowStar()
    {
        completedLevelStar.gameObject.SetActive(true);
    }

    public void HideStar()
    {
        completedLevelStar.gameObject.SetActive(false);
    }
}