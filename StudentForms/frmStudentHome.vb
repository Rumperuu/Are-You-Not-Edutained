Public Class frmStudentHome

    'Subroutine runs when the form loads
    Private Sub frmStudentHome_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Runs the account section population subroutine
        AccountSection()
    End Sub

    'Subroutine runs when called in the form load sub
    Sub AccountSection()
        'Places the logged-in student's name onto the form
        lblStudentName.Text = LoggedInStudent.Fname & " " & LoggedInStudent.Lname
        'Places the logged-in student's picture onto the form
        picStudent.ImageLocation = LoggedInStudent.Username & ".jpg"
    End Sub

    'Subroutine runs when the view profile button is clicked
    Private Sub btnViewProfile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnViewProfile.Click
        'Sets the viewed profile to that of the logged-in student
        Viewing = 1

        'Opens the teacher account form
        frmStudentAccount.Show()
    End Sub


    'Subroutine runs when the Connect Four button is clicked
    Private Sub btnConnectFour_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnConnectFour.Click
        'Opens the Connect Four menu form
        frmConnect4Menu.Show()
        'Closes this form
        Me.Close()
    End Sub

    'Subroutine runs when the Noughts and Crosses button is clicked
    Private Sub btnNoughtsandCrosses_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNoughtsandCrosses.Click
        'Opens the Noughts and Crosses menu form
        frmNoughtsandCrossesMenu.Show()
        'Closes this form
        Me.Close()
    End Sub

    'Subroutine runs when the Rock, Paper, Scissors button is clicked
    Private Sub btnRPS_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRPS.Click
        'Opens the Rock, Paper, Scissors menu form
        frmRockPaperScissorsMenu.Show()
        'Closes this form
        Me.Close()
    End Sub

    'Subroutine runs when Connect Four button is moused over
    Private Sub btnConnectFour_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles btnConnectFour.MouseMove
        'Changes the game description
        lblGameDesc.Text = "Strike from the skies with your mighty red or yellow tokens, and slay the foul xenos with your glorious row of 4!" & vbCrLf & vbCrLf & "2 players"
        'Changes the game image
        picGameImg.Image = My.Resources.nac
    End Sub

    'Subroutine runs when Noughts and Crosses button is moused over
    Private Sub btnNoughtsandCrosses_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles btnNoughtsandCrosses.MouseMove
        'Changes the game description
        lblGameDesc.Text = "On the barren fields of battle, strike the enemy where he is most vulnerable by forming a line of three consecutive counters!" & vbCrLf & vbCrLf & "2 players"
        'Changes the game image
        picGameImg.Image = My.Resources.ox
    End Sub

    'Subroutine runs when Rock Paper Scissors button is moused over
    Private Sub btnRPS_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles btnRPS.MouseMove
        'Changes the game description
        lblGameDesc.Text = "It's a veritable battle royale out there, show no mercy as you lead your chosen item of stationary or geological formation to victory and glory!" & vbCrLf & vbCrLf & "2 players"
        'Changes the game image
        picGameImg.Image = My.Resources.rps
    End Sub

    'Subroutine runs when form is moused over
    Private Sub frmStudentHome_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseMove
        'Changes the game description back to the default
        lblGameDesc.Text = "Hello and welcome to the official Bourne Grammar School edutainment suite!" & vbCrLf & vbCrLf & "Pick a game or check out your account"
        'Changes the game image
        picGameImg.Image = My.Resources.edutained
    End Sub

    'Subroutine runs when game description label is moused over
    Private Sub lblGameDesc_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lblGameDesc.MouseMove
        'Changes the game description back to the default
        lblGameDesc.Text = "Hello and welcome to the official Bourne Grammar School edutainment suite!" & vbCrLf & vbCrLf & "Pick a game or check out your account"
        'Changes the game image
        picGameImg.Image = My.Resources.edutained
    End Sub
End Class