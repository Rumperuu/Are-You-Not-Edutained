Imports MySql.Data.MySqlClient

Public Class frmLogin

    'Declares the variables used for logging in
    Dim EnteredUsername, EnteredPassword As String

    'Subroutine runs when the form loads
    Private Sub frmLogin_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Closes the splash screen form
        frmSplash.Close()

        'Runs the first-time setup detection subroutine
        DetectFTS()
    End Sub

    'Subroutine runs when the OK button is clicked
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        'Runs the login subroutine
        Login()
    End Sub

    'Subroutine runs when the cancel button is clicked
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        'Closes the program
        Me.Close()
    End Sub

    'Subroutine runs when called in the form load sub
    Sub DetectFTS()
        'Declares the variable used to determine whether first-time setup has been run
        Dim FTS As String
        'Declares the variable used to read the first-time setup text file
        Dim Reader As System.IO.StreamReader

        'Gets the path to the FTS.txt text file
        Reader = My.Computer.FileSystem.OpenTextFileReader("FTS.txt")

        'Reads what is in the FTS.txt text file
        FTS = Reader.ReadToEnd
        'If the text file consists of 0 then the first-time setup has not been run before, so...
        If FTS = "0" Then
            'Opens the first-time setup message form
            frmFTSMsg.Show()
        End If
        'Closes the reader
        Reader.Close()
    End Sub

    'Subroutine runs when called in the OK button click sub
    Sub Login()
        'Establishes the connection to the database
        OpenDB()

        'Sets the EnteredUsername variable to the entered username
        EnteredUsername = txtUsername.Text
        'Sets the EnteredPassword variable to the entered password
        EnteredPassword = txtPassword.Text

        Dim sql As String
        Dim dbcomm As MySqlCommand
        Dim dbread As MySqlDataReader

        'Builds SQL query to execute
        sql = "SELECT * FROM tblstudents WHERE Username='" & EnteredUsername & "' AND Password='" & EnteredPassword & "'"

        dbcomm = New MySqlCommand(sql, DBConn)
        dbread = dbcomm.ExecuteReader()
        'Fills the various properties of the LoggedInStudent object with their respective values from the database
        While dbread.Read
            With LoggedInStudent
                .Fname = dbread("Fname")
                .Lname = dbread("Lname")
                .Form = dbread("FormNum") & dbread("FormLetter")
                .Wins = dbread("Wins")
                .Losses = dbread("Losses")
                .Draws = dbread("Draws")
                .Username = dbread("Username")
                .StudentID = dbread("StudentID")
            End With
            dbread.Close()
            DBConn.Close()

            'Opens the student home form
            frmStudentHome.Show()
            'Closes this form
            Me.Close()
            Exit Sub
        End While
        dbread.Close()

        sql = "SELECT * FROM tblteachers WHERE Username='" & EnteredUsername & "' AND Password='" & EnteredPassword & "'"

        dbcomm = New MySqlCommand(sql, DBConn)
        dbread = dbcomm.ExecuteReader()
        'Fills the various properties of the LoggedInTeacher object with their respective values from the database
        While dbread.Read
            With LoggedInTeacher
                .Fname = dbread("Fname")
                .Lname = dbread("Lname")
                .Username = dbread("Username")
                .TeacherID = dbread("TeacherID")
            End With
            dbread.Close()
            DBConn.Close()

            'Opens the teacher home form
            frmTeacherHome.Show()
            'Closes this form
            Me.Close()
            Exit Sub
        End While
        dbread.Close()

        'If no records are found in either table...
        'Displays an error message
        MsgBox("Invalid: Incorrect username or password.")
        'Blanks out the username textbox
        txtUsername.Text = ""
        'Blanks out the password textbox
        txtPassword.Text = ""
    End Sub
End Class
