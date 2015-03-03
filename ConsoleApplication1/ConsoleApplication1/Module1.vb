'Original written by the AQA COMP1 Programmer Team (suck at formatting)
'Extras by Jack Steel

Imports System.Math	'IMPORTS MATHS - FOR CALCULATIONS AND ABSOLUTE VALUES ------ NOTE: THE BUILT-IN FUNCTION -- abs(sbyte) -- returns the MAGNITUDE of a signed integer --> it ignores any minus sign

Module module1
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
			'INITIALISES TURN AND GAME STATE
			WhoseTurn = "W"
			GameOver = False


			Console.Write("Do you want to play the sample game (enter Y for Yes)? ")


			SampleGame = Console.ReadLine

			'http://www.asciitable.com/
			'VALUES 97 TO 122 ARE ALL THE LOWERCASE ALPHABET

			'VALIDATES AS ALPHABET CHARACTER
			If Asc(SampleGame) >= 97 And Asc(SampleGame) <= 122 Then

				'GETS THE UPPERCASE VERSION OF THE CHARACTER ENTERED USING ASCII VALUES
				SampleGame = Chr(Asc(SampleGame) - 32)

			End If
			InitialiseBoard(Board, SampleGame) 'CALLS InitialiseBoard, "Board" IS A 2D ARRAY OF STRING - 0-BASED - 9x9


			'START NEW TURN
			Do 'until playagain not equal to Y

				DisplayBoard(Board)	'CALLS DisplayBoard 

				DisplayWhoseTurnItIs(WhoseTurn)	'CALLS DisplayWhoseTurnItIs

				MoveIsLegal = False	'RESETS LEGAL MOVE BOOLEAN


				Do 'until gameover
					GetMove(StartSquare, FinishSquare) 'CALLS GetMove ||| AND GETS COORDS OF START SQUARE AND FINISH SQUARE FROM USER INPUT E.G(37, 25)

					StartRank = StartSquare Mod 10 'GETS "RANK"(ROW) BY GETTING THE REMAINDER OF A DIVISION BY 10, 37 MOD 10 = 7

					StartFile = StartSquare \ 10 'INTEGER DIVISION (BACKSLASH NOT FORWARDSLASH) GETS INTEGER PORTION OF DIVISION 37 \ 10 = 3

					'same for finish coords
					FinishRank = FinishSquare Mod 10
					FinishFile = FinishSquare \ 10

					'PASSES DATA ABOUT THE BOARD, START AND FINISH COORDS AND TURN TO CheckMoveIsLegal AT TO ENSURE MOVE IS LEGAL
					MoveIsLegal = CheckMoveIsLegal(Board, StartRank, StartFile, FinishRank, FinishFile, WhoseTurn)

					'informs user
					If Not MoveIsLegal Then
						Console.WriteLine("That is not a legal move - please try again")
					End If
					'keeps going until the user makes a legal move
				Loop Until MoveIsLegal

				GameOver = CheckIfGameWillBeWon(Board, FinishRank, FinishFile) 'CALLS CheckIfGameWillBeWon AT - CHECKS IF THE MOVE TAKEN RESULTS IN ENDGAME

				MakeMove(Board, StartRank, StartFile, FinishRank, FinishFile, WhoseTurn) 'CALLS MakeMove AT - CHANGES VALUES IN Board ARRAY TO REFLECT CURRENT MOVE

				If GameOver Then
					DisplayWinner(WhoseTurn) 'CALLS DisplayWinner
				End If

				'SWITCHES TURN TO NEXT PLAYER
				If WhoseTurn = "W" Then
					WhoseTurn = "B"
				Else
					WhoseTurn = "W"
				End If

				'REPEAT UNTIL SOMEONE WINS
			Loop Until GameOver

			'ASKS USER PLAY AGAIN?
			Console.Write("Do you want to play again (enter Y for Yes)? ")
			PlayAgain = Console.ReadLine

			'VALIDATES AS ALPHABET CHARACTER
			If Asc(PlayAgain) >= 97 And Asc(PlayAgain) <= 122 Then
				'GETS UPPERCASE CHARACTER
				PlayAgain = Chr(Asc(PlayAgain) - 32)
			End If

		Loop Until PlayAgain <> "Y"
	End Sub

	'CALLED AT START OF EACH TURN
	Sub DisplayWhoseTurnItIs(ByVal WhoseTurn As Char)
		If WhoseTurn = "W" Then
			Console.WriteLine("It is White's turn")
		Else
			Console.WriteLine("It is Black's turn")
		End If
	End Sub

	'NEVER CALLED -->, ************************LIKELY QUESTION AREA******************************************************************************************************************************************8
	Function GetTypeOfGame() As Char
		Dim TypeOfGame As Char
		Console.Write("Do you want to play the sample game (enter Y for Yes)? ")
		TypeOfGame = Console.ReadLine
		Return TypeOfGame
	End Function

	'CALLED WHEN GAMEOVER
	Sub DisplayWinner(ByVal WhoseTurn As Char)
		If WhoseTurn = "W" Then
			Console.WriteLine("Black's Sarrum has been captured.  White wins!")
		Else
			Console.WriteLine("White's Sarrum has been captured.  Black wins!")
		End If
		Console.WriteLine()
	End Sub

	'CALLED AT THE END OF EACH MOVE
	Function CheckIfGameWillBeWon(ByVal Board(,) As String, ByVal FinishRank As Integer, ByVal FinishFile As Integer) As Boolean
		If Board(FinishRank, FinishFile)(1) = "S" Then 'CHECKS IF PIECE THAT IS GOING TO BE CAPTURED IS THE Sarrum
			Return True
		Else
			Return False
		End If
	End Function

	'CALLED AT THE START OF EVERY TURN TO UPDATE VISUAL BOARD TO MATCH STORED ARRAY BOARD
	Sub DisplayBoard(ByVal Board(,) As String)
		Dim RankNo As Integer
		Dim FileNo As Integer

		Console.WriteLine()

		For RankNo = 1 To BoardDimension
			Console.WriteLine("    _________________________") 'ROW SEPERATOR
			Console.Write(RankNo & "   ") 'ROW NUMBER & PADDING

			'FILLS ROW ACCORDING TO CURRENT BOARD
			For FileNo = 1 To BoardDimension
				Console.Write("|" & Board(RankNo, FileNo))
			Next
			Console.WriteLine("|")

		Next

		Console.WriteLine("    _________________________")
		Console.WriteLine()
		Console.WriteLine("     1  2  3  4  5  6  7  8") 'COLUMN NUMBERS
		Console.WriteLine()
		Console.WriteLine()
	End Sub

	'CALLED FROM CheckMoveIsLegal FOR EVERY MOVE INVOLVING A REDUM
	Function CheckRedumMoveIsLegal(ByVal Board(,) As String, ByVal StartRank As Integer, ByVal StartFile As Integer, ByVal FinishRank As Integer, ByVal FinishFile As Integer, ByVal ColourOfPiece As Char) As Boolean
		'NESTED IFs CHECK; 
		If ColourOfPiece = "W" Then	'PIECE COLOUR

			If FinishRank = StartRank - 1 Then 'MOVED ONLY 1 ROW DOWN

				If FinishFile = StartFile And Board(FinishRank, FinishFile) = "  " Then	'FINISH POSITION IS EMPTY
					Return True

				ElseIf Abs(FinishFile - StartFile) = 1 And Board(FinishRank, FinishFile)(0) = "B" Then 'ELSE: MOVED DIAGONALLY TO TAKE A BLACK PIECE
					Return True

				End If
			End If

		Else 'PIECE COLOUR IS BLACK

			If FinishRank = StartRank + 1 Then 'MOVED ONLY 1 ROW UP

				If FinishFile = StartFile And Board(FinishRank, FinishFile) = "  " Then	'FINISH POSITION IS EMPTY
					Return True

				ElseIf Abs(FinishFile - StartFile) = 1 And Board(FinishRank, FinishFile)(0) = "W" Then 'ELSE: MOVED DIAGONALLY TO TAKE A WHITE PIECE
					Return True

				End If
			End If
		End If

		'ABOVE CONDITIONS OF A LEGAL MOVE ARE NOT MET --> MOVE IS NOT LEGAL ----> PROMPT USER TO TRY AGAIN
		Return False
	End Function

	'CALLED FROM CheckMoveIsLegal FOR EVERY MOVE INVOLVING A SARRUM
	Function CheckSarrumMoveIsLegal(ByVal Board(,) As String, ByVal StartRank As Integer, ByVal StartFile As Integer, ByVal FinishRank As Integer, ByVal FinishFile As Integer) As Boolean

		If Abs(FinishFile - StartFile) <= 1 And Abs(FinishRank - StartRank) <= 1 Then 'CHECKS PIECE MOVED ONLY 1 OR LESS SPACE ANY DIRECTION
			Return True

		End If

		'TRIED TO MOVE MORE THAN 1 SPACE --> MOVE IS NOT LEGAL ----> PROMPT USER TO TRY AGAIN
		Return False
	End Function

	'CALLED FROM CheckMoveIsLegal FOR EVERY MOVE INVOLVING A GISGIGIR(CHARIOT)																									who's idea was it to use names more complicated than the program itself?	
	Function CheckGisgigirMoveIsLegal(ByVal Board(,) As String, ByVal StartRank As Integer, ByVal StartFile As Integer, ByVal FinishRank As Integer, ByVal FinishFile As Integer) As Boolean
		Dim GisgigirMoveIsLegal As Boolean
		Dim Count As Integer
		Dim RankDifference As Integer
		Dim FileDifference As Integer

		'Resets
		GisgigirMoveIsLegal = False

		'DIFFERENCES
		RankDifference = FinishRank - StartRank
		FileDifference = FinishFile - StartFile

		'NESTED IFs CHECK;

		'**************************************************** MOVING HORIZONTALLY **********************************************************************************************************


		If RankDifference = 0 Then 'DIDN'T MOVE ANY ROWS?

			If FileDifference >= 1 Then	'MOVED ATLEAST 1 COLUMN RIGHT?
				GisgigirMoveIsLegal = True 'LEGAL SO FAR

				'LOOP FOR NUMBER OF SPACES BETWEEN START POSITION AND END POSITION
				For Count = 1 To FileDifference - 1

					If Board(StartRank, StartFile + Count) <> "  " Then	' EVERY SPACE BETWEEN START AND FINISH WAS EMPTY? - DID NOT JUMP OVER ANY PIECES
						GisgigirMoveIsLegal = False	'IT JUMPED A PIECE --> NOT LEGAL
					End If

				Next

			ElseIf FileDifference <= -1 Then 'DIDN'T MOVE RIGHT - MOVED ATLEAST 1 COLUMN LEFT?
				GisgigirMoveIsLegal = True 'LEGAL SO FAR

				'LOOP FOR NUMBER OF SPACES BETWEEN START POSITION AND END POSITION - BUT negative since it's in opposite direction
				For Count = -1 To FileDifference + 1 Step -1

					If Board(StartRank, StartFile + Count) <> "  " Then	'EVERY SPACE BETWEEN START AND FINISH WAS EMPTY? - DID NOT JUMP OVER ANY PIECES
						GisgigirMoveIsLegal = False	'IT JUMPED A PIECE --> NOT LEGAL
					End If

				Next
			End If

			'**************************************************** END **********************************************************************************************************

			'**************************************************** MOVING VERTICALLY **********************************************************************************************************


		ElseIf FileDifference = 0 Then 'DIDN'T MOVE LEFT OR RIGHT - MUST HAVE MOVED VERTICALLY

			If RankDifference >= 1 Then	'MOVED ATLEAST 1 ROW UP?
				GisgigirMoveIsLegal = True 'LEGAL SO FAR

				'LOOP FOR NUMBER OF SPACES BETWEEN START POSITION AND END POSITION																									i hope you get the idea by now, this happens alot..
				For Count = 1 To RankDifference - 1

					If Board(StartRank + Count, StartFile) <> "  " Then	'EVERY SPACE BETWEEN START AND FINISH WAS EMPTY? - DID NOT JUMP OVER ANY PIECES
						GisgigirMoveIsLegal = False	'IT JUMPED A PIECE --> NOT LEGAL
					End If

				Next

			ElseIf RankDifference <= -1 Then 'DIDN'T MOVE UP - MUST HAVE MOVED ATLEAST 1 ROW DOWN?
				GisgigirMoveIsLegal = True

				'LOOP FOR NUMBER OF SPACES BETWEEN START POSITION AND END POSITION - BUT negative since it's in opposite direction		
				For Count = -1 To RankDifference + 1 Step -1

					If Board(StartRank + Count, StartFile) <> "  " Then	'EVERY SPACE BETWEEN START AND FINISH WAS EMPTY? - DID NOT JUMP OVER ANY PIECES
						GisgigirMoveIsLegal = False	'IT JUMPED A PIECE --> NOT LEGAL																											(stop trying to cheat)
					End If

				Next
			End If

			'**************************************************** END **********************************************************************************************************

		End If

		'WAS IT LEGAL? BOOLEAN RETURNED
		Return GisgigirMoveIsLegal
	End Function

	'CALLED FROM CheckMoveIsLegal FOR EVERY MOVE INVOLVING A NABU
	Function CheckNabuMoveIsLegal(ByVal Board(,) As String, ByVal StartRank As Integer, ByVal StartFile As Integer, ByVal FinishRank As Integer, ByVal FinishFile As Integer) As Boolean

		If Abs(FinishFile - StartFile) = 1 And Abs(FinishRank - StartRank) = 1 Then	'MOVED 1 DIAGONALLY IN ANY DIRECTION? - <AND> THEREFORE MUST MOVE 1 ROW !AND! ONE COLUMN == DIAGONAL NOT JUST HORIOZNTAL OR VERTICAL
			Return True	'NOT CHEATING
		End If
		Return False 'CHEATING..stop it
	End Function

	'CALLED FROM CheckMoveIsLegal FOR EVERY MOVE INVOLVING A MARZAZ PANI
	Function CheckMarzazPaniMoveIsLegal(ByVal Board(,) As String, ByVal StartRank As Integer, ByVal StartFile As Integer, ByVal FinishRank As Integer, ByVal FinishFile As Integer) As Boolean

		'EITHER MOVED 1 COLUMN                                                !OR!                                    ONE ROW IN ANY DIRECTION -- CAN'T MOVE DIAGONALLY
		If Abs(FinishFile - StartFile) = 1 And Abs(FinishRank - StartRank) = 0 Or Abs(FinishFile - StartFile) = 0 And Abs(FinishRank - StartRank) = 1 Then
			Return True
		End If

		Return False
	End Function

	'CALLED FROM CheckMoveIsLegal FOR EVERY MOVE INVOLVING A ETLU
	Function CheckEtluMoveIsLegal(ByVal Board(,) As String, ByVal StartRank As Integer, ByVal StartFile As Integer, ByVal FinishRank As Integer, ByVal FinishFile As Integer) As Boolean

		'EITHER MOVE 2 COLUMNS ALONG                                          !OR!                                    2 ROWS VERTICALLY -- MUST MOVE EXACTLY 2 NO MORE NO LESS, CANNOT MOVE DIAGONALLY
		If Abs(FinishFile - StartFile) = 2 And Abs(FinishRank - StartRank) = 0 Or Abs(FinishFile - StartFile) = 0 And Abs(FinishRank - StartRank) = 2 Then
			Return True
		End If
		Return False
	End Function

	'CALLED WHENEVER A PLAYER !ATTEMPTS! TO MAKE A MOVE
	Function CheckMoveIsLegal(ByVal Board(,) As String, ByVal StartRank As Integer, ByVal StartFile As Integer, ByVal FinishRank As Integer, ByVal FinishFile As Integer, ByVal WhoseTurn As Char) As Boolean
		Dim PieceType As Char
		Dim PieceColour As Char

		'DID THEY JUST TRY AND GET OUT OF A MOVE? - IS EVERYTHING THE SAME?
		If FinishFile = StartFile And FinishRank = StartRank Then
			Return False 'YEAH THEY DID, NOT GONNA HAPPEN
		End If


		PieceType = Board(StartRank, StartFile)(1) 'INITIAL OF PIECE
		PieceColour = Board(StartRank, StartFile)(0) 'BLACK OR WHITE

		'NESTED IFs CHECKS;
		If WhoseTurn = "W" Then	'WHITE TURN?

			If PieceColour <> "W" Then 'WHITE TRYING TO MOVE BLACK PIECE?
				Return False 'YES
			End If
			'NO

			If Board(FinishRank, FinishFile)(0) = "W" Then 'TRYING TO STACK WHITE PIECES?
				Return False 'YES
			End If
			'NO --> SKIP TO CASE STATEMENT

		Else 'MUST BE BLACK TURN

			If PieceColour <> "B" Then 'BLACK TRYING TO MOVE WHITE PIECE?
				Return False 'YES
			End If
			'NO

			If Board(FinishRank, FinishFile)(0) = "B" Then 'TRYING TO STACK BLACK PIECES?
				Return False 'YES
			End If
			'NO --> CASE STATEMENT
		End If


		Select Case PieceType 'WHAT PIECE IS IT? ----> ASKS THE RESPECTIVE SUB TO FIGURE IT OUT AND GIVE ME AN ANSWER															please

			Case "R" 'REDUM
				Return CheckRedumMoveIsLegal(Board, StartRank, StartFile, FinishRank, FinishFile, PieceColour)

			Case "S" 'SARRUM
				Return CheckSarrumMoveIsLegal(Board, StartRank, StartFile, FinishRank, FinishFile)

			Case "M" 'MARZAZ PANI
				Return CheckMarzazPaniMoveIsLegal(Board, StartRank, StartFile, FinishRank, FinishFile)

			Case "G" 'GISGIGIR
				Return CheckGisgigirMoveIsLegal(Board, StartRank, StartFile, FinishRank, FinishFile)

			Case "N" 'NABU
				Return CheckNabuMoveIsLegal(Board, StartRank, StartFile, FinishRank, FinishFile)

			Case "E" 'ETLU
				Return CheckEtluMoveIsLegal(Board, StartRank, StartFile, FinishRank, FinishFile)


			Case Else
				Return False 'TRYING TO MOVE AN EMPTY SPACE OR SOMETHING ELSE WENT WRONG --> CAN'T POSSIBLY BE LEGAL ----> TRY AGAIN? HOPE FOR THE BEST
		End Select
	End Function

	'CALLED AT START OF A NEW GAME
	Sub InitialiseBoard(ByRef Board(,) As String, ByVal SampleGame As Char)
		Dim RankNo As Integer
		Dim FileNo As Integer


		If SampleGame = "Y" Then 'PLAY SAMPLE GAME?

			'EMPTY THE ENTIRE BOARD
			For RankNo = 1 To BoardDimension
				For FileNo = 1 To BoardDimension
					Board(RankNo, FileNo) = "  "
				Next
			Next
			'= EMPTY BOARD

			'FILL THESE COORDINATES WITH THESE PIECES !ONLY!
			Board(1, 2) = "BG"
			Board(1, 4) = "BS"
			Board(1, 8) = "WG"
			Board(2, 1) = "WR"
			Board(3, 1) = "WS"
			Board(3, 2) = "BE"
			Board(3, 8) = "BE"
			Board(6, 8) = "BR"
			'START GAME NEAR END


		Else 'NO, PLAY FULL GAME

			For RankNo = 1 To BoardDimension 'LOOP THROUGH ALL ROWS
				For FileNo = 1 To BoardDimension 'LOOP THROUGH ALL COLUMNS OF THE CURRENT ROW

					If RankNo = 2 Then 'SECOND ROW UP?
						Board(RankNo, FileNo) = "BR" 'PUT A BLACK REDUM THERE

					ElseIf RankNo = 7 Then 'SEVENTH ROW UP?
						Board(RankNo, FileNo) = "WR" 'PUT A WHITE REDUM THERE

					ElseIf RankNo = 1 Or RankNo = 8 Then 'BOTTOM OR TOP ROW?
						If RankNo = 1 Then Board(RankNo, FileNo) = "B" 'BOTTOM ROW? --> ITS GOING TO BE A BLACK PIECE
						If RankNo = 8 Then Board(RankNo, FileNo) = "W" 'TOP ROW? --> ITS GOING TO BE A WHITE PIECE

						'PLACE PIECES
						Select Case FileNo 'WHICH COLUMN?
							Case 1, 8 'COLUMN 1 OR 8?
								Board(RankNo, FileNo) = Board(RankNo, FileNo) & "G"	'KEEP PIECE COLOUR AND PUT A GISGIGIR THERE

							Case 2, 7 'COLUMN 2 OR 7?
								Board(RankNo, FileNo) = Board(RankNo, FileNo) & "E"	'KEEP PIECE COLOUR AND PUT A ETLU THERE

							Case 3, 6 'COLUMN 3 OR 6?
								Board(RankNo, FileNo) = Board(RankNo, FileNo) & "N"	'KEEP PIECE COLOUR AND PUT A NABU THERE

							Case 4 'COLUMN 4?
								Board(RankNo, FileNo) = Board(RankNo, FileNo) & "M"	'KEEP PIECE COLOUR AND PUT A MARZAZ PANI THERE

							Case 5 'COLUMN 5?
								Board(RankNo, FileNo) = Board(RankNo, FileNo) & "S"	'KEEP PIECE COLOUR AND PUT A SARRUM THERE

						End Select


					Else 'NOT ANY OF THE SPACES THAT START WITH A PIECE
						Board(RankNo, FileNo) = "  " 'PUT A BLANK SPACE THERE
					End If
				Next
			Next
		End If
	End Sub

	'CALLED AT THE START OF EVERY TURN
	Sub GetMove(ByRef StartSquare As Integer, ByRef FinishSquare As Integer)

		Console.Write("Enter coordinates of square containing piece to move (file first): ") 'ASK USER FOR COORDS OF PIECE THEY WANT TO MOVE E.G(37)
		StartSquare = Console.ReadLine 'SET INPUT

		Console.Write("Enter coordinates of square to move piece to (file first): ") 'ASK USER FOR COORDS TO MOVE PIECE TO E.G(38)
		FinishSquare = Console.ReadLine	'SET INPUT

	End Sub

	'CALLED NEAR THE END OF EVERY TURN
	Sub MakeMove(ByRef Board(,) As String, ByVal StartRank As Integer, ByVal StartFile As Integer, ByVal FinishRank As Integer, ByVal FinishFile As Integer, ByVal WhoseTurn As Char)

		'WHITE TURN? <AND> THE PIECE FINISHES ON BLACK'S BACK ROW? <AND> THE PIECE IS A REDUM?
		If WhoseTurn = "W" And FinishRank = 1 And Board(StartRank, StartFile)(1) = "R" Then

			Board(FinishRank, FinishFile) = "WM" 'UPGRADE THE REDUM TO A MARZAZ PANI

			Board(StartRank, StartFile) = "  " 'EMPTY THE SPACE IT CAME FROM

			'BLACK TURN? <AND> PIECE FINISHES AT WHITE'S BACK ROW? <AND> PIECE IS A REDUM?
		ElseIf WhoseTurn = "B" And FinishRank = 8 And Board(StartRank, StartFile)(1) = "R" Then

			Board(FinishRank, FinishFile) = "BM" 'UPGRADE THE REDUM TO A MARZAZ PANI

			Board(StartRank, StartFile) = "  " 'EMPTY THE SPACE IT CAME FROM

			'NIETHER OF THOSE ARE ALL TRUE? --> DOESN'T MATTER WHOSE TURN IT IS (ALL CHECKS DONE PRIOR) ----> MOVE THE PIECE
		Else
			Board(FinishRank, FinishFile) = Board(StartRank, StartFile)	'MOVE PIECE FROM START POSITION TO FINISH POSITION
			Board(StartRank, StartFile) = "  " 'EMPTY THE START POSITION IT CAME FROM
		End If
	End Sub


	'DONE
End Module
