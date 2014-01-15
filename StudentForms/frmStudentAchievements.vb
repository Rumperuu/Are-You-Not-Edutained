Imports MySql.Data.MySqlClient

Public Class frmStudentAchievements

    'Subroutine runs when the form loads
    Private Sub frmStudentAchievements_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Establishes the connection to the database
        OpenDB()

        'If the account to be viewed is the logged-in or searched-for student's...
        If Viewing = 1 Then
            'Places the logged-in student's name onto the form
            lblAchievements.Text = LoggedInStudent.Fname & " " & LoggedInStudent.Lname & " Achievements"
            'However, if it is the opponent student's...
        Else
            'Places the opponent student's name onto the form
            lblAchievements.Text = OppStudent.Fname & " " & OppStudent.Lname & " Achievements"
        End If

        'Runs the achievements population subroutine
        Achievements()
    End Sub

    'Subroutine runs when the form runs
    Sub Achievements()
        'Runs the achievment subroutines
        FirstBlood()
    End Sub

    'Subroutines run when the achievement subroutine calls them
    Sub FirstBlood()
        Dim sql As String
        Dim dbcomm As MySqlCommand
        Dim dbread As MySqlDataReader

        'If the account to be viewed is the logged-in or searched-for student's...
        If Viewing = 1 Then
            'Builds SQL query to execute
            sql = "SELECT * FROM `tblstudents` WHERE `username`='" & LoggedInStudent.Username & "' AND `wins` >0;"
            'However, if it is the opponent student's...
        Else
            'Builds SQL query to execute
            sql = "SELECT * FROM `tblstudents` WHERE `username`='" & OppStudent.Username & "' AND `wins` >0;"
        End If

        dbcomm = New MySqlCommand(sql, DBConn)
        dbread = dbcomm.ExecuteReader()
        'Fills the various properties of the LoggedInStudent object with their respective values from the database
        While dbread.Read
            'Unlocks the achievement
            lblFirstBlood.Text = "First Blood"
            lblFirstBloodDeets.Text = "Win your first game"
            picFirstBlood.Image = My.Resources.firstblood

            dbread.Close()
            DBConn.Close()
            Exit Sub
        End While
        dbread.Close()
    End Sub
End Class