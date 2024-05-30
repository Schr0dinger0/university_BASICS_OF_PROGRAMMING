using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceClickHandler : MonoBehaviour
{
    public Vector3 pieceRotation = new Vector3(-90, 0, 0);
    public Vector2 startSquarePosition = new Vector2(0, 0);
    public float startHeight = 3.7f;
    public float PieceScale = 1.5f;
    public float PieceSpacing = 10;

    public GameObject BluePad;
    public GameObject RedPad;
    public GameObject YellowPad;

    int width = 1;
    int lenght = 1;

    
    void OnMouseDown()
    {
        GameObject piecePrefab = null;

        piecePrefab = BluePad;

        Vector3 position = new Vector3(
                   startSquarePosition[0] + width * PieceSpacing,
                   startHeight,
                   startSquarePosition[1] + lenght * PieceSpacing
               );
        GameObject piece = Instantiate(piecePrefab, position, Quaternion.Euler(pieceRotation));
        piece.transform.localScale *= PieceScale;
        // Код для обработки клика на фигуре
        Debug.Log("Figure clicked: " + gameObject.name);
        // Вы можете добавить здесь логику для выделения фигуры, перемещения и т.д.
    }
}
