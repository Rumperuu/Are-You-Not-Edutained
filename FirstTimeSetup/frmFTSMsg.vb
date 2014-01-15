Public Class frmFTSMsg

    'Subroutine runs when the yes button is clicked
    Private Sub btnYes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnYes.Click
        'Closes the login form
        frmLogin.Close()
        'Opens the first-time setup form
        frmFirstTimeSetup.Show()
        'Closes this form
        Me.Close()
    End Sub

    'Subroutine runs when the no button is clicked
    Private Sub btnNo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNo.Click
        'Declares the variable used for writing to the text file
        Dim writer As System.IO.StreamWriter
        'Gets the filepath to the first-time setup text file
        writer = My.Computer.FileSystem.OpenTextFileWriter("FTS.txt", False)
        'Amends the text file to indicate that the first-time setup has been run
        writer.WriteLine("1")
        'Saves the log file
        writer.Close()
        'Closes this form
        Me.Close()
    End Sub
End Class