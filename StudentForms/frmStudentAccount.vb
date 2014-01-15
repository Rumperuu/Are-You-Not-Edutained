Public Class frmStudentAccount

    'Subroutine runs when the form loads
    Private Sub frmAccount_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Runs the account section population subroutine
        AccountSection()
    End Sub

    'Subroutine runs when the form loads
    Sub AccountSection()
        'If the account to be viewed is the logged-in or searched-for student's...
        If Viewing = 1 Then
            'Places the logged-in student's namse onto the form
            lblStudentName.Text = LoggedInStudent.Fname & " " & LoggedInStudent.Lname
            'Places the logged-in student's form onto the form
            lblForm.Text = LoggedInStudent.Form
            'Places the logged-in student's picture onto the form
            picStudent.ImageLocation = LoggedInStudent.Username & ".jpg"
            'Places the logged-in student's wins onto the form
            lblWins.Text = "Wins: " & LoggedInStudent.Wins
            'Places the logged-in student's losses onto the form
            lblLosses.Text = "Losses: " & LoggedInStudent.Losses
            'Places the logged-in student's draws onto the form
            lblDraws.Text = "Draws: " & LoggedInStudent.Draws
            'However, if it is the opponent student's...
        Else
            'Places the opponent student's name onto the form
            lblStudentName.Text = OppStudent.Fname & " " & OppStudent.Lname
            'Places the opponent student's form onto the form
            lblForm.Text = OppStudent.Form
            'Places the opponent student's picture onto the form
            picStudent.ImageLocation = OppStudent.Username & ".jpg"
            'Places the opponent student's wins onto the form
            lblWins.Text = "Wins: " & OppStudent.Wins
            'Places the opponent student's losses onto the form
            lblLosses.Text = "Losses: " & OppStudent.Losses
            'Places the opponent student's draws onto the form
            lblDraws.Text = "Draws: " & OppStudent.Draws
        End If
    End Sub

    'Subroutine runs when the view breakdown button is clicked
    Private Sub btnBreakdown_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBreakdown.Click
        'Opens the student breakdown form
        frmStudentBreakdown.Show()
    End Sub

    'Subroutine runs when the view achievments button is clicked
    Private Sub btnAchievements_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAchievements.Click
        'Opens the student achievments form
        frmStudentAchievements.Show()
    End Sub

End Class