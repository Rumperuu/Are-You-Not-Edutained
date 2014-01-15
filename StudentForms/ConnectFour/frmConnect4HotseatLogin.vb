Imports MySql.Data.MySqlClient

Public Class frmConnect4HotseatLogin

    'Declares the variables used to log in
    Dim EnteredUsername, EnteredPassword As String

    'Subroutine runs when the form loads
    Private Sub frmConnect4HotseatLogin_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Runs the OpenDB subroutine
        OpenDB()
    End Sub

    'Subroutine runs when the okay button is clicked
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        'Runs the Login subroutine
        Login()
    End Sub

    'Subroutine runs when called in btnOK_Click subroutine
    Sub Login()
        'Sets the EnteredUsername and EnteredPassword variables to the entered username and password
        EnteredUsername = txtUsername.Text
        EnteredPassword = txtPassword.Text

        If EnteredUsername <> LoggedInStudent.Username Then
            Dim sql As String
            Dim dbcomm As MySqlCommand
            Dim dbread As MySqlDataReader
            Dim foo As Integer = 0
            sql = "SELECT * FROM tblstudents WHERE Username='" & EnteredUsername & "' AND Password='" & EnteredPassword & "';"
            dbcomm = New MySqlCommand(sql, DBConn)
            dbread = dbcomm.ExecuteReader()
            While dbread.Read
                'Fills the various properties of the OppStudent object with their respective values from the database
                OppStudent.Fname = dbread("Fname")
                OppStudent.Lname = dbread("Lname")
                OppStudent.Form = dbread("FormNum") & dbread("FormLetter")
                OppStudent.Wins = dbread("Wins")
                OppStudent.Losses = dbread("Losses")
                OppStudent.Draws = dbread("Draws")
                OppStudent.Username = dbread("Username")
                OppStudent.StudentID = dbread("StudentID")
                foo = 1
            End While
            If foo = 1 Then
                'Opens the Connect Four subject selection form
                frmConnect4HotseatSubject.Show()
                'Closes this form
                Me.Close()
                dbread.Close()
                Exit Sub
            End If
            'If the login details were invalid an error message will appear and the username and password textboxes will be blanked out
            MsgBox("Invalid: Incorrect username or password.")
            txtUsername.Text = ""
            txtPassword.Text = ""
        Else
            'If the login details where the same as those of the currently logged-in student, an error message will appear and the username and password textboxes will be blanked out
            MsgBox("Invalid: That's you.")
            txtUsername.Text = ""
            txtPassword.Text = ""
        End If
    End Sub

    'Subroutine runs when the cancel button is clicked
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        'Opens the Connect Four menu form
        frmConnect4Menu.Show()
        'Closes this form
        Me.Close()
    End Sub

End Class