namespace Chess_final
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ChessGame  game = new ChessGame();
            game.Play();
        }
    }
    class ChessGame
    {
        ChessPiece[,] board;
        int _50movesCount; 
        int MoveCount;
        ChessPiece[][,] boardHistory;
        public ChessGame()
        {
            this.board = new ChessPiece[8,8];
            this._50movesCount = 0;
            this.MoveCount = 0;
            boardHistory = new ChessPiece[50][,];
        }
        private void Init()
        {
            board = new ChessPiece[,]{{new Toora(false),new Knight(false),new Bishop(false),new Queen(false),new King(false, "04"),new Bishop(false),new Knight(false),new Toora(false)},
                                      {new Pawn(false),new Pawn(false),new Pawn(false),new Pawn(false),new Pawn(false),new Pawn(false),new Pawn(false),new Pawn(false)},
                                      {new EmptyPlace(false),new EmptyPlace(false),new EmptyPlace(false),new EmptyPlace(false),new EmptyPlace(false),new EmptyPlace(false),new EmptyPlace(false),new EmptyPlace(false)},
                                      {new EmptyPlace(false),new EmptyPlace(false),new EmptyPlace(false),new EmptyPlace(false),new EmptyPlace(false),new EmptyPlace(false),new EmptyPlace(false),new EmptyPlace(false)},
                                      {new EmptyPlace(false),new EmptyPlace(false),new EmptyPlace(false),new EmptyPlace(false),new EmptyPlace(false),new EmptyPlace(false),new EmptyPlace(false),new EmptyPlace(false)},
                                      {new EmptyPlace(false),new EmptyPlace(false),new EmptyPlace(false),new EmptyPlace(false),new EmptyPlace(false),new EmptyPlace(false),new EmptyPlace(false),new EmptyPlace(false) },
                                      {new Pawn(true),new Pawn(true),new Pawn(true),new Pawn(true),new Pawn(true),new Pawn(true),new Pawn(true),new Pawn(true)},
                                      {new Toora(true),new Knight(true),new Bishop(true),new Queen(true),new King(true, "74"),new Bishop(true),new Knight(true),new Toora(true)}}; 
        }
        private string GetUserInput(bool IsTurnWhite)
        {
            string turnFor = (IsTurnWhite) ? "White" : "Black";
            Console.WriteLine();
            Console.WriteLine(turnFor + " turn");
            Console.WriteLine("Enter your move:");
            string positionToReturn = Console.ReadLine();
            positionToReturn = positionToReturn.Trim();
            return positionToReturn;
        }
        private int ConvertRowCharacterToInt(char row)
        {
            int idxToReturn;

            switch (row)
            {
                case '1':
                    idxToReturn = 7;
                    break;
                case '2':
                    idxToReturn = 6;
                    break;
                case '3':
                    idxToReturn = 5;
                    break;
                case '4':
                    idxToReturn = 4;
                    break;
                case '5':
                    idxToReturn = 3;
                    break;
                case '6':
                    idxToReturn = 2;
                    break;
                case '7':
                    idxToReturn = 1;
                    break;
                case '8':
                    idxToReturn = 0;
                    break;
                default:
                    idxToReturn = -1;
                    break;
            }
            return idxToReturn;
        }
        private int ConvertColCharacterToInt(char col)
        {
            int idxToReturn;

            switch (col)
            {
                case 'A':
                    idxToReturn = 0;
                    break;
                case 'B':
                    idxToReturn = 1;
                    break;
                case 'C':
                    idxToReturn = 2;
                    break;
                case 'D':
                    idxToReturn = 3;
                    break;
                case 'E':
                    idxToReturn = 4;
                    break;
                case 'F':
                    idxToReturn = 5;
                    break;
                case 'G':
                    idxToReturn = 6;
                    break;
                case 'H':
                    idxToReturn = 7;
                    break;
                default:
                    idxToReturn = -1;
                    break;
            }
            return idxToReturn;
        }
        private bool IsLegalInput(string position, int current_row, int current_col, int new_row, int new_col, bool IsTurnWhite)
        {
            if (position.Length != 4 || new_col == -1 || new_row == -1 || current_col == -1 || current_row == -1)
            {
                Console.WriteLine();
                Console.WriteLine("Not Valid idx");
                return false;
            }
            else if (IsTurnWhite) //אם תור הלבן
            {
                if (!board[current_row, current_col].GetIsWhiteColor()) // אם רוצה להזיז שחקן שחור או ריק
                    return false;
                else if (board[new_row, new_col].GetIsWhiteColor()) //אם במיקום החדש כלי של אותו השחקן
                    return false;
            }
            else if (!IsTurnWhite) //אם תור השחור
            {
                if (board[current_row,current_col].GetIsWhiteColor() || board[current_row,current_col].ToString() == "-") //אם רוצה להזיז שחקן לבן או מקום ריק
                    return false;
                else if (!board[new_row, new_col].GetIsWhiteColor() && board[new_row, new_col].ToString() != "-") //אם במיקום החדש כלי של אותו השחקן
                    return false;
            }
            return true;
        }
       
        private string FindKing(bool IsTurnWhite)
        {
            bool turn = (IsTurnWhite) ? true : false;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if ((board[i, j] is King && board[i, j].GetIsWhiteColor() && turn) ||
                        (board[i, j] is King && !board[i, j].GetIsWhiteColor() && !turn))
                        return ((King)board[i, j]).GetPositionKing();
                }
            }
            return "";
        }
        private ChessPiece[,] CopyBoard()
        {
            ChessPiece[,] board_copy = new ChessPiece[8,8];
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    board_copy[i, j] = board[i, j].Copy();
                }
            }
            return board_copy;
        }
        public override string ToString()
        {
            string result = "";
            int k = 8;
            for (int i = 0; i < board.GetLength(0); i++)
            {
                result+=result+(k + " ");
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    result+=result+(board[i, j].ToString() + (board[i, j].ToString().Length == 2 ? " " : "  "));
                }
                result += result + "\n";
                k--;
            }
            result+=result+("  A  B  C  D  E  F  G  H\n");
            result+=result+("===========================\n");
            return result;
        }
        private void PrintBoard()
        {
            int k = 8;
            for (int i = 0; i < board.GetLength(0); i++)
            {
                Console.Write(k + " ");
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    Console.Write(board[i, j].ToString() + (board[i, j].ToString().Length == 2 ? " " : "  "));
                }
                Console.WriteLine();
                k--;
            }
            Console.WriteLine("  A  B  C  D  E  F  G  H");
            Console.WriteLine("===========================");
        }
        private void PromotePawns(ChessPiece[,] board)
        {
            ReplacePawn(board, 0, true, "WR", "WN", "WB", "WQ");
            ReplacePawn(board, 7, false, "BR", "BN", "BB", "BQ");
        }
        private void ReplacePawn(ChessPiece[,] board, int row, bool isWhite, string Toora, string knight, string bishop, string queen)
        {
            for (int i = 0; i < 8; i++)
            {
                string pawn = isWhite ? "WP" : "BP";
                string message = (isWhite) ? "Choose a replacement piece please:WR/WN/WB/WQ:" : "Choose a replacement piece please:BR/BN/BB/BQ:";
                if (board[row, i].ToString() == pawn)
                {
                    bool res_input = false;
                    while (!res_input)
                    {
                        Console.WriteLine(message);
                        string piece = Console.ReadLine().Trim();

                        if (piece == Toora)
                        {
                            board[row, i] = new Toora(isWhite);
                            res_input = true;
                        }
                        else if (piece == knight)
                        {
                            board[row, i] = new Knight(isWhite);
                            res_input = true;
                        }
                        else if (piece == bishop)
                        {
                            board[row, i] = new Bishop(isWhite);
                            res_input = true;
                        }
                        else if (piece == queen)
                        {
                            board[row, i] = new Queen(isWhite);
                            res_input = true;
                        }
                        else 
                        {
                            Console.WriteLine("You can't switch to this piece, please try again.");
                        }
                    }
                }
            }
        }
        private void UnDoMove(ChessPiece[,] board_copy)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    board[i, j] = board_copy[i, j].Copy();
                    if (board_copy[i, j] is King)
                        ((King)board[i, j]).SetPositionKing(i + "" + j);
                }
            }
        }
        private void Castling(int current_row, int current_col, int new_row, int new_col)//פונקציה שמזיזה את הצריח למקום הנכון במידה והיתה הצרחה
        {
            if (board[new_row, new_col].ToString() == "WK" && (current_col + 2 == new_col || current_col - 2 == new_col)) // הצרחה
            {
                EmptyPlace empty = new EmptyPlace(false);
                bool goToLeft = (current_col - 2 == new_col) ? true : false;
                ((King)board[new_row, new_col]).SetLastMove(MoveCount);
                if (goToLeft)
                {
                    board[7, 3] = board[7, 0].Copy();
                    board[7, 0] = empty.Copy();
                    ((Toora)board[7, 3]).SetLastMove(MoveCount);
                }
                else
                {
                    board[7, 5] = board[7, 7].Copy();
                    board[7, 7] = empty.Copy();
                    ((Toora)board[7, 5]).SetLastMove(MoveCount);
                }
            }
            else if (board[new_row, new_col].ToString() == "BK" && (current_col + 2 == new_col || current_col - 2 == new_col))
            {
                EmptyPlace empty = new EmptyPlace(false);
                bool goToLeft = (current_col - 2 == new_col) ? true : false;
                ((King)board[new_row, new_col]).SetLastMove(MoveCount);
                if (goToLeft)
                {
                    board[0, 3] = board[0, 0].Copy();
                    board[0, 0] = empty.Copy();
                    ((Toora)board[0, 3]).SetLastMove(MoveCount);
                }
                else
                {
                    board[0, 5] = board[0, 7].Copy();
                    board[0, 7] = empty.Copy();
                    ((Toora)board[0, 5]).SetLastMove(MoveCount);
                }
            }
        }
        private void DoMove(int current_row, int current_col, int new_row, int new_col)
        {
            EmptyPlace empty = new EmptyPlace(false);
            if (board[new_row, new_col].ToString() != "-")
                _50movesCount = 0;
            else if (board[new_row, new_col].ToString() == "-" && board[current_row, current_col].ToString() != "WP" &&
                board[current_row, current_col].ToString() != "BP")
                _50movesCount++;
            if ((current_col + 1 == new_col || current_col - 1 == new_col) && current_row - 1 == new_row &&
                    board[current_row, new_col].ToString() == "BP" && board[new_row, new_col].ToString() == "-" && board[current_row,current_col].ToString() == "WP") //במקרה של en passant
            {
                ((Pawn)board[current_row, current_col]).SetDoEnPassant(true);
                board[current_row, new_col] = empty;
            }
            else if ((current_col + 1 == new_col || current_col - 1 == new_col) && current_row + 1 == new_row &&
                board[current_row, new_col].ToString() == "WP" && board[new_row, new_col].ToString() == "-" && board[current_row, current_col].ToString() == "BP") //במקרה של en passant
            {
                ((Pawn)board[current_row, current_col]).SetDoEnPassant(true);
                board[current_row, new_col] = empty;
            }

            EmptyPlace emptyPlace = new EmptyPlace(false);
            board[new_row, new_col] = board[current_row, current_col].Copy();
            board[current_row, current_col] = emptyPlace.Copy();
            if (board[new_row, new_col] is King)
                ((King)board[new_row, new_col]).SetPositionKing(new_row + "" + new_col);
            Castling(current_row, current_col, new_row, new_col);
        }
        private bool IsLegalPlace(int current_row, int current_col, int new_row, int new_col, bool isTurnWhite)
        {
            if ((isTurnWhite && board[new_row, new_col].GetIsWhiteColor() == board[current_row, current_col].GetIsWhiteColor() && board[new_row, new_col].ToString() != "-") ||
                     (!(isTurnWhite) && board[new_row, new_col].GetIsWhiteColor() == board[current_row, current_col].GetIsWhiteColor() && board[new_row, new_col].ToString() != "-") ||
                     (isTurnWhite && !board[current_row, current_col].GetIsWhiteColor()) ||
                     (!(isTurnWhite) && board[current_row, current_col].GetIsWhiteColor()))
            {
                return false;
            }
            return true;
        }
        private void UpDateBoardAndVariablesAfterMoves(int current_row, int current_col, int new_row, int new_col)
        {
            if (board[new_row, new_col].ToString() == "WP" || board[new_row, new_col].ToString() == "BP")
            {
                _50movesCount = 0;
            }
            if (board[new_row, new_col] is King)
                ((King)board[new_row, new_col]).SetLastMove(MoveCount);
            else if (board[new_row, new_col] is Toora)
                ((Toora)board[new_row, new_col]).SetLastMove(MoveCount);
            else if (board[new_row, new_col] is Pawn)
                ((Pawn)board[new_row, new_col]).SetLastMove(MoveCount);
            PromotePawns(board);
        }
        public bool IsDrawKingVsKing() //מלך מול מלך
        {
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    if (board[i, j].ToString() != "-")
                    {
                        if (board[i, j].ToString() != "WK" && board[i, j].ToString() != "BK")
                        {
                            return false; //אין תיקו
                        }
                    }
                }
            }
            Console.WriteLine("Is Draw KingVsKing");
            return true; // תיקו
        }
        public bool IsDraw50Move() //50 תורות ללא תזוזה או אכילה ברגלי
        {
            if (_50movesCount == 50)
                return true;
            return false;
        }
        public bool IsDrawStalemate(bool isTurnWhite, int moveCount) // פט
        {
            string idxKing = FindKing(isTurnWhite);
            if (!Chess(idxKing, moveCount))
            {
                ChessPiece[,] board_copy = CopyBoard();
                for (int current_row = 0; current_row < 8; current_row++)
                {
                    for (int current_col = 0; current_col < 8; current_col++)
                    {
                        if (board[current_row, current_col].GetIsWhiteColor() == isTurnWhite)
                        {
                            for (int to_row = 0; to_row < 8; to_row++)
                            {
                                for (int to_col = 0; to_col < 8; to_col++)
                                {
                                    if (IsLegalPlace(current_row, current_col, to_row, to_col, isTurnWhite) &&
                                       board[current_row, current_col].IsLegalMove(board, current_row, current_col, to_row, to_col, moveCount))
                                    {
                                        if (board[to_row, to_col] is King)
                                            continue;
                                        DoMove(current_row, current_col, to_row, to_col);
                                        if (!Chess(FindKing(isTurnWhite), moveCount))
                                        {
                                            UnDoMove(board_copy);
                                            return false; //אין תיקו
                                        }
                                        UnDoMove(board_copy);
                                    }
                                }
                            }

                        }
                    }
                }
                Console.WriteLine("Is Draw Stalemate");
                return true;
            }
            return false;
        }
        public bool IsDrawKingAndBishopVsKing() // מלך ורץ מול מלך
        {
            bool foundWK = false;
            bool foundBK = false;
            bool foundWB = false;
            bool foundBB = false;

            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    if (board[i, j].ToString() != "-")
                    {
                        if (board[i, j].ToString() == "WK")
                            foundWK = true;
                        else if (board[i, j].ToString() == "BK")
                            foundBK = true;
                        else if (board[i, j].ToString() == "WB")
                            foundWB = true;
                        else if (board[i, j].ToString() == "BB")
                            foundBB = true;
                        else
                            return false;
                    }
                }
            }
            if (foundWK && foundBK && (foundWB || foundBB))
                return true; // תיקו

            return false; // אין תיקו
        }
        public bool IsDrawThreefoldRepetition(bool isLegalMove) // חזרה משולשת
        {
            if (!isLegalMove)
                return false;
            // יצירת עותק של מצב הלוח הנוכחי
            ChessPiece[,] boardCopy = CopyBoard();

            for (int i = 0; i < 50; i++)
            {
                if (boardHistory[i] == null)
                {
                    boardHistory[i] = new ChessPiece[8, 8];
                    for (int k = 0; k < 8; k++)
                    {
                        for (int j = 0; j < 8; j++)
                        {
                            boardHistory[i][k, j] = boardCopy[k, j].Copy();
                        }
                    }
                    break; 
                }
            }
            for (int i = 0; i < boardHistory.Length; i++)
            {
                if (boardHistory[i] != null)
                {
                    int counterBoardsAreTheSame = 1;
                    for (int j = 0; j < boardHistory.Length; j++)
                    {
                        if (i != j && boardHistory[j] != null)
                        {
                            if (AreBoardsEqual(boardHistory[i], boardHistory[j]))
                            {
                                counterBoardsAreTheSame++;
                            }
                        }
                    }
                    if (counterBoardsAreTheSame == 3) 
                    {
                        Console.WriteLine("Is Draw ThreefoldRepetition");
                        return true; // יש תיקו
                    }
                }
            }

            return false; // אין תיקו
        }
        private bool AreBoardsEqual(ChessPiece[,] board1, ChessPiece[,] board2)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if ((board1[i, j] is Pawn && board2[i, j] is Pawn) && (board1[i, j].GetIsWhiteColor() ==  board2[i, j].GetIsWhiteColor()) &&
                        ((Pawn)board1[i, j]).GetDoEnPassant() == ((Pawn)board2[i, j]).GetDoEnPassant())
                        continue;
                    else if (board1[i, j] is Toora && board2[i, j] is Toora && board1[i, j].GetIsWhiteColor() == board2[i, j].GetIsWhiteColor() &&
                        (((Toora)board1[i,j]).GetLastMove() == -1 && ((Toora)board2[i, j]).GetLastMove() == -1 ||
                        ((Toora)board1[i, j]).GetLastMove() != -1 && ((Toora)board2[i, j]).GetLastMove() != -1))
                        continue;
                    else if (board1[i, j] is King && board2[i, j] is King && board1[i, j].GetIsWhiteColor() == board2[i, j].GetIsWhiteColor() &&
                        ((King)board1[i, j]).GetPositionKing() == ((King)board2[i, j]).GetPositionKing() &&
                        (((King)board1[i, j]).GetLastMove() == -1 && ((King)board2[i, j]).GetLastMove() == -1 ||
                        ((King)board1[i, j]).GetLastMove() != -1 && ((King)board2[i, j]).GetLastMove() != -1))
                        continue;
                    else if (board1[i, j].ToString() == board2[i, j].ToString() && board1[i, j].GetIsWhiteColor() == board2[i, j].GetIsWhiteColor()
                        && !(board1[i, j] is King) && !(board1[i, j] is Toora) && !(board1[i, j] is Pawn) && 
                        !(board2[i, j] is King) && !(board2[i, j] is Toora) && !(board2[i, j] is Pawn))
                        continue;
                    else
                        return false;
                }
            }
            return true; // הלוחות זהים
        }
        private bool Chess(string idx_king, int moveCount)
        {
            int rowKing = idx_king[0] - '0'; //המרת תו למספר שלם
            int colKing = idx_king[1] - '0';
            bool IsWhite = (board[rowKing, colKing].GetIsWhiteColor()) ? true : false;

            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    if (board[i, j].GetIsWhiteColor() != IsWhite && board[i, j].ToString() != "-")
                    {
                        if (board[i, j].IsLegalMove(board, i, j, rowKing, colKing, moveCount))
                        {
                            return true; //יש שח
                        }
                    }
                }
            }
            return false; //אין שח
        }
        private bool CheckMate(ChessPiece[,] board, bool isTurnWhite,int moveCount)
        {
            if (!Chess(FindKing(isTurnWhite), moveCount))
                return false;
            bool mate;
            ChessPiece[,] board_copy = CopyBoard();
            for (int current_row = 0; current_row < 8; current_row++)
                for (int current_col = 0; current_col < 8; current_col++)
                    for (int to_row = 0; to_row < 8; to_row++)
                        for (int to_col = 0; to_col < 8; to_col++)    
                            if (IsLegalPlace(current_row, current_col, to_row, to_col, isTurnWhite) &&
                                board[current_row, current_col].IsLegalMove(board, current_row, current_col, to_row, to_col, moveCount))
                            {
                                if (board[to_row, to_col] is King)
                                {
                                    continue;
                                }
                                DoMove(current_row, current_col, to_row, to_col);
                                mate = Chess(FindKing(isTurnWhite), moveCount);
                                UnDoMove(board_copy);
                                if (!mate)
                                    return false; //אין מט
                            }
            return true; // יש מט
        }
        public void Play()
        {
            Init();
            bool IsWhiteTurn = true;
            bool endGame = false;
            bool isLegal = true;
            PrintBoard();

            while (!endGame)
            {
                if (CheckMate(board, IsWhiteTurn, MoveCount))
                {
                    Console.WriteLine("CheckMate!!!");
                    return;
                }
                if (IsDrawThreefoldRepetition(isLegal) || IsDraw50Move() || IsDrawKingVsKing() || IsDrawKingAndBishopVsKing() || IsDrawStalemate(IsWhiteTurn, MoveCount))
                {
                    Console.WriteLine("Is Draw!");
                    return;
                }
                if (Chess(FindKing(IsWhiteTurn), MoveCount))
                {
                    Console.WriteLine("You are threatened with chess!!!");
                }
                string position = GetUserInput(IsWhiteTurn);
                int current_row = 0;
                int current_col = 0;
                int new_col = 0;
                int new_row = 0;
                if (position.Length == 4)
                {
                    current_row = ConvertRowCharacterToInt(position[1]);
                    current_col = ConvertColCharacterToInt(position[0]);
                    new_col = ConvertColCharacterToInt(position[2]);
                    new_row = ConvertRowCharacterToInt(position[3]);
                }
                else
                    isLegal = false;
                ChessPiece[,] boardForUnDoMove = CopyBoard();
                if (!IsLegalInput(position, current_row, current_col, new_row, new_col, IsWhiteTurn))
                {
                    isLegal = false;
                    continue;
                }
                if (!board[current_row, current_col].IsLegalMove(board, current_row, current_col, new_row, new_col, MoveCount))
                {
                    isLegal = false;
                    continue;
                }
                isLegal = true;
                if (board[current_row, current_col].IsLegalMove(board, current_row, current_col, new_row, new_col, MoveCount) &&
                    Chess(FindKing(IsWhiteTurn), MoveCount))
                {
                    bool somebodyEat = (board[new_row, new_col].ToString() != "-") ? true : false;
                    DoMove(current_row, current_col, new_row, new_col);
                    if (Chess(FindKing(IsWhiteTurn), MoveCount))
                    {
                        Console.WriteLine("Chess!!");
                        UnDoMove(boardForUnDoMove);
                        isLegal = false;
                        if (board[current_row,current_col].ToString() == "WP" || board[current_row, current_col].ToString() == "BP" || somebodyEat)
                        _50movesCount--;
                        continue;
                    }
                    else 
                    {
                        isLegal = true;
                        UpDateBoardAndVariablesAfterMoves(current_row, current_col, new_row, new_col);
                    }
                }
                else
                {
                    bool somebodyEat = (board[new_row, new_col].ToString() != "-") ? true : false;
                    DoMove(current_row, current_col, new_row, new_col);
                    if (Chess(FindKing(IsWhiteTurn), MoveCount))
                    {
                        Console.WriteLine("Chess!!");
                        UnDoMove(boardForUnDoMove);
                        isLegal = false;
                        if (board[current_row, current_col].ToString() == "WP" || board[current_row, current_col].ToString() == "BP" || somebodyEat)
                            _50movesCount--;
                        continue;
                    }
                    else
                    {
                        isLegal = true;
                        UpDateBoardAndVariablesAfterMoves(current_row, current_col, new_row, new_col);
                    }
                }
                PrintBoard();
                MoveCount++;
                IsWhiteTurn = !IsWhiteTurn;
            }
        }
    }
    class ChessPiece
    {
        bool IsWhite;
        public ChessPiece(bool IsWhiteColor)
        {
            this.IsWhite = IsWhiteColor;
        }
        public ChessPiece() { }
        public bool GetIsWhiteColor()
        {
            return IsWhite;
        }
        public virtual ChessPiece Copy()
        {
                return new ChessPiece (IsWhite = this.IsWhite);
        }
        public virtual bool IsLegalMove(ChessPiece[,] board, int current_row, int current_col, int new_row, int new_col, int moveCount)
        {
            return false;
        }
    }
    class Pawn : ChessPiece
    {
        // באיזה מספר מהלך המשחק הפיון זז לאחרונה

        // int GameMovePawnLastMoved;
        int lastMove;
        bool doEnPassant;
        public Pawn(bool IsWhite) : base(IsWhite) 
        {
            this.lastMove = -1;
            this.doEnPassant = false;
        }
        public override string ToString()
        {
            return GetIsWhiteColor() ? "WP" : "BP";
        }
        public int GetLastMove()
        {
            return lastMove;
        }
        public void SetLastMove(int lastMove)
        {
            this.lastMove = lastMove;
        }
        public bool GetDoEnPassant()
        {
            return doEnPassant;
        }
        public void SetDoEnPassant(bool doEnPassant)
        {
            this.doEnPassant = doEnPassant;
        }
        public override Pawn Copy()
        {
            Pawn pawn = new Pawn(this.GetIsWhiteColor());
            pawn.SetLastMove(this.lastMove);
            pawn.SetDoEnPassant(this.doEnPassant);
            return pawn;
        }
        public override bool IsLegalMove(ChessPiece[,] board, int current_row, int current_col, int new_row, int new_col, int moveCount)
        {
            bool IsWhite = (board[current_row, current_col].GetIsWhiteColor()) ? true : false;
            bool IsWantToMoveToEmpty = (board[new_row, new_col].ToString() == "-") ? true:false;
            EmptyPlace empty = new EmptyPlace(false);

            if ((IsWhite && IsWantToMoveToEmpty && current_col == new_col && new_row == current_row - 1) ||
                (!IsWhite && IsWantToMoveToEmpty && current_col == new_col && new_row == current_row + 1))
                return true;
            else if ((IsWhite && IsWantToMoveToEmpty && current_col == new_col && this.lastMove == -1 && new_row == current_row - 2) ||
                    (!IsWhite && IsWantToMoveToEmpty && current_col == new_col && this.lastMove == -1 && new_row == current_row + 2))
                return true;
            else if ((IsWhite && IsWantToMoveToEmpty && current_col - 1 >= 0 && board[current_row, current_col - 1].ToString() == "BP" &&
                    ((Pawn)board[current_row, current_col - 1]).GetLastMove() - moveCount == -1 && current_col - 1 == new_col && current_row - 1 == new_row && current_row == 3) ||
                    (!IsWhite && IsWantToMoveToEmpty && current_col - 1 >= 0 && board[current_row, current_col - 1].ToString() == "WP" &&
                    ((Pawn)board[current_row, current_col - 1]).GetLastMove() - moveCount == -1 && current_col - 1 == new_col && current_row + 1 == new_row && current_row == 4)) // en passant
                return true;

            else if ((IsWhite && IsWantToMoveToEmpty && current_col + 1 < 8 && current_col + 1 == new_col && current_row - 1 == new_row &&
                     board[current_row, current_col + 1].ToString() == "BP" && ((Pawn)board[current_row, current_col + 1]).GetLastMove() - moveCount == -1 && current_row == 3) ||
                    (!IsWhite && IsWantToMoveToEmpty && current_col + 1 < 8 && current_col + 1 == new_col && current_row + 1 == new_row && board[current_row, current_col + 1].ToString() == "WP" &&
                    ((Pawn)board[current_row, current_col + 1]).GetLastMove() - moveCount == -1 && current_row == 4)) // en passant 
                return true;
            else if ((!IsWantToMoveToEmpty && IsWhite && !board[new_row, new_col].GetIsWhiteColor() && ((current_col + 1 == new_col && current_col + 1 < 8) || (current_col - 1 == new_col && current_col - 1 >= 0)) && current_row - 1 >= 0 && current_row - 1 == new_row) ||
                    (!IsWantToMoveToEmpty && !IsWhite && board[new_row, new_col].GetIsWhiteColor() && ((current_col + 1 == new_col && current_col + 1 < 8) || (current_col - 1 == new_col && current_col - 1 >= 0)) && current_row + 1 < 8 && current_row + 1 == new_row)) //מקרה של אכילה רגילה
                return true;
            else
                return false;
        }
    }
    class Toora : ChessPiece
    {
        int lastMove;
        public Toora(bool IsWhite) : base(IsWhite) 
        {
            this.lastMove = -1;
        }
        public override string ToString()
        {
            return GetIsWhiteColor() ? "WR" : "BR";
        }
        public override Toora Copy()
        {
            Toora Toora = new Toora(this.GetIsWhiteColor());
            Toora.SetLastMove(this.lastMove);
            return Toora;
        }
        public int GetLastMove()
        {
            return lastMove;
        }
        public void SetLastMove(int lastMove)
        {
            this.lastMove = lastMove;
        }
        public bool IsEmptyRoad(ChessPiece[,] board, int current_row, int current_col, int new_row, int new_col)
        {
            bool IsEmptyPlace = (board[new_row, new_col].ToString() == "-") ? true : false;
            bool IsWhite = (board[current_row, current_col].GetIsWhiteColor()) ? true : false;

            if (current_col == new_col && current_row < new_row)
            {
                int idx = current_row + 1;
                for (int i = idx; i <= new_row; i++)
                {
                    if (i == new_row && (board[i, current_col].GetIsWhiteColor() != board[current_row, current_col].GetIsWhiteColor() ||
                        board[i, current_col].ToString() == "-"))
                        return true;
                    else if (board[i, current_col].ToString() != "-") //אם קיים כלי במסלולו
                    {
                        return false;
                    }
                }
                return true;
            }
            else if (current_col == new_col && current_row > new_row)
            {
                int idx = current_row - 1;
                for (int i = idx; i >= new_row; i--)
                {
                    if (i == new_row && (board[i, current_col].GetIsWhiteColor() != board[current_row, current_col].GetIsWhiteColor() ||
                        board[i, current_col].ToString() == "-"))
                        return true;
                    else if (board[i, current_col].ToString() != "-")
                    {
                        return false;
                    }
                }
                return true;/* לקרוא לזה אחרי שמתבצע התור */
            }
            else if (current_row == new_row && current_col < new_col)
            {
                int idx = current_col + 1;
                for (int i = idx; i <= new_col; i++)
                {
                    if (i == new_col && (board[current_row, i].GetIsWhiteColor() != board[current_row, current_col].GetIsWhiteColor() ||
                        board[current_row, i].ToString() == "-"))
                        return true;
                    else if (board[current_row, i].ToString() != "-")
                    {
                        return false;
                    }
                }
                return true;
            }
            else if (current_row == new_row && current_col > new_col)
            {
                int idx = current_col - 1;
                for (int i = idx; i >= new_col; i--)
                {
                    if (i == new_col && (board[current_row, i].GetIsWhiteColor() != board[current_row, current_col].GetIsWhiteColor() ||
                        board[current_row, i].ToString() == "-"))
                        return true;

                    else if (board[current_row, i].ToString() != "-")
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }
        public override bool IsLegalMove(ChessPiece[,] board, int current_row, int current_col, int new_row, int new_col, int moveCount)
        { 
            if(IsEmptyRoad(board, current_row, current_col, new_row, new_col))
            {
                return true;
            }
            return false;
        }
    }
    class Knight : ChessPiece
    {
        public Knight(bool IsWhite) : base(IsWhite) { }
        public override string ToString()
        {
            return GetIsWhiteColor() ? "WN" : "BN";
        }
        public override Knight Copy()
        {
            return new Knight(this.GetIsWhiteColor());
        }
        public override bool IsLegalMove(ChessPiece[,] board, int current_row, int current_col, int new_row, int new_col, int moveCount)
        {
            int remainderRow = Math.Abs(current_row - new_row);
            int remainderCol = Math.Abs(current_col - new_col);
            if (remainderCol > 7 && remainderCol < 0 && remainderRow > 7 && remainderRow < 0)
                return false;
            return (remainderCol == 1 && remainderRow == 2) || (remainderCol == 2 && remainderRow == 1);
        }
    }
    class Bishop : ChessPiece
    {
        public Bishop(bool IsWhite) : base(IsWhite) { }
        public override string ToString()
        {
            return GetIsWhiteColor() ? "WB" : "BB";
        }
        public override Bishop Copy()
        {
            return new Bishop(this.GetIsWhiteColor());
        }
        public override bool IsLegalMove(ChessPiece[,] board, int current_row, int current_col, int new_row, int new_col, int moveCount)
        {
            int remainderRow = Math.Abs(current_row - new_row);
            int remainderCol = Math.Abs(current_col - new_col);

            if (remainderRow != remainderCol)
                return false;

            // קובע את הכיוון שהרץ רוצה לזוז
            int rowDirection = (new_row - current_row) > 0 ? 1 : -1;//אם זז ימינה אז 1,אחרת -1
            int colDirection = (new_col - current_col) > 0 ? 1 : -1;//אם זז שמאלה אז 1, אחרת -1

            //בדיקת המסלול 
            int currentCheckRow = current_row + rowDirection;
            int currentCheckCol = current_col + colDirection;

            while (currentCheckRow >= 0 && currentCheckRow < 8 && currentCheckCol >= 0 && currentCheckCol < 8
                   && currentCheckRow != new_row && currentCheckCol != new_col)
            {
                // אם יש כלי במסלול 
                if (currentCheckRow > 7 || currentCheckRow < 0 || currentCheckCol > 7 || currentCheckCol < 0 ||
                    board[currentCheckRow, currentCheckCol].ToString() != "-")
                {
                    return false;
                }

                // ממשיך לזוז להמשך המסלול ולבדוק
                currentCheckRow += rowDirection;
                currentCheckCol += colDirection;
            }
            return true; // אם לא נמצא כלי במסלול
        }
    }
    class Queen : ChessPiece
    {
        public Queen(bool IsWhite) : base(IsWhite) { }
        public override string ToString()
        {
            return GetIsWhiteColor() ? "WQ" : "BQ";
        }
        public override Queen Copy()
        {
            return new Queen(this.GetIsWhiteColor());
        }
        public override bool IsLegalMove(ChessPiece[,] board, int current_row, int current_col, int new_row, int new_col, int moveCount)
        {
            Toora Toora = new Toora(false);
            Bishop bishop = new Bishop(false);
            if (Toora.IsLegalMove(board, current_row, current_col, new_row, new_col, moveCount) || 
                bishop.IsLegalMove(board, current_row, current_col, new_row, new_col, moveCount))
            {
                return true;
            }
            return false;
        }
    }
    class Location
    {
        public int row;
        public int col;
        public Location(int row, int col)
        {
            this.row = row;
            this.col = col;
        }
    }
    class King : ChessPiece
    {

        string position;
        int lastMove;
        public King(bool IsWhite, string position) : base(IsWhite) 
        {
            this.position = position;
            this.lastMove = -1;
        }
        public string GetPositionKing()
        {
            return this.position;
        }
        public int GetLastMove()
        {
            return lastMove;
        }
        public void SetPositionKing(string position)
        {
            this.position = position;
        }
        public void SetLastMove(int lastMove)
        {
            this.lastMove = lastMove;
        }
        public override string ToString()
        {
            return (GetIsWhiteColor()) ? "WK" : "BK";
        }
        public override King Copy()
        {
            King king = new King(this.GetIsWhiteColor(), this.position);
            king.SetLastMove(this.lastMove);
            return king;
        }
        private ChessPiece[,] CopyBoard(ChessPiece[,] board_to_copy)
        {
            ChessPiece[,] board_to_return = new ChessPiece[8, 8];
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    board_to_return[i, j] = board_to_copy[i, j].Copy();
                }
            }
            return board_to_return;
        }
        private bool ChessKing(ChessPiece[,] board, string idx_king, int moveCount)
        {
            int rowKing = idx_king[0] - '0'; //המרת תו למספר שלם
            int colKing = idx_king[1] - '0';
            bool IsWhite = (board[rowKing, colKing].ToString() == "WK");

            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    if (board[i, j].GetIsWhiteColor() != IsWhite)
                    {
                        if (board[i,j] is King)
                        {
                            if (((King)board[i, j]).IsLegalMoveForFuncChessKing(board, i, j, rowKing, colKing, moveCount))
                            {
                                return true;
                            }
                        }
                        else if (board[i, j].IsLegalMove(board, i, j, rowKing, colKing, moveCount))
                        {
                            return true; //יש שח
                        }
                    }
                }
            }
            return false; //אין שח
        }
        private bool CheckingWhiteKingThreatenedInRightForCastling(ChessPiece[,] board, int current_row, int current_col, int new_row, int new_col, int moveCount)
        {
            ChessPiece[,] board_copy = CopyBoard(board);
            ChessPiece[,] board_copy1 = CopyBoard(board);
            EmptyPlace empty = new EmptyPlace(false);
            board_copy[7, 5] = board_copy[current_row, current_col];
            board_copy[current_row, current_col] = empty;
            board_copy1[7, 6] = board_copy1[current_row, current_col];
            board_copy1[current_row, current_col] = empty;

            if (!ChessKing(board_copy, "75", moveCount) && !ChessKing(board_copy1, "76", moveCount)) // אם אין איום על הלך
            {
                return true;
            }
            return false;
        }
        private bool CheckingWhiteKingThreatenedInLeftForCastling(ChessPiece[,] board, int current_row, int current_col, int new_row, int new_col, int moveCount)
        {
            ChessPiece[,] board_copy = CopyBoard(board);
            ChessPiece[,] board_copy1 = CopyBoard(board);
            EmptyPlace empty = new EmptyPlace(false);
            board_copy[7, 3] = board_copy[current_row, current_col];
            board_copy[current_row, current_col] = empty;
            board_copy1[7, 2] = board_copy1[current_row, current_col];
            board_copy1[current_row, current_col] = empty;

            if (!ChessKing(board_copy, "73", moveCount) && !ChessKing(board_copy1, "72", moveCount))
            {
                return true;
            }
            return false;
        }
        private bool CheckingBlackKingThreatenedInRightForCastling(ChessPiece[,] board, int current_row, int current_col, int row, int col, int moveCount)
        {
            ChessPiece[,] board_copy = CopyBoard(board);
            ChessPiece[,] board_copy1 = CopyBoard(board);
            EmptyPlace empty = new EmptyPlace(false);
            board_copy[0, 5] = board_copy[current_row, current_col];
            board_copy[current_row, current_col] = empty;
            board_copy1[0, 6] = board_copy1[current_row, current_col];
            board_copy1[current_row, current_col] = empty;

            if (!ChessKing(board_copy, "05", moveCount) && !ChessKing(board_copy1, "06", moveCount)) // אם אין איום על הלך
            {
                return true;
            }
            return false;
        }
        private bool CheckingBlackKingThreatenedInLeftForCastling(ChessPiece[,] board, int current_row, int current_col, int new_row, int new_col, int moveCount)
        {
            ChessPiece[,] board_copy = CopyBoard(board);
            ChessPiece[,] board_copy1 = CopyBoard(board);
            EmptyPlace empty = new EmptyPlace(false);
            board_copy[0, 3] = board_copy[current_row, current_col];
            board_copy[current_row, current_col] = empty;
            board_copy1[0, 2] = board_copy1[current_row, current_col];
            board_copy1[current_row, current_col] = empty;

            if (!ChessKing(board_copy, "03", moveCount) && !ChessKing(board_copy1, "02", moveCount))
            {
                return true;
            }
            return false;
        }
        public override bool IsLegalMove(ChessPiece[,] board, int current_row, int current_col, int new_row, int new_col, int moveCount)
        {
            bool chess = ChessKing(board, position, moveCount);

            if (!chess && board[7, 4].ToString() == "WK" && this.lastMove == -1 &&
                new_row == current_row && new_col == current_col + 2 && board[7, 7].ToString() == "WR" &&
                board[current_row, current_col + 1].ToString() == "-" && board[current_row, current_col + 2].ToString() == "-" &&
                board[7, 7] is Toora && ((Toora)board[7, 7]).GetLastMove() == -1 && CheckingWhiteKingThreatenedInRightForCastling(board, current_row, current_col, new_row, new_col, moveCount))
                return true;
            else if (!chess && board[current_row, current_col].ToString() == "WK" && this.lastMove == -1 &&
                new_row == current_row && new_col == current_col - 2 && board[current_row, current_col - 1].ToString() == "-"
                && board[current_row, current_col - 2].ToString() == "-" && board[current_row, current_col - 3].ToString() == "-" && board[7, 0] is Toora && ((Toora)board[7, 0]).GetLastMove() == -1 &&
                board[7, 0].ToString() == "WR" && CheckingWhiteKingThreatenedInLeftForCastling(board, current_row, current_col, new_row, new_col, moveCount))
                return true;
            else if (!chess && board[current_row, current_col].ToString() == "BK" && this.lastMove == -1 &&
                new_row == current_row && new_col == current_col + 2 && board[0, 7] is Toora && ((Toora)board[0, 7]).GetLastMove() == -1 &&
                board[current_row, current_col + 1].ToString() == "-" && board[current_row, current_col + 2].ToString() == "-" &&
                board[0, 7].ToString() == "BR" && CheckingBlackKingThreatenedInRightForCastling(board, current_row, current_col, new_row, new_col, moveCount))
                return true;
            else if (!chess && board[current_row, current_col].ToString() == "BK" && this.lastMove == -1 &&
                new_row == current_row && new_col == current_col - 2 && board[0, 0] is Toora && ((Toora)board[0, 0]).GetLastMove() == -1
                && board[current_row, current_col - 1].ToString() == "-" && board[current_row, current_col - 2].ToString() == "-" &&
                board[current_row, current_col - 3].ToString() == "-" && board[0, 0].ToString() == "BR" && CheckingBlackKingThreatenedInLeftForCastling(board, current_row, current_col, new_row, new_col, moveCount))
            {
                return true;
            }
            else if (IsLegalMoveForFuncChessKing(board, current_row, current_col, new_row, new_col, moveCount))
            {
                return true;
            }
            return false;
        }
        public bool IsLegalMoveForFuncChessKing(ChessPiece[,] board, int current_row, int current_col, int new_row, int new_col, int moveCount)
        {
            int remainderRow = Math.Abs(current_row - new_row);
            int remainderCol = Math.Abs(current_col - new_col);

            if (remainderCol > 7 && remainderCol < 0 && remainderRow > 7 && remainderRow < 0)
                return false;
            if (remainderCol <= 1 && remainderRow <= 1)
                return true;
            return false;
        }
    }
    class EmptyPlace : ChessPiece
    {
        public EmptyPlace(bool IsWhite) : base(false) { }
        public override string ToString()
        {
            return "-";
        }
        public override EmptyPlace Copy()
        {
            return new EmptyPlace(this.GetIsWhiteColor());
        }
        public override bool IsLegalMove(ChessPiece[,] board, int current_row, int current_col, int new_row, int new_col, int moveCount)
        {
            return false;
        }
    }
}