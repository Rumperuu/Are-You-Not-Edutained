Public Class frmStudentBreakdown

    'Subroutine runs when the form loads
    Private Sub frmStudentBreakdown_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Declares the variable used to read the student breakdown text file
        Dim Reader As System.IO.StreamReader

        'If the account to be viewed is the logged-in or searched-for student's...
        If Viewing = 1 Then
            'Places the logged-in student's name onto the form
            lblBreakdown.Text = LoggedInStudent.Fname & " " & LoggedInStudent.Lname & " Win Breakdown"

            'Gets the path to the logged-in student's breakdown text file
            Reader = My.Computer.FileSystem.OpenTextFileReader(LoggedInStudent.Username & ".txt")
            'However, if it is the opponent student's...
        Else
            'Places the opponent student's name onto the form
            lblBreakdown.Text = OppStudent.Fname & " " & OppStudent.Lname & " Win Breakdown"

            'Gets the path to the opponent student's breakdown text file
            Reader = My.Computer.FileSystem.OpenTextFileReader(OppStudent.Username & ".txt")
        End If

        'Whilst the end of the text file hasn't been reached...
        While Not Reader.EndOfStream
            'Adds a line to the breakdown listbox
            lstBreakdown.Items.Add(Reader.ReadLine)
        End While

        'Closes the recordset
        Reader.Close()
    End Sub

End Class