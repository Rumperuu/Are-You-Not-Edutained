Public Class frmTeacherHome

    'Subroutine runs when the form loads
    Private Sub frmTeacherHome_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Runs the account section population subroutine
        AccountSection()
    End Sub

    'Subroutine runs when called in the form load sub
    Sub AccountSection()
        'Places the logged-in teacher's name onto the form
        lblTeacherName.Text = LoggedInTeacher.Fname & " " & LoggedInTeacher.Lname
        'Places the logged-in teacher's picture onto the form
        picTeacher.ImageLocation = LoggedInTeacher.Username & ".jpg"
    End Sub

    'Subroutine runs when the view profile button is clicked
    Private Sub btnViewProfile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnViewProfile.Click
        'Opens the teacher account form
        frmTeacherAccount.Show()
    End Sub

    'Subroutine runs when the view student button is clicked
    Private Sub btnViewStudent_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnViewStudent.Click
        'Opens the view student form
        frmViewStudent.Show()
        'Closes this form
        Me.Close()
    End Sub

    'Subroutine runs when the create question button is clicked
    Private Sub btnCreateQuestion_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCreateQuestion.Click
        'Opens the add question form
        frmAddQuestion.Show()
    End Sub

    'Subroutine runs when the add student button is clicked
    Private Sub btnAddStudent_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddStudent.Click
        'Opens the add student form
        frmAddStudent.Show()
    End Sub

End Class