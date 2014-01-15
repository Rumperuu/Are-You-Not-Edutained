Imports MySql.Data.MySqlClient

Public Class frmViewStudent

    'Subroutine runs when the form loads
    Private Sub frmViewStudent_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Establishes the connection to the database
        OpenDB()

        'Runs the account section population subroutine
        AccountSection()

        Dim sql As String
        Dim dbcomm As MySqlCommand
        Dim dbread As MySqlDataReader

        'Builds SQL query to execute
        sql = "SELECT * FROM tblstudents"

        dbcomm = New MySqlCommand(sql, DBConn)
        dbread = dbcomm.ExecuteReader()
        'Fills the various properties of the LoggedInStudent object with their respective values from the database
        While dbread.Read
            'Adds a line to the students listbox
            lstStudents.Items.Add(dbread("StudentID") & " - " & dbread("Fname") & " " & dbread("Lname"))
        End While
        dbread.Close()
    End Sub

    'Subroutine runs when called in the form loads
    Sub AccountSection()
        'Places the logged-in teacher's name onto the form
        lblTeacherName.Text = LoggedInTeacher.Fname & " " & LoggedInTeacher.Lname
        'Places the logged-in teacher's picture onto the form
        picTeacher.ImageLocation = LoggedInTeacher.Username & ".jpg"
    End Sub

    'Subroutine runs when the view profile button is clicked
    Private Sub btnViewProfile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnViewProfile.Click
        'Opens teacher account form
        frmTeacherAccount.Show()
    End Sub

    'Subroutine runs when the search button is clicked
    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Dim sql As String
        Dim dbcomm As MySqlCommand
        Dim dbread As MySqlDataReader

        'Blanks the listbox
        lstStudents.Items.Clear()

        'Builds SQL query to execute
        If (txtFname.Text <> "") And (txtLname.Text <> "") Then
            'Builds SQL query to execute
            sql = "SELECT * FROM tblstudents WHERE Fname LIKE '%" & txtFname.Text & "%' AND Lname LIKE '%" & txtLname.Text & "%'"
        ElseIf (txtFname.Text <> "") And (txtLname.Text = "") Then
            'Builds SQL query to execute
            sql = "SELECT * FROM tblstudents WHERE Fname LIKE '%" & txtFname.Text & "%'"
        ElseIf (txtFname.Text = "") And (txtLname.Text <> "") Then
            'Builds SQL query to execute
            sql = "SELECT * FROM tblstudents WHERE Lname LIKE '%" & txtLname.Text & "%'"
        ElseIf (txtFname.Text = "") And (txtLname.Text = "") Then
            'Builds SQL query to execute
            sql = "SELECT * FROM tblstudents"
        End If

        dbcomm = New MySqlCommand(sql, DBConn)
        dbread = dbcomm.ExecuteReader()
        Dim foo As Integer = 0
        While dbread.Read
            'Adds a line to the students listbox
            lstStudents.Items.Add(dbread("StudentID") & " - " & dbread("Fname") & " " & dbread("Lname"))
            foo = 1
        End While
        If foo = 1 Then
            dbread.Close()
            Exit Sub
        End If
        'If no records are found...
        'Displays a message box
        MsgBox("No records found")
        dbread.Close()
    End Sub

    'Subroutine runs when the selected index of the students listbox changes
    Private Sub lstStudents_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstStudents.SelectedIndexChanged
        'Declares the variable used for getting the end of the StudentID
        Dim EndofStudentID As Integer = InStr(1, lstStudents.SelectedItem, " ", CompareMethod.Text)
        'Declares the variable used for storing the StudentID
        Dim StudID As String = Mid(lstStudents.SelectedItem, 1, EndofStudentID)


        Dim sql As String
        Dim dbcomm As MySqlCommand
        Dim dbread As MySqlDataReader

        'Builds SQL query to execute
        sql = "SELECT * FROM tblstudents WHERE StudentID='" & StudID & "'"

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
            End With
        End While
        dbread.Close()

        'Enables the view student profile button
        btnViewStudentProfile.Enabled = True
    End Sub

    'Subroutine runs when the view student button is clicked
    Private Sub btnViewStudentProfile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnViewStudentProfile.Click
        'Opens the student account form
        frmStudentAccount.Show()
    End Sub

    'Subroutine runs when the back button is clicked
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        'Opens the teacher home form
        frmTeacherHome.Show()
        'Closes this form
        Me.Close()
    End Sub
End Class