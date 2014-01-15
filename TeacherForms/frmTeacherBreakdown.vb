Public Class frmTeacherBreakdown

    'Subroutine runs when the form loads
    Private Sub frmTeacherBreakdown_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Declares the variable used to read the teacher breakdown text file
        Dim Reader As System.IO.StreamReader

        'Places the logged-in teacher's name onto the form
        lblBreakdown.Text = LoggedInTeacher.Fname & " " & LoggedInTeacher.Lname & " Question Breakdown"

        'Gets the path to the logged-in teacher's breakdown text file
        Reader = My.Computer.FileSystem.OpenTextFileReader(LoggedInTeacher.Username & ".txt")

        'Whilst the end of the text file hasn't been reached...
        While Not Reader.EndOfStream
            'Adds a line to the breakdown listbox
            lstBreakdown.Items.Add(Reader.ReadLine)
        End While

        'Closes the recordset
        Reader.Close()
    End Sub

End Class