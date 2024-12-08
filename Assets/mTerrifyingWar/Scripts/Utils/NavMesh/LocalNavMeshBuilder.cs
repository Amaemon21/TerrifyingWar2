using System;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;
using NavMeshBuilder = UnityEngine.AI.NavMeshBuilder;

[DefaultExecutionOrder(-102)]
public class LocalNavMeshBuilder : MonoBehaviour
{
    [SerializeField] private  Vector3 _size = new Vector3(80.0f, 20.0f, 80.0f);

    private Transform _transform;
    
    private NavMeshData _navMeshData;
    private AsyncOperation _operation;
    private NavMeshDataInstance _instance;
    private List<NavMeshBuildSource> _sources = new();

    private void Awake()
    {
        _transform = transform;
    }

    IEnumerator Start()
    {
        while (true)
        {
            UpdateNavMesh(true);
            yield return _operation;
        }
    }

    void OnEnable()
    {
        _navMeshData = new NavMeshData();
        _instance = NavMesh.AddNavMeshData(_navMeshData);

        UpdateNavMesh(false);
    }

    void OnDisable()
    {
        _instance.Remove();
    }

    void UpdateNavMesh(bool asyncUpdate = false)
    {
        NavMeshSourceTag.Collect(ref _sources);
        NavMeshBuildSettings defaultBuildSettings = NavMesh.GetSettingsByID(0);
        Bounds bounds = QuantizedBounds();

        if (asyncUpdate)
            _operation = NavMeshBuilder.UpdateNavMeshDataAsync(_navMeshData, defaultBuildSettings, _sources, bounds);
        else
            NavMeshBuilder.UpdateNavMeshData(_navMeshData, defaultBuildSettings, _sources, bounds);
    }

    static Vector3 Quantize(Vector3 v, Vector3 quant)
    {
        float x = quant.x * Mathf.Floor(v.x / quant.x);
        float y = quant.y * Mathf.Floor(v.y / quant.y);
        float z = quant.z * Mathf.Floor(v.z / quant.z);
        return new Vector3(x, y, z);
    }

    Bounds QuantizedBounds()
    {
        Vector3 center = _transform.transform.position;
        return new Bounds(Quantize(center, 0.1f * _size), _size);
    }

    void OnDrawGizmosSelected()
    {
        if (_transform == null)
            return;

        if (_navMeshData)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(_navMeshData.sourceBounds.center, _navMeshData.sourceBounds.size);
        }

        Gizmos.color = Color.yellow;
        Bounds bounds = QuantizedBounds();
        Gizmos.DrawWireCube(bounds.center, bounds.size);

        Gizmos.color = Color.green;
        Vector3 center = _transform.transform.position;
        Gizmos.DrawWireCube(center, _size);
    }
}