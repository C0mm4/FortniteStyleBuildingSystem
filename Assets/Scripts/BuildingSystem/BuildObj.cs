using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BuildObj : MonoBehaviour
{

    [SerializeField]
    public BuildKey key;


    public Action OnHitEvent { get; set; }

    public MeshRenderer mesh;
    private Coroutine coroutine;

    public void Initialize()
    {
        OnHitEvent += OnHit;
    }

    private void OnDisable()
    {
        BuildingManager.Instance?.UnregisterBuild(key);
        OnHitEvent -= OnHit;
    }

    public void OnHit()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = StartCoroutine(SetMeshColorAtDamage());
    }

    private IEnumerator SetMeshColorAtDamage()
    {
        Color startColor = Color.red;       // 시작 색 (빨강)
        Color endColor = Color.white;       // 최종 색 (하양)
        float duration = 1.0f;              // 효과 지속 시간 (1초)
        float elapsed = 0f;


        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration; // 0 → 1 로 증가
            mesh.material.color = Color.Lerp(startColor, endColor, t);
            yield return null;
        }

        // 보정: 최종적으로 흰색 확정
        mesh.material.color = endColor;
    }
}
