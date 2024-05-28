using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 orthographicOffset = new Vector3(0, 30, 0);
    [SerializeField] private float orthographicSize = 3f;

    private Vector3 previousPosition;
    private Vector3 initialCameraPosition;
    private Quaternion initialCameraRotation;
    private bool isOrthographic = false;

    void Start()
    {
        // ��������� ��������� ������� � �������� ������
        initialCameraPosition = cam.transform.position;
        initialCameraRotation = cam.transform.rotation;

        // ������������� ������ � ������������� ����� �� ���������
        cam.orthographic = false;
        isOrthographic = false;

    }

    void Update()
    {
        // �������� ������� ������� "F" ��� ������������ �������� ������
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (isOrthographic)
            {
                SwitchToPerspective();
            }
            else
            {
                SwitchToOrthographic();
            }
        }

        if (!isOrthographic)
        {
            // ���������� ��������� ������ ��� ������� ������ ������ ����
            if (Input.GetMouseButtonDown(1))
            {
                previousPosition = cam.ScreenToViewportPoint(Input.mousePosition);
            }
            if (Input.GetMouseButton(1))
            {
                Vector3 direction = previousPosition - cam.ScreenToViewportPoint(Input.mousePosition);

                cam.transform.position = target.position;

                cam.transform.Rotate(new Vector3(1, 0, 0), direction.y * 180);
                cam.transform.Rotate(new Vector3(0, -1, 0), direction.x * 180, Space.World);
                cam.transform.Translate(new Vector3(0, 3, -40));

                previousPosition = cam.ScreenToViewportPoint(Input.mousePosition);
            }
        }
    }

    void SwitchToOrthographic()
    {
        // ��������� ������� ������� � �������� ������
        initialCameraPosition = cam.transform.position;
        initialCameraRotation = cam.transform.rotation;

        // ����������� �� ��������������� ��������
        cam.orthographic = true;
        isOrthographic = true;

        cam.orthographicSize = orthographicSize;

        cam.transform.position = target.position + orthographicOffset;
        cam.transform.rotation = Quaternion.Euler(90, 180, 0);

        // ��������� ���� � ���� ��������
        UpdateRenderers();
        foreach (Renderer rend in FindObjectsOfType<Renderer>())
        {
            rend.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            rend.receiveShadows = false;
        }
    }

    void SwitchToPerspective()
    {
        // ����������� �� ������������� ��������
        cam.orthographic = false;
        isOrthographic = false;

        // ���������� ������ �� �������� ������� � ��������
        cam.transform.position = initialCameraPosition;
        cam.transform.rotation = initialCameraRotation;

        // �������� ���� � ���� ��������
        foreach (Renderer rend in FindObjectsOfType<Renderer>())
        {
            rend.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
            rend.receiveShadows = true;
        }
    }

    void UpdateRenderers()
    {
        // ��������� ������ ����������
        List<Renderer> renderers = new List<Renderer>(FindObjectsOfType<Renderer>());

        foreach (Renderer rend in renderers)
        {
            if (isOrthographic)
            {
                rend.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                rend.receiveShadows = false;
            }
            else
            {
                rend.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                rend.receiveShadows = true;
            }
        }
    }
}
