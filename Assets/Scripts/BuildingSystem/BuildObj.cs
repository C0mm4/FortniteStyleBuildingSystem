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
        Color startColor = Color.red;       // ���� �� (����)
        Color endColor = Color.white;       // ���� �� (�Ͼ�)
        float duration = 1.0f;              // ȿ�� ���� �ð� (1��)
        float elapsed = 0f;


        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration; // 0 �� 1 �� ����
            mesh.material.color = Color.Lerp(startColor, endColor, t);
            yield return null;
        }

        // ����: ���������� ��� Ȯ��
        mesh.material.color = endColor;
    }
}
