using NaughtyAttributes;
using UnityEngine;

public class ScreenshotCapture : MonoBehaviour
{
    [Header("Настройки скриншота")]
    [Tooltip("Имя файла скриншота.")]
    public string screenshotName = "Screenshot";
    [Tooltip("Качество скриншота. 1 = текущий размер экрана.")]
    public int resolutionMultiplier = 1;

    [Tooltip("Папка для сохранения скриншотов.")]
    public string saveFolder = "Screenshots";
    
    [Button]
    private void TakeScreenshot()
    {
        if (!System.IO.Directory.Exists(saveFolder))
        {
            System.IO.Directory.CreateDirectory(saveFolder);
        }
        
        // Создать уникальное имя файла с учетом времени
        string timestamp = System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        string filePath = System.IO.Path.Combine(saveFolder, $"{screenshotName}_{timestamp}.png");

        // Сохранить скриншот
        ScreenCapture.CaptureScreenshot(filePath, resolutionMultiplier);
        Debug.Log($"Скриншот сохранен: {filePath}");
    }
}