Imports MySql.Data.MySqlClient

Public Class frmTeacherAchievements

    'Subroutine runs when the form loads
    Private Sub frmTeacherAchievements_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Establishes the connection to the database
        OpenDB()

        'Places the logged-in teacher's name onto the form
        lblAchievements.Text = LoggedInTeacher.Fname & " " & LoggedInTeacher.Lname & " Achievements"

        'Runs the achievements population subroutine
        Achievements()
    End Sub

    'Subroutine runs when the form runs
    Sub Achievements()
        'Runs the achievment subroutines
        TeachingAssistant()
        TenuredProf()
    End Sub

    'Subroutines run when the achievement subroutine calls them
    Sub TeachingAssistant()
        Dim sql As String
        Dim dbcomm As MySqlCommand
        Dim dbread As MySqlDataReader

        'Builds SQL query to execute
        sql = "SELECT * FROM `tblquestions` WHERE `TeacherID`='" & LoggedInTeacher.TeacherID & "' AND `QsAdded` >0"

        dbcomm = New MySqlCommand(sql, DBConn)
        dbread = dbcomm.ExecuteReader()
        'Fills the various properties of the LoggedInStudent object with their respective values from the database
        While dbread.Read
            'Unlocks the achievement
            lblTeachingAssistant.Text = "Teaching Assistant"
            lblTeachingAssistantDeets.Text = "Create your first question"
            picTeachingAssistant.Image = My.Resources.Assistant

            dbread.Close()
            DBConn.Close()
            Exit Sub
        End While
        dbread.Close()
    End Sub

    Sub TenuredProf()
        Dim sql As String
        Dim dbcomm As MySqlCommand
        Dim dbread As MySqlDataReader

        'Builds SQL query to execute
        sql = "SELECT * FROM `tblquestions` WHERE `TeacherID`='" & LoggedInTeacher.TeacherID & "' AND `QsAdded` >19"

        dbcomm = New MySqlCommand(sql, DBConn)
        dbread = dbcomm.ExecuteReader()
        'Fills the various properties of the LoggedInStudent object with their respective values from the database
        While dbread.Read
            'Unlocks the achievement
            lblTenuredProf.Text = "Tenured Professor"
            lblTenuredProfDeets.Text = "Create 20 questions"
            picTenuredProf.Image = My.Resources.professor

            dbread.Close()
            DBConn.Close()
            Exit Sub
        End While
        dbread.Close()
    End Sub
End Class