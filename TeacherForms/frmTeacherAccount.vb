Imports MySql.Data.MySqlClient

Public Class frmTeacherAccount

    'Subroutine runs when the form loads
    Private Sub frmTeacherAccount_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Establishes the connection to the database
        OpenDB()

        'Runs the account section population subroutine
        AccountSection()
    End Sub

    'Subroutine runs when the form loads
    Sub AccountSection()
        'Places the logged-in teacher's name onto the form
        lblTeacherName.Text = LoggedInTeacher.Fname & " " & LoggedInTeacher.Lname
        'Places the logged-in teacher's picture onto the form
        picTeacher.ImageLocation = LoggedInTeacher.Username & ".jpg"

        Dim sql As String
        Dim dbcomm As MySqlCommand
        Dim dbread As MySqlDataReader

        'Builds SQL query to execute
        sql = "SELECT * FROM `tblquestions` WHERE `TeacherID`='" & LoggedInTeacher.TeacherID & "';"
        dbcomm = New MySqlCommand(sql, DBConn)
        dbread = dbcomm.ExecuteReader()
        Dim i As Integer = 0
        'Populates the questions created label
        If dbread.HasRows = True Then
            While dbread.Read
                i = i + 1
            End While
            lblQsCreated.Text = "Questions Created: " & i
        Else
            lblQsCreated.Text = "Questions Created: 0"
        End If
        'Closes the recordset
        dbread.Close()
    End Sub

    'Subroutine runs when the view breakdown button is clicked
    Private Sub btnBreakdown_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBreakdown.Click
        'Shows the teacher breakdown form
        frmTeacherBreakdown.Show()
    End Sub

    'Subroutine runs when the view achievements button is clicked
    Private Sub btnAchievements_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAchievements.Click
        'Shows the teacher achievements form
        frmTeacherAchievements.Show()
    End Sub

End Class