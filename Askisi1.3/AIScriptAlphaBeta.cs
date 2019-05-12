using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using Unity.Collections;

public class AIScriptAlphaBeta : MonoBehaviour
{

    public GameObject[,] board;
    public GameObject[] pieces;

    public Transform blackCapturedPieces;
    public Transform whiteCapturedPieces;

    private int dummyCounter = 0;

    public int[,] whitePawnExtraPoints;
    public int[,] whiteBishopExtraPoints;
    public int[,] whiteKnightExtraPoints;
    public int[,] whiteRookExtraPoints;
    public int[,] whiteQueenExtraPoints;
    public int[,] whiteKingExtraPoints;
    public int[,] whiteKingEndGameExtraPoints;
    public int[,] blackPawnExtraPoints;
    public int[,] blackBishopExtraPoints;
    public int[,] blackKnightExtraPoints;
    public int[,] blackRookExtraPoints;
    public int[,] blackQueenExtraPoints;
    public int[,] blackKingExtraPoints;
    public int[,] blackKingEndGameExtraPoints;

    public bool isEndGame;
    public int depth;

    public int threadsDone;

    // Use this for initialization
    void Start()
    {
        depth = 3;

        whitePawnExtraPoints = new int[8, 8];
        whiteBishopExtraPoints = new int[8, 8];
        whiteKnightExtraPoints = new int[8, 8];
        whiteRookExtraPoints = new int[8, 8];
        whiteQueenExtraPoints = new int[8, 8];
        whiteKingExtraPoints = new int[8, 8];
        whiteKingEndGameExtraPoints = new int[8, 8];

        blackPawnExtraPoints = new int[8, 8];
        blackBishopExtraPoints = new int[8, 8];
        blackKnightExtraPoints = new int[8, 8];
        blackRookExtraPoints = new int[8, 8];
        blackQueenExtraPoints = new int[8, 8];
        blackKingExtraPoints = new int[8, 8];
        blackKingEndGameExtraPoints = new int[8, 8];

        //Pawns
        int[] tmp = { 0, 0, 0, 0, 0, 0, 0, 0, 50, 50, 50, 50, 50, 50, 50, 50, 10, 10, 20, 30, 30, 20, 10, 10, 5, 5, 10, 25, 25, 10, 5, 5, 0, 0, 0, 20, 20, 0, 0, 0, 5, -5, -10, 0, 0, -10, -5, 5, 5, 10, 10, -20, -20, 10, 10, 5, 0, 0, 0, 0, 0, 0, 0, 0 };
        int[] tmpReversed = new int[tmp.Length];
        System.Array.Copy(tmp, tmpReversed, tmp.Length);
        System.Array.Reverse(tmpReversed); //reversing because the points are meant for the white pieces to be at the bottom. On my array they are at the top.

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                whitePawnExtraPoints[i, j] = tmpReversed[i * 8 + j];
                blackPawnExtraPoints[i, j] = tmp[i * 8 + j];
            }
        }

        //Knights
        int[] tmp2 = { -50, -40, -30, -30, -30, -30, -40, -50, -40, -20, 0, 0, 0, 0, -20, -40, -30, 0, 10, 15, 15, 10, 0, -30, -30, 5, 15, 20, 20, 15, 5, -30, -30, 0, 15, 20, 20, 15, 0, -30, -30, 5, 10, 15, 15, 10, 5, -30, -40, -20, 0, 5, 5, 0, -20, -40, -50, -40, -30, -30, -30, -30, -40, -50 };
        int[] tmp2Reversed = new int[tmp2.Length];
        System.Array.Copy(tmp2, tmp2Reversed, tmp2.Length);
        System.Array.Reverse(tmp2Reversed);

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                whiteKnightExtraPoints[i, j] = tmp2Reversed[i * 8 + j];
                blackKnightExtraPoints[i, j] = tmp2[i * 8 + j];
            }
        }

        //Bishops
        int[] tmp3 = { -20, -10, -10, -10, -10, -10, -10, -20, -10, 0, 0, 0, 0, 0, 0, -10, -10, 0, 5, 10, 10, 5, 0, -10, -10, 5, 5, 10, 10, 5, 5, -10, -10, 0, 10, 10, 10, 10, 0, -10, -10, 10, 10, 10, 10, 10, 10, -10, -10, 5, 0, 0, 0, 0, 5, -10, -20, -10, -10, -10, -10, -10, -10, -2 };
        int[] tmp3Reversed = new int[tmp3.Length];
        System.Array.Copy(tmp3, tmp3Reversed, tmp3.Length);
        System.Array.Reverse(tmp3Reversed);

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                whiteBishopExtraPoints[i, j] = tmp3Reversed[i * 8 + j];
                blackBishopExtraPoints[i, j] = tmp3[i * 8 + j];
            }
        }

        //Rooks
        int[] tmp4 = { 0, 0, 0, 0, 0, 0, 0, 0, 5, 10, 10, 10, 10, 10, 10, 5, -5, 0, 0, 0, 0, 0, 0, -5, -5, 0, 0, 0, 0, 0, 0, -5, -5, 0, 0, 0, 0, 0, 0, -5, -5, 0, 0, 0, 0, 0, 0, -5, -5, 0, 0, 0, 0, 0, 0, -5, 0, 0, 0, 5, 5, 0, 0, 0 };
        int[] tmp4Reversed = new int[tmp4.Length];
        System.Array.Copy(tmp4, tmp4Reversed, tmp4.Length);
        System.Array.Reverse(tmp4Reversed);

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                whiteRookExtraPoints[i, j] = tmp4Reversed[i * 8 + j];
                blackRookExtraPoints[i, j] = tmp4[i * 8 + j];
            }
        }

        //Queens
        int[] tmp5 = { -20, -10, -10, -5, -5, -10, -10, -20, -10, 0, 0, 0, 0, 0, 0, -10, -10, 0, 5, 5, 5, 5, 0, -10, -5, 0, 5, 5, 5, 5, 0, -5, 0, 0, 5, 5, 5, 5, 0, -5, -10, 5, 5, 5, 5, 5, 0, -10, -10, 0, 5, 0, 0, 0, 0, -10, -20, -10, -10, -5, -5, -10, -10, -20 };
        int[] tmp5Reversed = new int[tmp5.Length];
        System.Array.Copy(tmp5, tmp5Reversed, tmp5.Length);
        System.Array.Reverse(tmp5Reversed);

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                whiteQueenExtraPoints[i, j] = tmp5Reversed[i * 8 + j];
                blackQueenExtraPoints[i, j] = tmp5[i * 8 + j];
            }
        }

        //King
        int[] tmp6 = { -30, -40, -40, -50, -50, -40, -40, -30, -30, -40, -40, -50, -50, -40, -40, -30, -30, -40, -40, -50, -50, -40, -40, -30, -30, -40, -40, -50, -50, -40, -40, -30, -20, -30, -30, -40, -40, -30, -30, -20, -10, -20, -20, -20, -20, -20, -20, -10, 20, 20, 0, 0, 0, 0, 20, 20, 20, 30, 10, 0, 0, 10, 30, 20 };
        int[] tmp6Reversed = new int[tmp6.Length];
        System.Array.Copy(tmp6, tmp6Reversed, tmp6.Length);
        System.Array.Reverse(tmp6Reversed);

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                whiteKingExtraPoints[i, j] = tmp6Reversed[i * 8 + j];
                blackKingExtraPoints[i, j] = tmp6[i * 8 + j];
            }
        }

        //King End Game
        int[] tmp7 = { -50, -40, -30, -20, -20, -30, -40, -50, -30, -20, -10, 0, 0, -10, -20, -30, -30, -10, 20, 30, 30, 20, -10, -30, -30, -10, 30, 40, 40, 30, -10, -30, -30, -10, 30, 40, 40, 30, -10, -30, -30, -10, 20, 30, 30, 20, -10, -30, -30, -30, 0, 0, 0, 0, -30, -30, -50, -30, -30, -30, -30, -30, -30, -50 };
        int[] tmp7Reversed = new int[tmp7.Length];
        System.Array.Copy(tmp7, tmp7Reversed, tmp7.Length);
        System.Array.Reverse(tmp7Reversed);

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                whiteKingEndGameExtraPoints[i, j] = tmp7Reversed[i * 8 + j];
                blackKingEndGameExtraPoints[i, j] = tmp7[i * 8 + j];
            }
        }




        board = new GameObject[8, 8];
        FillArray();
    }

    public void BlacksTurn()
    {
        depth = 3;
        FillArray();
        //PrintBoard(board);

        GameObject[,] originalBoard = new GameObject[8, 8];
        System.Array.Copy(board, originalBoard, board.Length);

    //    isEndGame = IsEndGame();

        board = minimaxRoot(isEndGame ? depth + 1 : depth, board, false);

        //PrintBoard(originalBoard);
        //PrintBoard(board);
        MakeMove(originalBoard, board);
    }

    public void WhitesTurn()
    {
        depth = 4;
        FillArray();
        //PrintBoard(board);

        GameObject[,] originalBoard = new GameObject[8, 8];
        System.Array.Copy(board, originalBoard, board.Length);

     //   isEndGame = IsEndGame();

        board = minimaxRoot(isEndGame ? depth + 1 : depth, board, true);

        //PrintBoard(board);
        MakeMove(originalBoard, board);
    }

    public void FillArray()
    {
        int counter = 0;
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                board[i, j] = null;
            }
        }
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.GetChild(counter++).position, Vector3.up, out hit, Mathf.Infinity))
                {
                    board[i, j] = hit.transform.gameObject;
                }
            }
        }
    }

    private GameObject[,] minimaxRoot(int depth, GameObject[,] currentBoard, bool isWhite)
    {
        GameObject[,] originalBoard = new GameObject[8, 8];
        System.Array.Copy(currentBoard, originalBoard, currentBoard.Length);

        List<GameObject[,]> legalMoves = getLegalMoves(currentBoard, isWhite);
        float bestMove = Mathf.Infinity;

        GameObject[,] bestMoveFound = new GameObject[8, 8];

        for (var i = 0; i < legalMoves.Count; i++)
        {
            float value;

            currentBoard = legalMoves[i];
            //PrintBoard(currentBoard);
            value = alphaBetaMax(currentBoard, -9999999, 9999999, depth - 1);
            if (value <= bestMove)
            {
                bestMove = value;
                System.Array.Copy(currentBoard, bestMoveFound, currentBoard.Length);
            }
            currentBoard = originalBoard;
        }
        return bestMoveFound;
    }

    private int alphaBetaMax(GameObject[,] currentBoard, int alpha, int beta, int depthleft)
    {
        if (depthleft == 0) return CalculateBoardValue(currentBoard);
        List<GameObject[,]> legalMoves = getLegalMoves(currentBoard, true);
        int score = 0;
        for (int i = 0; i < legalMoves.Count; i++)
        {
            score = alphaBetaMin(legalMoves[i], alpha, beta, depthleft - 1);
            if (score >= beta)
                return beta;
            if (score > alpha)
                alpha = score;
        }
        return alpha;
    }

    private int alphaBetaMin(GameObject[,] currentBoard, int alpha, int beta, int depthleft)
    {
        if (depthleft == 0) return -CalculateBoardValue(currentBoard);
        List<GameObject[,]> legalMoves = getLegalMoves(currentBoard, false);
        int score = 0;
        for (int i = 0; i < legalMoves.Count; i++)
        {
            score = alphaBetaMax(legalMoves[i], alpha, beta, depthleft - 1);
            if (score <= alpha)
                return alpha;
            if (score < beta)
                beta = score;
        }
        return beta;
    }

    private List<GameObject[,]> getLegalMoves(GameObject[,] currentBoard, bool isWhite)
    {
        List<GameObject[,]> legalMoves = new List<GameObject[,]>();
        GameObject[,] originalBoard = currentBoard;

        if (isWhite)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (currentBoard[i, j] != null)
                    {
                        if (currentBoard[i, j].GetComponent<Piece>().isWhite)
                        {
                            if (currentBoard[i, j].GetComponent<Piece>().codeName.Contains("Pawn"))
                            {
                                if (i < 7)
                                {
                                    if (currentBoard[i + 1, j] == null) //If the block forward is available
                                    {
                                        GameObject[,] tempBoard = new GameObject[8, 8];
                                        System.Array.Copy(currentBoard, tempBoard, currentBoard.Length);

                                        tempBoard[i + 1, j] = tempBoard[i, j]; //Moving pawn one block forward
                                        tempBoard[i, j] = null;
                                        legalMoves.Add(tempBoard);
                                    }
                                    if (j < 7) //Pawn can capture sideways
                                    {
                                        if (currentBoard[i + 1, j + 1] != null && !currentBoard[i + 1, j + 1].GetComponent<Piece>().isWhite)
                                        {
                                            GameObject[,] tempBoard = new GameObject[8, 8];
                                            System.Array.Copy(currentBoard, tempBoard, currentBoard.Length);

                                            tempBoard[i + 1, j + 1] = tempBoard[i, j]; //Capturing with pawn
                                            tempBoard[i, j] = null;
                                            legalMoves.Add(tempBoard);
                                        }
                                    }
                                    if (j > 0) //Pawn can capture sideways
                                    {
                                        if (currentBoard[i + 1, j - 1] != null && !currentBoard[i + 1, j - 1].GetComponent<Piece>().isWhite)
                                        {
                                            GameObject[,] tempBoard = new GameObject[8, 8];
                                            System.Array.Copy(currentBoard, tempBoard, currentBoard.Length);

                                            tempBoard[i + 1, j - 1] = tempBoard[i, j]; //Capturing with pawn
                                            tempBoard[i, j] = null;
                                            legalMoves.Add(tempBoard);
                                        }
                                    }
                                }
                                if (i == 1)
                                { //Pawn can move two squares at the beginning
                                    if (currentBoard[i + 1, j] == null && currentBoard[i + 2, j] == null) //If the block forward is available
                                    {
                                        GameObject[,] tempBoard = new GameObject[8, 8];
                                        System.Array.Copy(currentBoard, tempBoard, currentBoard.Length);

                                        tempBoard[i + 2, j] = tempBoard[i, j]; //Moving pawn one block forward
                                        tempBoard[i, j] = null;
                                        legalMoves.Add(tempBoard);
                                    }
                                }
                            }
                            if (currentBoard[i, j].GetComponent<Piece>().codeName.Contains("Knight"))
                            {
                                if ((i + 1 < 8) && (j + 2 < 8) && (currentBoard[i + 1, j + 2] == null || !currentBoard[i + 1, j + 2].GetComponent<Piece>().isWhite))
                                {
                                    GameObject[,] tempBoard = new GameObject[8, 8];
                                    System.Array.Copy(currentBoard, tempBoard, currentBoard.Length);

                                    tempBoard[i + 1, j + 2] = tempBoard[i, j];
                                    tempBoard[i, j] = null;
                                    legalMoves.Add(tempBoard);
                                }
                                if ((i + 2 < 8) && (j + 1 < 8) && (currentBoard[i + 2, j + 1] == null || !currentBoard[i + 2, j + 1].GetComponent<Piece>().isWhite))
                                {
                                    GameObject[,] tempBoard = new GameObject[8, 8];
                                    System.Array.Copy(currentBoard, tempBoard, currentBoard.Length);

                                    tempBoard[i + 2, j + 1] = tempBoard[i, j];
                                    tempBoard[i, j] = null;
                                    legalMoves.Add(tempBoard);
                                }
                                if ((i + 2 < 8) && (j - 1 > -1) && (currentBoard[i + 2, j - 1] == null || !currentBoard[i + 2, j - 1].GetComponent<Piece>().isWhite))
                                {
                                    GameObject[,] tempBoard = new GameObject[8, 8];
                                    System.Array.Copy(currentBoard, tempBoard, currentBoard.Length);

                                    tempBoard[i + 2, j - 1] = tempBoard[i, j];
                                    tempBoard[i, j] = null;
                                    legalMoves.Add(tempBoard);
                                }
                                if ((i + 1 < 8) && (j - 2 > -1) && (currentBoard[i + 1, j - 2] == null || !currentBoard[i + 1, j - 2].GetComponent<Piece>().isWhite))
                                {
                                    GameObject[,] tempBoard = new GameObject[8, 8];
                                    System.Array.Copy(currentBoard, tempBoard, currentBoard.Length);

                                    tempBoard[i + 1, j - 2] = tempBoard[i, j];
                                    tempBoard[i, j] = null;
                                    legalMoves.Add(tempBoard);
                                }
                                if ((i - 1 > -1) && (j - 2 > -1) && (currentBoard[i - 1, j - 2] == null || !currentBoard[i - 1, j - 2].GetComponent<Piece>().isWhite))
                                {
                                    GameObject[,] tempBoard = new GameObject[8, 8];
                                    System.Array.Copy(currentBoard, tempBoard, currentBoard.Length);

                                    tempBoard[i - 1, j - 2] = tempBoard[i, j];
                                    tempBoard[i, j] = null;
                                    legalMoves.Add(tempBoard);
                                }
                                if ((i - 2 > -1) && (j - 1 > -1) && (currentBoard[i - 2, j - 1] == null || !currentBoard[i - 2, j - 1].GetComponent<Piece>().isWhite))
                                {
                                    GameObject[,] tempBoard = new GameObject[8, 8];
                                    System.Array.Copy(currentBoard, tempBoard, currentBoard.Length);

                                    tempBoard[i - 2, j - 1] = tempBoard[i, j];
                                    tempBoard[i, j] = null;
                                    legalMoves.Add(tempBoard);
                                }
                                if ((i - 2 > -1) && (j + 1 < 8) && (currentBoard[i - 2, j + 1] == null || !currentBoard[i - 2, j + 1].GetComponent<Piece>().isWhite))
                                {
                                    GameObject[,] tempBoard = new GameObject[8, 8];
                                    System.Array.Copy(currentBoard, tempBoard, currentBoard.Length);

                                    tempBoard[i - 2, j + 1] = tempBoard[i, j];
                                    tempBoard[i, j] = null;
                                    legalMoves.Add(tempBoard);
                                }
                                if ((i - 1 > -1) && (j + 2 < 8) && (currentBoard[i - 1, j + 2] == null || !currentBoard[i - 1, j + 2].GetComponent<Piece>().isWhite))
                                {
                                    GameObject[,] tempBoard = new GameObject[8, 8];
                                    System.Array.Copy(currentBoard, tempBoard, currentBoard.Length);

                                    tempBoard[i - 1, j + 2] = tempBoard[i, j];
                                    tempBoard[i, j] = null;
                                    legalMoves.Add(tempBoard);
                                }
                            }
                            if (currentBoard[i, j].GetComponent<Piece>().codeName.Contains("Bishop"))
                            {
                                int tmp = 1;
                                while ((i + tmp < 8) && (j + tmp < 8) && (currentBoard[i + tmp, j + tmp] == null || !currentBoard[i + tmp, j + tmp].GetComponent<Piece>().isWhite))
                                {
                                    GameObject[,] tempBoard = new GameObject[8, 8];
                                    System.Array.Copy(currentBoard, tempBoard, currentBoard.Length);

                                    tempBoard[i + tmp, j + tmp] = tempBoard[i, j];
                                    tempBoard[i, j] = null;
                                    legalMoves.Add(tempBoard);

                                    if (currentBoard[i + tmp, j + tmp] != null && !currentBoard[i + tmp, j + tmp].GetComponent<Piece>().isWhite)
                                        break;
                                    tmp++;
                                }
                                tmp = 1;
                                while ((i + tmp < 8) && (j - tmp > -1) && (currentBoard[i + tmp, j - tmp] == null || !currentBoard[i + tmp, j - tmp].GetComponent<Piece>().isWhite))
                                {
                                    GameObject[,] tempBoard = new GameObject[8, 8];
                                    System.Array.Copy(currentBoard, tempBoard, currentBoard.Length);

                                    tempBoard[i + tmp, j - tmp] = tempBoard[i, j];
                                    tempBoard[i, j] = null;
                                    legalMoves.Add(tempBoard);

                                    if (currentBoard[i + tmp, j - tmp] != null && !currentBoard[i + tmp, j - tmp].GetComponent<Piece>().isWhite)
                                        break;
                                    tmp++;
                                }
                                tmp = 1;
                                while ((i - tmp > -1) && (j - tmp > -1) && (currentBoard[i - tmp, j - tmp] == null || !currentBoard[i - tmp, j - tmp].GetComponent<Piece>().isWhite))
                                {
                                    GameObject[,] tempBoard = new GameObject[8, 8];
                                    System.Array.Copy(currentBoard, tempBoard, currentBoard.Length);

                                    tempBoard[i - tmp, j - tmp] = tempBoard[i, j];
                                    tempBoard[i, j] = null;
                                    legalMoves.Add(tempBoard);

                                    if (currentBoard[i - tmp, j - tmp] != null && !currentBoard[i - tmp, j - tmp].GetComponent<Piece>().isWhite)
                                        break;
                                    tmp++;
                                }
                                tmp = 1;
                                while ((i - tmp > -1) && (j + tmp < 8) && (currentBoard[i - tmp, j + tmp] == null || !currentBoard[i - tmp, j + tmp].GetComponent<Piece>().isWhite))
                                {
                                    GameObject[,] tempBoard = new GameObject[8, 8];
                                    System.Array.Copy(currentBoard, tempBoard, currentBoard.Length);

                                    tempBoard[i - tmp, j + tmp] = tempBoard[i, j];
                                    tempBoard[i, j] = null;
                                    legalMoves.Add(tempBoard);

                                    if (currentBoard[i - tmp, j + tmp] != null && !currentBoard[i - tmp, j + tmp].GetComponent<Piece>().isWhite)
                                        break;
                                    tmp++;
                                }
                            }
                            if (currentBoard[i, j].GetComponent<Piece>().codeName.Contains("Rook"))
                            {
                                int tmp = 1;
                                while ((i + tmp < 8) && (currentBoard[i + tmp, j] == null || !currentBoard[i + tmp, j].GetComponent<Piece>().isWhite))
                                {
                                    GameObject[,] tempBoard = new GameObject[8, 8];
                                    System.Array.Copy(currentBoard, tempBoard, currentBoard.Length);

                                    tempBoard[i + tmp, j] = tempBoard[i, j];
                                    tempBoard[i, j] = null;
                                    legalMoves.Add(tempBoard);

                                    if (currentBoard[i + tmp, j] != null && !currentBoard[i + tmp, j].GetComponent<Piece>().isWhite)
                                        break;
                                    tmp++;
                                }
                                tmp = 1;
                                while ((i - tmp > -1) && (currentBoard[i - tmp, j] == null || !currentBoard[i - tmp, j].GetComponent<Piece>().isWhite))
                                {
                                    GameObject[,] tempBoard = new GameObject[8, 8];
                                    System.Array.Copy(currentBoard, tempBoard, currentBoard.Length);

                                    tempBoard[i - tmp, j] = tempBoard[i, j];
                                    tempBoard[i, j] = null;
                                    legalMoves.Add(tempBoard);

                                    if (currentBoard[i - tmp, j] != null && !currentBoard[i - tmp, j].GetComponent<Piece>().isWhite)
                                        break;
                                    tmp++;
                                }
                                tmp = 1;
                                while ((j + tmp < 8) && (currentBoard[i, j + tmp] == null || !currentBoard[i, j + tmp].GetComponent<Piece>().isWhite))
                                {
                                    GameObject[,] tempBoard = new GameObject[8, 8];
                                    System.Array.Copy(currentBoard, tempBoard, currentBoard.Length);

                                    tempBoard[i, j + tmp] = tempBoard[i, j];
                                    tempBoard[i, j] = null;
                                    legalMoves.Add(tempBoard);

                                    if (currentBoard[i, j + tmp] != null && !currentBoard[i, j + tmp].GetComponent<Piece>().isWhite)
                                        break;
                                    tmp++;
                                }
                                tmp = 1;
                                while ((j - tmp > -1) && (currentBoard[i, j - tmp] == null || !currentBoard[i, j - tmp].GetComponent<Piece>().isWhite))
                                {
                                    GameObject[,] tempBoard = new GameObject[8, 8];
                                    System.Array.Copy(currentBoard, tempBoard, currentBoard.Length);

                                    tempBoard[i, j - tmp] = tempBoard[i, j];
                                    tempBoard[i, j] = null;
                                    legalMoves.Add(tempBoard);

                                    if (currentBoard[i, j - tmp] != null && !currentBoard[i, j - tmp].GetComponent<Piece>().isWhite)
                                        break;
                                    tmp++;
                                }
                            }
                            if (currentBoard[i, j].GetComponent<Piece>().codeName.Contains("Queen"))
                            {
                                int tmp = 1;
                                while ((i + tmp < 8) && (currentBoard[i + tmp, j] == null || !currentBoard[i + tmp, j].GetComponent<Piece>().isWhite))
                                {
                                    GameObject[,] tempBoard = new GameObject[8, 8];
                                    System.Array.Copy(currentBoard, tempBoard, currentBoard.Length);

                                    tempBoard[i + tmp, j] = tempBoard[i, j];
                                    tempBoard[i, j] = null;
                                    legalMoves.Add(tempBoard);

                                    if (currentBoard[i + tmp, j] != null && !currentBoard[i + tmp, j].GetComponent<Piece>().isWhite)
                                        break;
                                    tmp++;
                                }
                                tmp = 1;
                                while ((i - tmp > -1) && (currentBoard[i - tmp, j] == null || !currentBoard[i - tmp, j].GetComponent<Piece>().isWhite))
                                {
                                    GameObject[,] tempBoard = new GameObject[8, 8];
                                    System.Array.Copy(currentBoard, tempBoard, currentBoard.Length);

                                    tempBoard[i - tmp, j] = tempBoard[i, j];
                                    tempBoard[i, j] = null;
                                    legalMoves.Add(tempBoard);

                                    if (currentBoard[i - tmp, j] != null && !currentBoard[i - tmp, j].GetComponent<Piece>().isWhite)
                                        break;
                                    tmp++;
                                }
                                tmp = 1;
                                while ((j + tmp < 8) && (currentBoard[i, j + tmp] == null || !currentBoard[i, j + tmp].GetComponent<Piece>().isWhite))
                                {
                                    GameObject[,] tempBoard = new GameObject[8, 8];
                                    System.Array.Copy(currentBoard, tempBoard, currentBoard.Length);

                                    tempBoard[i, j + tmp] = tempBoard[i, j];
                                    tempBoard[i, j] = null;
                                    legalMoves.Add(tempBoard);

                                    if (currentBoard[i, j + tmp] != null && !currentBoard[i, j + tmp].GetComponent<Piece>().isWhite)
                                        break;
                                    tmp++;
                                }
                                tmp = 1;
                                while ((j - tmp > -1) && (currentBoard[i, j - tmp] == null || !currentBoard[i, j - tmp].GetComponent<Piece>().isWhite))
                                {
                                    GameObject[,] tempBoard = new GameObject[8, 8];
                                    System.Array.Copy(currentBoard, tempBoard, currentBoard.Length);

                                    tempBoard[i, j - tmp] = tempBoard[i, j];
                                    tempBoard[i, j] = null;
                                    legalMoves.Add(tempBoard);

                                    if (currentBoard[i, j - tmp] != null && !currentBoard[i, j - tmp].GetComponent<Piece>().isWhite)
                                        break;
                                    tmp++;
                                }
                                //Diagonal
                                tmp = 1;
                                while ((i + tmp < 8) && (j + tmp < 8) && (currentBoard[i + tmp, j + tmp] == null || !currentBoard[i + tmp, j + tmp].GetComponent<Piece>().isWhite))
                                {
                                    GameObject[,] tempBoard = new GameObject[8, 8];
                                    System.Array.Copy(currentBoard, tempBoard, currentBoard.Length);

                                    tempBoard[i + tmp, j + tmp] = tempBoard[i, j];
                                    tempBoard[i, j] = null;
                                    legalMoves.Add(tempBoard);

                                    if (currentBoard[i + tmp, j + tmp] != null && !currentBoard[i + tmp, j + tmp].GetComponent<Piece>().isWhite)
                                        break;
                                    tmp++;
                                }
                                tmp = 1;
                                while ((i + tmp < 8) && (j - tmp > -1) && (currentBoard[i + tmp, j - tmp] == null || !currentBoard[i + tmp, j - tmp].GetComponent<Piece>().isWhite))
                                {
                                    GameObject[,] tempBoard = new GameObject[8, 8];
                                    System.Array.Copy(currentBoard, tempBoard, currentBoard.Length);

                                    tempBoard[i + tmp, j - tmp] = tempBoard[i, j];
                                    tempBoard[i, j] = null;
                                    legalMoves.Add(tempBoard);

                                    if (currentBoard[i + tmp, j - tmp] != null && !currentBoard[i + tmp, j - tmp].GetComponent<Piece>().isWhite)
                                        break;
                                    tmp++;
                                }
                                tmp = 1;
                                while ((i - tmp > -1) && (j - tmp > -1) && (currentBoard[i - tmp, j - tmp] == null || !currentBoard[i - tmp, j - tmp].GetComponent<Piece>().isWhite))
                                {
                                    GameObject[,] tempBoard = new GameObject[8, 8];
                                    System.Array.Copy(currentBoard, tempBoard, currentBoard.Length);

                                    tempBoard[i - tmp, j - tmp] = tempBoard[i, j];
                                    tempBoard[i, j] = null;
                                    legalMoves.Add(tempBoard);

                                    if (currentBoard[i - tmp, j - tmp] != null && !currentBoard[i - tmp, j - tmp].GetComponent<Piece>().isWhite)
                                        break;
                                    tmp++;
                                }
                                tmp = 1;
                                while ((i - tmp > -1) && (j + tmp < 8) && (currentBoard[i - tmp, j + tmp] == null || !currentBoard[i - tmp, j + tmp].GetComponent<Piece>().isWhite))
                                {
                                    GameObject[,] tempBoard = new GameObject[8, 8];
                                    System.Array.Copy(currentBoard, tempBoard, currentBoard.Length);

                                    tempBoard[i - tmp, j + tmp] = tempBoard[i, j];
                                    tempBoard[i, j] = null;
                                    legalMoves.Add(tempBoard);

                                    if (currentBoard[i - tmp, j + tmp] != null && !currentBoard[i - tmp, j + tmp].GetComponent<Piece>().isWhite)
                                        break;
                                    tmp++;
                                }
                            }
                            //IMPLEMENT IS CHECK STATE
                            if (currentBoard[i, j].GetComponent<Piece>().codeName.Contains("King"))
                            {
                                if ((i + 1 < 8) && (currentBoard[i + 1, j] == null || !currentBoard[i + 1, j].GetComponent<Piece>().isWhite))
                                {
                                    GameObject[,] tempBoard = new GameObject[8, 8];
                                    System.Array.Copy(currentBoard, tempBoard, currentBoard.Length);

                                    tempBoard[i + 1, j] = tempBoard[i, j];
                                    tempBoard[i, j] = null;
                                    legalMoves.Add(tempBoard);
                                }
                                if ((j + 1 < 8) && (currentBoard[i, j + 1] == null || !currentBoard[i, j + 1].GetComponent<Piece>().isWhite))
                                {
                                    GameObject[,] tempBoard = new GameObject[8, 8];
                                    System.Array.Copy(currentBoard, tempBoard, currentBoard.Length);

                                    tempBoard[i, j + 1] = tempBoard[i, j];
                                    tempBoard[i, j] = null;
                                    legalMoves.Add(tempBoard);
                                }
                                if ((i - 1 > -1) && (currentBoard[i - 1, j] == null || !currentBoard[i - 1, j].GetComponent<Piece>().isWhite))
                                {
                                    GameObject[,] tempBoard = new GameObject[8, 8];
                                    System.Array.Copy(currentBoard, tempBoard, currentBoard.Length);

                                    tempBoard[i - 1, j] = tempBoard[i, j];
                                    tempBoard[i, j] = null;
                                    legalMoves.Add(tempBoard);
                                }
                                if ((j - 1 > -1) && (currentBoard[i, j - 1] == null || !currentBoard[i, j - 1].GetComponent<Piece>().isWhite))
                                {
                                    GameObject[,] tempBoard = new GameObject[8, 8];
                                    System.Array.Copy(currentBoard, tempBoard, currentBoard.Length);

                                    tempBoard[i, j - 1] = tempBoard[i, j];
                                    tempBoard[i, j] = null;
                                    legalMoves.Add(tempBoard);
                                }
                                //Diagonal
                                if ((i + 1 < 8) && (j + 1 < 8) && (currentBoard[i + 1, j + 1] == null || !currentBoard[i + 1, j + 1].GetComponent<Piece>().isWhite))
                                {
                                    GameObject[,] tempBoard = new GameObject[8, 8];
                                    System.Array.Copy(currentBoard, tempBoard, currentBoard.Length);

                                    tempBoard[i + 1, j + 1] = tempBoard[i, j];
                                    tempBoard[i, j] = null;
                                    legalMoves.Add(tempBoard);
                                }
                                if ((i + 1 < 8) && (j - 1 > -1) && (currentBoard[i + 1, j - 1] == null || !currentBoard[i + 1, j - 1].GetComponent<Piece>().isWhite))
                                {
                                    GameObject[,] tempBoard = new GameObject[8, 8];
                                    System.Array.Copy(currentBoard, tempBoard, currentBoard.Length);

                                    tempBoard[i + 1, j - 1] = tempBoard[i, j];
                                    tempBoard[i, j] = null;
                                    legalMoves.Add(tempBoard);
                                }
                                if ((i - 1 > -1) && (j + 1 < 8) && (currentBoard[i - 1, j + 1] == null || !currentBoard[i - 1, j + 1].GetComponent<Piece>().isWhite))
                                {
                                    GameObject[,] tempBoard = new GameObject[8, 8];
                                    System.Array.Copy(currentBoard, tempBoard, currentBoard.Length);

                                    tempBoard[i - 1, j + 1] = tempBoard[i, j];
                                    tempBoard[i, j] = null;
                                    legalMoves.Add(tempBoard);
                                }
                                if ((i - 1 > -1) && (j - 1 > -1) && (currentBoard[i - 1, j - 1] == null || !currentBoard[i - 1, j - 1].GetComponent<Piece>().isWhite))
                                {
                                    GameObject[,] tempBoard = new GameObject[8, 8];
                                    System.Array.Copy(currentBoard, tempBoard, currentBoard.Length);

                                    tempBoard[i - 1, j - 1] = tempBoard[i, j];
                                    tempBoard[i, j] = null;
                                    legalMoves.Add(tempBoard);
                                }
                            }
                        }
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (currentBoard[i, j] != null)
                    {
                        if (!currentBoard[i, j].GetComponent<Piece>().isWhite)
                        {
                            if (currentBoard[i, j].GetComponent<Piece>().codeName.Contains("Pawn"))
                            {
                                if (i > 0)
                                {
                                    if (currentBoard[i - 1, j] == null) //If the block forward is available
                                    {
                                        GameObject[,] tempBoard = new GameObject[8, 8];
                                        System.Array.Copy(currentBoard, tempBoard, currentBoard.Length);

                                        tempBoard[i - 1, j] = tempBoard[i, j]; //Moving pawn one block forward
                                        tempBoard[i, j] = null;
                                        legalMoves.Add(tempBoard);
                                    }
                                    if (j < 7) //Pawn can capture sideways
                                    {
                                        if (currentBoard[i - 1, j + 1] != null && currentBoard[i - 1, j + 1].GetComponent<Piece>().isWhite)
                                        {
                                            GameObject[,] tempBoard = new GameObject[8, 8];
                                            System.Array.Copy(currentBoard, tempBoard, currentBoard.Length);

                                            tempBoard[i - 1, j + 1] = tempBoard[i, j]; //Capturing with pawn
                                            tempBoard[i, j] = null;
                                            legalMoves.Add(tempBoard);
                                        }
                                    }
                                    if (j > 0) //Pawn can capture sideways
                                    {
                                        if (currentBoard[i - 1, j - 1] != null && currentBoard[i - 1, j - 1].GetComponent<Piece>().isWhite)
                                        {
                                            GameObject[,] tempBoard = new GameObject[8, 8];
                                            System.Array.Copy(currentBoard, tempBoard, currentBoard.Length);

                                            tempBoard[i - 1, j - 1] = tempBoard[i, j]; //Capturing with pawn
                                            tempBoard[i, j] = null;
                                            legalMoves.Add(tempBoard);
                                        }
                                    }
                                }
                                if (i == 6)
                                { //Pawn can move two squares at the beginning
                                    if (currentBoard[i - 1, j] == null && currentBoard[i - 2, j] == null) //If the block forward is available
                                    {
                                        GameObject[,] tempBoard = new GameObject[8, 8];
                                        System.Array.Copy(currentBoard, tempBoard, currentBoard.Length);

                                        tempBoard[i - 2, j] = tempBoard[i, j]; //Moving pawn one block forward
                                        tempBoard[i, j] = null;
                                        legalMoves.Add(tempBoard);
                                    }
                                }
                            }
                            if (currentBoard[i, j].GetComponent<Piece>().codeName.Contains("Knight"))
                            {
                                if ((i + 1 < 8) && (j + 2 < 8) && (currentBoard[i + 1, j + 2] == null || currentBoard[i + 1, j + 2].GetComponent<Piece>().isWhite))
                                {
                                    GameObject[,] tempBoard = new GameObject[8, 8];
                                    System.Array.Copy(currentBoard, tempBoard, currentBoard.Length);

                                    tempBoard[i + 1, j + 2] = tempBoard[i, j];
                                    tempBoard[i, j] = null;
                                    legalMoves.Add(tempBoard);
                                }
                                if ((i + 2 < 8) && (j + 1 < 8) && (currentBoard[i + 2, j + 1] == null || currentBoard[i + 2, j + 1].GetComponent<Piece>().isWhite))
                                {
                                    GameObject[,] tempBoard = new GameObject[8, 8];
                                    System.Array.Copy(currentBoard, tempBoard, currentBoard.Length);

                                    tempBoard[i + 2, j + 1] = tempBoard[i, j];
                                    tempBoard[i, j] = null;
                                    legalMoves.Add(tempBoard);
                                }
                                if ((i + 2 < 8) && (j - 1 > -1) && (currentBoard[i + 2, j - 1] == null || currentBoard[i + 2, j - 1].GetComponent<Piece>().isWhite))
                                {
                                    GameObject[,] tempBoard = new GameObject[8, 8];
                                    System.Array.Copy(currentBoard, tempBoard, currentBoard.Length);

                                    tempBoard[i + 2, j - 1] = tempBoard[i, j];
                                    tempBoard[i, j] = null;
                                    legalMoves.Add(tempBoard);
                                }
                                if ((i + 1 < 8) && (j - 2 > -1) && (currentBoard[i + 1, j - 2] == null || currentBoard[i + 1, j - 2].GetComponent<Piece>().isWhite))
                                {
                                    GameObject[,] tempBoard = new GameObject[8, 8];
                                    System.Array.Copy(currentBoard, tempBoard, currentBoard.Length);

                                    tempBoard[i + 1, j - 2] = tempBoard[i, j];
                                    tempBoard[i, j] = null;
                                    legalMoves.Add(tempBoard);
                                }
                                if ((i - 1 > -1) && (j - 2 > -1) && (currentBoard[i - 1, j - 2] == null || currentBoard[i - 1, j - 2].GetComponent<Piece>().isWhite))
                                {
                                    GameObject[,] tempBoard = new GameObject[8, 8];
                                    System.Array.Copy(currentBoard, tempBoard, currentBoard.Length);

                                    tempBoard[i - 1, j - 2] = tempBoard[i, j];
                                    tempBoard[i, j] = null;
                                    legalMoves.Add(tempBoard);
                                }
                                if ((i - 2 > -1) && (j - 1 > -1) && (currentBoard[i - 2, j - 1] == null || currentBoard[i - 2, j - 1].GetComponent<Piece>().isWhite))
                                {
                                    GameObject[,] tempBoard = new GameObject[8, 8];
                                    System.Array.Copy(currentBoard, tempBoard, currentBoard.Length);

                                    tempBoard[i - 2, j - 1] = tempBoard[i, j];
                                    tempBoard[i, j] = null;
                                    legalMoves.Add(tempBoard);
                                }
                                if ((i - 2 > -1) && (j + 1 < 8) && (currentBoard[i - 2, j + 1] == null || currentBoard[i - 2, j + 1].GetComponent<Piece>().isWhite))
                                {
                                    GameObject[,] tempBoard = new GameObject[8, 8];
                                    System.Array.Copy(currentBoard, tempBoard, currentBoard.Length);

                                    tempBoard[i - 2, j + 1] = tempBoard[i, j];
                                    tempBoard[i, j] = null;
                                    legalMoves.Add(tempBoard);
                                }
                                if ((i - 1 > -1) && (j + 2 < 8) && (currentBoard[i - 1, j + 2] == null || currentBoard[i - 1, j + 2].GetComponent<Piece>().isWhite))
                                {
                                    GameObject[,] tempBoard = new GameObject[8, 8];
                                    System.Array.Copy(currentBoard, tempBoard, currentBoard.Length);

                                    tempBoard[i - 1, j + 2] = tempBoard[i, j];
                                    tempBoard[i, j] = null;
                                    legalMoves.Add(tempBoard);
                                }
                            }
                            if (currentBoard[i, j].GetComponent<Piece>().codeName.Contains("Bishop"))
                            {
                                int tmp = 1;
                                while ((i + tmp < 8) && (j + tmp < 8) && (currentBoard[i + tmp, j + tmp] == null || currentBoard[i + tmp, j + tmp].GetComponent<Piece>().isWhite))
                                {
                                    GameObject[,] tempBoard = new GameObject[8, 8];
                                    System.Array.Copy(currentBoard, tempBoard, currentBoard.Length);

                                    tempBoard[i + tmp, j + tmp] = tempBoard[i, j];
                                    tempBoard[i, j] = null;
                                    legalMoves.Add(tempBoard);

                                    if (currentBoard[i + tmp, j + tmp] != null && currentBoard[i + tmp, j + tmp].GetComponent<Piece>().isWhite)
                                        break;
                                    tmp++;
                                }
                                tmp = 1;
                                while ((i + tmp < 8) && (j - tmp > -1) && (currentBoard[i + tmp, j - tmp] == null || currentBoard[i + tmp, j - tmp].GetComponent<Piece>().isWhite))
                                {
                                    GameObject[,] tempBoard = new GameObject[8, 8];
                                    System.Array.Copy(currentBoard, tempBoard, currentBoard.Length);

                                    tempBoard[i + tmp, j - tmp] = tempBoard[i, j];
                                    tempBoard[i, j] = null;
                                    legalMoves.Add(tempBoard);

                                    if (currentBoard[i + tmp, j - tmp] != null && currentBoard[i + tmp, j - tmp].GetComponent<Piece>().isWhite)
                                        break;
                                    tmp++;
                                }
                                tmp = 1;
                                while ((i - tmp > -1) && (j - tmp > -1) && (currentBoard[i - tmp, j - tmp] == null || currentBoard[i - tmp, j - tmp].GetComponent<Piece>().isWhite))
                                {
                                    GameObject[,] tempBoard = new GameObject[8, 8];
                                    System.Array.Copy(currentBoard, tempBoard, currentBoard.Length);

                                    tempBoard[i - tmp, j - tmp] = tempBoard[i, j];
                                    tempBoard[i, j] = null;
                                    legalMoves.Add(tempBoard);

                                    if (currentBoard[i - tmp, j - tmp] != null && currentBoard[i - tmp, j - tmp].GetComponent<Piece>().isWhite)
                                        break;
                                    tmp++;
                                }
                                tmp = 1;
                                while ((i - tmp > -1) && (j + tmp < 8) && (currentBoard[i - tmp, j + tmp] == null || currentBoard[i - tmp, j + tmp].GetComponent<Piece>().isWhite))
                                {
                                    GameObject[,] tempBoard = new GameObject[8, 8];
                                    System.Array.Copy(currentBoard, tempBoard, currentBoard.Length);

                                    tempBoard[i - tmp, j + tmp] = tempBoard[i, j];
                                    tempBoard[i, j] = null;
                                    legalMoves.Add(tempBoard);

                                    if (currentBoard[i - tmp, j + tmp] != null && currentBoard[i - tmp, j + tmp].GetComponent<Piece>().isWhite)
                                        break;
                                    tmp++;
                                }
                            }
                            if (currentBoard[i, j].GetComponent<Piece>().codeName.Contains("Rook"))
                            {
                                int tmp = 1;
                                while ((i + tmp < 8) && (currentBoard[i + tmp, j] == null || currentBoard[i + tmp, j].GetComponent<Piece>().isWhite))
                                {
                                    GameObject[,] tempBoard = new GameObject[8, 8];
                                    System.Array.Copy(currentBoard, tempBoard, currentBoard.Length);

                                    tempBoard[i + tmp, j] = tempBoard[i, j];
                                    tempBoard[i, j] = null;
                                    legalMoves.Add(tempBoard);

                                    if (currentBoard[i + tmp, j] != null && currentBoard[i + tmp, j].GetComponent<Piece>().isWhite)
                                        break;
                                    tmp++;
                                }
                                tmp = 1;
                                while ((i - tmp > -1) && (currentBoard[i - tmp, j] == null || currentBoard[i - tmp, j].GetComponent<Piece>().isWhite))
                                {
                                    GameObject[,] tempBoard = new GameObject[8, 8];
                                    System.Array.Copy(currentBoard, tempBoard, currentBoard.Length);

                                    tempBoard[i - tmp, j] = tempBoard[i, j];
                                    tempBoard[i, j] = null;
                                    legalMoves.Add(tempBoard);

                                    if (currentBoard[i - tmp, j] != null && currentBoard[i - tmp, j].GetComponent<Piece>().isWhite)
                                        break;
                                    tmp++;
                                }
                                tmp = 1;
                                while ((j + tmp < 8) && (currentBoard[i, j + tmp] == null || currentBoard[i, j + tmp].GetComponent<Piece>().isWhite))
                                {
                                    GameObject[,] tempBoard = new GameObject[8, 8];
                                    System.Array.Copy(currentBoard, tempBoard, currentBoard.Length);

                                    tempBoard[i, j + tmp] = tempBoard[i, j];
                                    tempBoard[i, j] = null;
                                    legalMoves.Add(tempBoard);

                                    if (currentBoard[i, j + tmp] != null && currentBoard[i, j + tmp].GetComponent<Piece>().isWhite)
                                        break;
                                    tmp++;
                                }
                                tmp = 1;
                                while ((j - tmp > -1) && (currentBoard[i, j - tmp] == null || currentBoard[i, j - tmp].GetComponent<Piece>().isWhite))
                                {
                                    GameObject[,] tempBoard = new GameObject[8, 8];
                                    System.Array.Copy(currentBoard, tempBoard, currentBoard.Length);

                                    tempBoard[i, j - tmp] = tempBoard[i, j];
                                    tempBoard[i, j] = null;
                                    legalMoves.Add(tempBoard);

                                    if (currentBoard[i, j - tmp] != null && currentBoard[i, j - tmp].GetComponent<Piece>().isWhite)
                                        break;
                                    tmp++;
                                }
                            }
                            if (currentBoard[i, j].GetComponent<Piece>().codeName.Contains("Queen"))
                            {
                                int tmp = 1;
                                while ((i + tmp < 8) && (currentBoard[i + tmp, j] == null || currentBoard[i + tmp, j].GetComponent<Piece>().isWhite))
                                {
                                    GameObject[,] tempBoard = new GameObject[8, 8];
                                    System.Array.Copy(currentBoard, tempBoard, currentBoard.Length);

                                    tempBoard[i + tmp, j] = tempBoard[i, j];
                                    tempBoard[i, j] = null;
                                    legalMoves.Add(tempBoard);

                                    if (currentBoard[i + tmp, j] != null && currentBoard[i + tmp, j].GetComponent<Piece>().isWhite)
                                        break;
                                    tmp++;
                                }
                                tmp = 1;
                                while ((i - tmp > -1) && (currentBoard[i - tmp, j] == null || currentBoard[i - tmp, j].GetComponent<Piece>().isWhite))
                                {
                                    GameObject[,] tempBoard = new GameObject[8, 8];
                                    System.Array.Copy(currentBoard, tempBoard, currentBoard.Length);

                                    tempBoard[i - tmp, j] = tempBoard[i, j];
                                    tempBoard[i, j] = null;
                                    legalMoves.Add(tempBoard);

                                    if (currentBoard[i - tmp, j] != null && currentBoard[i - tmp, j].GetComponent<Piece>().isWhite)
                                        break;
                                    tmp++;
                                }
                                tmp = 1;
                                while ((j + tmp < 8) && (currentBoard[i, j + tmp] == null || currentBoard[i, j + tmp].GetComponent<Piece>().isWhite))
                                {
                                    GameObject[,] tempBoard = new GameObject[8, 8];
                                    System.Array.Copy(currentBoard, tempBoard, currentBoard.Length);

                                    tempBoard[i, j + tmp] = tempBoard[i, j];
                                    tempBoard[i, j] = null;
                                    legalMoves.Add(tempBoard);

                                    if (currentBoard[i, j + tmp] != null && currentBoard[i, j + tmp].GetComponent<Piece>().isWhite)
                                        break;
                                    tmp++;
                                }
                                tmp = 1;
                                while ((j - tmp > -1) && (currentBoard[i, j - tmp] == null || currentBoard[i, j - tmp].GetComponent<Piece>().isWhite))
                                {
                                    GameObject[,] tempBoard = new GameObject[8, 8];
                                    System.Array.Copy(currentBoard, tempBoard, currentBoard.Length);

                                    tempBoard[i, j - tmp] = tempBoard[i, j];
                                    tempBoard[i, j] = null;
                                    legalMoves.Add(tempBoard);

                                    if (currentBoard[i, j - tmp] != null && currentBoard[i, j - tmp].GetComponent<Piece>().isWhite)
                                        break;
                                    tmp++;
                                }
                                //Diagonal
                                tmp = 1;
                                while ((i + tmp < 8) && (j + tmp < 8) && (currentBoard[i + tmp, j + tmp] == null || currentBoard[i + tmp, j + tmp].GetComponent<Piece>().isWhite))
                                {
                                    GameObject[,] tempBoard = new GameObject[8, 8];
                                    System.Array.Copy(currentBoard, tempBoard, currentBoard.Length);

                                    tempBoard[i + tmp, j + tmp] = tempBoard[i, j];
                                    tempBoard[i, j] = null;
                                    legalMoves.Add(tempBoard);

                                    if (currentBoard[i + tmp, j + tmp] != null && currentBoard[i + tmp, j + tmp].GetComponent<Piece>().isWhite)
                                        break;
                                    tmp++;
                                }
                                tmp = 1;
                                while ((i + tmp < 8) && (j - tmp > -1) && (currentBoard[i + tmp, j - tmp] == null || currentBoard[i + tmp, j - tmp].GetComponent<Piece>().isWhite))
                                {
                                    GameObject[,] tempBoard = new GameObject[8, 8];
                                    System.Array.Copy(currentBoard, tempBoard, currentBoard.Length);

                                    tempBoard[i + tmp, j - tmp] = tempBoard[i, j];
                                    tempBoard[i, j] = null;
                                    legalMoves.Add(tempBoard);

                                    if (currentBoard[i + tmp, j - tmp] != null && currentBoard[i + tmp, j - tmp].GetComponent<Piece>().isWhite)
                                        break;
                                    tmp++;
                                }
                                tmp = 1;
                                while ((i - tmp > -1) && (j - tmp > -1) && (currentBoard[i - tmp, j - tmp] == null || currentBoard[i - tmp, j - tmp].GetComponent<Piece>().isWhite))
                                {
                                    GameObject[,] tempBoard = new GameObject[8, 8];
                                    System.Array.Copy(currentBoard, tempBoard, currentBoard.Length);

                                    tempBoard[i - tmp, j - tmp] = tempBoard[i, j];
                                    tempBoard[i, j] = null;
                                    legalMoves.Add(tempBoard);

                                    if (currentBoard[i - tmp, j - tmp] != null && currentBoard[i - tmp, j - tmp].GetComponent<Piece>().isWhite)
                                        break;
                                    tmp++;
                                }
                                tmp = 1;
                                while ((i - tmp > -1) && (j + tmp < 8) && (currentBoard[i - tmp, j + tmp] == null || currentBoard[i - tmp, j + tmp].GetComponent<Piece>().isWhite))
                                {
                                    GameObject[,] tempBoard = new GameObject[8, 8];
                                    System.Array.Copy(currentBoard, tempBoard, currentBoard.Length);

                                    tempBoard[i - tmp, j + tmp] = tempBoard[i, j];
                                    tempBoard[i, j] = null;
                                    legalMoves.Add(tempBoard);

                                    if (currentBoard[i - tmp, j + tmp] != null && currentBoard[i - tmp, j + tmp].GetComponent<Piece>().isWhite)
                                        break;
                                    tmp++;
                                }
                            }
                            if (currentBoard[i, j].GetComponent<Piece>().codeName.Contains("King"))
                            {
                                if ((i + 1 < 8) && (currentBoard[i + 1, j] == null || currentBoard[i + 1, j].GetComponent<Piece>().isWhite))
                                {
                                    GameObject[,] tempBoard = new GameObject[8, 8];
                                    System.Array.Copy(currentBoard, tempBoard, currentBoard.Length);

                                    tempBoard[i + 1, j] = tempBoard[i, j];
                                    tempBoard[i, j] = null;
                                    legalMoves.Add(tempBoard);
                                }
                                if ((j + 1 < 8) && (currentBoard[i, j + 1] == null || currentBoard[i, j + 1].GetComponent<Piece>().isWhite))
                                {
                                    GameObject[,] tempBoard = new GameObject[8, 8];
                                    System.Array.Copy(currentBoard, tempBoard, currentBoard.Length);

                                    tempBoard[i, j + 1] = tempBoard[i, j];
                                    tempBoard[i, j] = null;
                                    legalMoves.Add(tempBoard);
                                }
                                if ((i - 1 > -1) && (currentBoard[i - 1, j] == null || currentBoard[i - 1, j].GetComponent<Piece>().isWhite))
                                {
                                    GameObject[,] tempBoard = new GameObject[8, 8];
                                    System.Array.Copy(currentBoard, tempBoard, currentBoard.Length);

                                    tempBoard[i - 1, j] = tempBoard[i, j];
                                    tempBoard[i, j] = null;
                                    legalMoves.Add(tempBoard);
                                }
                                if ((j - 1 > -1) && (currentBoard[i, j - 1] == null || currentBoard[i, j - 1].GetComponent<Piece>().isWhite))
                                {
                                    GameObject[,] tempBoard = new GameObject[8, 8];
                                    System.Array.Copy(currentBoard, tempBoard, currentBoard.Length);

                                    tempBoard[i, j - 1] = tempBoard[i, j];
                                    tempBoard[i, j] = null;
                                    legalMoves.Add(tempBoard);
                                }
                                //Diagonal
                                if ((i + 1 < 8) && (j + 1 < 8) && (currentBoard[i + 1, j + 1] == null || currentBoard[i + 1, j + 1].GetComponent<Piece>().isWhite))
                                {
                                    GameObject[,] tempBoard = new GameObject[8, 8];
                                    System.Array.Copy(currentBoard, tempBoard, currentBoard.Length);

                                    tempBoard[i + 1, j + 1] = tempBoard[i, j];
                                    tempBoard[i, j] = null;
                                    legalMoves.Add(tempBoard);
                                }
                                if ((i + 1 < 8) && (j - 1 > -1) && (currentBoard[i + 1, j - 1] == null || currentBoard[i + 1, j - 1].GetComponent<Piece>().isWhite))
                                {
                                    GameObject[,] tempBoard = new GameObject[8, 8];
                                    System.Array.Copy(currentBoard, tempBoard, currentBoard.Length);

                                    tempBoard[i + 1, j - 1] = tempBoard[i, j];
                                    tempBoard[i, j] = null;
                                    legalMoves.Add(tempBoard);
                                }
                                if ((i - 1 > -1) && (j + 1 < 8) && (currentBoard[i - 1, j + 1] == null || currentBoard[i - 1, j + 1].GetComponent<Piece>().isWhite))
                                {
                                    GameObject[,] tempBoard = new GameObject[8, 8];
                                    System.Array.Copy(currentBoard, tempBoard, currentBoard.Length);

                                    tempBoard[i - 1, j + 1] = tempBoard[i, j];
                                    tempBoard[i, j] = null;
                                    legalMoves.Add(tempBoard);
                                }
                                if ((i - 1 > -1) && (j - 1 > -1) && (currentBoard[i - 1, j - 1] == null || currentBoard[i - 1, j - 1].GetComponent<Piece>().isWhite))
                                {
                                    GameObject[,] tempBoard = new GameObject[8, 8];
                                    System.Array.Copy(currentBoard, tempBoard, currentBoard.Length);

                                    tempBoard[i - 1, j - 1] = tempBoard[i, j];
                                    tempBoard[i, j] = null;
                                    legalMoves.Add(tempBoard);
                                }
                            }
                        }
                    }
                }
            }

        }
        return legalMoves;
    }

    public void calculate()
    {
        FillArray();
        Debug.Log(CalculateBoardValue(board));
    }

    private int CalculateBoardValue(GameObject[,] currentBoard)
    {
        int points = 0;
        for (var i = 0; i < 8; i++)
        {
            for (var j = 0; j < 8; j++)
            {
                if (currentBoard[i, j] != null)
                {
                    if (currentBoard[i, j].GetComponent<Piece>().isWhite)
                    {
                        points += currentBoard[i, j].GetComponent<Piece>().value;
                        if (currentBoard[i, j].GetComponent<Piece>().codeName.Contains("Pawn"))
                        {
                            points += whitePawnExtraPoints[i, j];
                        }
                        else if (currentBoard[i, j].GetComponent<Piece>().codeName.Contains("Bishop"))
                        {
                            points += whiteBishopExtraPoints[i, j];
                        }
                        else if (currentBoard[i, j].GetComponent<Piece>().codeName.Contains("Knight"))
                        {
                            points += whiteKnightExtraPoints[i, j];
                        }
                        else if (currentBoard[i, j].GetComponent<Piece>().codeName.Contains("Rook"))
                        {
                            points += whiteRookExtraPoints[i, j];
                        }
                        else if (currentBoard[i, j].GetComponent<Piece>().codeName.Contains("Queen"))
                        {
                            points += whiteQueenExtraPoints[i, j];
                        }
                        else if (currentBoard[i, j].GetComponent<Piece>().codeName.Contains("King"))
                        {
                            if (!isEndGame)
                                points += whiteKingExtraPoints[i, j];
                            else
                                points += whiteKingEndGameExtraPoints[i, j];
                        }
                    }
                    else
                    {
                        points -= currentBoard[i, j].GetComponent<Piece>().value;
                        if (currentBoard[i, j].GetComponent<Piece>().codeName.Contains("Pawn"))
                        {
                            points -= blackPawnExtraPoints[i, j];
                        }
                        else if (currentBoard[i, j].GetComponent<Piece>().codeName.Contains("Bishop"))
                        {
                            points -= blackBishopExtraPoints[i, j];
                        }
                        else if (currentBoard[i, j].GetComponent<Piece>().codeName.Contains("Knight"))
                        {
                            points -= blackKnightExtraPoints[i, j];
                        }
                        else if (currentBoard[i, j].GetComponent<Piece>().codeName.Contains("Rook"))
                        {
                            points -= blackRookExtraPoints[i, j];
                        }
                        else if (currentBoard[i, j].GetComponent<Piece>().codeName.Contains("Queen"))
                        {
                            points -= blackQueenExtraPoints[i, j];
                        }
                        else if (currentBoard[i, j].GetComponent<Piece>().codeName.Contains("King"))
                        {
                            if (!isEndGame)
                                points -= blackKingExtraPoints[i, j];
                            else
                                points -= blackKingEndGameExtraPoints[i, j];
                        }
                    }
                }
            }
        }
        return points;
    }

    private void PrintBoard(GameObject[,] thisBoard)
    {
        string temp = "";
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (thisBoard[i, j] != null)
                    temp += thisBoard[i, j].GetComponent<Piece>().codeName;
                else
                    temp += "0";
                temp += " ";
            }
            temp += "\n";
        }
        Debug.Log(temp);
    }

    private void MakeMove(GameObject[,] oldBoard, GameObject[,] newBoard)
    {
        //We have to find what the last move was
        int[] firstDifference = new int[2]; //i, j
        int[] secondDifference = new int[2]; //i, j
        int differencesCounter = 0;
        for (int i = 0; i < 8; i++)
        {
            if (differencesCounter == 2) //There can't be more than 2 differences
            {
                break;
            }
            for (int j = 0; j < 8; j++)
            {
                if (differencesCounter == 2)
                {
                    break;
                }
                if (oldBoard[i, j] != newBoard[i, j])
                {
                    if (differencesCounter == 0)
                    {
                        firstDifference[0] = i;
                        firstDifference[1] = j;
                    }
                    else
                    {
                        secondDifference[0] = i;
                        secondDifference[1] = j;
                    }
                    differencesCounter++;
                }
            }
        }

        if (board[firstDifference[0], firstDifference[1]] == null) //This means that the piece was moved from this square, to the square of the second difference
        {
            if (oldBoard[secondDifference[0], secondDifference[1]] != null)
            {
                oldBoard[secondDifference[0], secondDifference[1]].transform.SendMessage("Capture");
            }
            StartCoroutine(MoveObject(oldBoard[firstDifference[0], firstDifference[1]].transform, new Vector3(transform.GetChild(secondDifference[0] * 8 + secondDifference[1]).position.x, oldBoard[firstDifference[0], firstDifference[1]].transform.position.y, transform.GetChild(secondDifference[0] * 8 + secondDifference[1]).position.z), 1f));
        }
        else //This means that the piece was moved to this square, from the square of the second difference
        {
            if (oldBoard[firstDifference[0], firstDifference[1]] != null)
            {
                oldBoard[firstDifference[0], firstDifference[1]].transform.SendMessage("Capture");
            }
            StartCoroutine(MoveObject(oldBoard[secondDifference[0], secondDifference[1]].transform, new Vector3(transform.GetChild(firstDifference[0] * 8 + firstDifference[1]).position.x, oldBoard[secondDifference[0], secondDifference[1]].transform.position.y, transform.GetChild(firstDifference[0] * 8 + firstDifference[1]).position.z), 1f));
        }
    }

    private bool IsInCheck(GameObject[,] currentBoard, bool isWhite)
    {
        return false;
    }

    private bool IsEndGame()
    {
        int pieces = 0;
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (board[i, j] != null && !board[i, j].GetComponent<Piece>().codeName.Contains("King"))
                {
                    pieces += board[i, j].GetComponent<Piece>().value / 100;
                }
            }
        }
        if (pieces <= 14)
            return true;
        return false;
    }

    IEnumerator MoveObject(Transform obj, Vector3 target, float overTime)
    {
        obj.GetComponent<Rigidbody>().isKinematic = true;
        foreach (Collider c in obj.GetComponents<Collider>()) //Disabling colliders so pawns dont collide when AI is moving them
        {
            c.enabled = false;
        }
        target = new Vector3(target.x, target.y + 0.4f, target.z);
        float startTime = Time.time;
        while (Time.time < startTime + overTime / 3f)
        {
            obj.position = Vector3.Lerp(obj.position, target, (Time.time - startTime) / overTime);
            yield return null;
        }
        obj.position = target;
        foreach (Collider c in obj.GetComponents<Collider>())
        {
            c.enabled = true;
        }
        obj.GetComponent<Rigidbody>().isKinematic = false;
    }
}