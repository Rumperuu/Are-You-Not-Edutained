Imports MySql.Data.MySqlClient

Public Class frmRPSHotseat

    'Declares the variable used to store the primary key of the question record in the database
    Dim QuestionID As Integer
    'Declares the y-coords for moving the current player label
    Dim StudentCurrLocationY As Integer = 35
    Dim OppStudentCurrLocationY As Integer = 140
    'Declares the variable used to determine if a question was answered correctly
    Dim QCorrect As Boolean = False
    'Declares the variable used to determine it both players have chosen their weapons
    Dim goes As Integer = 0

    'Subroutine runs on form load
    Private Sub frmRPSHotseat_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Runs the OpenDB subroutine
        OpenDB()

        'Sets the players
        LoggedInStudent.RPSPlayer = 1
        OppStudent.RPSPlayer = 2

        'Runs the AccountSection subroutine
        AccountSection()

        'Sets the current player
        RPSPlayer = 1

        'Runs the Question subroutine
        Question()
    End Sub

    'Subroutine runs when called in form load subroutine
    Sub AccountSection()
        'Populates the player name labels with data and get a picture of each player
        lblStudentName.Text = LoggedInStudent.Fname & " " & LoggedInStudent.Lname
        picStudent.ImageLocation = LoggedInStudent.Username & ".jpg"

        lblOppStudentName.Text = OppStudent.Fname & " " & OppStudent.Lname
        picOppStudentPic.ImageLocation = OppStudent.Username & ".jpg"
    End Sub

    'Subroutine runs when called in form load and button click subroutines
    Sub ChangePlayer()
        'Increments the number of goes there have been
        goes = goes + 1
        'If both players haven't been
        If goes < 2 Then
            'Changes the current player
            If RPSPlayer = 1 Then
                RPSPlayer = 2
                CurrPlayer()
            Else
                RPSPlayer = 1
                CurrPlayer()
            End If

            'Runs the Question subroutine
            Question()
        Else
            'Shows the Rock, Paper, Scissors fight animation form
            frmRPSFight.Show()
        End If
    End Sub

    'Subroutine runs when called in the ChangePlayer subroutine
    Sub CurrPlayer()
        If RPSPlayer = 1 Then
            'Places the current player label pointing to the logged-in student
            lblCurrPlayer.Top = StudentCurrLocationY
        Else
            'Places the current player label pointing to the opponent student
            lblCurrPlayer.Top = OppStudentCurrLocationY
        End If
    End Sub

    'Subroutine runs when called in ChangePlayer subroutine
    Sub Question()
        Dim sql As String
        Dim dbcomm As MySqlCommand
        Dim dbread As MySqlDataReader

        sql = "SELECT * FROM tblquestions WHERE SubjectID='" & RPSSubject & "' AND Difficulty='" & RPSDifficulty & "' AND Topic='" & RPSTopic & "' ORDER BY RAND() LIMIT 1;"
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

                'Makes the weapon selection controls visible
                grpWeapon.Visible = True


                'However if the answer if incorrect...
            Else
                'Display a message box
                MsgBox("Incorrect!")

                'Sets the question correct flag to false
                QCorrect = False

                foob = 1

                'Sets the current player's weapon to the rubber chicken of shame
                If RPSPlayer = 1 Then
                    LoggedInWeapon = 0
                Else
                    OppWeapon = 0
                End If

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

        If RPSPlayer = 1 Then
            sql = "INSERT INTO `cl51-ben`.`tblattempted` (`SubjectID`, `QuestionID`, `StudentID`, `When`, `Correct`) VALUES ('" & RPSSubject & "', '" & QuestionID & "', '" & LoggedInStudent.StudentID & "', '" & TimeOfDay & " " & DateValue(Now) & "', "
        Else
            sql = "INSERT INTO `cl51-ben`.`tblattempted` (`SubjectID`, `QuestionID`, `StudentID`, `When`, `Correct`) VALUES ('" & RPSSubject & "', '" & QuestionID & "', '" & OppStudent.StudentID & "', '" & TimeOfDay & " " & DateValue(Now) & "', "
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

    'Subroutine runs when back button is clicked
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        'Shows the Rock, Paper, Scissors menu
        frmRockPaperScissorsMenu.Show()
        'Closes this form
        Me.Close()
    End Sub

    'Subroutine runs when the rock button is clicked
    Private Sub btnRock_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRock.Click
        'Sets the current player's weapon to rock
        If RPSPlayer = 1 Then
            LoggedInWeapon = 1
        Else
            OppWeapon = 1
        End If

        'Hides the weapon selection controls
        grpWeapon.Visible = False
        'Runs the ChangePlayer subroutine
        ChangePlayer()
    End Sub

    'Subroutine runs when the paper button is clicked
    Private Sub btnPaper_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPaper.Click
        'Sets the current player's weapon to paper
        If RPSPlayer = 1 Then
            LoggedInWeapon = 2
        Else
            OppWeapon = 2
        End If

        'Hides the weapon selection controls
        grpWeapon.Visible = False
        'Runs the ChangePlayer subroutine
        ChangePlayer()
    End Sub

    'Subroutine runs when the scissors button is clicked
    Private Sub btnScissors_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnScissors.Click
        'Sets the current player's weapon to scissors
        If RPSPlayer = 1 Then
            LoggedInWeapon = 3
        Else
            OppWeapon = 3
        End If

        'Hides the weapon selection controls
        grpWeapon.Visible = False
        'Runs the ChangePlayer subroutine
        ChangePlayer()
    End Sub

    'Subroutine runs when called in frmRPSFight
    Sub Reset()
        'Sets both players' weapons to default and resets the current player
        LoggedInWeapon = 0
        goes = 0
        RPSPlayer = 1
        OppWeapon = 0

        'Runs the Question subroutine
        Question()
    End Sub

End Class