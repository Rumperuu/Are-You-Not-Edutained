Imports MySql.Data.MySqlClient

Public Class frmRPSFight

    'Declares the variables for the timers
    Dim count As Integer = 100
    Dim countdown As Integer = 3
    Dim time As Integer = 0

    'Declares the variables used for storing the winner
    Dim WinnerName As String
    Dim Winner As Integer

    'Subroutine runs on form load
    Private Sub frmRPSFight_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        'Runs the AccountSection subroutine
        AccountSection()

        'Runs the Weapons subroutine
        Weapons()

        'Enables the countdown timer
        tmrCountdown.Enabled = True
    End Sub

    'Subroutine runs when called in form load subroutine
    Sub Weapons()
        'Sets the images to the players' chosen weapons
        If LoggedInWeapon = 1 Then
            picLoggedIn.Image = My.Resources.bigrock
        ElseIf LoggedInWeapon = 2 Then
            picLoggedIn.Image = My.Resources.bigpaper
        ElseIf LoggedInWeapon = 3 Then
            picLoggedIn.Image = My.Resources.bigscissorsloggedin
        ElseIf LoggedInWeapon = 0 Then
            picLoggedIn.Image = My.Resources.bigchickenloggedin
        End If

        If OppWeapon = 1 Then
            picOpp.Image = My.Resources.bigrock
        ElseIf OppWeapon = 2 Then
            picOpp.Image = My.Resources.bigpaper
        ElseIf OppWeapon = 3 Then
            picOpp.Image = My.Resources.bigscissorsopp
        ElseIf OppWeapon = 0 Then
            picOpp.Image = My.Resources.bigchickenopp
        End If
    End Sub

    'Subroutine runs when called in form load subroutine
    Sub AccountSection()
        'Populates the player name labels with data
        lblLoggedInName.Text = LoggedInStudent.Fname & " " & LoggedInStudent.Lname
        lblOppName.Text = OppStudent.Fname & " " & OppStudent.Lname
    End Sub

    'Subroutine runs when countdown timer ticks
    Private Sub tmrCountdown_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrCountdown.Tick
        'Decrements count by 10
        count = count - 10

        'If count reaches 0...
        If count = 0 Then
            'Changes the number of the countdown displayed
            countdown = countdown - 1
            If countdown > 0 Then
                lblCountdown.Text = countdown
                count = 100
                'If the coundown reaches the end...
            Else
                'Disables the timer, displays the weapons
                picLoggedIn.Visible = True
                picOpp.Visible = True
                lblCountdown.Visible = False
                count = 1000
                tmrCountdown.Enabled = False
                tmrAnimate.Enabled = True

                'Plays 3 Inches of Blood - Deady Sinners
                My.Computer.Audio.Play(My.Resources.deadly, AudioPlayMode.Background)
            End If
        End If
    End Sub

    'Subroutine runs when animation timer ticks
    Private Sub tmrAnimate_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrAnimate.Tick
        'Declares the variables used for the coords of the pictures
        Dim x, y, xx, yy As Integer

        'Decrements count by 1000
        count = count - 1000

        'Runs the random number generator subroutine
        Randomize()

        'Gets the random coords of both pictureboxes
        x = CInt(Int((140) * Rnd() - 20))
        y = CInt(Int((50) * Rnd()) + 60)
        xx = CInt(Int((140) * Rnd()) + 390)
        yy = CInt(Int((50) * Rnd()) + 60)

        'Changes the position of both pictureboxes fast enough to resemble animation
        If count = 0 Then
            If time <> 40 Then
                count = 1000
                time = time + 1
                picLoggedIn.Location = New Point(x, y)
                picOpp.Location = New Point(xx, yy)
                'If the time has come for the animation to stop...
            Else
                'Stops playing the music
                My.Computer.Audio.Stop()
                'Runs the CheckWinner subroutine
                CheckWinner()
                'Disables the timer
                tmrAnimate.Enabled = False
                'Runs the DatabaseDetails subroutine
                DatabaseDetails()
                'Runs the ChangeImage subroutine
                ChangeImage()
                'Declares the winner
                MsgBox("Winner: " & WinnerName)
                'Shows the Rock, Paper, Scissors hotseat game form
                frmRPSHotseat.Reset()
                'Closes this form
                Me.Close()
            End If
        End If
    End Sub

    'Subroutine runs when called in tmrAnimate_Tick subroutine
    Sub ChangeImage()
        'Changes the images of the players to reflect the result of the fight
        If Winner = 1 Then
            If OppWeapon <> 0 Then
                If LoggedInWeapon = 1 Then
                    picLoggedIn.Image = My.Resources.bigrockwinner
                    picOpp.Image = My.Resources.bigscissorsdeadopp
                ElseIf LoggedInWeapon = 2 Then
                    picLoggedIn.Image = My.Resources.bigpaperwinner
                    picOpp.Image = My.Resources.bigrockdead
                ElseIf LoggedInWeapon = 3 Then
                    picLoggedIn.Image = My.Resources.bigscissorswinloggedin
                    picOpp.Image = My.Resources.bigpaperdead
                End If
            Else
                If LoggedInWeapon = 1 Then
                    picLoggedIn.Image = My.Resources.bigrockwinner
                    picOpp.Image = My.Resources.bigchickendeadopp
                ElseIf LoggedInWeapon = 2 Then
                    picLoggedIn.Image = My.Resources.bigpaperwinner
                    picOpp.Image = My.Resources.bigchickendeadopp
                ElseIf LoggedInWeapon = 3 Then
                    picLoggedIn.Image = My.Resources.bigscissorswinloggedin
                    picOpp.Image = My.Resources.bigchickendeadopp
                End If
            End If
        ElseIf Winner = 2 Then
            If LoggedInWeapon <> 0 Then
                If LoggedInWeapon = 1 Then
                    picLoggedIn.Image = My.Resources.bigrockdead
                    picOpp.Image = My.Resources.bigpaperwinner
                ElseIf LoggedInWeapon = 2 Then
                    picLoggedIn.Image = My.Resources.bigpaperdead
                    picOpp.Image = My.Resources.bigscissorswinopp
                ElseIf LoggedInWeapon = 3 Then
                    picLoggedIn.Image = My.Resources.bigscissorsdeadloggedin
                    picOpp.Image = My.Resources.bigrockwinner
                End If
            Else
                If LoggedInWeapon = 1 Then
                    picLoggedIn.Image = My.Resources.bigrockwinner
                    picOpp.Image = My.Resources.bigchickendeadopp
                ElseIf LoggedInWeapon = 2 Then
                    picLoggedIn.Image = My.Resources.bigpaperwinner
                    picOpp.Image = My.Resources.bigchickendeadopp
                ElseIf LoggedInWeapon = 3 Then
                    picLoggedIn.Image = My.Resources.bigscissorswinloggedin
                    picOpp.Image = My.Resources.bigchickendeadopp
                End If
            End If
        End If
    End Sub

    'Subroutine runs when called in tmrAnimate_Tick subroutine
    Sub CheckWinner()
        'Determines the winner of the game
        If (LoggedInWeapon = 2 And OppWeapon = 1) Or (LoggedInWeapon = 3 And OppWeapon = 2) Or (LoggedInWeapon = 1 And OppWeapon = 3) Or (LoggedInWeapon <> 0 And OppWeapon = 0) Then
            WinnerName = LoggedInStudent.Fname & " " & LoggedInStudent.Lname
            Winner = 1
        End If
        If (LoggedInWeapon = 1 And OppWeapon = 2) Or (LoggedInWeapon = 2 And OppWeapon = 3) Or (LoggedInWeapon = 3 And OppWeapon = 1) Or (LoggedInWeapon = 0 And OppWeapon <> 0) Then
            WinnerName = OppStudent.Fname & " " & OppStudent.Lname
            Winner = 2
        End If
        If LoggedInWeapon = OppWeapon Then
            WinnerName = "No-one, it's a draw"
            Winner = 0
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
        If Winner = 1 Then
            'Builds SQL query to execute
            sql = "UPDATE  `tblstudents` SET  `Wins`=`Wins`+1 WHERE `Username`='" & LoggedInStudent.Username & "';"

            dbcomm = New MySqlCommand(sql, DBConn)
            dbread = dbcomm.ExecuteReader()
            dbread.Close()

            'Sets the path to where the log shall be generated, and the filename
            writer = My.Computer.FileSystem.OpenTextFileWriter(LoggedInStudent.Username & ".txt", True)

            'Writes the relevant data to the log
            writer.WriteLine(LoggedInStudent.Fname & " " & LoggedInStudent.Lname & " beat " & OppStudent.Fname & " " & OppStudent.Lname & " in Rock, Paper, Scissors - " & TimeOfDay & " " & DateValue(Now))
            'Saves the log file
            writer.Close()

            'Builds SQL query to execute
            sql = "UPDATE  `tblstudents` SET  `Losses`=`Losses`+1 WHERE `Username`='" & OppStudent.Username & "';"

            dbcomm = New MySqlCommand(sql, DBConn)
            dbread = dbcomm.ExecuteReader()
            dbread.Close()

            'Sets the path to where the log shall be generated, and the filename
            writer = My.Computer.FileSystem.OpenTextFileWriter(OppStudent.Username & ".txt", True)

            'Writes the relevant data to the log
            writer.WriteLine(OppStudent.Fname & " " & OppStudent.Lname & " was beaten by " & LoggedInStudent.Fname & " " & OppStudent.Lname & " in Rock, Paper, Scissors - " & TimeOfDay & " " & DateValue(Now))
            'Saves the log file
            writer.Close()
        ElseIf Winner = 2 Then
            'Builds SQL query to execute
            sql = "UPDATE  `tblstudents` SET  `Wins`=`Wins`+1 WHERE `Username`='" & OppStudent.Username & "';"

            dbcomm = New MySqlCommand(sql, DBConn)
            dbread = dbcomm.ExecuteReader()
            dbread.Close()

            'Sets the path to where the log shall be generated, and the filename
            writer = My.Computer.FileSystem.OpenTextFileWriter(OppStudent.Username & ".txt", True)

            'Writes the relevant data to the log
            writer.WriteLine(OppStudent.Fname & " " & OppStudent.Lname & " beat " & LoggedInStudent.Fname & " " & LoggedInStudent.Lname & " in Rock, Paper, Scissors - " & TimeOfDay & " " & DateValue(Now))
            'Saves the log file
            writer.Close()

            'Builds SQL query to execute
            sql = "UPDATE  `tblstudents` SET  `Losses`=`Losses`+1 WHERE `Username`='" & LoggedInStudent.Username & "';"

            dbcomm = New MySqlCommand(sql, DBConn)
            dbread = dbcomm.ExecuteReader()
            dbread.Close()

            'Sets the path to where the log shall be generated, and the filename
            writer = My.Computer.FileSystem.OpenTextFileWriter(LoggedInStudent.Username & ".txt", True)

            'Writes the relevant data to the log
            writer.WriteLine(LoggedInStudent.Fname & " " & LoggedInStudent.Lname & " was beaten by " & OppStudent.Fname & " " & LoggedInStudent.Lname & " in Rock, Paper, Scissors - " & TimeOfDay & " " & DateValue(Now))
            'Saves the log file
            writer.Close()
        ElseIf Winner = 0 Then
             'Builds SQL query to execute
            sql = "UPDATE  `tblstudents` SET  `Draws`=`Draws`+1 WHERE `Username`='" & LoggedInStudent.Username & "';"

            dbcomm = New MySqlCommand(sql, DBConn)
            dbread = dbcomm.ExecuteReader()
            dbread.Close()

            'Sets the path to where the log shall be generated, and the filename
            writer = My.Computer.FileSystem.OpenTextFileWriter(LoggedInStudent.Username & ".txt", True)

            'Writes the relevant data to the log
            writer.WriteLine(LoggedInStudent.Fname & " " & LoggedInStudent.Lname & " drew with " & OppStudent.Fname & " " & OppStudent.Lname & " in Rock, Paper, Scissors - " & TimeOfDay & " " & DateValue(Now))
            'Saves the log file
            writer.Close()

            'Builds SQL query to execute
            sql = "UPDATE  `tblstudents` SET  `Draws`=`Draws`+1 WHERE `Username`='" & OppStudent.Username & "';"

            dbcomm = New MySqlCommand(sql, DBConn)
            dbread = dbcomm.ExecuteReader()
            dbread.Close()

            'Sets the path to where the log shall be generated, and the filename
            writer = My.Computer.FileSystem.OpenTextFileWriter(OppStudent.Username & ".txt", True)

            'Writes the relevant data to the log
            writer.WriteLine(OppStudent.Fname & " " & OppStudent.Lname & " drew with " & LoggedInStudent.Fname & " " & OppStudent.Lname & " in Rock, Paper, Scissors - " & TimeOfDay & " " & DateValue(Now))
            'Saves the log file
            writer.Close()
        End If
    End Sub

End Class