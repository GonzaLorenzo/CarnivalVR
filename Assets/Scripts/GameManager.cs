using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class GameManager : MonoBehaviour
{
    private PlayableDirector _playableDirector;
    void Start()
    {
        _playableDirector = GetComponent<PlayableDirector>();
    }

    public void PlayEasy()
    {
        _playableDirector.Play();
    }
}
