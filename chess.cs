using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace chessAharon
{
    class Program
    {
        static void Main(string[] args)
        {
            ChessBoard chessboard = new ChessBoard();
            chessboard = chessboard.createChessBoard();
            chessboard.print();
            chessboard.game();
        }
}
        class Piece
        {
            public bool isWhite;
            public virtual bool legalMoves(string index, ChessBoard chessboard)
            {
                int x1, x2, y1, y2;
                x1 = chessboard.getLinePosition(index);
                y1 = chessboard.getColumnPosition(index);
                x2 = chessboard.getNextLinePosition(index);
                y2 = chessboard.getNextColumnPosition(index);

                return (!(x1 == x2 && y1 == y2)) && chessboard.board[x1][y1] != null && ((chessboard.board[x2][y2] == null || (chessboard.board[x2][y2].isWhite != chessboard.board[x1][y1].isWhite/* && !chessboard.board[x2][y2] is King*/)));
            }
            
            public Piece(bool isWhite)
            {
                this.isWhite = isWhite;
            }
            public override string ToString()
            {
                return base.ToString();
            }

        }
        class Pawn : Piece
        {
            public bool firstMove;
            public bool enPassantRisk;
            public Pawn(bool isWhite)
                : base(isWhite)
            {
                this.firstMove = true;
                this.isWhite = isWhite;
                this.enPassantRisk = false;
            }
            public bool ifBeingPromoted(int line)
            {

                if (isWhite && line == 0)
                    return true;
                if (!isWhite && line == 7)
                    return true;

                return false;
            }
            public bool enpassantLegal(string index, ChessBoard chessboard)
            {
                int x1, x2, y1, y2;
                x1 = chessboard.getLinePosition(index);
                y1 = chessboard.getColumnPosition(index);
                x2 = chessboard.getNextLinePosition(index);
                y2 = chessboard.getNextColumnPosition(index);
                if (chessboard.board[x1][y2] is Pawn && ((Pawn)chessboard.board[x1][y2]).enPassantRisk && 1 == Math.Abs(y2 - y1) && ((isWhite && x2 == x1 - 1) || (!isWhite && x2 == x1 + 1)))
                    return true;
                return false;
            }
            public override bool legalMoves(string index, ChessBoard chessboard)
            {
                int x1, x2, y1, y2;
                x1 = chessboard.getLinePosition(index);
                y1 = chessboard.getColumnPosition(index);
                x2 = chessboard.getNextLinePosition(index);
                y2 = chessboard.getNextColumnPosition(index);
                bool check1 = false;
                bool check2 = false;
                bool check3 = false;
                bool check4 = false;


                if (!isWhite)
                {
                    if (((((firstMove) && (x2 == x1 + 2) && (chessboard.board[x1 + 1][y1] == null)) || (x2 == x1 + 1)) && y1 == y2) && chessboard.board[x2][y2] == null)
                        check1 = true;
                    if (x2 == x1 + 1 && Math.Abs(y2 - y1) == 1 && chessboard.board[x2][y2] != null && chessboard.board[x2][y2].isWhite != chessboard.board[x1][y1].isWhite)
                        check3 = true;
                }

                if (isWhite)
                {
                    if (((((firstMove) && (x2 == x1 - 2) && (chessboard.board[x1 - 1][y1] == null)) || (x2 == x1 - 1)) && y1 == y2) && chessboard.board[x2][y2] == null)
                        check2 = true;
                    if (x2 == x1 - 1 && Math.Abs(y2 - y1) == 1 && chessboard.board[x2][y2] != null && chessboard.board[x2][y2].isWhite != chessboard.board[x1][y1].isWhite)
                        check4 = true;
                }

                bool test = base.legalMoves(index, chessboard) && (check1 || check3 || check2 || check4 || enpassantLegal(index, chessboard));

                return test;
            }
            public override string ToString()
            {
                return isWhite ? " White " : " Black " + "Pawn";
            }
        }
        class Rook : Piece
        {
            public bool firstMove;
            public Rook(bool isWhite)
                : base(isWhite)
            {
                firstMove = true;
            }
            public override bool legalMoves(string index, ChessBoard chessboard)
            {
                int x1, x2, y1, y2;
                x1 = chessboard.getLinePosition(index);
                y1 = chessboard.getColumnPosition(index);
                x2 = chessboard.getNextLinePosition(index);
                y2 = chessboard.getNextColumnPosition(index);
                if (!(x1 == x2 || y2 == y1))
                    return false;
                int advance = x1 == x2 ? y2 - y1 : x2 - x1;
                int abs = advance > 0 ? 1 : -1;
                for (int i = 1; i < Math.Abs(advance); i++)
                {
                    if (((x1 == x2) && (chessboard.board[x1][y1 + i * abs] != null)) || ((y1 == y2) && (chessboard.board[x1 + i * abs][y1] != null)))
                        return false;
                }


                return base.legalMoves(index, chessboard);// check1;
            }
            public override string ToString()
            {
                return isWhite ? " White " : " Black " + "Rook";
            }

        }
        class Bishop : Piece
        {

            public Bishop(bool isWhite)
                : base(isWhite)
            {

            }
            public override bool legalMoves(string index, ChessBoard chessboard)
            {
                int x1, x2, y1, y2;
                x1 = chessboard.getLinePosition(index);
                y1 = chessboard.getColumnPosition(index);
                x2 = chessboard.getNextLinePosition(index);
                y2 = chessboard.getNextColumnPosition(index);
                if (!((x1 - x2 == y1 - y2) || (-(x1 - x2) == (y1 - y2))))
                    return false;
                int advance = x1 - x2;

                for (int i = 1; i < Math.Abs(advance); i++)//להשלים תנאי למאלכסון שלילי או חיובי ושלילי+ ולבדוק מה קורה אם הנקודה האחרונה מאוכלסת
                {
                    if (((x1 - x2 > 0) && (y1 - y2 > 0) && chessboard.board[x1 - i][y1 - i] != null) || ((x1 - x2 < 0) && (y1 - y2 < 0) && chessboard.board[x1 + i][y1 + i] != null) ||
                        ((x1 - x2 < 0) && (y1 - y2 > 0) && chessboard.board[x1 + i][y1 - i] != null) || ((x1 - x2 > 0) && (y1 - y2 < 0) && chessboard.board[x1 - i][y1 + i] != null))
                        return false;
                }
                return base.legalMoves(index, chessboard);
            }
            public override string ToString()
            {
                return isWhite ? " White " : " Black " + "Bishop";
            }
        }
        class Knight : Piece
        {

            public Knight(bool isWhite)
                : base(isWhite)
            {
            }
            public override bool legalMoves(string index, ChessBoard chessboard)
            {
                int x1, x2, y1, y2;
                x1 = chessboard.getLinePosition(index);
                y1 = chessboard.getColumnPosition(index);
                x2 = chessboard.getNextLinePosition(index);
                y2 = chessboard.getNextColumnPosition(index);
                if (!((Math.Abs(x1 - x2) == 2 && Math.Abs(y1 - y2) == 1) || (Math.Abs(x1 - x2) == 1 && Math.Abs(y1 - y2) == 2)))
                    return false;
                return base.legalMoves(index, chessboard);
            }
            public override string ToString()
            {
                return isWhite ? " White " : " Black " + "Knight";
            }
        }
        class Queen : Piece
        {

            public Queen(bool isWhite)
                : base(isWhite)
            {
            }
          public override bool legalMoves(string index, ChessBoard chessboard)
            {
                int x1, x2, y1, y2;
                x1 = chessboard.getLinePosition(index);
                y1 = chessboard.getColumnPosition(index);
                x2 = chessboard.getNextLinePosition(index);
                y2 = chessboard.getNextColumnPosition(index);

                bool check3 = true;
                if (!(x1 == x2 || y2 == y1))
                    check3 = false;
                int advance = x1 == x2 ? y2 - y1 : x2 - x1;
                int abs = advance > 0 ? 1 : -1;
                bool check4 = true;
                for (int i = 1; i < Math.Abs(advance); i++)
                {
                    if (((x1 == x2) && (chessboard.board[x1][y1 + i * abs] != null)) || ((y1 == y2) && (chessboard.board[x1 + i * abs][y1] != null)))
                        check4 = false;
                }
                bool check1 = true;
                if (!((x1 - x2 == y1 - y2) || (-(x1 - x2) == (y1 - y2))))
                    check1 = false;
                if ((!check1 && !check3))
                    return false;
                advance = x1 - x2;
                bool check2 = true;

                for (int i = 1; i < Math.Abs(advance); i++)
                {
                    if (((x1 - x2 > 0) && (y1 - y2 > 0) && chessboard.board[x1 - i][y1 - i] != null) || ((x1 - x2 < 0) && (y1 - y2 < 0) && chessboard.board[x1 + i][y1 + i] != null) ||
                        ((x1 - x2 < 0) && (y1 - y2 > 0) && chessboard.board[x1 + i][y1 - i] != null) || ((x1 - x2 > 0) && (y1 - y2 < 0) && chessboard.board[x1 - i][y1 + i] != null))
                        check2 = false;
                }
                return base.legalMoves(index, chessboard) && ((check1 && check2) || (check3 && check4));
            }
            public override string ToString()
            {
                return isWhite?" White ":" Black "+"Queen";
            }
        }
        class King : Piece
        {
            public bool firstMove;

            public King(bool isWhite)
                : base(isWhite)
            {
                firstMove = true;

            }
            public bool castling(string index, ChessBoard chessboard)
            {
                int x1, x2, y1, y2;
                x1 = chessboard.getLinePosition(index);
                y1 = chessboard.getColumnPosition(index);
                x2 = chessboard.getNextLinePosition(index);
                y2 = chessboard.getNextColumnPosition(index);
                if ((x1 != 7 || x1 != 0) && y1 != 4)
                    return false;
                bool smallHazraha = chessboard.board[x1][y1] is King && ((King)chessboard.board[x1][y1]).firstMove && (chessboard.board[x1][7] is Rook && ((Rook)chessboard.board[x1][7]).firstMove &&
                     x1 == x2 && y2 == 6 && chessboard.board[x1][6] == null && chessboard.board[x1][5] == null);
                bool bigHazraha = chessboard.board[x1][y1] is King && ((King)chessboard.board[x1][y1]).firstMove && (chessboard.board[x1][0] is Rook && ((Rook)chessboard.board[x1][0]).firstMove &&
                    x1 == x2 && y2 == 2 && chessboard.board[x1][3] == null && chessboard.board[x1][2] == null && chessboard.board[x1][1] == null);
                bool y5chess = false, y6chess = false, y3chess = false, y2chess = false, y1chess = false;
                if (smallHazraha && x1 == 7)
                {
                    chessboard.board[7][5] = chessboard.board[7][4];
                    chessboard.board[7][4] = null;
                    ChessBoard.wKingPosition = "f1";
                    y5chess = !chessboard.whiteChess();
                    chessboard.board[7][6] = chessboard.board[7][5];
                    chessboard.board[7][5] = null;
                    ChessBoard.wKingPosition = "g1";
                    y6chess = !chessboard.whiteChess();
                    chessboard.board[7][4] = chessboard.board[7][6];
                    chessboard.board[7][6] = null;
                    ChessBoard.wKingPosition = "e1";
                }
                if (bigHazraha && x1 == 7)
                {
                    chessboard.board[7][3] = chessboard.board[7][4];
                    chessboard.board[7][4] = null;
                    ChessBoard.wKingPosition = "d1";
                    y3chess = !chessboard.whiteChess();
                    chessboard.board[7][2] = chessboard.board[7][3];
                    chessboard.board[7][3] = null;
                    ChessBoard.wKingPosition = "c1";
                    y2chess = !chessboard.whiteChess();
                    chessboard.board[7][1] = chessboard.board[7][2];
                    chessboard.board[7][2] = null;
                    ChessBoard.wKingPosition = "b1";
                    y1chess = !chessboard.whiteChess();
                    chessboard.board[7][4] = chessboard.board[7][1];
                    chessboard.board[7][1] = null;
                    ChessBoard.wKingPosition = "e1";
                }

                if (y6chess && y5chess && smallHazraha)
                    return true;
                if (y3chess && y2chess && y1chess && bigHazraha)
                    return true;
                if (smallHazraha && x1 == 0)
                {
                    chessboard.board[0][5] = chessboard.board[0][4];
                    chessboard.board[0][4] = null;
                    ChessBoard.bKingPosition = "f8";
                    y5chess = !chessboard.blackChess();
                    chessboard.board[0][6] = chessboard.board[0][5];
                    chessboard.board[0][5] = null;
                    ChessBoard.bKingPosition = "g8";
                    y6chess = !chessboard.blackChess();
                    chessboard.board[0][4] = chessboard.board[0][6];
                    chessboard.board[0][6] = null;
                    ChessBoard.bKingPosition = "e8";
                }
                if (bigHazraha && x1 == 0)
                {
                    chessboard.board[0][3] = chessboard.board[0][4];
                    chessboard.board[0][4] = null;
                    ChessBoard.bKingPosition = "d8";
                    y3chess = !chessboard.blackChess();
                    chessboard.board[0][2] = chessboard.board[0][3];
                    chessboard.board[0][3] = null;
                    ChessBoard.bKingPosition = "c8";
                    y2chess = !chessboard.blackChess();
                    chessboard.board[0][1] = chessboard.board[0][2];
                    chessboard.board[0][2] = null;
                    ChessBoard.bKingPosition = "b8";
                    y1chess = !chessboard.blackChess();
                    chessboard.board[0][4] = chessboard.board[0][1];
                    chessboard.board[0][1] = null;
                    ChessBoard.bKingPosition = "e8";
                }

                if (y6chess && y5chess && smallHazraha)
                    return true;
                if (y3chess && y2chess && y1chess && bigHazraha)
                    return true;
                return false;
            }
            public override bool legalMoves(string index, ChessBoard chessboard)
            {
                int x1, x2, y1, y2;
                x1 = chessboard.getLinePosition(index);
                y1 = chessboard.getColumnPosition(index);
                x2 = chessboard.getNextLinePosition(index);
                y2 = chessboard.getNextColumnPosition(index);

                return (base.legalMoves(index, chessboard) && ((Math.Abs(x1 - x2) <= 1) && (Math.Abs(y1 - y2) <= 1)));//|| castling(index, chessboard);
            }

            public override string ToString()
            {
                return isWhite ? " White " : " Black " + "king";
            }
        }
        class ChessBoard
        {
            public static string wKingPosition = "e1";

            public static string bKingPosition = "e8";
            public bool threatOnBlackKing;
            public bool threatOnWhiteKing;

            public Piece[][] board;
            public bool blackChess()
            {
                bool checkIfChess = false;
                string bkindex;
                string lettersOnBoard = "abcdefgh";
                string numsOnBoard = "87654321";
                for (int i = 0; i < 8; i++)
                    for (int j = 0; j < 8; j++)
                    {

                        bkindex = "" + lettersOnBoard[j] + numsOnBoard[i] + bKingPosition;
                        if ((board[i][j] is Pawn) || (board[i][j] is Queen) || (board[i][j] is Knight) || (board[i][j] is Rook) || (board[i][j] is Bishop) || (board[i][j] is King))
                            checkIfChess = this.board[i][j].legalMoves(bkindex, this);
                        if (checkIfChess)
                        {
                            Console.WriteLine(this.board[i][j] + bkindex);
                            return true;
                        }
                    }
                return false;
            }
            public bool whiteChess()
            {
                bool checkIfChess = false;
                string wkindex;
                string lettersOnBoard = "abcdefgh";
                string numsOnBoard = "87654321";
                for (int i = 0; i < 8; i++)
                    for (int j = 0; j < 8; j++)
                    {
                        wkindex = "" + lettersOnBoard[j] + numsOnBoard[i] + wKingPosition;

                        if ((board[i][j] is Pawn) || (board[i][j] is Queen) || (board[i][j] is Knight) || (board[i][j] is Rook) || (board[i][j] is King) || (board[i][j] is Bishop))
                            checkIfChess = this.board[i][j].legalMoves(wkindex, this);
                        if (checkIfChess)
                        {
                            Console.WriteLine(this.board[i][j] + wkindex);
                            return true;
                        }
                    }
                return false;
            }
            public string getInput()
            {
                string input;
                Console.WriteLine("please choose your move and press enter");
                input = Console.ReadLine();
                char[] charToTrim = { ' ' };
                input = input.Trim(charToTrim);
                return input;
            }
            public string changeInputToIndex(string input)
            {
                string stringIndex = "";
                string letters = "abcdefghABCDEFGH";
                string nums = "87654321";
                for (int j = 0; j < 4; j++)
                    for (int i = 0; i < 8; i++)
                        if (input[j] == letters[i] || input[j] == letters[i + 8] || input[j] == nums[i])
                        {
                            stringIndex += (i);
                            break;
                        }
                stringIndex = "" + stringIndex[1] + stringIndex[0] + stringIndex[3] + stringIndex[2] + "";

                return stringIndex;
            }
            public int getLinePosition(string input)
            {
                return int.Parse(changeInputToIndex(input)[0] + "");
            }
            public int getColumnPosition(string input)
            {
                return int.Parse(changeInputToIndex(input)[1] + "");
            }
            public int getNextLinePosition(string input)
            {
                return int.Parse(changeInputToIndex(input)[2] + "");
            }
            public int getNextColumnPosition(string input)
            {
                return int.Parse(changeInputToIndex(input)[3] + "");
            }
            public bool checkInput(string input)
            {
                bool check1 = false, check3 = false;

                if (input.Length != 4)
                    return false;
                string legalLetters = "abcdefghABCDEFGH";
                string legalNums = "12345678";
                for (int i = 0; i < legalLetters.Length; i++)
                {
                    if (input[0] == legalLetters[i])
                    {
                        check1 = true;
                        break;
                    }
                }
                for (int i = 0; i < legalLetters.Length; i++)
                {
                    if (input[2] == legalLetters[i])
                    {
                        check3 = true;
                        break;
                    }
                }
                bool check2 = false, check4 = false;
                for (int i = 0; i < legalNums.Length; i++)
                {
                    if (input[1] == legalNums[i])
                    {
                        check2 = true;
                        break;
                    }
                }
                for (int i = 0; i < legalNums.Length; i++)
                {
                    if (input[3] == legalNums[i])
                    {
                        check4 = true;
                        break;
                    }
                }
                return check1 && check2 && check3 && check4;
            }
            public void print()
            {
                String numOnBoard = "87654321";
                Console.WriteLine();
                Console.WriteLine("     a    b    c    d    e    f    g    h");
                for (int line = 0; line < 8; line++)
                {
                    Console.WriteLine("   ________________________________________");
                    Console.WriteLine();
                    Console.Write(" " + numOnBoard[line] + " ");
                    for (int column = 0; column < 8; column++)
                    {

                        if (this.board[line][column] == null)
                        {
                            Console.Write("|    ");
                            continue;
                        }
                        if (this.board[line][column] is Pawn)
                        {
                            if (this.board[line][column].isWhite)
                                Console.Write("| WP ");
                            else Console.Write("| BP ");
                            continue;
                        }
                        if (this.board[line][column] is King)
                        {
                            if (this.board[line][column].isWhite)
                                Console.Write("| WK ");
                            else Console.Write("| BK ");
                            continue;
                        }
                        if (this.board[line][column] is Queen)
                        {
                            if (this.board[line][column].isWhite)
                                Console.Write("| WQ ");
                            else Console.Write("| BQ ");
                            continue;
                        }
                        if (this.board[line][column] is Rook)
                        {
                            if (this.board[line][column].isWhite)
                                Console.Write("| WR ");
                            else Console.Write("| BR ");
                            continue;
                        }
                        if (this.board[line][column] is Bishop)
                        {
                            if (this.board[line][column].isWhite)
                                Console.Write("| WB ");
                            else Console.Write("| BB ");

                            continue;
                        }
                        if (this.board[line][column] is Knight)
                        {
                            if (this.board[line][column].isWhite)
                                Console.Write("| WN ");
                            else Console.Write("| BN ");

                        }

                        continue;

                    }
                    Console.Write("| " + numOnBoard[line] + " ");
                    Console.WriteLine();

                }
                Console.WriteLine("   ________________________________________");
                Console.WriteLine();
                Console.WriteLine("     a    b    c    d    e    f    g    h");
                Console.WriteLine();
            }
            public ChessBoard()
            {
                threatOnBlackKing = false;
                threatOnWhiteKing = false;
                this.board = new Piece[8][];
                for (int i = 0; i < 8; i++)
                    board[i] = new Piece[8];
            }
            public ChessBoard createChessBoard()
            {
               
                this.board[0][0] = new Rook(false);
                this.board[0][1] = new Knight(false);
                this.board[0][2] = new Bishop(false);
                this.board[0][3] = new Queen(false);
                this.board[0][4] = new King(false);
                this.board[0][5] = new Bishop(false);
                this.board[0][6] = new Knight(false);
                this.board[0][7] = new Rook(false);
                for (int column = 0; column < 8; column++)
                {
                    this.board[1][column] = new Pawn(false);
                    this.board[6][column] = new Pawn(true);
                }
                this.board[7][0] = new Rook(true);
                this.board[7][1] = new Knight(true);
                this.board[7][2] = new Bishop(true);
                this.board[7][3] = new Queen(true);
                this.board[7][4] = new King(true);
                this.board[7][5] = new Bishop(true);
                this.board[7][6] = new Knight(true);
                this.board[7][7] = new Rook(true);

                return this;

            }
            public Piece promotion(int line, int column)
            {
                string input;
                while (board[line][column] is Pawn && ((Pawn)board[line][column]).ifBeingPromoted(line))
                {
                    Console.WriteLine("please choose your promotion Q for Qween, K for Knight, B for Bishop  or R for Rook end prees enter");
                    input = Console.ReadLine(); 
                    input = input.Trim();
                    switch (input)
                    {
                        case "Q":
                        case "q": return new Queen(board[line][column].isWhite);
                        case "k":
                        case "K": return new Knight(board[line][column].isWhite);
                        case "r":
                        case "R": return new Rook(board[line][column].isWhite);
                        default: Console.WriteLine("Wrong input.");
                            break;
                    }
                }
                return board[line][column];
            }
            public void enPassant(bool ifEnpassantLegal, int linePosition, int columnPosition)
            {
                int direction = board[linePosition][columnPosition].isWhite ? -1 : 1;
                if (ifEnpassantLegal)
                    board[linePosition - direction][columnPosition] = null;
                for (int i = 0; i < 8; i++)
                {
                    if (board[linePosition][columnPosition].isWhite)
                    {
                        if (board[3][i] is Pawn)
                            ((Pawn)board[3][i]).enPassantRisk = false;
                    }
                    else if (board[4][i] is Pawn)
                        ((Pawn)board[4][i]).enPassantRisk = false;
                }

            }
            public bool blackStalemate()
            {
                ChessBoard tempChessboard = new ChessBoard();
                tempChessboard.board = this.copyBoard();
                string lettersOnBoard = "abcdefgh";
                string numsOnBoard = "87654321";
                string index;
                string kingposition = bKingPosition;
                for (int i = 0; i < 8; i++)
                    for (int j = 0; j < 8; j++)
                    {
                        if (((board[i][j] is Pawn) || (board[i][j] is Queen) || (board[i][j] is Knight) || (board[i][j] is Rook) || (board[i][j] is King) || (board[i][j] is Bishop)) && !board[i][j].isWhite)
                        {
                            for (int x = 0; x < 8; x++)
                                for (int y = 0; y < 8; y++)
                                {
                                    index = "" + lettersOnBoard[j] + numsOnBoard[i] + lettersOnBoard[y] + numsOnBoard[x];
                                    if (tempChessboard.movement(index, false))
                                    {
                                        if (board[i][j] is King)
                                            bKingPosition = kingposition;
                                        return false;
                                    }
                                }
                        }
                    }
                return true;
            }
            public bool whiteStalemate()
            {
                string lettersOnBoard = "abcdefgh";
                string numsOnBoard = "87654321";
                string index, kingposition=wKingPosition;
                ChessBoard tempChessboard = new ChessBoard();
                tempChessboard.board = this.copyBoard();
                for (int i = 0; i < 8; i++)
                    for (int j = 0; j < 8; j++)
                    {

                        if (((board[i][j] is Pawn) || (board[i][j] is Queen) || (board[i][j] is Knight) || (board[i][j] is Rook) || (board[i][j] is King) || (board[i][j] is Bishop)) && board[i][j].isWhite)
                        {
                            for (int x = 0; x < 8; x++)
                                for (int y = 0; y < 8; y++)
                                {
                                    index = "" + lettersOnBoard[j] + numsOnBoard[i] + lettersOnBoard[y] + numsOnBoard[x];
                                    if (tempChessboard.movement(index, true))
                                    {
                                        if (board[i][j] is King)
                                            wKingPosition = kingposition;
                                        return false;
                                    }
                                }
                        }
                    }
                return true;
            }
            public bool movement(string index, bool whiteOrBlack)
            {
                if (!checkInput(index))
                    return false;
                Piece[][] tempChessboard;
                tempChessboard = copyBoard();
                int x1, x2, y1, y2;
                x1 = getLinePosition(index);
                y1 = getColumnPosition(index);
                x2 = getNextLinePosition(index);
                y2 = getNextColumnPosition(index);
                bool test1 = this.board[x1][y1] is King;
                bool enpassant = false;
                bool playerTurn=false ;
                if (this.board[x1][y1]!=null)
                    playerTurn=(whiteOrBlack == this.board[x1][y1].isWhite);
                bool test2 = test1 && ((King)this.board[x1][y1]).castling(index, this);
                if ((this.board[x1][y1] != null && (this.board[x1][y1].legalMoves(index, this) || test2)) && playerTurn)
                {
                    this.board[x2][y2] = this.board[x1][y1];
                    this.board[x1][y1] = null;


                    if (this.board[x2][y2] is Pawn)
                    {
                        enpassant = ((Pawn)board[x2][y2]).enpassantLegal(index, this);

                        ((Pawn)board[x2][y2]).firstMove = false;
                        if (Math.Abs(x1 - x2) == 2)
                            ((Pawn)board[x2][y2]).enPassantRisk = true;
                    }
                    this.enPassant(enpassant, x2, y2);
                    if (this.board[x2][y2] is King)
                    {
                        if (this.board[x2][y2].isWhite)
                            wKingPosition = "" + index[2] + index[3];
                        else bKingPosition = "" + index[2] + index[3];
                        if (test2)//castling(index, this))
                        {
                            ((King)this.board[x2][y2]).firstMove = false;
                            if (x2 == 0 && y2 == 6)
                            {
                                board[0][5] = board[0][7];
                                board[0][7] = null;
                                ((Rook)board[0][5]).firstMove = false;
                            }

                            if (x2 == 0 && y2 == 2)
                            {
                                board[0][3] = board[0][0];
                                board[0][0] = null;
                                ((Rook)board[0][3]).firstMove = false;
                            }
                            if (x2 == 7 && y2 == 6)
                            {
                                board[7][5] = board[7][7];
                                board[7][7] = null;
                                ((Rook)board[7][5]).firstMove = false;
                            }
                            if (x2 == 7 && y2 == 2)
                            {
                                board[7][3] = board[7][0];
                                board[7][0] = null;
                                ((Rook)board[7][3]).firstMove = false;
                            }
                        }

                    }
                    board[x2][y2] = promotion(x2, y2);

                    if (((board[x2][y2].isWhite && whiteChess()) || (!board[x2][y2].isWhite && blackChess())))
                    {

                        if (this.board[x2][y2] is King)
                        {
                            if (this.board[x2][y2].isWhite)
                                wKingPosition = "" + index[0] + index[1];
                            else bKingPosition = "" + index[0] + index[1];

                        }
                        this.board = tempChessboard;
                    }
                }
                else this.board = tempChessboard;
                return !(this.board == tempChessboard) && playerTurn ;
            }
            public bool ImpossibilityOfCheckmate()
            {
                int knightnum = 0;
                Piece[,] bisops = new Piece[,] { { null, null }, { null, null } };
                for(int i=0;i<8;i++)
                    for (int j = 0; j < 8; j++)
                    {
                        if ((board[i][j] is Pawn) || (board[i][j] is Queen) || (board[i][j] is Rook))
                            return false;
                        if ((board[i][j] is Knight))
                        {
                            knightnum++;
                                if(knightnum>1)
                                return false;
                        }
                        if ((board[i][j] is Bishop))
                        {
                            bisops[board[i][j].isWhite ? 0 : 1, (i + j) % 2] = board[i][j];
                            if ((bisops[0, 0] != null) && (bisops[0, 1] != null) || (bisops[1, 0] != null && bisops[1, 1] != null) || (bisops[0, 0] != null && bisops[1, 1] != null) || (bisops[0, 1] != null && bisops[1, 0] != null))
                                return false;
                        }

                    }
                return true;
            }
            public string[] getInformationFromBoard()
            {
                string[] infoboard = new string[8];
          
                for (int line = 0; line < 8; line++)
                {
                    for (int column = 0; column < 8; column++)
                    {
                        infoboard[line] = "";
                        if (this.board[line][column] is Pawn)
                        {
                                infoboard[line] += " "+board[line][column].ToString();
                            if (((Pawn)this.board[line][column]).firstMove)
                                infoboard[line] += " first move ";
                            if (((Pawn)board[line][column]).enPassantRisk)
                                infoboard[line] += "enPassantRisk ";
                            continue;
                        }
                        if (this.board[line][column] is King)
                        {
                                infoboard[line] += " "+board[line][column].ToString();
                            if (((King)this.board[line][column]).firstMove)
                                infoboard[line] += " first move ";
                            continue;
                        }
                        if (this.board[line][column] is Queen)
                        {
                                infoboard[line] +=" "+ board[line][column].ToString();
                            continue;
                        }
                        if (this.board[line][column] is Rook)
                        {
                                infoboard[line] += " "+board[line][column].ToString();
                            if (((Rook)this.board[line][column]).firstMove)
                                infoboard[line] += " first move ";
                            continue;
                        }
                        if (this.board[line][column] is Bishop)
                        {
                                infoboard[line] += " "+board[line][column].ToString();
                            continue;
                        }
                        if (this.board[line][column] is Knight)
                                infoboard[line] += " "+board[line][column].ToString();
                    }                              
                }
                return infoboard;
            }
            public string[][] insertInfoToPreviousbords(string[] info, string[][] previousbords)
            {
                string[][] newPreviousbords;
                if (previousbords.Length==1)
                    newPreviousbords= new string[previousbords.Length][];
                else newPreviousbords = new string[previousbords.Length+1][];
                newPreviousbords[previousbords.Length] = info;
                for (int i = 0; i < previousbords.Length;i++)
                {
                    newPreviousbords[i] = previousbords[i];
                }
                return newPreviousbords;
            }
            public bool threefoldRepetition(string[][] previousboards)
            {
                
                for (int iboards = 0; iboards < previousboards.Length; iboards++)
                {
                    int equalbords = 0;
                    bool ifEqual = true;
                    for (int i = 0; i < previousboards.Length; i++)
                    {
                        for (int j = 0; j < 8; j++)
                        {
                            if (previousboards[iboards][j] != previousboards[i][j] && iboards != i)
                                ifEqual= false;
                        }
                       if(ifEqual) 
                           equalbords++;
                    }
                    if (equalbords > 3)
                        return true;
                }
                return false;
            }
            public int piecesOnBoard()
            {
                int sum = 0;
                return sum;
            }
            public void game()
            {
                bool isItWhiteTurn = true;
                string index;
                bool legalMove, stalemate = false;// Checkmate = false;
                int movesCount = 0;
                string[][] preciousbords = new string[1][];
                bool ifThreefoldRepetition=false;
                int howManyPiecesOnBoard = 32; 
                while ( !stalemate && movesCount<50 && !ImpossibilityOfCheckmate() && ifThreefoldRepetition )
                {
                     index = getInput();
                    legalMove = movement(index, isItWhiteTurn);
                   
                    print();
                    stalemate = blackStalemate() || whiteStalemate();
                    if (legalMove)
                    {
                        preciousbords = this.insertInfoToPreviousbords(getInformationFromBoard(), preciousbords);
                        ifThreefoldRepetition = threefoldRepetition(preciousbords);
                      //  if (howManyPiecesOnBoard==piecesOnBoard() && Not pawn)
                        movesCount++;
                        if (stalemate)
                        {
                            if (whiteChess())
                                Console.WriteLine("black is the winner");
                            if (blackChess())
                                Console.WriteLine("white is the winner");
                            Console.WriteLine((stalemate && !whiteChess() && !blackChess()) || movesCount==50 ||ImpossibilityOfCheckmate() ? "There is a tie" : "");
                            break;
                        }
                        if (whiteChess())
                            Console.WriteLine("CHESS!!! threat on the white king");
                        if (blackChess())
                            Console.WriteLine("CHESS!!! threat on the black king ");
                        isItWhiteTurn = !isItWhiteTurn;
                    }
                    else
                        Console.WriteLine("illegal move");
                }

            }
            public Piece[][] copyBoard()
            {
                ChessBoard copyChessboard = new ChessBoard();

                for (int line = 0; line < 8; line++)
                {
                    for (int column = 0; column < 8; column++)
                    {
                        if (this.board[line][column] is Pawn)
                        {
                            if (this.board[line][column].isWhite)
                                copyChessboard.board[line][column] = new Pawn(true);
                            else copyChessboard.board[line][column] = new Pawn(false);
                           ((Pawn) copyChessboard.board[line][column]).firstMove = ((Pawn)this.board[line][column]).firstMove;
                           ((Pawn)copyChessboard.board[line][column]).enPassantRisk = ((Pawn)this.board[line][column]).enPassantRisk;
                            continue;
                        }
                        if (this.board[line][column] is King)
                        {
                            if (this.board[line][column].isWhite)
                                copyChessboard.board[line][column] = new King(true);
                            else copyChessboard.board[line][column] = new King(false);
                            ((King)copyChessboard.board[line][column]).firstMove = ((King)this.board[line][column]).firstMove;
                            continue;
                        }
                        if (this.board[line][column] is Queen)
                        {
                            if (this.board[line][column].isWhite)
                                copyChessboard.board[line][column] = new Queen(true);
                            else copyChessboard.board[line][column] = new Queen(false); ;
                            continue;
                        }
                        if (this.board[line][column] is Rook)
                        {
                            if (this.board[line][column].isWhite)
                                copyChessboard.board[line][column] = new Rook(true);
                            else copyChessboard.board[line][column] = new Rook(false);
                            ((Rook)copyChessboard.board[line][column]).firstMove = ((Rook)this.board[line][column]).firstMove;
                            continue;
                        }
                        if (this.board[line][column] is Bishop)
                        {
                            if (this.board[line][column].isWhite)
                                copyChessboard.board[line][column] = new Bishop(true);
                            else copyChessboard.board[line][column] = new Bishop(false);

                            continue;
                        }
                        if (this.board[line][column] is Knight)
                        {
                            if (this.board[line][column].isWhite)
                                copyChessboard.board[line][column] = new Knight(true);
                            else copyChessboard.board[line][column] = new Knight(false);

                        }

                        continue;

                    }
                }
                return copyChessboard.board;
            }

        }
    }