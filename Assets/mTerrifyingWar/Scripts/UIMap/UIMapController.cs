using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class UIMapController : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    [Inject] private readonly PlayerProvider _playerProvider;
    
    [Header("Map Settings")]
    [SerializeField] private RectTransform mapRect; 
    [SerializeField] private RectTransform viewportRect;
    [SerializeField] private RectTransform playerIndicator; 
    [SerializeField] private Vector2 mapSize; 
    [SerializeField] private float zoomSpeed = 0.5f;
    [SerializeField] private float minZoom = 0.5f; 
    [SerializeField] private float maxZoom = 2f;

    [Header("Icon Settings")] 
    [SerializeField] private List<RectTransform> _mapIcons = new List<RectTransform>();

    private Transform playerTransform;

    private Vector2 pointerOffset; 
    private bool isDragging = false;

    private void Awake()
    {
        playerTransform = _playerProvider.PlayerController.transform;
    }

    private void Update()
    {
        HandleZoom();
        
        UpdatePlayerPosition();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(viewportRect, eventData.position, eventData.pressEventCamera, out Vector2 localMousePosition))
        {
            pointerOffset = mapRect.anchoredPosition - localMousePosition;
            isDragging = true;
        }
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging && RectTransformUtility.ScreenPointToLocalPointInRectangle(viewportRect, eventData.position, eventData.pressEventCamera, out Vector2 localMousePosition))
        {
            mapRect.anchoredPosition = localMousePosition + pointerOffset;
            ClampPosition();
        }
    }
    
    private void UpdatePlayerPosition()
    {
        float iconScale = 1.5f / mapRect.transform.localScale.x;
        
        Vector3 playerPos = playerTransform.position;
        
        float x = Mathf.Clamp(playerPos.x / mapSize.x, -0.5f, 0.5f);
        float y = Mathf.Clamp(playerPos.z / mapSize.y, -0.5f, 0.5f); 
        
        Vector2 indicatorPos = new Vector2((x * mapRect.rect.width) / 2, (y * mapRect.rect.height) / 2);
        
        playerIndicator.anchoredPosition = indicatorPos;

        Vector3 rotation = playerTransform.transform.rotation.eulerAngles;
        playerIndicator.localRotation = Quaternion.AngleAxis(-rotation.y, Vector3.forward);

        foreach (RectTransform icon in _mapIcons)
        {
            icon.localScale = Vector3.one * iconScale;
        }
    } 
    
    /*
    private void HandleZoom()
    {
        if (Input.mouseScrollDelta.y != 0)
        {
            // Определение текущего масштаба и шага изменения
            float scaleStep = Input.mouseScrollDelta.y * (zoomSpeed * 0.5f);
            float targetScale = Mathf.Clamp(mapRect.localScale.x + scaleStep, minZoom, maxZoom);

            if (viewportRect != null)
            {
                Vector2 mapSize = mapRect.rect.size * targetScale;
                Vector2 viewportSize = viewportRect.rect.size;

                // Учет минимального масштаба, чтобы карта не стала меньше вьюпорта
                if (mapSize.x < viewportSize.x || mapSize.y < viewportSize.y)
                {
                    float minScaleByWidth = viewportSize.x / mapRect.rect.width;
                    float minScaleByHeight = viewportSize.y / mapRect.rect.height;
                    targetScale = Mathf.Max(targetScale, Mathf.Max(minScaleByWidth, minScaleByHeight));
                }
            }

            // Получение позиции playerIndicator относительно карты
            Vector2 playerIndicatorPosition = playerIndicator.anchoredPosition;

            // Пересчет позиции карты относительно новой точки зума
            Vector2 preZoomPosition = mapRect.anchoredPosition - playerIndicatorPosition;
            Vector2 postZoomPosition = preZoomPosition * (targetScale / mapRect.localScale.x);

            // Плавная анимация масштаба и позиции
            DOTween.To(() => mapRect.localScale, x => mapRect.localScale = x, Vector3.one * targetScale, 0.3f).OnUpdate(ClampPosition);
            DOTween.To(() => mapRect.anchoredPosition, x => mapRect.anchoredPosition = x, playerIndicatorPosition + postZoomPosition, 0.3f).OnUpdate(ClampPosition);
        }
    }
    */
    
    private void HandleZoom()
    {
        if (Input.mouseScrollDelta.y != 0)
        {
            float scaleStep = Input.mouseScrollDelta.y * (zoomSpeed * 0.5f);
            float targetScale = Mathf.Clamp(mapRect.localScale.x + scaleStep, minZoom, maxZoom);

            if (viewportRect != null)
            {
                Vector2 mapSize = mapRect.rect.size * targetScale;
                Vector2 viewportSize = viewportRect.rect.size;
                
                if (mapSize.x < viewportSize.x || mapSize.y < viewportSize.y)
                {
                    float minScaleByWidth = viewportSize.x / mapRect.rect.width;
                    float minScaleByHeight = viewportSize.y / mapRect.rect.height;
                    targetScale = Mathf.Max(targetScale, Mathf.Max(minScaleByWidth, minScaleByHeight));
                }
            }
            
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(viewportRect, Input.mousePosition, null, out Vector2 localCursorPosition))
            {
                Vector2 preZoomPosition = mapRect.anchoredPosition - localCursorPosition;
                Vector2 postZoomPosition = preZoomPosition * (targetScale / mapRect.localScale.x);
                
                DOTween.To(() => mapRect.localScale, x => mapRect.localScale = x, Vector3.one * targetScale, 0.3f).OnUpdate(ClampPosition);
                DOTween.To(() => mapRect.anchoredPosition, x => mapRect.anchoredPosition = x, localCursorPosition + postZoomPosition, 0.3f).OnUpdate(ClampPosition);
            }
        }
    }

    private void ClampPosition()
    {
        if (viewportRect == null) return;

        Vector2 mapSize = mapRect.rect.size * mapRect.localScale;
        Vector2 viewportSize = viewportRect.rect.size;
        
        float minX = Mathf.Min(0, (viewportSize.x - mapSize.x) / 2);
        float maxX = Mathf.Max(0, -minX);
        float minY = Mathf.Min(0, (viewportSize.y - mapSize.y) / 2);
        float maxY = Mathf.Max(0, -minY);
        
        Vector2 clampedPosition = mapRect.anchoredPosition;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, minX, maxX);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, minY, maxY);

        mapRect.anchoredPosition = clampedPosition;
    }
}
