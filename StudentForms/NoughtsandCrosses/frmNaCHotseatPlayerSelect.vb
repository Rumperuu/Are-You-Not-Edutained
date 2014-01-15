Public Class frmNaCHotseatPlayerSelect

    'Subroutine runs on form load
    Private Sub frmNACHotseatPlayerSelect_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Populates the player name labels with data
        lblLoggedInStudent.Text = LoggedInStudent.Fname & " " & LoggedInStudent.Lname
        lblOppStudent.Text = OppStudent.Fname & " " & OppStudent.Lname
    End Sub

    'Subroutines run when the counter selection buttons are clicked
    Private Sub btnOSX_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOSX.Click
        'Sets the opponent student's counter to X
        OppStudent.NaCPlayer = "X"
        'Updates the appearance of the form
        picOSX.Image = My.Resources.x
        'Disables changing the counter and the logged-in student also picking X
        btnOSX.Enabled = False
        btnOSO.Enabled = False
        btnLISX.Enabled = False
        With picX
            .Image = Nothing
            .Tag = "None"
        End With
        'Runs the CheckBoth subroutine
        CheckBoth()
    End Sub
    Private Sub btnOSO_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOSO.Click
        OppStudent.NaCPlayer = "O"
        picOSO.Image = My.Resources.o
        btnOSO.Enabled = False
        btnOSX.Enabled = False
        btnLISO.Enabled = False
        With picO
            .Image = Nothing
            .Tag = "None"
        End With
        CheckBoth()
    End Sub
    Private Sub btnLISX_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLISX.Click
        LoggedInStudent.NaCPlayer = "X"
        picLISX.Image = My.Resources.x
        btnLISX.Enabled = False
        btnLISO.Enabled = False
        btnOSX.Enabled = False
        With picX
            .Image = Nothing
            .Tag = "None"
        End With
        CheckBoth()
    End Sub
    Private Sub btnLISO_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLISO.Click
        LoggedInStudent.NaCPlayer = "O"
        picLISO.Image = My.Resources.o
        btnLISO.Enabled = False
        btnLISX.Enabled = False
        btnOSO.Enabled = False
        With picO
            .Image = Nothing
            .Tag = "None"
        End With
        CheckBoth()
    End Sub

    'Subroutine runs when called in the counter selection button click subroutines
    Sub CheckBoth()
        'If both players have chosen...
        If picO.Tag = "None" And picX.Tag = "None" Then
            'Enables the button to leave the form
            btnContinue.Enabled = True
        End If
    End Sub

    'Subroutine runs when the continue button is clicked
    Private Sub btnContinue_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnContinue.Click
        'Shows the Noughts and Crosses hotseat form
        frmNaCHotseat.Show()
        'Closes this form
        Me.Close()
    End Sub

    'Subroutine runs when the cancel button is clicked
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        'Shows the Noughts and Crosses hotseat question selection form
        frmNaCHotseatSubject.Show()
        'Closes this form
        Me.Close()
    End Sub

End Class