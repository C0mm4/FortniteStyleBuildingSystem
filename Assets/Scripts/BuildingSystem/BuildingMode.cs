using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;



public class BuildingMode : MonoBehaviour
{
    [Header("Build Setting")]
    public float checkRate = 0.05f;
    public bool isBuild = false;
    public float rayDistance;
    public LayerMask buildMask;
    public LayerMask buildableLayer;
    public BuildMode buildMode;

    private BuildKey buildKey;
    private GameObject preViewObj;
    private float lastCheckTime;
    private RaycastHit hit;

    private GameObject[] preViewObjs = new GameObject[2];

    // �Ⱥ��̴� ���� ���� ���̾� ������Ʈ ����, ���
    public GameObject[] invisibleLayer = new GameObject[2];
    [SerializeField]

    private void Start()
    {
        // Preview ������Ʈ �ʱ�ȭ
        for(int i = 0; i < preViewObjs.Length; i++)
        {
            preViewObjs[i] = Instantiate(Resources.Load<GameObject>($"Prefab/Preview{i}"));
            preViewObjs[i].SetActive(false);
            preViewObjs[i].GetComponentInChildren<MeshRenderer>().material.color = new Color(0, 1, 0, 0.6f);
        }

    }

    public void Update()
    {
        if (isBuild)
        {
            // Ű �Է����� ���� ��� ����
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                DestroyPrevObj();
                buildMode = BuildMode.Floor;
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                DestroyPrevObj();
                buildMode = BuildMode.Wall;
            }
            if (Time.time - lastCheckTime > checkRate)
            {
                lastCheckTime = Time.time;

                Ray(out hit, rayDistance);
                if (hit.collider != null)
                {
                    Debug.Log("�ϴ� ���� ������ �Ǿ���");
                    var (pos, dir, rot) = BuildingManager.Instance.GetBuildPos(hit.point);
                    BuildKey newKey = new(buildMode, pos, dir);

                    // �ش� ��ġ�� �̹� �Ǽ��� ������ ���
                    if (BuildingManager.Instance.IsOccupied(newKey))
                    {
                        DestroyPrevObj();
                        Debug.Log($"�̹� {buildMode} �� ��ġ�� �ڸ��Դϴ�!");
                        return;
                    }

                    if (preViewObj == null || !buildKey.Equals(newKey))
                    {
                        DestroyPrevObj();
                        buildKey = newKey;
                        CreatePreviewObj(hit, buildMode);
                    }

                }
            }
        }
    }

    public void TryBuild()
    {
        if (CanBuildAt(buildKey.Position, buildKey.rot, buildMode))
        {
            CreateBuildObj(hit, buildMode);
        }
        else
        {
            Debug.Log("�Ǽ� �Ұ� ��ġ!");
        }
    }

    private bool Ray(out RaycastHit hit, float distance)
    {
        Vector3 origin, dir;
        if (Camera.main != null)
        {
            origin = Camera.main.transform.position;
            dir = Camera.main.transform.forward;
        }
        else
        {
            origin = transform.position + Vector3.up * 0.8f;
            dir = transform.forward;
        }
        return Physics.Raycast(origin, dir, out hit, distance, buildMask, QueryTriggerInteraction.Collide);
    }

    public void DestroyPrevObj()
    {
        if (preViewObj != null)
        {
            preViewObj.SetActive(false);
            preViewObj = null;
        }
    }

    public void CreatePreviewObj(RaycastHit hit, BuildMode mode)
    {
        GameObject go = preViewObjs[((int)mode)];

        go.transform.SetPositionAndRotation(buildKey.Position, buildKey.rot);
        bool canBuild = CanBuildAt(buildKey.Position, buildKey.rot, buildMode);
        var renderer = go.GetComponentInChildren<MeshRenderer>();
        if (renderer != null)
        {
            renderer.material.color = canBuild ? new Color(0, 1, 0, 0.6f) : new Color(1, 0, 0, 0.6f);
        }
        go.SetActive(true);
        preViewObj = go;
    }

    public void CreateBuildObj(RaycastHit hit, BuildMode mode)
    {
        var buildObj = Instantiate(Resources.Load<GameObject>($"Prefab/BuildObj{(int)mode}"));

        var obj = buildObj.GetComponent<BuildObj>();
        obj.key = buildKey;
        obj.Initialize();
        obj.GetComponentInChildren<MeshRenderer>().material.color = Color.white;
        buildObj.transform.SetPositionAndRotation(buildKey.Position, buildKey.rot);

        
        BuildingManager.Instance.RegisterBuild(buildKey);
    }
    private bool CanBuildAt(Vector3 pos, Quaternion rot, BuildMode mode)
    {
        Vector3 halfExtents = Vector3.one; // �ǹ� ũ�⿡ �°� ���� �ʿ�
        Collider[] hits = Physics.OverlapBox(pos, halfExtents, rot, buildableLayer);

        return hits.Length > 0;
    }


}
