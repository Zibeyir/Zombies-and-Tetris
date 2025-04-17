using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private Slider hpSlider;
    public float HPMax;
    public void UpdateHP(float hp)
    {
        hpSlider.value = hp;
    }
}
