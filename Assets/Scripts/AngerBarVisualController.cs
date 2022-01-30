using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AngerBarVisualController : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private List<Sprite> _emojiSprites;
    [SerializeField] private Image _emojiImageHolder;
    [SerializeField] private Image _barImage;
    [SerializeField] private Gradient _barGradient;

    public void OnAngerChanged(float newAnger)
    {
        float val = newAnger / 100;
        int index = (int) Mathf.Clamp(val * _emojiSprites.Count, 0, _emojiSprites.Count - 1);
        _emojiImageHolder.sprite = _emojiSprites[index];
        _barImage.color = _barGradient.Evaluate(val);
        _slider.value = val;
    }
}