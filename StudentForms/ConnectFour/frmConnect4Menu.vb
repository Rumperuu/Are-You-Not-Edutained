Public Class frmConnect4Menu

    'Subroutine runs when the hotseat game button is clicked
    Private Sub btnHotseat_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnHotseat.Click
        'Shows the Connect Four hotseat login form
        frmConnect4HotseatLogin.Show()
        'Closes this form
        Me.Close()
    End Sub

    'Subroutine runs when the network game button is clicked
    Private Sub btnNetwork_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNetwork.Click
        'Shows the Connect Four network lobby form
        frmConnect4NetworkLobby.Show()
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