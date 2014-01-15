Public Class frmFirstTimeSetup

    'Subroutine runs when the add subject button is clicked
    Private Sub btnAddSubject_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddSubject.Click
        'Opens the add subject form
        frmAddSubject.Show()
    End Sub

    'Subroutine runs when the add topic button is clicked
    Private Sub btnAddTopic_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddTopic.Click
        'Opens the add topic form
        frmAddTopic.Show()
    End Sub

    'Subroutine runs when the add teacher button is clicked
    Private Sub btnAddTeacher_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddTeacher.Click
        'Opens the add teacher form
        frmAddTeacher.Show()
    End Sub

    'Subroutine runs when the add student button is clicked
    Private Sub btnAddStudent_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddStudent.Click
        'Opens the add student form
        frmAddStudent.Show()
    End Sub

    'Subroutine runs when the done button is clicked
    Private Sub btnDone_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDone.Click
        'Declares the variable used for writing to the text file
        Dim writer As System.IO.StreamWriter
        'Gets the filepath to the first-time setup text file
        writer = My.Computer.FileSystem.OpenTextFileWriter("FTS.txt", False)
        'Amends the text file to indicate that the first-time setup has been run
        writer.WriteLine("1")
        'Saves the log file
        writer.Close()

        'Opens the login form
        frmLogin.Show()
        'Closes this form
        Me.Close()
    End Sub

End Class