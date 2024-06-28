using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PieceClickHandler : MonoBehaviour
{
    public Vector3 pieceRotation = new Vector3(-90, 0, 0);
    public float PieceScale = 1.5f;
    public int PieceSpacing = 4;

    public GameObject RedPad;
    public GameObject YellowPad;
    public GameObject BluePad;
    public Vector3 spawnOffset = new Vector3(0, 0, 0);  // Смещение для нового объекта

    public Transform Parent;

    private GameObject newPiece = null;

    private void OnMouseDown()
    {
        // Проверка очереди хода
        if ((FigurePlacement.CurrentTurn == "White" && !gameObject.name.Contains("White")) ||
            (FigurePlacement.CurrentTurn == "Black" && !gameObject.name.Contains("Black")))
        {
            return; // Игнорируем клик, если не текущая очередь хода
        }

        // Уничтожаем все объекты с тегом "Pad"
        GameObject[] existingPads = GameObject.FindGameObjectsWithTag("Pad");
        foreach (GameObject pad in existingPads)
        {
            Destroy(pad);
        }

        Vector3 spawnPosition = transform.position + spawnOffset;
        string pieceName = gameObject.name;
        switch (pieceName)
        {
            case string a when a.Contains("White_pawn(Clone)"):
                SpawnPawnMoveIndicators(-1);
                break;
            case string a when a.Contains("Black_pawn(Clone)"):
                SpawnPawnMoveIndicators(1);
                break;
            case string a when a.Contains("_rook(Clone)"):
                SpawnLinearMoveIndicators(Vector3.forward);
                SpawnLinearMoveIndicators(Vector3.back);
                SpawnLinearMoveIndicators(Vector3.left);
                SpawnLinearMoveIndicators(Vector3.right);
                break;
            case string a when a.Contains("_bishop(Clone)"):
                SpawnLinearMoveIndicators(new Vector3(1, 0, 1));
                SpawnLinearMoveIndicators(new Vector3(-1, 0, 1));
                SpawnLinearMoveIndicators(new Vector3(1, 0, -1));
                SpawnLinearMoveIndicators(new Vector3(-1, 0, -1));
                break;
            case string a when a.Contains("_knight(Clone)"):
                SpawnKnightMoveIndicators();
                break;
            case string a when a.Contains("_king(Clone)"):
                SpawnKingMoveIndicators();
                break;
            case string a when a.Contains("_queen(Clone)"):
                SpawnLinearMoveIndicators(Vector3.forward);
                SpawnLinearMoveIndicators(Vector3.back);
                SpawnLinearMoveIndicators(Vector3.left);
                SpawnLinearMoveIndicators(Vector3.right);
                SpawnLinearMoveIndicators(new Vector3(1, 0, 1));
                SpawnLinearMoveIndicators(new Vector3(-1, 0, 1));
                SpawnLinearMoveIndicators(new Vector3(1, 0, -1));
                SpawnLinearMoveIndicators(new Vector3(-1, 0, -1));
                break;
        }
    }

    private bool IsOpponentPiece(Collider collider)
    {
        return (gameObject.name.Contains("White") && collider.name.Contains("Black")) ||
               (gameObject.name.Contains("Black") && collider.name.Contains("White"));
    }

    private bool IsPathClear(Vector3 startPosition, Vector3 direction, float distance, out Collider opponentCollider)
    {
        RaycastHit hit;
        if (Physics.Raycast(startPosition, direction, out hit, distance))
        {
            opponentCollider = hit.collider;
            if (opponentCollider.CompareTag("Pad") || opponentCollider.CompareTag("Table"))
            {
                return true; // Игнорируем объекты с тегом "Pad" и "Table"
            }
            return false; // Если есть попадание и это не "Pad" или "Table", путь не свободен
        }
        opponentCollider = null;
        return true; // Путь свободен
    }

    private void SpawnIndicator(Vector3 spawnPosition, GameObject padPrefab)
    {
        newPiece = Instantiate(padPrefab, spawnPosition, Quaternion.Euler(pieceRotation));
        newPiece.transform.localScale *= PieceScale;
        newPiece.tag = "Pad";
        Parent = GameObject.Find(gameObject.name).transform;
        newPiece.transform.SetParent(Parent);
        newPiece.AddComponent<BoxCollider>();
        newPiece.AddComponent<PadClickHandler>();
    }

    private void RedSpawnIndicator(Vector3 spawnPosition, GameObject padPrefab)
    {
        newPiece = Instantiate(padPrefab, spawnPosition, Quaternion.Euler(pieceRotation));
        newPiece.transform.localScale *= PieceScale;
        newPiece.tag = "Pad";
        Parent = GameObject.Find(gameObject.name).transform;
        newPiece.transform.SetParent(Parent);
        newPiece.AddComponent<BoxCollider>();
        BoxCollider boxCollider = newPiece.GetComponent<BoxCollider>();
        boxCollider.size = new Vector3(0.02f, 0.02f, 0.06f);
        Vector3 v = boxCollider.center;
        v.z = 0.03f;
        boxCollider.center = v;
        newPiece.AddComponent<PadClickHandler>();
    }

    private void SpawnPawnMoveIndicators(int direction)
    {
        int maxSteps = gameObject.CompareTag("Used") ? 1 : 2;

        // Шаг вперед
        for (int i = 1; i <= maxSteps; i++)
        {
            Vector3 forwardPosition = transform.position + new Vector3(0, 0, direction * PieceSpacing * i);
            Collider[] forwardColliders = Physics.OverlapBox(forwardPosition, new Vector3(PieceSpacing / 2, PieceSpacing / 2, PieceSpacing / 2));
            bool isForwardFree = true;
            foreach (var collider in forwardColliders)
            {
                if (!collider.CompareTag("Pad") && !collider.CompareTag("Table"))
                {
                    isForwardFree = false;
                    break;
                }
            }

            if (isForwardFree)
            {
                SpawnIndicator(forwardPosition, BluePad);
            }
            else
            {
                break;
            }
        }

        // Проверка и установка красных клеток для съедания по диагонали
        Vector3[] attackOffsets = { new Vector3(-1 * PieceSpacing, 0, direction * PieceSpacing), new Vector3(1 * PieceSpacing, 0, direction * PieceSpacing) };
        foreach (var offset in attackOffsets)
        {
            Vector3 spawnPosition = transform.position + offset;
            if (spawnPosition.z < PieceSpacing * 4 && spawnPosition.z > PieceSpacing * -4 &&
                spawnPosition.x < PieceSpacing * 4 && spawnPosition.x > PieceSpacing * -4)
            {
                Collider[] colliders = Physics.OverlapBox(spawnPosition, new Vector3(PieceSpacing / 2, PieceSpacing / 2, PieceSpacing / 2));
                foreach (var collider in colliders)
                {
                    if (IsOpponentPiece(collider))
                    {
                        RedSpawnIndicator(spawnPosition, RedPad);
                        break;
                    }
                }
            }
        }
    }

    private void SpawnLinearMoveIndicators(Vector3 direction)
    {
        float maxDistance = 7 * PieceSpacing;

        for (float distance = PieceSpacing; distance <= maxDistance; distance += PieceSpacing)
        {
            Vector3 spawnPosition = transform.position + direction * distance;

            if (spawnPosition.z < PieceSpacing * 4 && spawnPosition.z > PieceSpacing * -4 &&
                spawnPosition.x < PieceSpacing * 4 && spawnPosition.x > PieceSpacing * -4)
            {
                Collider[] colliders = Physics.OverlapBox(spawnPosition, new Vector3(PieceSpacing / 2, PieceSpacing / 2, PieceSpacing / 2));
                bool isPositionFree = true;
                foreach (var collider in colliders)
                {
                    if (!collider.CompareTag("Pad") && !collider.CompareTag("Table"))
                    {
                        isPositionFree = false;
                        break;
                    }
                }

                if (isPositionFree)
                {
                    SpawnIndicator(spawnPosition, BluePad);
                }
                else
                {
                    foreach (var collider in colliders)
                    {
                        if (IsOpponentPiece(collider))
                        {
                            RedSpawnIndicator(spawnPosition, RedPad);
                            return;
                        }
                    }
                    break; // Прерываем цикл, если конечная позиция занята
                }
            }
        }
    }

    private void SpawnKnightMoveIndicators()
    {
        Vector3[] knightMoves = new Vector3[]
        {
            new Vector3(2 * PieceSpacing, 0, 1 * PieceSpacing),
            new Vector3(2 * PieceSpacing, 0, -1 * PieceSpacing),
            new Vector3(-2 * PieceSpacing, 0, 1 * PieceSpacing),
            new Vector3(-2 * PieceSpacing, 0, -1 * PieceSpacing),
            new Vector3(1 * PieceSpacing, 0, 2 * PieceSpacing),
            new Vector3(1 * PieceSpacing, 0, -2 * PieceSpacing),
            new Vector3(-1 * PieceSpacing, 0, 2 * PieceSpacing),
            new Vector3(-1 * PieceSpacing, 0, -2 * PieceSpacing)
        };

        foreach (Vector3 move in knightMoves)
        {
            Vector3 spawnPosition = transform.position + move;
            if (spawnPosition.z < PieceSpacing * 4 && spawnPosition.z > PieceSpacing * -4 &&
                spawnPosition.x < PieceSpacing * 4 && spawnPosition.x > PieceSpacing * -4)
            {
                Collider[] colliders = Physics.OverlapBox(spawnPosition, new Vector3(PieceSpacing / 2, PieceSpacing / 2, PieceSpacing / 2));
                bool isPositionFree = true;
                foreach (var collider in colliders)
                {
                    if (!collider.CompareTag("Pad") && !collider.CompareTag("Table"))
                    {
                        isPositionFree = false;
                        break;
                    }
                }

                if (isPositionFree)
                {
                    SpawnIndicator(spawnPosition, BluePad);
                }
                else
                {
                    foreach (var collider in colliders)
                    {
                        if (IsOpponentPiece(collider))
                        {
                            RedSpawnIndicator(spawnPosition, RedPad);
                            break;
                        }
                    }
                }
            }
        }
    }

    private void SpawnKingMoveIndicators()
    {
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue; // Пропустить текущую клетку
                spawnOffset = new Vector3(y * PieceSpacing, 0, x * PieceSpacing);
                Vector3 spawnPosition = transform.position + spawnOffset;
                if (spawnPosition.z < PieceSpacing * 4 && spawnPosition.z > PieceSpacing * -4 &&
                    spawnPosition.x < PieceSpacing * 4 && spawnPosition.x > PieceSpacing * -4)
                {
                    Collider[] colliders = Physics.OverlapBox(spawnPosition, new Vector3(PieceSpacing / 2, PieceSpacing / 2, PieceSpacing / 2));
                    bool isPositionFree = true;
                    foreach (var collider in colliders)
                    {
                        if (!collider.CompareTag("Pad") && !collider.CompareTag("Table"))
                        {
                            isPositionFree = false;
                            break;
                        }
                    }

                    if (isPositionFree)
                    {
                        SpawnIndicator(spawnPosition, BluePad);
                    }
                    else
                    {
                        foreach (var collider in colliders)
                        {
                            if (IsOpponentPiece(collider))
                            {
                                RedSpawnIndicator(spawnPosition, RedPad);
                                break;
                            }
                        }
                    }
                }
            }
        }
    }
}
