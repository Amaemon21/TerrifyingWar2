using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    [Space]
    [SerializeField] private Light sun;
    [SerializeField] private float secondsInFullDay = 120f;

    [Space]
    [Range(0f, 1f)]
    [SerializeField] public float currentTimeOfDay = 0.03f;

    [Space]
    [SerializeField] public float startHours = 0f;

    [SerializeField] public int hours = 5;
    [SerializeField] public int minutes = 0;

    [SerializeField] private TextMeshProUGUI timeText = null;

    private float timeMultiplier = 1f;
    private float sunInitialIntensity;

    private void Awake()
    {
        float totalMinutes = currentTimeOfDay * 24 * 60;

        hours = (int)totalMinutes / 60;

        StartCoroutine(WaitAndPrint());
        
        currentTimeOfDay = 0.401f;
    }
    
    IEnumerator WaitAndPrint()
    {
        yield return new WaitForSeconds(0.01f);
        currentTimeOfDay = startHours / 24;
    }
    
    private void Start()
    {
        sunInitialIntensity = sun.intensity;
    }

    private void Update()
    {
        UpdateSun();

        currentTimeOfDay += (Time.deltaTime / secondsInFullDay) * timeMultiplier;

        if (currentTimeOfDay >= 1)
        {
            currentTimeOfDay = 0;
        }
    }

    private void UpdateSun()
    {
        DispleyTime();

        sun.transform.localRotation = Quaternion.Euler((currentTimeOfDay * 360f) - 90, 170, 0);

        float intesityMultiplier = 1f;

        if (currentTimeOfDay  <= 0.23f || currentTimeOfDay >= 0.75f)    
        {
            intesityMultiplier = 0.001f;
        }
        else if (currentTimeOfDay <= 0.25f)
        {
            intesityMultiplier = Mathf.Clamp01((currentTimeOfDay - 0.23f) * (1/ 0.02f));
        }
        else if(currentTimeOfDay >= 0.73f)
        {
            intesityMultiplier = Mathf.Clamp01(1 - (currentTimeOfDay - 0.73f) * (1 / 0.02f));
        }

        sun.intensity = sunInitialIntensity * intesityMultiplier;
    }

    private void DispleyTime()
    {
        float totalMinutes = currentTimeOfDay * 24 * 60;
        
        hours = (int)totalMinutes / 60;
        
        minutes = (int)totalMinutes % 60;
     

        string displayHourse = hours.ToString("D2");
        string displayMinutes = minutes.ToString("D2");

        string total = displayHourse + ":" + displayMinutes;

        timeText.text = total;
    }

 
}