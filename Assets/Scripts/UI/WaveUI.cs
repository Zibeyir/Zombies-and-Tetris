using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveUI : MonoBehaviour
{
    [SerializeField] private Text waveText;
    [SerializeField] private List<Slider> waveSlider=new List<Slider>();
    [SerializeField] private List<Image> waveImages = new List<Image>();
    [SerializeField] private Sprite waveActiveImage;

    public void ShowWave(float wave, int waveNumber)
    {
        //waveText.text = "Wave " + wave;
        
        if (wave == 1&& waveNumber < 3)
        {
            waveSlider[waveNumber - 1].value = wave;

            waveImages[waveNumber-1].sprite=waveActiveImage;
        }
    }
}
