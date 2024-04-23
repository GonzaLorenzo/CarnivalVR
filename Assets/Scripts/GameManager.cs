using Unity.XRContent.Interaction;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class GameManager : MonoBehaviour
{
    public delegate void PointsUpdateDelegate(int points);
    public PointsUpdateDelegate onPointsUpdate;

    private PlayableDirector _playableDirector;

    [SerializeField] private TimelineAsset _easyTimeline;
    [SerializeField] private TimelineAsset _mediumTimeline;
    [SerializeField] private TimelineAsset _hardTimeline;

    [SerializeField] private XRSlider _difficultySlider;

    private int _points;

    void Start()
    {
        _playableDirector = GetComponent<PlayableDirector>();
        _difficultySlider.onDifficultyChanged += ChangeDifficulty;
    }

    public void PlayTimeline()
    {
        _playableDirector.Play();
        _points = 0;
    }

    public void ChangeDifficulty(string difficulty)
    {
        switch(difficulty)
        {
            case "easy":
                _playableDirector.playableAsset = _easyTimeline;
                break;

            case "medium":
                _playableDirector.playableAsset = _mediumTimeline;
                break;

            case "hard":
                _playableDirector.playableAsset = _hardTimeline;
                break;
        }
    }

    public void AddPoints(int points)
    {
        _points +=  points;
        onPointsUpdate.Invoke(_points);
    }
}
