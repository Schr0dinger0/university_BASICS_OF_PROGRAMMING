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
        // Сохраняем начальные позицию и вращение камеры
        initialCameraPosition = cam.transform.position;
        initialCameraRotation = cam.transform.rotation;

        // Устанавливаем камеру в перспективный режим по умолчанию
        cam.orthographic = false;
        isOrthographic = false;

    }

    void Update()
    {
        // Проверка нажатия клавиши "F" для переключения проекции камеры
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
            // Управление движением камеры при нажатии правой кнопки мыши
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
        // Сохраняем текущую позицию и вращение камеры
        initialCameraPosition = cam.transform.position;
        initialCameraRotation = cam.transform.rotation;

        // Переключаем на ортографическую проекцию
        cam.orthographic = true;
        isOrthographic = true;

        cam.orthographicSize = orthographicSize;

        cam.transform.position = target.position + orthographicOffset;
        cam.transform.rotation = Quaternion.Euler(90, 180, 0);

        // Отключаем тени у всех объектов
        UpdateRenderers();
        foreach (Renderer rend in FindObjectsOfType<Renderer>())
        {
            rend.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            rend.receiveShadows = false;
        }
    }

    void SwitchToPerspective()
    {
        // Переключаем на перспективную проекцию
        cam.orthographic = false;
        isOrthographic = false;

        // Возвращаем камеру на исходную позицию и вращение
        cam.transform.position = initialCameraPosition;
        cam.transform.rotation = initialCameraRotation;

        // Включаем тени у всех объектов
        foreach (Renderer rend in FindObjectsOfType<Renderer>())
        {
            rend.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
            rend.receiveShadows = true;
        }
    }

    void UpdateRenderers()
    {
        // Обновляем список рендереров
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
