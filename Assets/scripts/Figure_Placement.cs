using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Figure_Placement : MonoBehaviour
{
    public Vector3 pieceRotation = new Vector3(-90, 0, 0);
    public Vector2 startSquarePosition = new Vector2(0, 0);
    public float startHeight = 3.7f;
    public float PieceScale = 1.5f;
    public float PieceSpacing = 10;

    public GameObject Black_pawn;
    public GameObject Black_rook;
    public GameObject Black_bishop;
    public GameObject Black_knight;
    public GameObject Black_queen;
    public GameObject Black_king;

    public GameObject White_pawn;
    public GameObject White_rook;
    public GameObject White_bishop;
    public GameObject White_knight;
    public GameObject White_queen;
    public GameObject White_king;

    // Start is called before the first frame update
    void Start()
    {
        GameObject piecePrefab = null;
        for (int width = 0; width < 8; width++)//white spawn
        {
            for (int lenght = 0; lenght > -2; lenght--)
            {
                if (lenght == 0)
                {
                    switch (width)
                    {
                        case 0:
                        case 7:
                            piecePrefab = White_rook;
                            break;
                        case 1:
                        case 6:
                            piecePrefab = White_knight;
                            break;
                        case 2:
                        case 5:
                            piecePrefab = White_bishop;
                            break;
                        case 4:
                            piecePrefab = White_queen;
                            break;
                        case 3:
                            piecePrefab = White_king;
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    piecePrefab = White_pawn;
                }

                Vector3 position = new Vector3(
                    startSquarePosition[0] + width * PieceSpacing,
                    startHeight,
                    startSquarePosition[1] + lenght * PieceSpacing
                );
                GameObject piece = Instantiate(piecePrefab, position, Quaternion.Euler(pieceRotation));
                piece.transform.localScale *= PieceScale;
                piece.AddComponent<BoxCollider>();  // Добавление BoxCollider
                piece.AddComponent<PieceClickHandler>();  // Добавление обработчика кликов
            }

        }

        for (int width = 0; width < 8; width++)//black spawn
        {
            for (int lenght = -6; lenght > -8; lenght--)
            {
                if (lenght == -7)
                {
                    switch (width)
                    {
                        case 0:
                        case 7:
                            piecePrefab = Black_rook;
                            break;
                        case 1:
                        case 6:
                            piecePrefab = Black_knight;
                            break;
                        case 2:
                        case 5:
                            piecePrefab = Black_bishop;
                            break;
                        case 3:
                            piecePrefab = Black_queen;
                            break;
                        case 4:
                            piecePrefab = Black_king;
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    piecePrefab = Black_pawn;
                }

                Vector3 position = new Vector3(
                    startSquarePosition[0] + width * PieceSpacing,
                    startHeight,
                    startSquarePosition[1] + lenght * PieceSpacing
                );
                GameObject piece = Instantiate(piecePrefab, position, Quaternion.Euler(pieceRotation));
                piece.transform.localScale *= PieceScale;
                piece.AddComponent<BoxCollider>();  // Добавление BoxCollider
                piece.AddComponent<PieceClickHandler>();  // Добавление обработчика кликов
                if ((width == 1 || width == 6) && (lenght == -7))
                {
                    piece.transform.Rotate(0, 0, 180);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}