Imports MySql.Data.MySqlClient

Public Class frmConnect4Network

    'Declares the variable used to determine the horizontal coordinate of the square being used
    Dim x As Integer = 1
    'Declares the variable used to determine the vertical coordinate of the square being used
    Dim y As Integer = 1
    'Declares the array containing the coordinates of all the pictureboxes that make up the grid
    Dim Group(7, 6) As PictureBox
    'Declares the array containing the coordinates of the squares of the grid and what condition they are currently in
    '0 = Empty
    '1 = Red
    '2=  Yellow
    '3 = Terminator
    Dim theGrid(7, 7) As Integer
    'Declares the array containing the number of the buttons for dropping counters
    Dim Buttons(7) As Button

    'Declares the class used for both players
    Public Class C4Play
        'Declares the variables used for storing the details of the Connect Four player
        Public Colour, Username, Fname, lname As String
        Public StudentID As Integer
    End Class
    'Declares the class used for both players' scores
    Public Class Score
        'Declares the variable used for storing score of the player
        Public ScoreNum As Integer
        'This subroutine runs when a player wins a game
        Public Sub Increase()
            'Increases the score by 1
            ScoreNum = ScoreNum + 1
        End Sub
    End Class
    'Creates two objects of the Score class, one for each player
    Dim RedScore As New Score
    Dim YellowScore As New Score
    'Creates two objects of the C4Play class, one for each player
    Dim RedPlayer As New C4Play
    Dim YellowPlayer As New C4Play

    'Declares the variable used to store the primary key of the question record in the database
    Dim QuestionID As Integer
    'Declares the y-coords for moving the current player label
    Dim StudentCurrLocationY As Integer = 35
    Dim OppStudentCurrLocationY As Integer = 140
    'Declares the variable used for detecting wins along the y-axis
    Dim Why As Integer
    'Declares the variable used to keep track of how many reds in a row there are
    Dim RedAddUp As Integer = 0
    'Declares the variable used to keep track of how many yellow in a row there are
    Dim YellowAddUp As Integer = 0
    'Declares the variable used to determine if a question was answered correctly
    Dim QCorrect As Boolean = False
    'Declares the variables used to store whether there is a winner or the game is a draw
    Dim Won As Boolean = False
    Dim Draw As Boolean = False
    'Declares the variable used to determine whose go it is
    Dim YourGo As Boolean

    'Subroutine runs on form load
    Private Sub frmConnect4Network_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Runs the OpenDB subroutine
        OpenDB()

        'If the logged-in student is red...
        If LoggedInStudent.C4Player = "Red" Then
            'Populate the RedPlayer object with the logged-in student's details
            With RedPlayer
                .Colour = "Red"
                .Username = LoggedInStudent.Username
                .Fname = LoggedInStudent.Fname
                .lname = LoggedInStudent.Lname
                .StudentID = LoggedInStudent.StudentID
            End With

            'Populate the YellowPlayer object with the opponent student's details
            With YellowPlayer
                .Colour = "Yellow"
                .Username = OppStudent.Username
                .Fname = OppStudent.Fname
                .lname = OppStudent.Lname
                .StudentID = OppStudent.StudentID
            End With
            'But if the logged-in student is yellow...
        Else
            'Populate the RedPlayer object with the logged-in student's details
            With RedPlayer
                .Colour = "Red"
                .Username = OppStudent.Username
                .Fname = OppStudent.Fname
                .lname = OppStudent.Lname
                .StudentID = OppStudent.StudentID
            End With

            'Populate the YellowPlayer object with the opponent student's details
            With YellowPlayer
                .Colour = "Yellow"
                .Username = LoggedInStudent.Username
                .Fname = LoggedInStudent.Fname
                .lname = LoggedInStudent.Lname
                .StudentID = LoggedInStudent.StudentID
            End With
        End If

        'Runs the AccountSection subroutine
        AccountSection()
        'Runs the MakeGrid subroutine
        MakeGrid()
        'Runs the Terminators subroutine
        Terminators()

        'Declares the variables used to store the current horizontal and vertical coordinates of the grid
        Dim GridHor, GridVer As Integer

        'For each column of the grid...
        For GridHor = 1 To 7
            'For each square in that column...
            For GridVer = 1 To 6
                'Set the value to empty
                theGrid(GridHor, GridVer) = 0
            Next
        Next

        'Sets the scores to the defaults
        RedScore.ScoreNum = 0
        YellowScore.ScoreNum = 0

        Dim sql As String
        Dim dbcomm As MySqlCommand
        Dim dbread As MySqlDataReader

        'Builds SQL query to execute
        sql = "SELECT * FROM `tblconnect4` WHERE `GameID`=" & GameID & ";"

        dbcomm = New MySqlCommand(sql, DBConn)
        dbread = dbcomm.ExecuteReader
        While dbread.Read
            C4Player = dbread("CurrentPlayer")
        End While
        dbread.Close()
        CurrPlayer()

        'Hides the counter placing buttons
        btn1.Visible = False
        btn2.Visible = False
        btn3.Visible = False
        btn4.Visible = False
        btn5.Visible = False
        btn6.Visible = False
        btn7.Visible = False

        'If the logged-in student is the current player...
        If LoggedInStudent.C4Player = C4Player Then
            'Hides the question controls
            grpQuestion.Visible = True
            txtAnswer.Visible = True
            lblQuestion.Visible = True
            btnSubmit.Visible = True

            'Runs the Question subroutine
            Question()
        Else
            'Shows the question controls
            grpQuestion.Visible = False
            txtAnswer.Visible = False
            lblQuestion.Visible = False
            btnSubmit.Visible = False
        End If
        'Starts the update timer
        tmrUpdate.Enabled = True
    End Sub

    'Subroutine runs when called in DetectWinner subroutine
    Sub DatabaseDetails()
        'Declares the StreamWriter used to write to the game breakdown text files
        Dim writer As System.IO.StreamWriter
        Dim sql As String
        Dim dbcomm As MySqlCommand
        Dim dbread As MySqlDataReader

        'If the red player is the winner...
        If RedScore.ScoreNum = 1 Then
            If RedPlayer.Username = LoggedInStudent.Username Then
                'Builds SQL query to execute
                sql = "UPDATE  `tblstudents` SET  `Wins`=`Wins`+1 WHERE `Username`='" & RedPlayer.Username & "';"

                dbcomm = New MySqlCommand(sql, DBConn)
                dbread = dbcomm.ExecuteReader()
                dbread.Close()

                'Sets the path to where the log shall be generated, and the filename
                writer = My.Computer.FileSystem.OpenTextFileWriter(RedPlayer.Username & ".txt", True)

                'Writes the relevant data to the log
                writer.WriteLine(RedPlayer.Fname & " " & RedPlayer.lname & " beat " & YellowPlayer.Fname & " " & YellowPlayer.lname & " in Connect Four - " & TimeOfDay & " " & DateValue(Now))
                'Saves the log file
                writer.Close()
            ElseIf YellowPlayer.Username = LoggedInStudent.Username Then
                'Builds SQL query to execute
                sql = "UPDATE  `tblstudents` SET  `Losses`=`Losses`+1 WHERE `Username`='" & YellowPlayer.Username & "';"

                dbcomm = New MySqlCommand(sql, DBConn)
                dbread = dbcomm.ExecuteReader()
                dbread.Close()

                'Sets the path to where the log shall be generated, and the filename
                writer = My.Computer.FileSystem.OpenTextFileWriter(YellowPlayer.Username & ".txt", True)

                'Writes the relevant data to the log
                writer.WriteLine(YellowPlayer.Fname & " " & YellowPlayer.lname & " was beaten by " & RedPlayer.Fname & " " & RedPlayer.lname & " in Connect Four - " & TimeOfDay & " " & DateValue(Now))
                'Saves the log file
                writer.Close()
            End If

            'Resets the red score back to its default
            RedScore.ScoreNum = 0
        End If
        'However, if the yellow player is the winner...
        If YellowScore.ScoreNum = 1 Then
            If YellowPlayer.Username = LoggedInStudent.Username Then
                'Builds SQL query to execute
                sql = "UPDATE  `tblstudents` SET  `Wins`=`Wins`+1 WHERE `Username`='" & YellowPlayer.Username & "';"

                dbcomm = New MySqlCommand(sql, DBConn)
                dbread = dbcomm.ExecuteReader()
                dbread.Close()

                'Sets the path to where the log shall be generated, and the filename
                writer = My.Computer.FileSystem.OpenTextFileWriter(YellowPlayer.Username & ".txt", True)

                'Writes the relevant data to the log
                writer.WriteLine(YellowPlayer.Fname & " " & YellowPlayer.lname & " beat " & RedPlayer.Fname & " " & RedPlayer.lname & " in Connect Four - " & TimeOfDay & " " & DateValue(Now))
                'Saves the log file
                writer.Close()
            ElseIf RedPlayer.Username = LoggedInStudent.Username Then
                'Builds SQL query to execute
                sql = "UPDATE  `tblstudents` SET  `Losses`=`Losses`+1 WHERE `Username`='" & RedPlayer.Username & "';"

                dbcomm = New MySqlCommand(sql, DBConn)
                dbread = dbcomm.ExecuteReader()
                dbread.Close()

                'Sets the path to where the log shall be generated, and the filename
                writer = My.Computer.FileSystem.OpenTextFileWriter(RedPlayer.Username & ".txt", True)

                'Writes the relevant data to the log
                writer.WriteLine(RedPlayer.Fname & " " & RedPlayer.lname & " was beaten by " & YellowPlayer.Fname & " " & YellowPlayer.lname & " in Connect Four - " & TimeOfDay & " " & DateValue(Now))
                'Saves the log file
                writer.Close()
            End If
            'Resets the red score back to its default
            YellowScore.ScoreNum = 0
        End If
        'However, if the game is a draw...
        If Draw = True Then
            If RedPlayer.Username = LoggedInStudent.Username Then
                'Builds SQL query to execute
                sql = "UPDATE  `tblstudents` SET  `Draws`=`Draws`+1 WHERE `Username`='" & RedPlayer.Username & "';"

                dbcomm = New MySqlCommand(sql, DBConn)
                dbread = dbcomm.ExecuteReader()
                dbread.Close()

                'Sets the path to where the log shall be generated, and the filename
                writer = My.Computer.FileSystem.OpenTextFileWriter(RedPlayer.Username & ".txt", True)

                'Writes the relevant data to the log
                writer.WriteLine(RedPlayer.Fname & " " & RedPlayer.lname & " drew with " & YellowPlayer.Fname & " " & YellowPlayer.lname & " in Connect Four - " & TimeOfDay & " " & DateValue(Now))
                'Saves the log file
                writer.Close()
            ElseIf YellowPlayer.Username = LoggedInStudent.Username Then
                'Builds SQL query to execute
                sql = "UPDATE  `tblstudents` SET  `Draws`=`Draws`+1 WHERE `Username`='" & YellowPlayer.Username & "';"

                dbcomm = New MySqlCommand(sql, DBConn)
                dbread = dbcomm.ExecuteReader()
                dbread.Close()

                'Sets the path to where the log shall be generated, and the filename
                writer = My.Computer.FileSystem.OpenTextFileWriter(YellowPlayer.Username & ".txt", True)

                'Writes the relevant data to the log
                writer.WriteLine(YellowPlayer.Fname & " " & YellowPlayer.lname & " drew with " & RedPlayer.Fname & " " & RedPlayer.lname & " in Connect Four - " & TimeOfDay & " " & DateValue(Now))
                'Saves the log file
                writer.Close()
            End If
        End If
    End Sub

    'Subroutine runs when called in the ChangePlayer subroutine
    Sub CurrPlayer()
        'If the current player is red...
        If C4Player = "Red" Then
            'Places the current player label pointing to the red player
            lblCurrPlayer.Top = StudentCurrLocationY
            'However if the current player is yellow...
        Else
            'Places the current player label pointing to the yellow player
            lblCurrPlayer.Top = OppStudentCurrLocationY
        End If
    End Sub

    'Subroutine runs when called in form load subroutine
    Sub AccountSection()
        'Populates the player name labels with data and get a picture of each player
        lblStudentName.Text = RedPlayer.Fname & " " & RedPlayer.lname
        picStudent.ImageLocation = RedPlayer.Username & ".jpg"

        lblOppStudentName.Text = YellowPlayer.Fname & " " & YellowPlayer.lname
        picOppStudentPic.ImageLocation = YellowPlayer.Username & ".jpg"
    End Sub

    'Subroutine runs when called in form load subroutine
    Sub MakeGrid()
        'Ties the Buttons array with the button controls for dropping counters
        Buttons(1) = btn1
        Buttons(2) = btn2
        Buttons(3) = btn3
        Buttons(4) = btn4
        Buttons(5) = btn5
        Buttons(6) = btn6
        Buttons(7) = btn7

        'Ties the Group coordinates with the pictureboxes on the form
        Group(1, 1) = pb1dash1
        Group(1, 2) = pb1dash2
        Group(1, 3) = pb1dash3
        Group(1, 4) = pb1dash4
        Group(1, 5) = pb1dash5
        Group(1, 6) = pb1dash6

        Group(2, 1) = pb2dash1
        Group(2, 2) = pb2dash2
        Group(2, 3) = pb2dash3
        Group(2, 4) = pb2dash4
        Group(2, 5) = pb2dash5
        Group(2, 6) = pb2dash6

        Group(3, 1) = pb3dash1
        Group(3, 2) = pb3dash2
        Group(3, 3) = pb3dash3
        Group(3, 4) = pb3dash4
        Group(3, 5) = pb3dash5
        Group(3, 6) = pb3dash6

        Group(4, 1) = pb4dash1
        Group(4, 2) = pb4dash2
        Group(4, 3) = pb4dash3
        Group(4, 4) = pb4dash4
        Group(4, 5) = pb4dash5
        Group(4, 6) = pb4dash6

        Group(5, 1) = pb5dash1
        Group(5, 2) = pb5dash2
        Group(5, 3) = pb5dash3
        Group(5, 4) = pb5dash4
        Group(5, 5) = pb5dash5
        Group(5, 6) = pb5dash6

        Group(6, 1) = pb6dash1
        Group(6, 2) = pb6dash2
        Group(6, 3) = pb6dash3
        Group(6, 4) = pb6dash4
        Group(6, 5) = pb6dash5
        Group(6, 6) = pb6dash6

        Group(7, 1) = pb7dash1
        Group(7, 2) = pb7dash2
        Group(7, 3) = pb7dash3
        Group(7, 4) = pb7dash4
        Group(7, 5) = pb7dash5
        Group(7, 6) = pb7dash6
    End Sub

    'Subroutine runs when called in MakeGrid subroutine
    Sub NetworkCheckGrid()
        Dim sql As String
        Dim dbcomm As MySqlCommand
        Dim dbread As MySqlDataReader

        'Builds SQL query to execute
        sql = "SELECT * FROM `tblconnect4` WHERE `GameID`=" & GameID & ";"

        dbcomm = New MySqlCommand(sql, DBConn)
        dbread = dbcomm.ExecuteReader()

        While dbread.Read
            'For each column...
            For x = 1 To 7
                'For each square in the current column...
                For y = 1 To 6
                    'If the current square is taken by the red player...
                    If dbread(x & "," & y) = 1 Then
                        'Update the graphics to reflect this
                        theGrid(x, y) = 1
                        Group(x, y).Image = My.Resources.red
                    End If
                    'If the current square is taken by the yellow player...
                    If dbread(x & "," & y) = 2 Then
                        'Update the graphics to reflect this
                        theGrid(x, y) = 2
                        Group(x, y).Image = My.Resources.yellow
                    End If
                Next
            Next

            'For each column...
            For x = 1 To 7
                'If any buttons are disabled...
                If dbread("btn" & x) = 0 Then
                    'Disabled them in the form
                    Buttons(x).Enabled = False
                End If
            Next
        End While
        dbread.Close()

        'Resets x and y to their default values
        x = 1
        y = 1
    End Sub

    'Subroutine runs when called in form load subroutine
    Sub Terminators()
        'Sets the extra seventh horizontal line of squares in theGrid to terminators
        While y <> 8
            theGrid(y, 7) = 3
            y = y + 1
        End While

        'Sets x back to 1 for use later
        y = 1
    End Sub

    'Subroutine runs when called in button click subroutines
    Sub CounterPlace()

        Dim sql As String
        Dim dbcomm As MySqlCommand
        Dim dbread As MySqlDataReader

        'Runs down the column to find the next blank space
        While theGrid(x, y) = 0
            y = y + 1
        End While

        'Changes the next blank space into the current player's marker and claims the square for them
        If theGrid(x, 2) = 0 Then
            y = y - 1
            If C4Player = "Red" Then
                theGrid(x, y) = 1
                sql = "UPDATE  `tblconnect4` SET  `" & x & "," & y & "`='1' WHERE  `GameID`=" & GameID & ";"
                dbcomm = New MySqlCommand(sql, DBConn)
                dbread = dbcomm.ExecuteReader()
                dbread.Close()
                Group(x, y).Image = My.Resources.red
            Else
                theGrid(x, y) = 2
                sql = "UPDATE  `tblconnect4` SET  `" & x & "," & y & "`='2' WHERE  `GameID`=" & GameID & ";"
                dbcomm = New MySqlCommand(sql, DBConn)
                dbread = dbcomm.ExecuteReader()
                dbread.Close()
                Group(x, y).Image = My.Resources.yellow
            End If
        Else
            'If the only square left in the current row is the topmost one, the button disables after being pressed to seal off the column
            y = y - 2
            If C4Player = "Red" Then
                theGrid(x, y) = 1
                sql = "UPDATE  `tblconnect4` SET  `" & x & "," & y & "`='1' WHERE  `GameID`=" & GameID & ";"
                dbcomm = New MySqlCommand(sql, DBConn)
                dbread = dbcomm.ExecuteReader()
                dbread.Close()
                Group(x, y).Image = My.Resources.red
            Else
                theGrid(x, y) = 2
                sql = "UPDATE  `tblconnect4` SET  `" & x & "," & y & "`='2' WHERE  `GameID`=" & GameID & ";"
                dbcomm = New MySqlCommand(sql, DBConn)
                dbread = dbcomm.ExecuteReader()
                dbread.Close()
                Group(x, y).Image = My.Resources.yellow
            End If
            Buttons(x).Enabled = False
            sql = "UPDATE  `tblconnect4` SET  `btn" & x & "`='0' WHERE  `GameID`=" & GameID & ";"
            dbcomm = New MySqlCommand(sql, DBConn)
            dbread = dbcomm.ExecuteReader()
            dbread.Close()
        End If

        'Runs the DetectWinner subroutine
        DetectWinner()

        'If the game is still on, runs the ChangePlayer subroutine
        If Won = False Then
            ChangePlayer()
        End If
    End Sub

    'Subroutine runs when called in CounterPlace subroutine
    Sub DetectWinner()
        'Declares the variable used to determine number of spaces to the left of the last-placed counter
        Dim SpacestoLeft As Integer
        'Declares the variable used  to determine number of spaces to the right of the last-placed counter
        Dim SpacestoRight As Integer
        'Declares the variable used  to determine number of spaces above the last-placed counter
        Dim SpacesAbove As Integer
        'Declares the variable used  to determine number of spaces below the last-placed counter
        Dim SpacesBelow As Integer

        Dim sql As String
        Dim dbcomm As MySqlCommand
        Dim dbread As MySqlDataReader

        sql = "SELECT * FROM `tblconnect4` WHERE `GameID`=" & GameID & ";"
        dbcomm = New MySqlCommand(sql, DBConn)
        dbread = dbcomm.ExecuteReader()

        While dbread.Read
            'If the game has been won by red...
            If dbread("Won") = 1 Then
                'Ends the game, Red is the winner
                tmrUpdate.Enabled = False
                MsgBox("Red wins")
                lblCurrPlayer.Visible = False
                btnQuit.Visible = True
                dbread.Close()
                Exit Sub
                'However if the game has been won by yellow...
            ElseIf dbread("Won") = 2 Then
                'Ends the game, Yellow is the winner
                tmrUpdate.Enabled = False
                MsgBox("Yellow wins")
                lblCurrPlayer.Visible = False
                btnQuit.Visible = True
                dbread.Close()
                Exit Sub
            End If
        End While
        dbread.Close()

        '///WIN CONDITIONS\\\

        '/HORIZONTAL\

        'Determines no. of spaces to left and right of last-placed coutner
        SpacestoLeft = x - 1
        SpacestoRight = 7 - x

        'Runs along the row of the last-placed counter to see if there are four red or four yellows in a row
        For HorizSquare As Integer = x - SpacestoLeft To x + SpacestoRight
            If theGrid(HorizSquare, y) = 1 Then
                YellowAddUp = 0
                RedAddUp = RedAddUp + 1
                'If there are four reds in a row horizontally...
                If RedAddUp = 4 Then
                    'Gives the red player a win, pops up a messagebox, makes changes to the database and sets up the game to be reset
                    Won = True
                    tmrUpdate.Enabled = False
                    RedScore.Increase()
                    MsgBox("Red wins")
                    DatabaseDetails()
                    lblCurrPlayer.Visible = False
                    btnQuit.Visible = True
                    sql = "UPDATE  `tblconnect4` SET  `Won`='1' WHERE  `GameID`=" & GameID & ";"
                    dbcomm = New MySqlCommand(sql, DBConn)
                    dbread = dbcomm.ExecuteReader()
                    dbread.Close()
                    Exit Sub
                End If
            End If
            If theGrid(HorizSquare, y) = 2 Then
                RedAddUp = 0
                YellowAddUp = YellowAddUp + 1
                'If there are four yellow in a row horizontally...
                If YellowAddUp = 4 Then
                    'Gives the yellow player a win, pops up a messagebox, makes changes to the database and sets up the game to be reset
                    Won = True
                    tmrUpdate.Enabled = False
                    YellowScore.Increase()
                    MsgBox("Yellow wins")
                    DatabaseDetails()
                    lblCurrPlayer.Visible = False
                    btnQuit.Visible = True
                    sql = "UPDATE  `tblconnect4` SET  `Won`='2' WHERE  `GameID`=" & GameID & ";"
                    dbcomm = New MySqlCommand(sql, DBConn)
                    dbread = dbcomm.ExecuteReader()
                    dbread.Close()
                    Exit Sub
                End If
            End If
            If theGrid(HorizSquare, y) = 0 Then
                RedAddUp = 0
                YellowAddUp = 0
            End If
        Next

        '/VERTICAL\

        'Determines no. of spaces above and below the last-placed coutner
        SpacesAbove = y - 1
        SpacesBelow = 7 - y

        'Runs down the column of the last-placed counter to see if there are four red or four yellows in a row
        For VertSquare As Integer = y - SpacesAbove To y + SpacesBelow
            If theGrid(x, VertSquare) = 1 Then
                YellowAddUp = 0
                RedAddUp = RedAddUp + 1
                'If there are four reds in a row vertically...
                If RedAddUp = 4 Then
                    'Gives the red player a win, pops up a messagebox, makes changes to the database and sets up the game to be reset
                    Won = True
                    tmrUpdate.Enabled = False
                    RedScore.Increase()
                    MsgBox("Red wins")
                    DatabaseDetails()
                    lblCurrPlayer.Visible = False
                    btnQuit.Visible = True
                    sql = "UPDATE  `tblconnect4` SET  `Won`='1' WHERE  `GameID`=" & GameID & ";"
                    dbcomm = New MySqlCommand(sql, DBConn)
                    dbread = dbcomm.ExecuteReader()
                    dbread.Close()
                    Exit Sub
                End If
            End If
            If theGrid(x, VertSquare) = 2 Then
                RedAddUp = 0
                YellowAddUp = YellowAddUp + 1
                'If there are four yellow in a row vertically...
                If YellowAddUp = 4 Then
                    'Gives the yellow player a win, pops up a messagebox, makes changes to the database and sets up the game to be reset
                    Won = True
                    tmrUpdate.Enabled = False
                    YellowScore.Increase()
                    MsgBox("Yellow wins")
                    DatabaseDetails()
                    lblCurrPlayer.Visible = False
                    btnQuit.Visible = True
                    sql = "UPDATE  `tblconnect4` SET  `Won`='2' WHERE  `GameID`=" & GameID & ";"
                    dbcomm = New MySqlCommand(sql, DBConn)
                    dbread = dbcomm.ExecuteReader()
                    dbread.Close()
                    Exit Sub
                End If
            End If
            If theGrid(x, VertSquare) = 0 Then
                RedAddUp = 0
                YellowAddUp = 0
            End If
        Next

        'If a diagonal win in possible with the position of the last-placed counter...
        If x < 5 Then

            '/DIAGONAL BOTTOM-UP\

            'Runs the Diag (Bottom-Up) Win Detection subroutine
            DiagBUWin()

        End If

        'If a diagonal win in possible with the position of the last-placed counter...
        If x > 3 Then

            '/DIAGONAL TOP-DOWN\

            'Runs the Diag (Top-Down) Win Detection subroutine
            DiagTDWin()

        End If

        'If there is no winner...
        If Won <> True Then
            'Declares the variable used to keep track of how many blank squares there are left
            Dim Blanks As Integer = 42
            'Goes through the grid row-by-row, column-by-column, decrementing the blanks value when a non-blank square in encountered
            For Why = 1 To 6
                For x = 1 To 7
                    If theGrid(x, Why) <> 0 Then
                        Blanks = Blanks - 1
                    End If
                Next
            Next
            'If there are no blanks left...
            If Blanks = 0 Then
                'Declares the game a draw, amends the database, sets the form up to reset
                Draw = True
                MsgBox("No-one wins, it's a draw")
                DatabaseDetails()
                lblCurrPlayer.Visible = False
                btnQuit.Visible = True
            End If
        End If
    End Sub

    'Subroutine runs when called in DetectWinner subroutine
    Sub DiagTDWin()
        'Declares the varables used to run through the diagonals
        Dim v, z, a, w As Integer
        Dim sql As String
        Dim dbcomm As MySqlCommand
        Dim dbread As MySqlDataReader

        'Sets the variables to their defaults
        x = 1
        v = x
        a = x
        z = x + 3
        y = 1

        'Runs through the diagonals to determine if there are four red or yellow counters in a row
        For Why = 1 To 3
            For x = 1 To 4
                w = x
                y = Why
                If theGrid(x, y) = 1 Then
                    RedAddUp = 1
                    For v = a To z
                        x = x + 1
                        y = y + 1
                        If x < 8 Then
                            If theGrid(x, y) = 1 Then
                                RedAddUp = RedAddUp + 1
                            End If
                            'If there are four reds in a row diagonally...
                            If RedAddUp = 4 Then
                                'Gives the red player a win, pops up a messagebox, makes changes to the database and sets up the game to be reset
                                Won = True
                                tmrUpdate.Enabled = False
                                RedScore.Increase()
                                MsgBox("Red wins")
                                DatabaseDetails()
                                lblCurrPlayer.Visible = False
                                btnQuit.Visible = True
                                Sql = "UPDATE  `tblconnect4` SET  `Won`='1' WHERE  `GameID`=" & GameID & ";"
                                dbcomm = New MySqlCommand(Sql, DBConn)
                                dbread = dbcomm.ExecuteReader()
                                dbread.Close()
                                Exit Sub
                            End If
                        End If
                    Next
                End If

                If theGrid(w, y) = 2 Then
                    YellowAddUp = 1
                    For v = a To z
                        w = w + 1
                        y = y + 1
                        If w < 8 Then
                            If theGrid(w, y) = 1 Then
                                YellowAddUp = YellowAddUp + 1
                            End If
                            'If there are four yellow in a row diagonally...
                            If YellowAddUp = 4 Then
                                'Gives the yellow player a win, pops up a messagebox, makes changes to the database and sets up the game to be reset
                                Won = True
                                tmrUpdate.Enabled = False
                                YellowScore.Increase()
                                MsgBox("Yellow wins")
                                DatabaseDetails()
                                lblCurrPlayer.Visible = False
                                btnQuit.Visible = True
                                Sql = "UPDATE  `tblconnect4` SET  `Won`='2' WHERE  `GameID`=" & GameID & ";"
                                dbcomm = New MySqlCommand(Sql, DBConn)
                                dbread = dbcomm.ExecuteReader()
                                dbread.Close()
                                Exit Sub
                            End If
                        End If
                    Next
                End If
            Next
        Next



        'Resets the values to their defaults
        RedAddUp = 0
        YellowAddUp = 0
    End Sub

    'Subroutine runs when called in DetectWinner subroutine
    Sub DiagBUWin()
        'Declares the varables used to run through the diagonals
        Dim v, z, a, w As Integer
        Dim sql As String
        Dim dbcomm As MySqlCommand
        Dim dbread As MySqlDataReader


        'Sets the variables to their defaults
        x = 1
        v = x
        a = x
        z = x + 3
        y = 6

        'Runs through the diagonals to determine if there are four red or yellow counters in a row
        For Why = 4 To 6
            For x = 1 To 4
                w = x
                y = Why
                If theGrid(x, y) = 1 Then
                    RedAddUp = 1
                    For v = a To z
                        x = x + 1
                        y = y - 1
                        If x < 8 Then
                            If theGrid(x, y) = 1 Then
                                RedAddUp = RedAddUp + 1
                            End If
                            'If there are four reds in a row diagonally...
                            If RedAddUp = 4 Then
                                'Gives the red player a win, pops up a messagebox, makes changes to the database and sets up the game to be reset
                                Won = True
                                tmrUpdate.Enabled = False
                                RedScore.Increase()
                                MsgBox("Red wins")
                                DatabaseDetails()
                                lblCurrPlayer.Visible = False
                                btnQuit.Visible = True
                                Sql = "UPDATE  `tblconnect4` SET  `Won`='1' WHERE  `GameID`=" & GameID & ";"
                                dbcomm = New MySqlCommand(Sql, DBConn)
                                dbread = dbcomm.ExecuteReader()
                                dbread.Close()
                                Exit Sub
                            End If
                        End If
                    Next
                End If

                If theGrid(w, y) = 2 Then
                    YellowAddUp = 1
                    For v = a To z
                        w = w + 1
                        y = y - 1
                        If w < 8 Then
                            If theGrid(w, y) = 1 Then
                                YellowAddUp = YellowAddUp + 1
                            End If
                            'If there are four yellows in a row diagonally...
                            If YellowAddUp = 4 Then
                                'Gives the yellow player a win, pops up a messagebox, makes changes to the database and sets up the game to be reset
                                Won = True
                                tmrUpdate.Enabled = False
                                YellowScore.Increase()
                                MsgBox("Yellow wins")
                                DatabaseDetails()
                                lblCurrPlayer.Visible = False
                                btnQuit.Visible = True
                                Sql = "UPDATE  `tblconnect4` SET  `Won`='2' WHERE  `GameID`=" & GameID & ";"
                                dbcomm = New MySqlCommand(Sql, DBConn)
                                dbread = dbcomm.ExecuteReader()
                                dbread.Close()
                                Exit Sub
                            End If
                        End If
                    Next
                End If
            Next
        Next

        'Resets the values to their defaults
        RedAddUp = 0
        YellowAddUp = 0
    End Sub

    'Subroutines run when their respective buttons are clicked
    Private Sub btn1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn1.Click
        'Sets x to the x-coord of the selected column
        x = 1
        'Runs the CounterPlace subroutine
        CounterPlace()
    End Sub
    Private Sub btn2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn2.Click
        x = 2
        CounterPlace()
    End Sub
    Private Sub btn3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn3.Click
        x = 3
        CounterPlace()
    End Sub
    Private Sub btn4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn4.Click
        x = 4
        CounterPlace()
    End Sub
    Private Sub btn5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn5.Click
        x = 5
        CounterPlace()
    End Sub
    Private Sub btn6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn6.Click
        x = 6
        CounterPlace()
    End Sub
    Private Sub btn7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn7.Click
        x = 7
        CounterPlace()
    End Sub

    'Subroutine runs when back button is clicked
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        'Shows the Connect Four menu
        frmConnect4Menu.Show()
        'Closes this form
        Me.Close()
    End Sub

    'Subroutine runs when called in ChangePlayer and btnReset_Click subroutines
    Sub Question()
        Dim sql As String
        Dim dbcomm As MySqlCommand
        Dim dbread As MySqlDataReader

        sql = "SELECT * FROM `tblconnect4` WHERE `GameID`=" & GameID & ";"
        dbcomm = New MySqlCommand(sql, DBConn)
        dbread = dbcomm.ExecuteReader()

        While dbread.Read
            'Gets the question parameters of the current game
            C4HSubject = dbread("C4NSubject")
            C4HDifficulty = dbread("C4NDifficulty")
            C4HTopic = dbread("C4NTopic")
        End While

        'Closes the recordset
        dbread.Close()

        sql = "SELECT * FROM `tblquestions` WHERE `SubjectID`='" & C4HSubject & "' AND `Difficulty`='" & C4HDifficulty & "' AND `Topic`='" & C4HTopic & "' ORDER BY RAND() LIMIT 1;"
        dbcomm = New MySqlCommand(sql, DBConn)
        dbread = dbcomm.ExecuteReader()

        While dbread.Read
            'Sets the QuestionID variable to that of the selected question
            QuestionID = dbread("QuestionID")

            'Displays the selected question
            lblQuestion.Text = dbread("Question")
        End While
        'Closes the recordset
        dbread.Close()
    End Sub

    'Subroutine runs when called in form load and CounterPlace subroutines
    Sub ChangePlayer()
        'Sets x & y back to 1 for use later
        x = 1
        y = 1

        'Changes the current player
        If C4Player = "Red" Then
            If LoggedInStudent.C4Player = "Yellow" Then
                YourGo = True
            Else
                YourGo = False
            End If

            C4Player = "Yellow"
            CurrPlayer()
        Else
            If LoggedInStudent.C4Player = "Red" Then
                YourGo = True
            Else
                YourGo = False
            End If

            C4Player = "Red"
            CurrPlayer()
        End If

        'If the logged-in player is up...
        If YourGo = True Then
            'Displays the question controls
            grpQuestion.Visible = True
            txtAnswer.Visible = True
            lblQuestion.Visible = True
            btnSubmit.Visible = True

            'Runs the Question subroutine
            Question()
        Else
            'Hides the question controls
            grpQuestion.Visible = False
            txtAnswer.Visible = False
            lblQuestion.Visible = False
            btnSubmit.Visible = False
        End If

        Dim sql As String
        Dim dbcomm As MySqlCommand
        Dim dbread As MySqlDataReader

        sql = "UPDATE  `tblconnect4` SET  `CurrentPlayer`='" & C4Player & "' WHERE  `GameID`=" & GameID & ";"
        dbcomm = New MySqlCommand(sql, DBConn)
        dbread = dbcomm.ExecuteReader()
        dbread.Close()

        'Hides the counter placing buttons
        btn1.Visible = False
        btn2.Visible = False
        btn3.Visible = False
        btn4.Visible = False
        btn5.Visible = False
        btn6.Visible = False
        btn7.Visible = False
    End Sub

    'Subroutine runs when called in the tmrUpdate_Tick subroutine
    Sub CheckPlayer()
        Dim sql As String
        Dim dbcomm As MySqlCommand
        Dim dbread As MySqlDataReader

        sql = "SELECT * FROM `tblconnect4` WHERE `GameID`=" & GameID & ";"
        dbcomm = New MySqlCommand(sql, DBConn)
        dbread = dbcomm.ExecuteReader()

        While dbread.Read
            'If the logged-in student and current player in the database are the same but the current player on the form isn't, changes the current player on the form
            If LoggedInStudent.C4Player = "Red" Then
                If dbread("CurrentPlayer") = "Red" Then
                    If C4Player = "Yellow" Then
                        dbread.Close()
                        ChangePlayer()
                        Exit Sub
                    End If
                End If
            ElseIf LoggedInStudent.C4Player = "Yellow" Then
                If dbread("CurrentPlayer") = "Yellow" Then
                    If C4Player = "Red" Then
                        dbread.Close()
                        ChangePlayer()
                        Exit Sub
                    End If
                End If
            End If
        End While
        dbread.Close()
    End Sub

    'Subroutine runs then answer submit button is clicked
    Private Sub btnSubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        Dim sql As String
        Dim dbcomm As MySqlCommand
        Dim dbread As MySqlDataReader

        'Hides the question controls
        grpQuestion.Visible = False
        lblQuestion.Visible = False
        txtAnswer.Visible = False
        btnSubmit.Visible = False

        sql = "SELECT * FROM `tblquestions` WHERE `QuestionID`='" & QuestionID & "';"
        dbcomm = New MySqlCommand(sql, DBConn)
        dbread = dbcomm.ExecuteReader()
        Dim z As Integer
        If dbread.Read Then
            'If the answer is correct...
            If txtAnswer.Text = dbread("Answer") Then
                dbread.Close()
                z = 1
                'Display a message box
                MsgBox("Correct!")

                'Sets the question correct flag to true
                QCorrect = True

                'Makes the counter placement buttons visible
                btn1.Visible = True
                btn2.Visible = True
                btn3.Visible = True
                btn4.Visible = True
                btn5.Visible = True
                btn6.Visible = True
                btn7.Visible = True

                'Runs the QDatabase subroutine
                QDatabase()
                'However if the answer if incorrect...
            Else
                dbread.Close()
                z = 0
                'Display a message box
                MsgBox("Incorrect!")

                'Sets the question correct flag to false
                QCorrect = False

                'Runs the QDatabase subroutine
                QDatabase()
                'Runs the ChangePlayer subroutine
                ChangePlayer()
            End If
        End If

        'Blanks the answer textbox for the next question
        txtAnswer.Text = ""
    End Sub

    'Subroutine runs when called in btnSubmit_Click subroutine
    Sub QDatabase()
        Dim sql As String
        Dim dbcomm As MySqlCommand
        Dim dbread As MySqlDataReader

        If C4Player = "Red" Then
            If LoggedInStudent.C4Player = "Red" Then
                sql = "INSERT INTO `cl51-ben`.`tblattempted` (`SubjectID`, `QuestionID`, `StudentID`, `When`, `Correct`) VALUES ('" & C4HSubject & "', '" & QuestionID & "', '" & RedPlayer.StudentID & "', '" & TimeOfDay & " " & DateValue(Now) & "', "
            Else
                sql = "INSERT INTO `cl51-ben`.`tblattempted` (`SubjectID`, `QuestionID`, `StudentID`, `When`, `Correct`) VALUES ('" & C4HSubject & "', '" & QuestionID & "', '" & YellowPlayer.StudentID & "', '" & TimeOfDay & " " & DateValue(Now) & "', "
            End If
        Else
            If LoggedInStudent.C4Player = "Yellow" Then
                sql = "INSERT INTO `cl51-ben`.`tblattempted` (`SubjectID`, `QuestionID`, `StudentID`, `When`, `Correct`) VALUES ('" & C4HSubject & "', '" & QuestionID & "', '" & LoggedInStudent.StudentID & "', '" & TimeOfDay & " " & DateValue(Now) & "', "
            Else
                sql = "INSERT INTO `cl51-ben`.`tblattempted` (`SubjectID`, `QuestionID`, `StudentID`, `When`, `Correct`) VALUES ('" & C4HSubject & "', '" & QuestionID & "', '" & OppStudent.StudentID & "', '" & TimeOfDay & " " & DateValue(Now) & "', "
            End If
        End If
        If QCorrect = True Then
            sql = sql & "'1');"
        Else
            sql = sql & "'0');"
        End If

        dbcomm = New MySqlCommand(sql, DBConn)
        dbread = dbcomm.ExecuteReader()

        'Resets the question correct flag
        QCorrect = False

        dbread.Close()
    End Sub

    'Subroutine runs then the quit button is clicked
    Private Sub btnQuit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnQuit.Click
        Dim sql As String
        Dim dbcomm As MySqlCommand
        Dim dbread As MySqlDataReader

        sql = "DELETE  FROM `tblconnect4` WHERE `GameID`=" & GameID & ";"
        dbcomm = New MySqlCommand(sql, DBConn)
        dbread = dbcomm.ExecuteReader()
        dbread.Close()

        frmConnect4NetworkLobby.Show()
        Me.Close()
    End Sub

    'Subroutine runs every time the update timer ticks
    Private Sub tmrUpdate_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrUpdate.Tick
        'Runs the NetworkCheckGrid subroutine
        NetworkCheckGrid()
        'Runs the DetectWinner subroutine
        DetectWinner()
        'Runs the CheckPlayer subroutine
        CheckPlayer()
    End Sub

    'Subroutine runs when view logged-in student's profile button is clicked
    Private Sub btnViewProfile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnViewProfile.Click
        'If the logged-in student is the red player...
        If LoggedInStudent.C4Player = "Red" Then
            'Sets viewed profile to logged-in student's
            Viewing = 1
            'Shows the student account form
            frmStudentAccount.Show()
        Else
            'Sets viewed profile to opponent student's
            Viewing = 2
            'Shows the student account form
            frmStudentAccount.Show()
        End If
    End Sub

    'Subroutine runs when view opponent student's profile button is clicked
    Private Sub btnOppViewProfile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOppViewProfile.Click
        'If the logged-in student is the red player...
        If LoggedInStudent.C4Player = "Red" Then
            'Sets viewed profile to opponent student's
            Viewing = 2
            'Shows the student account form
            frmStudentAccount.Show()
        Else
            'Sets viewed profile to logged-in student's
            Viewing = 1
            'Shows the student account form
            frmStudentAccount.Show()
        End If
    End Sub

End Class