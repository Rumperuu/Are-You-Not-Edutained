Public Class frmNoughtsandCrossesMenu

    'Subroutine runs when the hotseat game button is clicked
    Private Sub btnHotseat_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnHotseat.Click
        'Shows the Noughts and Crosses hotseat login form
        frmNaCHotseatLogin.Show()
        'Closes this form
        Me.Close()
    End Sub

    'Subroutine runs when the back button is clicked
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        'Shows the student home form
        frmStudentHome.Show()
        'Closes this form
        Me.Close()
    End Sub

End Class