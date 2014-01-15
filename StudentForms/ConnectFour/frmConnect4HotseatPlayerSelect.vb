Public Class frmConnect4HotseatPlayerSelect

    'Subroutine runs on form load
    Private Sub frmConnect4HotseatPlayerSelect_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Populates the player name labels with data
        lblLoggedInStudent.Text = LoggedInStudent.Fname & " " & LoggedInStudent.Lname
        lblOppStudent.Text = OppStudent.Fname & " " & OppStudent.Lname
    End Sub

    'Subroutines run when the counter selection buttons are clicked
    Private Sub btnOSred_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOSred.Click
        'Sets the opponent student's colour to red
        OppStudent.C4Player = "Red"
        'Updates the appearance of the form
        picOSred.Image = My.Resources.red
        'Disables changing the colour and the logged-in student also picking red
        btnOSred.Enabled = False
        btnOSyellow.Enabled = False
        btnLISred.Enabled = False
        With picRed
            .Image = Nothing
            .Tag = "None"
        End With
        'Runs the CheckBoth subroutine
        CheckBoth()
    End Sub
    Private Sub btnOSyellow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOSyellow.Click
        OppStudent.C4Player = "Yellow"
        picOSyellow.Image = My.Resources.yellow
        btnOSyellow.Enabled = False
        btnOSred.Enabled = False
        btnLISyellow.Enabled = False
        With picYellow
            .Image = Nothing
            .Tag = "None"
        End With
        CheckBoth()
    End Sub
    Private Sub btnLISred_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLISred.Click
        LoggedInStudent.C4Player = "Red"
        picLISred.Image = My.Resources.red
        btnLISred.Enabled = False
        btnLISyellow.Enabled = False
        btnOSred.Enabled = False
        With picRed
            .Image = Nothing
            .Tag = "None"
        End With
        CheckBoth()
    End Sub
    Private Sub btnLISyellow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLISyellow.Click
        LoggedInStudent.C4Player = "Yellow"
        picLISyellow.Image = My.Resources.yellow
        btnLISyellow.Enabled = False
        btnLISred.Enabled = False
        btnOSyellow.Enabled = False
        With picYellow
            .Image = Nothing
            .Tag = "None"
        End With
        CheckBoth()
    End Sub

    'Subroutine runs when called in the counter selection button click subroutines
    Sub CheckBoth()
        'If both players have chosen...
        If picYellow.Tag = "None" And picRed.Tag = "None" Then
            'Enables the button to leave the form
            btnContinue.Enabled = True
        End If
    End Sub

    'Subroutine runs when the continue button is clicked
    Private Sub btnContinue_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnContinue.Click
        'Shows the Connect Four hotseat form
        frmConnect4Hotseat.Show()
        'Closes this form
        Me.Close()
    End Sub

    'Subroutine runs when the cancel button is clicked
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        'Shows the Connect Four hotseat question selection form
        frmConnect4HotseatSubject.Show()
        'Closes this form
        Me.Close()
    End Sub

End Class