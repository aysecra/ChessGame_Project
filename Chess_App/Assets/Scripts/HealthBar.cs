using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    Vector3 offset = new Vector3(0, 0.28f, -1.0f);
    private void Update()
    {
        slider.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + offset);
    }
    public void setHealth(int health, int max)
    {
        slider.gameObject.SetActive(health<max);
        slider.maxValue = max;
        slider.value = health;
    }
}
