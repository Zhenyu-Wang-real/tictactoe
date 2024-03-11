using UnityEngine;

public class TicTacToe : MonoBehaviour
{
    public enum Player { None, Human, AI };

    private Player[,] board;
    private Player currentPlayer;

    void Start()
    {
        InitializeBoard();
        currentPlayer = Player.Human;
    }

    void InitializeBoard()
    {
        board = new Player[3, 3];
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                board[i, j] = Player.None;
            }
        }
    }

    void OnGUI()
    {
        DrawBoard();

        Player winner = CheckForWinner();
        if (winner != Player.None)
        {
            GUILayout.Label("Winner: " + winner);
            return;
        }

        if (currentPlayer == Player.AI)
        {
            AIMove();
            currentPlayer = Player.Human;
        }
    }

    void DrawBoard()
    {
        GUILayout.BeginVertical();

        for (int i = 0; i < 3; i++)
        {
            GUILayout.BeginHorizontal();

            for (int j = 0; j < 3; j++)
            {
                if (GUILayout.Button(GetPlayerSymbol(board[i, j]), GUILayout.Width(50), GUILayout.Height(50)))
                {
                    if (board[i, j] == Player.None)
                    {
                        board[i, j] = Player.Human;
                        currentPlayer = Player.AI;
                    }
                }
            }

            GUILayout.EndHorizontal();
        }

        GUILayout.EndVertical();
    }

    string GetPlayerSymbol(Player player)
    {
        switch (player)
        {
            case Player.Human:
                return "X";
            case Player.AI:
                return "O";
            default:
                return "";
        }
    }

    Player CheckForWinner()
    {
        // Check rows
        for (int i = 0; i < 3; i++)
        {
            if (board[i, 0] != Player.None && board[i, 0] == board[i, 1] && board[i, 1] == board[i, 2])
                return board[i, 0];
        }

        // Check columns
        for (int j = 0; j < 3; j++)
        {
            if (board[0, j] != Player.None && board[0, j] == board[1, j] && board[1, j] == board[2, j])
                return board[0, j];
        }

        // Check diagonals
        if (board[0, 0] != Player.None && board[0, 0] == board[1, 1] && board[1, 1] == board[2, 2])
            return board[0, 0];
        if (board[0, 2] != Player.None && board[0, 2] == board[1, 1] && board[1, 1] == board[2, 0])
            return board[0, 2];

        return Player.None;
    }

    bool IsBoardFull()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (board[i, j] == Player.None)
                    return false;
            }
        }
        return true;
    }

    int MiniMax(Player player, int depth, int alpha, int beta)
    {
        Player winner = CheckForWinner();
        if (winner == Player.AI)
            return 1;
        if (winner == Player.Human)
            return -1;
        if (IsBoardFull())
            return 0;

        if (player == Player.AI)
        {
            int maxEval = int.MinValue;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (board[i, j] == Player.None)
                    {
                        board[i, j] = Player.AI;
                        int eval = MiniMax(Player.Human, depth + 1, alpha, beta);
                        board[i, j] = Player.None;

                        maxEval = Mathf.Max(maxEval, eval);
                        alpha = Mathf.Max(alpha, eval);
                        if (beta <= alpha)
                            break;
                    }
                }
            }
            return maxEval;
        }
        else
        {
            int minEval = int.MaxValue;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (board[i, j] == Player.None)
                    {
                        board[i, j] = Player.Human;
                        int eval = MiniMax(Player.AI, depth + 1, alpha, beta);
                        board[i, j] = Player.None;

                        minEval = Mathf.Min(minEval, eval);
                        beta = Mathf.Min(beta, eval);
                        if (beta <= alpha)
                            break;
                    }
                }
            }
            return minEval;
        }
    }

    void AIMove()
    {
        int bestScore = int.MinValue;
        Vector2 bestMove = new Vector2(-1, -1);

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (board[i, j] == Player.None)
                {
                    board[i, j] = Player.AI;
                    int score = MiniMax(Player.Human, 0, int.MinValue, int.MaxValue);
                    board[i, j] = Player.None;

                    if (score > bestScore)
                    {
                        bestScore = score;
                        bestMove = new Vector2(i, j);
                    }
                }
            }
        }

        if (bestMove.x != -1 && bestMove.y != -1)
            board[(int)bestMove.x, (int)bestMove.y] = Player.AI;
    }
}
