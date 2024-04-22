using UnityEngine;
using Unity.XRContent.Interaction;

public class SliderCollider : MonoBehaviour
{
    [SerializeField] private string difficulty;
    private XRSlider _slider;

    void Start()
    {
        _slider = GetComponentInParent<XRSlider>();
    }

    void OnTriggerEnter(Collider other)
    {
        _slider.ChangeDifficulty(difficulty);
    }
}
