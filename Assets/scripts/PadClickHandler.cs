using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PadClickHandler : MonoBehaviour
{
    public GameObject Piece;
    private Transform parentPiece;

    private void OnMouseDown()
    {
        Piece = this.gameObject;
        parentPiece = Piece.transform.parent;

        if (Piece.name.Contains("Blue"))
        {
            // ��������� ��� ��� ����������
            parentPiece.position = Piece.transform.position;
            parentPiece.gameObject.tag = "Used";
            FigurePlacement.CurrentTurn = (FigurePlacement.CurrentTurn == "White") ? "Black" : "White";
        }
        else
        {
            // ���������, ���� �� ��������� ������ �� ������� �������� �������
            Collider[] colliders = Physics.OverlapSphere(Piece.transform.position, 0.1f);
            foreach (Collider collider in colliders)
            {
                if (collider.gameObject != Piece && collider.gameObject.tag != "Pad" && collider.gameObject.tag != "Table")
                {
                    Destroy(collider.gameObject);
                }
            }
            // ���������� ������������ ������ �� ������� �������� �������
            parentPiece.position = Piece.transform.position;
            parentPiece.gameObject.tag = "Used";
            FigurePlacement.CurrentTurn = (FigurePlacement.CurrentTurn == "White") ? "Black" : "White";

        }

        ClearPads();
    }

    private void ClearPads()
    {
        GameObject[] pads = GameObject.FindGameObjectsWithTag("Pad");
        foreach (GameObject pad in pads)
        {
            Destroy(pad);
        }
    }
}