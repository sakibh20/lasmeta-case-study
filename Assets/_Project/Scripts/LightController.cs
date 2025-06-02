using UnityEngine;
using UnityEngine.UI;

public class LightController : MonoBehaviour
{
    [SerializeField] private Light spotLight;
    [SerializeField] private Slider lightIntensitySlider;

    private void Start()
    {
        lightIntensitySlider.minValue = 0;
        lightIntensitySlider.maxValue = 100;

        lightIntensitySlider.onValueChanged.AddListener(UpdateLightIntensity);
    }

    private void UpdateLightIntensity(float value)
    {
        spotLight.intensity = value;
    }
}
