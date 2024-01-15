using UnityEngine;
using UnityEngine.UI;

public class ChooseBut : MonoBehaviour
{
    [SerializeField] Image _currentBrightButton;
    private Color Bright = Color.white;
    private Color Dim = new Color(0.6f, 0.6f, 0.6f, 1);

    public void ChangeCurrentButton(Image _nextBrightButton){
        _currentBrightButton.color = Dim;
        _currentBrightButton = _nextBrightButton;
        _currentBrightButton.color = Bright;
    }
}
