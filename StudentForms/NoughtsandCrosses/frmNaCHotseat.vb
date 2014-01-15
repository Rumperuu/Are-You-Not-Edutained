Imports MySql.Data.MySqlClient

Public Class frmNaCHotseat

    'Declares the array containing the coordinates of the squares of the grid and what condition they are currently in
    Dim theGrid(9) As String

    'Declares the class used for both players
    Public Class NaCPlay
        'Declares the variables used for storing the details of the Noughts and Crosses player
        Public Letter, Username, Fname, lname As String
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
    'Creates two objects of the score class, one for each player
    Dim XScore As New Score
    Dim OScore As New Score
    'Creates two objects of the C4Play class, one for each player
    Dim XPlayer As New NaCPlay
    Dim OPlayer As New NaCPlay

    'Declares the variable used to store the primary key of the question record in the database
    Dim QuestionID As Integer
    'Declares the y-coords for moving the current player label
    Dim StudentCurrLocationY As Integer = 35
    Dim OppStudentCurrLocationY As Integer = 140
    'Declares the variable used to determine if a question was answered correctly
    Dim QCorrect As Boolean = False

    'Subroutine runs on form load
    Private Sub frmNaCHotseat_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Runs the OpenDB subroutine
        OpenDB()

        'Hides the reset button
        btnReset.Visible = False

        'If the logged-in student is X...
        If LoggedInStudent.NaCPlayer = "X" Then
            'Populate the XPlayer object with the logged-in student's details
            With XPlayer
                .Letter = "X"
                .Username = LoggedInStudent.Username
                .Fname = LoggedInStudent.Fname
                .lname = LoggedInStudent.Lname
            End With

            'Populate the OPlayer object with the opponent student's details
            With OPlayer
                .Letter = "O"
                .Username = OppStudent.Username
                .Fname = OppStudent.Fname
                .lname = OppStudent.Lname
            End With
            'But if the logged-in student is yellow...
        Else
            'Populate the XPlayer object with the logged-in student's details
            With XPlayer
                .Letter = "X"
                .Username = OppStudent.Username
                .Fname = OppStudent.Fname
                .lname = OppStudent.Lname
            End With

            'Populate the OPlayer object with the opponent student's details
            With OPlayer
                .Letter = "O"
                .Username = LoggedInStudent.Username
                .Fname = LoggedInStudent.Fname
                .lname = LoggedInStudent.Lname
            End With
        End If

        'Runs the AccountSection subroutine
        AccountSection()

        'Sets the scores to the defaults
        XScore.ScoreNum = 0
        OScore.ScoreNum = 0

        'Sets the current player, runs the ChangePlayer and CurrPlayer subroutines
        NaCPlayer = "O"
        ChangePlayer()
        CurrPlayer()
    End Sub

    'Subroutines run when their respective buttons are clicked
    Private Sub btntopleft_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btntopleft.Click
        'Sets the chosen square to the current player
        btntopleft.Text = NaCPlayer
        theGrid(1) = NaCPlayer

        'Runs the ChangePlayer subroutine
        ChangePlayer()
        'Disables the button
        btntopleft.Enabled = False
        'Runs the CheckForAWinner subroutine
        CheckForAWinner()
    End Sub
    Private Sub btntop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btntop.Click
        btntop.Text = NaCPlayer
        theGrid(2) = NaCPlayer

        ChangePlayer()
        btntop.Enabled = False
        CheckForAWinner()
    End Sub
    Private Sub btntopright_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btntopright.Click
        btntopright.Text = NaCPlayer
        theGrid(3) = NaCPlayer

        ChangePlayer()
        btntopright.Enabled = False
        CheckForAWinner()
    End Sub
    Private Sub btnleft_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnleft.Click
        btnleft.Text = NaCPlayer
        theGrid(4) = NaCPlayer

        ChangePlayer()
        btnleft.Enabled = False
        CheckForAWinner()
    End Sub
    Private Sub btnmiddle_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnmiddle.Click
        btnmiddle.Text = NaCPlayer
        theGrid(5) = NaCPlayer

        ChangePlayer()
        btnmiddle.Enabled = False
        CheckForAWinner()
    End Sub
    Private Sub btnright_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnright.Click
        btnright.Text = NaCPlayer
        theGrid(6) = NaCPlayer

        ChangePlayer()
        btnright.Enabled = False
        CheckForAWinner()
    End Sub
    Private Sub btnbottomleft_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnbottomleft.Click
        btnbottomleft.Text = NaCPlayer
        theGrid(7) = NaCPlayer

        ChangePlayer()
        btnbottomleft.Enabled = False
        CheckForAWinner()
    End Sub
    Private Sub btnbottom_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnbottom.Click
        btnbottom.Text = NaCPlayer
        theGrid(8) = NaCPlayer

        ChangePlayer()
        btnbottom.Enabled = False
        CheckForAWinner()
    End Sub
    Private Sub btnbottomright_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnbottomright.Click
        btnbottomright.Text = NaCPlayer
        theGrid(9) = NaCPlayer

        ChangePlayer()
        btnbottomright.Enabled = False
        CheckForAWinner()
    End Sub

    'Subroutine runs when called in button click subroutines
    Private Sub CheckForAWinner()
        'Declares the variable used to store the winner
        Dim WhoWon As String = ""

        'Checks top row to see if there are three Xs or Os in a row
        If theGrid(1) = theGrid(2) And theGrid(2) = theGrid(3) And theGrid(1) <> "" Then
            'Declares the player who has claimed the three squares the winner
            WhoWon = theGrid(1)
            If WhoWon = "X" Then
                XScore.Increase()
            Else
                OScore.Increase()
            End If
            DatabaseDetails()

            'Disables grid
            btntopleft.Enabled = False
            btntop.Enabled = False
            btntopright.Enabled = False
            btnleft.Enabled = False
            btnmiddle.Enabled = False
            btnright.Enabled = False
            btnbottomleft.Enabled = False
            btnbottom.Enabled = False
            btnbottomright.Enabled = False
        End If

        'Checks middle row to see if there are three Xs or Os in a row
        If theGrid(4) = theGrid(5) And theGrid(5) = theGrid(6) And theGrid(4) <> "" Then
            'Declares the player who has claimed the three squares the winner
            WhoWon = theGrid(4)
            If WhoWon = "X" Then
                XScore.Increase()
            Else
                OScore.Increase()
            End If
            DatabaseDetails()

            'Disables grid
            btntopleft.Enabled = False
            btntop.Enabled = False
            btntopright.Enabled = False
            btnleft.Enabled = False
            btnmiddle.Enabled = False
            btnright.Enabled = False
            btnbottomleft.Enabled = False
            btnbottom.Enabled = False
            btnbottomright.Enabled = False
        End If

        'Checks bottom row to see if there are three Xs or Os in a row
        If theGrid(7) = theGrid(8) And theGrid(8) = theGrid(9) And theGrid(7) <> "" Then
            'Declares the player who has claimed the three squares the winner
            WhoWon = theGrid(7)
            If WhoWon = "X" Then
                XScore.Increase()
            Else
                OScore.Increase()
            End If
            DatabaseDetails()

            'Disables grid
            btntopleft.Enabled = False
            btntop.Enabled = False
            btntopright.Enabled = False
            btnleft.Enabled = False
            btnmiddle.Enabled = False
            btnright.Enabled = False
            btnbottomleft.Enabled = False
            btnbottom.Enabled = False
            btnbottomright.Enabled = False
        End If

        'Checks left column to see if there are three Xs or Os in a row
        If theGrid(1) = theGrid(4) And theGrid(4) = theGrid(7) And theGrid(1) <> "" Then
            'Declares the player who has claimed the three squares the winner
            WhoWon = theGrid(1)
            If WhoWon = "X" Then
                XScore.Increase()
            Else
                OScore.Increase()
            End If
            DatabaseDetails()

            'Disables grid
            btntopleft.Enabled = False
            btntop.Enabled = False
            btntopright.Enabled = False
            btnleft.Enabled = False
            btnmiddle.Enabled = False
            btnright.Enabled = False
            btnbottomleft.Enabled = False
            btnbottom.Enabled = False
            btnbottomright.Enabled = False
        End If

        'Checks middle column to see if there are three Xs or Os in a row
        If theGrid(2) = theGrid(5) And theGrid(5) = theGrid(8) And theGrid(2) <> "" Then
            'Declares the player who has claimed the three squares the winner
            WhoWon = theGrid(2)
            If WhoWon = "X" Then
                XScore.Increase()
            Else
                OScore.Increase()
            End If
            DatabaseDetails()

            'Disables grid
            btntopleft.Enabled = False
            btntop.Enabled = False
            btntopright.Enabled = False
            btnleft.Enabled = False
            btnmiddle.Enabled = False
            btnright.Enabled = False
            btnbottomleft.Enabled = False
            btnbottom.Enabled = False
            btnbottomright.Enabled = False
        End If

        'Checks right column to see if there are three Xs or Os in a row
        If theGrid(3) = theGrid(6) And theGrid(6) = theGrid(9) And theGrid(3) <> "" Then
            'Declares the player who has claimed the three squares the winner
            WhoWon = theGrid(3)
            If WhoWon = "X" Then
                XScore.Increase()
            Else
                OScore.Increase()
            End If
            DatabaseDetails()

            'Disables grid
            btntopleft.Enabled = False
            btntop.Enabled = False
            btntopright.Enabled = False
            btnleft.Enabled = False
            btnmiddle.Enabled = False
            btnright.Enabled = False
            btnbottomleft.Enabled = False
            btnbottom.Enabled = False
            btnbottomright.Enabled = False
        End If

        'Checks top-left to bottom-right diagonal to see if there are three Xs or Os in a row
        If theGrid(1) = theGrid(5) And theGrid(5) = theGrid(9) And theGrid(1) <> "" Then
            'Declares the player who has claimed the three squares the winner
            WhoWon = theGrid(1)
            If WhoWon = "X" Then
                XScore.Increase()
            Else
                OScore.Increase()
            End If
            DatabaseDetails()

            'Disables grid
            btntopleft.Enabled = False
            btntop.Enabled = False
            btntopright.Enabled = False
            btnleft.Enabled = False
            btnmiddle.Enabled = False
            btnright.Enabled = False
            btnbottomleft.Enabled = False
            btnbottom.Enabled = False
            btnbottomright.Enabled = False
        End If

        'Checks top-right to bottom-left diagonal to see if there are three Xs or Os in a row
        If theGrid(3) = theGrid(5) And theGrid(5) = theGrid(7) And theGrid(3) <> "" Then
            'Declares the player who has claimed the three squares the winner
            WhoWon = theGrid(3)
            If WhoWon = "X" Then
                XScore.Increase()
            Else
                OScore.Increase()
            End If
            DatabaseDetails()

            'Disables grid
            btntopleft.Enabled = False
            btntop.Enabled = False
            btntopright.Enabled = False
            btnleft.Enabled = False
            btnmiddle.Enabled = False
            btnright.Enabled = False
            btnbottomleft.Enabled = False
            btnbottom.Enabled = False
            btnbottomright.Enabled = False
        End If

        'Declares the variables used to count the number of blank squares
        Dim n As Integer
        Dim Blanks As Integer

        'For each square...
        For n = 1 To 9
            'If the square isn't blank, increment blanks
            If theGrid(n) = "" Then Blanks = Blanks + 1
        Next

        'If there is a winner...
        If WhoWon <> "" Then
            'Makes buttons invisible
            btnReset.Visible = True
            lblQuestion.Visible = False
            lblCurrPlayer.Visible = False
            grpQuestion.Visible = False
            txtAnswer.Visible = False
            btnSubmit.Visible = False
            'Declares winner
            MsgBox(WhoWon & " wins.")
            'Stops game
            Exit Sub
        End If

        'Declares the StreamWriter used to write to the game breakdown text files
        Dim writer As System.IO.StreamWriter
        Dim sql As String
        Dim dbcomm As MySqlCommand
        Dim dbread As MySqlDataReader

        'If there a draw...
        If Blanks = 0 Then

            If OPlayer.Username = LoggedInStudent.Username Then
                'Builds SQL query to execute
                sql = "UPDATE  `tblstudents` SET  `Draws`=`Draws`+1 WHERE `Username`='" & OPlayer.Username & "';"

                dbcomm = New MySqlCommand(sql, DBConn)
                dbread = dbcomm.ExecuteReader()
                dbread.Close()

                'Sets the path to where the log shall be generated, and the filename
                writer = My.Computer.FileSystem.OpenTextFileWriter(OPlayer.Username & ".txt", True)

                'Writes the relevant data to the log
                writer.WriteLine(OPlayer.Fname & " " & OPlayer.lname & " drew with " & XPlayer.Fname & " " & XPlayer.lname & " in Noughts and Crosses - " & TimeOfDay & " " & DateValue(Now))
                'Saves the log file
                writer.Close()
            ElseIf XPlayer.Username = LoggedInStudent.Username Then
                'Builds SQL query to execute
                sql = "UPDATE  `tblstudents` SET  `Draws`=`Draws`+1 WHERE `Username`='" & XPlayer.Username & "';"

                dbcomm = New MySqlCommand(sql, DBConn)
                dbread = dbcomm.ExecuteReader()
                dbread.Close()

                'Sets the path to where the log shall be generated, and the filename
                writer = My.Computer.FileSystem.OpenTextFileWriter(XPlayer.Username & ".txt", True)

                'Writes the relevant data to the log
                writer.WriteLine(XPlayer.Fname & " " & XPlayer.lname & " drew with " & OPlayer.Fname & " " & OPlayer.lname & " in Noughts and Crosses - " & TimeOfDay & " " & DateValue(Now))
                'Saves the log file
                writer.Close()
            End If

            'Declares draw
            MsgBox("Draw")
            'Makes buttons invisible
            btnReset.Visible = True
            grpQuestion.Visible = False
            lblQuestion.Visible = False
            txtAnswer.Visible = False
            btnSubmit.Visible = False
            lblCurrPlayer.Visible = False
        End If
    End Sub

    'Subroutine runs when called in winner detection subroutine
    Sub DatabaseDetails()
        'Declares the StreamWriter used to write to the game breakdown text files
        Dim writer As System.IO.StreamWriter
        Dim sql As String
        Dim dbcomm As MySqlCommand
        Dim dbread As MySqlDataReader

        'If the O player is the winner...
        If OScore.ScoreNum = 1 Then
            If OPlayer.Username = LoggedInStudent.Username Then
                'Builds SQL query to execute
                sql = "UPDATE  `tblstudents` SET  `Wins`=`Wins`+1 WHERE `Username`='" & OPlayer.Username & "';"

                dbcomm = New MySqlCommand(sql, DBConn)
                dbread = dbcomm.ExecuteReader()
                dbread.Close()

                'Sets the path to where the log shall be generated, and the filename
                writer = My.Computer.FileSystem.OpenTextFileWriter(OPlayer.Username & ".txt", True)

                'Writes the relevant data to the log
                writer.WriteLine(OPlayer.Fname & " " & OPlayer.lname & " beat " & XPlayer.Fname & " " & XPlayer.lname & " in Noughts and Crosses - " & TimeOfDay & " " & DateValue(Now))
                'Saves the log file
                writer.Close()
            ElseIf XPlayer.Username = LoggedInStudent.Username Then
                'Builds SQL query to execute
                sql = "UPDATE  `tblstudents` SET  `Losses`=`Losses`+1 WHERE `Username`='" & XPlayer.Username & "';"

                dbcomm = New MySqlCommand(sql, DBConn)
                dbread = dbcomm.ExecuteReader()
                dbread.Close()

                'Sets the path to where the log shall be generated, and the filename
                writer = My.Computer.FileSystem.OpenTextFileWriter(XPlayer.Username & ".txt", True)

                'Writes the relevant data to the log
                writer.WriteLine(XPlayer.Fname & " " & XPlayer.lname & " was beaten by " & OPlayer.Fname & " " & OPlayer.lname & " in Noughts and Crosses - " & TimeOfDay & " " & DateValue(Now))
                'Saves the log file
                writer.Close()
            End If

            'Resets the O score back to its default
            OScore.ScoreNum = 0
        End If
        'However, if the X player is the winner...
        If XScore.ScoreNum = 1 Then
            If XPlayer.Username = LoggedInStudent.Username Then
                'Builds SQL query to execute
                sql = "UPDATE  `tblstudents` SET  `Wins`=`Wins`+1 WHERE `Username`='" & XPlayer.Username & "';"

                dbcomm = New MySqlCommand(sql, DBConn)
                dbread = dbcomm.ExecuteReader()
                dbread.Close()

                'Sets the path to where the log shall be generated, and the filename
                writer = My.Computer.FileSystem.OpenTextFileWriter(XPlayer.Username & ".txt", True)

                'Writes the relevant data to the log
                writer.WriteLine(XPlayer.Fname & " " & XPlayer.lname & " beat " & OPlayer.Fname & " " & OPlayer.lname & " in Noughts and Crosses - " & TimeOfDay & " " & DateValue(Now))
                'Saves the log file
                writer.Close()
            ElseIf OPlayer.Username = LoggedInStudent.Username Then
                'Builds SQL query to execute
                sql = "UPDATE  `tblstudents` SET  `Losses`=`Losses`+1 WHERE `Username`='" & OPlayer.Username & "';"

                dbcomm = New MySqlCommand(sql, DBConn)
                dbread = dbcomm.ExecuteReader()
                dbread.Close()

                'Sets the path to where the log shall be generated, and the filename
                writer = My.Computer.FileSystem.OpenTextFileWriter(OPlayer.Username & ".txt", True)

                'Writes the relevant data to the log
                writer.WriteLine(OPlayer.Fname & " " & OPlayer.lname & " was beaten by " & XPlayer.Fname & " " & XPlayer.lname & " in Noughts and Crosses - " & TimeOfDay & " " & DateValue(Now))
                'Saves the log file
                writer.Close()
            End If
            'Resets the O score back to its default
            XScore.ScoreNum = 0
        End If

        grpQuestion.Visible = False

        btnReset.Location = New Point(331, 206)
        btnReset.Visible = True
    End Sub

    'Subroutine runs when called in the ChangePlayer subroutine
    Sub CurrPlayer()
        'If the logged-in player is the same as the current player, which is X...
        If LoggedInStudent.NaCPlayer = "X" Then
            If NaCPlayer = "X" Then
                'Places the current player label pointing to the logged-in student
                lblCurrPlayer.Top = StudentCurrLocationY
                'However if the current player is O...
            Else
                'Places the current player label pointing to the opponent student
                lblCurrPlayer.Top = OppStudentCurrLocationY
            End If
            'However, if the opponent student is the same as the current player, which is O...
        Else
            If NaCPlayer = "O" Then
                'Places the current player label pointing to the logged-in student
                lblCurrPlayer.Top = StudentCurrLocationY
                'However if the current player is yellow...
            Else
                'Places the current player label pointing to the opponent student
                lblCurrPlayer.Top = OppStudentCurrLocationY
            End If
        End If
    End Sub

    'Subroutine runs when called in form load subroutine
    Sub AccountSection()
        'Populates the player name labels with data and get a picture of each player
        lblStudentName.Text = LoggedInStudent.Fname & " " & LoggedInStudent.Lname
        picStudent.ImageLocation = OPlayer.Username & ".jpg"

        lblOppStudentName.Text = OppStudent.Fname & " " & OppStudent.Lname
        picOppStudentPic.ImageLocation = XPlayer.Username & ".jpg"
    End Sub

    'Subroutine runs when reset button is clicked
    Private Sub btnReset_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReset.Click
        'This re-enables all of the buttons, removes the Xs and 0s from them as well as resetting the array
        btntopleft.Enabled = True
        btntop.Enabled = True
        btntopright.Enabled = True
        btnleft.Enabled = True
        btnmiddle.Enabled = True
        btnright.Enabled = True
        btnbottomleft.Enabled = True
        btnbottom.Enabled = True
        btnbottomright.Enabled = True

        btntopleft.Text = ""
        btntop.Text = ""
        btntopright.Text = ""
        btnleft.Text = ""
        btnmiddle.Text = ""
        btnright.Text = ""
        btnbottomleft.Text = ""
        btnbottom.Text = ""
        btnbottomright.Text = ""

        Dim n As Integer

        For n = 1 To 9
            theGrid(n) = ""
        Next

        'Resets the current player
        NaCPlayer = "X"
        CurrPlayer()

        'Runs the Question subroutine
        Question()

        'Makes the controls visible
        lblCurrPlayer.Visible = True
        grpQuestion.Visible = True
        lblQuestion.Visible = True
        txtAnswer.Visible = True
        btnSubmit.Visible = True
        btnReset.Visible = False
        btnReset.Location = New Point(19, 90)
    End Sub

    'Subroutine runs when back button is clicked
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        'Shows the Noughts and Crosses menu
        frmNoughtsandCrossesMenu.Show()
        'Closes this form
        Me.Close()
    End Sub

    'Subroutine runs when called in ChangePlayer and btnReset_Click subroutines
    Sub Question()
        Dim sql As String
        Dim dbcomm As MySqlCommand
        Dim dbread As MySqlDataReader

        sql = "SELECT * FROM tblquestions WHERE SubjectID='" & NaCSubject & "' AND Difficulty='" & NaCDifficulty & "' AND Topic='" & NaCTopic & "' ORDER BY RAND() LIMIT 1;"
        dbcomm = New MySqlCommand(sql, DBConn)
        dbread = dbcomm.ExecuteReader()

        While dbread.Read
            'Sets the QuestionID variable to that of the selected question
            QuestionID = dbread("QuestionID")

            'Makes the question controls visible
            grpQuestion.Visible = True
            lblQuestion.Visible = True
            txtAnswer.Visible = True
            btnSubmit.Visible = True

            'Displays the selected question
            lblQuestion.Text = dbread("Question")
        End While
        'Closes the recordset
        dbread.Close()
    End Sub

    'Subroutine runs when called in form load and button click subroutines
    Sub ChangePlayer()
        'Changes the current player
        If NaCPlayer = "X" Then
            NaCPlayer = "O"
            CurrPlayer()
        Else
            NaCPlayer = "X"
            CurrPlayer()
        End If

        'Hides the buttons
        btntopleft.Enabled = False
        btntop.Enabled = False
        btntopright.Enabled = False
        btnleft.Enabled = False
        btnmiddle.Enabled = False
        btnright.Enabled = False
        btnbottomleft.Enabled = False
        btnbottom.Enabled = False
        btnbottomright.Enabled = False

        'Runs the Question subroutine
        Question()
    End Sub

    'Subroutine runs then answer submit button is clicked
    Private Sub btnSubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        Dim sql As String
        Dim dbcomm As MySqlCommand
        Dim dbread As MySqlDataReader
        Dim foob As Integer = 0
        'Hides the question controls
        grpQuestion.Visible = False
        lblQuestion.Visible = False
        txtAnswer.Visible = False
        btnSubmit.Visible = False

        sql = "SELECT * FROM tblquestions WHERE QuestionID='" & QuestionID & "';"
        dbcomm = New MySqlCommand(sql, DBConn)
        dbread = dbcomm.ExecuteReader()
        While dbread.Read
            'If the answer is correct...
            If txtAnswer.Text = dbread("Answer") Then
                'Display a message box
                MsgBox("Correct!")

                'Sets the question correct flag to true
                QCorrect = True

                'Makes the buttons visible
                btntopleft.Enabled = True
                btntop.Enabled = True
                btntopright.Enabled = True
                btnleft.Enabled = True
                btnmiddle.Enabled = True
                btnright.Enabled = True
                btnbottomleft.Enabled = True
                btnbottom.Enabled = True
                btnbottomright.Enabled = True

                'However if the answer if incorrect...
            Else
                'Display a message box
                MsgBox("Incorrect!")

                'Sets the question correct flag to false
                QCorrect = False

                foob = 1
            End If
        End While

        dbread.Close()

        'Runs the QDatabase subroutine
        QDatabase()

        If foob = 1 Then
            'Runs the ChangePlayer subroutine
            ChangePlayer()
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
            If OPlayer.Username = LoggedInStudent.Username Then
                sql = "INSERT INTO `cl51-ben`.`tblattempted` (`SubjectID`, `QuestionID`, `StudentID`, `When`, `Correct`) VALUES ('" & NaCSubject & "', '" & QuestionID & "', '" & LoggedInStudent.StudentID & "', '" & TimeOfDay & " " & DateValue(Now) & "', "
            Else
                sql = "INSERT INTO `cl51-ben`.`tblattempted` (`SubjectID`, `QuestionID`, `StudentID`, `When`, `Correct`) VALUES ('" & NaCSubject & "', '" & QuestionID & "', '" & OppStudent.StudentID & "', '" & TimeOfDay & " " & DateValue(Now) & "', "
            End If
        Else
            If XPlayer.Username = LoggedInStudent.Username Then
                sql = "INSERT INTO `cl51-ben`.`tblattempted` (`SubjectID`, `QuestionID`, `StudentID`, `When`, `Correct`) VALUES ('" & NaCSubject & "', '" & QuestionID & "', '" & LoggedInStudent.StudentID & "', '" & TimeOfDay & " " & DateValue(Now) & "', "
            Else
                sql = "INSERT INTO `cl51-ben`.`tblattempted` (`SubjectID`, `QuestionID`, `StudentID`, `When`, `Correct`) VALUES ('" & NaCSubject & "', '" & QuestionID & "', '" & OppStudent.StudentID & "', '" & TimeOfDay & " " & DateValue(Now) & "', "
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

    'Subroutine runs when view logged-in student's profile button is clicked
    Private Sub btnViewProfile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnViewProfile.Click
        'Sets viewed profile to logged-in student's
        Viewing = 1
        'Shows the student account form
        frmStudentAccount.Show()
    End Sub

    'Subroutine runs when view opponent student's profile button is clicked
    Private Sub btnOppViewProfile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOppViewProfile.Click
        'Sets viewed profile to opponent student's
        Viewing = 2
        'Shows the student account form
        frmStudentAccount.Show()
    End Sub
End Class