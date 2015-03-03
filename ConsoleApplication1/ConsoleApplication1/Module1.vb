'Skeleton Program code for the AQA COMP1 Summer 2015 examination
'this code whould be used in conjunction with the Preliminary Material
'written by the AQA COMP1 Programmer Team
'developed in the Visual Studio 2008 programming environment
'Modified by Richard Clarke

Imports System.Math
Module CaptureTheSarrum
    Const BoardDimension As Integer = 8

    Sub Main()
        Dim Board(BoardDimension, BoardDimension) As String
        Dim GameOver As Boolean
        Dim StartSquare As Integer
        Dim FinishSquare As Integer
        Dim StartRank As Integer
        Dim StartFile As Integer
        Dim FinishRank As Integer
        Dim FinishFile As Integer
        Dim MoveIsLegal As Boolean
        Dim PlayAgain As Char
        Dim SampleGame As Char
        Dim WhoseTurn As Char
        PlayAgain = "Y"
        Do
            WhoseTurn = "W"
            GameOver = False
            GetTypeOfGame() 'Gets the type of game as a proceude instead of manually doing it. 
            'Console.Write("Do you want to play the sample game (enter Y for Yes)? ")
            'SampleGame = Console.ReadLine
            If Asc(SampleGame) >= 97 And Asc(SampleGame) <= 122 Then
                SampleGame = Chr(Asc(SampleGame) - 32)
            End If
            InitialiseBoard(Board, SampleGame)
            Do
                DisplayBoard(Board)
                DisplayWhoseTurnItIs(WhoseTurn)
                MoveIsLegal = False
                Do
                    GetMove(StartSquare, FinishSquare)
                    StartRank = StartSquare Mod 10
                    StartFile = StartSquare \ 10
                    FinishRank = FinishSquare Mod 10
                    FinishFile = FinishSquare \ 10
                    MoveIsLegal = CheckMoveIsLegal(Board, StartRank, StartFile, FinishRank, FinishFile, WhoseTurn)
                    If Not MoveIsLegal Then
                        Console.WriteLine("That is not a legal move - please try again")
                    End If
                Loop Until MoveIsLegal
                GameOver = CheckIfGameWillBeWon(Board, FinishRank, FinishFile)
                MakeMove(Board, StartRank, StartFile, FinishRank, FinishFile, WhoseTurn)
                If GameOver Then
                    DisplayWinner(WhoseTurn)
                End If
                If WhoseTurn = "W" Then
                    WhoseTurn = "B"
                Else
                    WhoseTurn = "W"
                End If
            Loop Until GameOver
            Console.Write("Do you want to play again (enter Y for Yes)? ")
            PlayAgain = Console.ReadLine
            If Asc(PlayAgain) >= 97 And Asc(PlayAgain) <= 122 Then
                PlayAgain = Chr(Asc(PlayAgain) - 32)
            End If
        Loop Until PlayAgain <> "Y"
    End Sub

    Sub DisplayWhoseTurnItIs(ByVal WhoseTurn As Char)
        If WhoseTurn = "W" Then
            Console.WriteLine("It is White's turn")
        Else
            Console.WriteLine("It is Black's turn")
        End If
    End Sub

    Function GetTypeOfGame() As Char
        Dim TypeOfGame As Char
        Console.Write("Do you want to play the sample game (enter Y for Yes)? ")
        TypeOfGame = Console.ReadLine
        Return TypeOfGame
    End Function

    Sub DisplayWinner(ByVal WhoseTurn As Char)
        If WhoseTurn = "W" Then
            Console.WriteLine("Black's Sarrum has been captured.  White wins!")
        Else
            Console.WriteLine("White's Sarrum has been captured.  Black wins!")
        End If
        Console.WriteLine()
    End Sub

    Function CheckIfGameWillBeWon(ByVal Board(,) As String, ByVal FinishRank As Integer, ByVal FinishFile As Integer) As Boolean
        If Board(FinishRank, FinishFile)(1) = "S" Then
            Return True
        Else
            Return False
        End If
    End Function

    Sub DisplayBoard(ByVal Board(,) As String)
        Dim RankNo As Integer
        Dim FileNo As Integer
        Console.WriteLine()
        For RankNo = 1 To BoardDimension
            Console.WriteLine("    _________________________")
            Console.Write(RankNo & "   ")
            For FileNo = 1 To BoardDimension
                Console.Write("|" & Board(RankNo, FileNo))
            Next
            Console.WriteLine("|")
        Next
        Console.WriteLine("    _________________________")
        Console.WriteLine()
        Console.WriteLine("     1  2  3  4  5  6  7  8")
        Console.WriteLine()
        Console.WriteLine()
    End Sub

    Function CheckRedumMoveIsLegal(ByVal Board(,) As String, ByVal StartRank As Integer, ByVal StartFile As Integer, ByVal FinishRank As Integer, ByVal FinishFile As Integer, ByVal ColourOfPiece As Char) As Boolean
        If ColourOfPiece = "W" Then
            If FinishRank = StartRank - 1 Then
                If FinishFile = StartFile And Board(FinishRank, FinishFile) = "  " Then
                    Return True
                ElseIf Abs(FinishFile - StartFile) = 1 And Board(FinishRank, FinishFile)(0) = "B" Then
                    Return True
                End If
            End If
        Else
            If FinishRank = StartRank + 1 Then
                If FinishFile = StartFile And Board(FinishRank, FinishFile) = "  " Then
                    Return True
                ElseIf Abs(FinishFile - StartFile) = 1 And Board(FinishRank, FinishFile)(0) = "W" Then
                    Return True
                End If
            End If
        End If
        Return False
    End Function

    Function CheckSarrumMoveIsLegal(ByVal Board(,) As String, ByVal StartRank As Integer, ByVal StartFile As Integer, ByVal FinishRank As Integer, ByVal FinishFile As Integer) As Boolean
        If Abs(FinishFile - StartFile) <= 1 And Abs(FinishRank - StartRank) <= 1 Then
            Return True
        End If
        Return False
    End Function

    Function CheckGisgigirMoveIsLegal(ByVal Board(,) As String, ByVal StartRank As Integer, ByVal StartFile As Integer, ByVal FinishRank As Integer, ByVal FinishFile As Integer) As Boolean
        Dim GisgigirMoveIsLegal As Boolean
        Dim Count As Integer
        Dim RankDifference As Integer
        Dim FileDifference As Integer
        GisgigirMoveIsLegal = False
        RankDifference = FinishRank - StartRank
        FileDifference = FinishFile - StartFile
        If RankDifference = 0 Then
            If FileDifference >= 1 Then
                GisgigirMoveIsLegal = True
                For Count = 1 To FileDifference - 1
                    If Board(StartRank, StartFile + Count) <> "  " Then
                        GisgigirMoveIsLegal = False
                    End If
                Next
            ElseIf FileDifference <= -1 Then
                GisgigirMoveIsLegal = True
                For Count = -1 To FileDifference + 1 Step -1
                    If Board(StartRank, StartFile + Count) <> "  " Then
                        GisgigirMoveIsLegal = False
                    End If
                Next
            End If
        ElseIf FileDifference = 0 Then
            If RankDifference >= 1 Then
                GisgigirMoveIsLegal = True
                For Count = 1 To RankDifference - 1
                    If Board(StartRank + Count, StartFile) <> "  " Then
                        GisgigirMoveIsLegal = False
                    End If
                Next
            ElseIf RankDifference <= -1 Then
                GisgigirMoveIsLegal = True
                For Count = -1 To RankDifference + 1 Step -1
                    If Board(StartRank + Count, StartFile) <> "  " Then
                        GisgigirMoveIsLegal = False
                    End If
                Next
            End If
        End If
        Return GisgigirMoveIsLegal
    End Function

    Function CheckNabuMoveIsLegal(ByVal Board(,) As String, ByVal StartRank As Integer, ByVal StartFile As Integer, ByVal FinishRank As Integer, ByVal FinishFile As Integer) As Boolean
        If Abs(FinishFile - StartFile) = 1 And Abs(FinishRank - StartRank) = 1 Then
            Return True
        End If
        Return False
    End Function

    Function CheckMarzazPaniMoveIsLegal(ByVal Board(,) As String, ByVal StartRank As Integer, ByVal StartFile As Integer, ByVal FinishRank As Integer, ByVal FinishFile As Integer) As Boolean
        If Abs(FinishFile - StartFile) = 1 And Abs(FinishRank - StartRank) = 0 Or Abs(FinishFile - StartFile) = 0 And Abs(FinishRank - StartRank) = 1 Then
            Return True
        End If
        Return False
    End Function

    Function CheckEtluMoveIsLegal(ByVal Board(,) As String, ByVal StartRank As Integer, ByVal StartFile As Integer, ByVal FinishRank As Integer, ByVal FinishFile As Integer) As Boolean
        If Abs(FinishFile - StartFile) = 2 And Abs(FinishRank - StartRank) = 0 Or Abs(FinishFile - StartFile) = 0 And Abs(FinishRank - StartRank) = 2 Then
            Return True
        End If
        Return False
    End Function

    Function CheckMoveIsLegal(ByVal Board(,) As String, ByVal StartRank As Integer, ByVal StartFile As Integer, ByVal FinishRank As Integer, ByVal FinishFile As Integer, ByVal WhoseTurn As Char) As Boolean
        Dim PieceType As Char
        Dim PieceColour As Char
        If FinishFile = StartFile And FinishRank = StartRank Then
            Return False
        End If
        PieceType = Board(StartRank, StartFile)(1)
        PieceColour = Board(StartRank, StartFile)(0)
        If WhoseTurn = "W" Then
            If PieceColour <> "W" Then
                Return False
            End If
            If Board(FinishRank, FinishFile)(0) = "W" Then
                Return False
            End If
        Else
            If PieceColour <> "B" Then
                Return False
            End If
            If Board(FinishRank, FinishFile)(0) = "B" Then
                Return False
            End If
        End If
        Select Case PieceType
            Case "R"
                Return CheckRedumMoveIsLegal(Board, StartRank, StartFile, FinishRank, FinishFile, PieceColour)
            Case "S"
                Return CheckSarrumMoveIsLegal(Board, StartRank, StartFile, FinishRank, FinishFile)
            Case "M"
                Return CheckMarzazPaniMoveIsLegal(Board, StartRank, StartFile, FinishRank, FinishFile)
            Case "G"
                Return CheckGisgigirMoveIsLegal(Board, StartRank, StartFile, FinishRank, FinishFile)
            Case "N"
                Return CheckNabuMoveIsLegal(Board, StartRank, StartFile, FinishRank, FinishFile)
            Case "E"
                Return CheckEtluMoveIsLegal(Board, StartRank, StartFile, FinishRank, FinishFile)
            Case Else
                Return False
        End Select
    End Function

    Sub InitialiseBoard(ByRef Board(,) As String, ByVal SampleGame As Char)
        Dim RankNo As Integer
        Dim FileNo As Integer
        If SampleGame = "Y" Then
            For RankNo = 1 To BoardDimension
                For FileNo = 1 To BoardDimension
                    Board(RankNo, FileNo) = "  "
                Next
            Next
            Board(1, 2) = "BG"
            Board(1, 4) = "BS"
            Board(1, 8) = "WG"
            Board(2, 1) = "WR"
            Board(3, 1) = "WS"
            Board(3, 2) = "BE"
            Board(3, 8) = "BE"
            Board(6, 8) = "BR"
        Else
            For RankNo = 1 To BoardDimension
                For FileNo = 1 To BoardDimension
                    If RankNo = 2 Then
                        Board(RankNo, FileNo) = "BR"
                    ElseIf RankNo = 7 Then
                        Board(RankNo, FileNo) = "WR"
                    ElseIf RankNo = 1 Or RankNo = 8 Then
                        If RankNo = 1 Then Board(RankNo, FileNo) = "B"
                        If RankNo = 8 Then Board(RankNo, FileNo) = "W"
                        Select Case FileNo
                            Case 1, 8
                                Board(RankNo, FileNo) = Board(RankNo, FileNo) & "G"
                            Case 2, 7
                                Board(RankNo, FileNo) = Board(RankNo, FileNo) & "E"
                            Case 3, 6
                                Board(RankNo, FileNo) = Board(RankNo, FileNo) & "N"
                            Case 4
                                Board(RankNo, FileNo) = Board(RankNo, FileNo) & "M"
                            Case 5
                                Board(RankNo, FileNo) = Board(RankNo, FileNo) & "S"
                        End Select
                    Else
                        Board(RankNo, FileNo) = "  "
                    End If
                Next
            Next
        End If
    End Sub

    Sub GetMove(ByRef StartSquare As Integer, ByRef FinishSquare As Integer)
        Console.Write("Enter coordinates of square containing piece to move (file first): ")
        StartSquare = Console.ReadLine
        Console.Write("Enter coordinates of square to move piece to (file first): ")
        FinishSquare = Console.ReadLine
    End Sub

    Sub MakeMove(ByRef Board(,) As String, ByVal StartRank As Integer, ByVal StartFile As Integer, ByVal FinishRank As Integer, ByVal FinishFile As Integer, ByVal WhoseTurn As Char)
        If WhoseTurn = "W" And FinishRank = 1 And Board(StartRank, StartFile)(1) = "R" Then
            Board(FinishRank, FinishFile) = "WM"
            Board(StartRank, StartFile) = "  "
        ElseIf WhoseTurn = "B" And FinishRank = 8 And Board(StartRank, StartFile)(1) = "R" Then
            Board(FinishRank, FinishFile) = "BM"
            Board(StartRank, StartFile) = "  "
        Else
            Board(FinishRank, FinishFile) = Board(StartRank, StartFile)
            Board(StartRank, StartFile) = "  "
        End If
    End Sub
End Module
