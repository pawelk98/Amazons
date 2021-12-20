using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardScript : MonoBehaviour
{
    enum Fields
    {
        Empty,
        PawnA,
        PawnB,
        Arrow
    }
    public List<GameObject> pawnPrefabs;
    public GameObject arrowPrefab;
    GameObject[] pawns;
    List<GameObject> arrows;
    Fields[,] board;


    void Start()
    {
        SetUpBoard();
        GenerateAllLegalMoves();
    }

    void SetUpBoard()
    {
        board = new Fields[10, 10];
        arrows = new List<GameObject>();
        pawns = new GameObject[8];

        pawns[0] = Instantiate(pawnPrefabs[0],
        GetScenePosition(new Vector2(2, 2)), Quaternion.identity);
        pawns[0].GetComponent<PawnScript>().boardScript = this;

        board[2,2] = Fields.PawnA;
    }

    void GenerateAllLegalMoves()
    {
        PawnScript pawnScript = pawns[0].GetComponent<PawnScript>();
        pawnScript.GenerateLegalMoves();
    }

    public bool IsActionLegal(Vector2Int position)
    {
        if (position.x >= 0 && position.x < 10
        && position.y >= 0 && position.y < 10
        && board[position.x, position.y] == Fields.Empty)
            return true;

        return false;
    }

    public Vector2 GetScenePosition(Vector2 boardPos)
    {
        return new Vector2((int)(boardPos.x - 5) + 0.5f, (int)(boardPos.y - 5) + 0.5f);
    }

    public Vector2Int GetBoardPosition(Vector2 position)
    {
        return new Vector2Int((int)(position.x + 5), (int)(position.y + 5));
    }

    public void MovePawn(GameObject pawn, Vector2 destinationScenePos)
    {
        PawnScript pawnScript = pawn.GetComponent<PawnScript>();
        Vector2Int currentPos = GetBoardPosition(pawn.transform.position);
        Vector2Int destinationPos = GetBoardPosition(destinationScenePos);
        board[currentPos.x, currentPos.y] = Fields.Empty;
        board[destinationPos.x, destinationPos.y] = pawnScript.player ? Fields.PawnB : Fields.PawnA;
        pawnScript.MovePawn(destinationScenePos);
    }

    public void Shoot(GameObject pawn, Vector2 coords)
    {
        Vector2Int arrowPos = GetBoardPosition(coords);
        board[arrowPos.x, arrowPos.y] = Fields.Arrow;
        GameObject arrow = Instantiate(arrowPrefab, GetScenePosition(arrowPos), Quaternion.identity);
        arrows.Add(arrow);
        pawn.GetComponent<PawnScript>().HideLegalMoves();

        GenerateAllLegalMoves();
    }
}
