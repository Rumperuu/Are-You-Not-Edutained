Public Class frmSplash
    'Declares the variable used for counting down the loading
    Dim Count As Integer = 0
    'Declares the variable used for the first half of the loading phrases
    Dim LiesP1(11) As String
    'Declares the variable used for the second half of the loading phrases
    Dim LiesP2(11) As String

    'Subroutine runs when the form loads
    Private Sub frmSplash_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Enables the timer used to simulate a loading
        tmrLoading.Enabled = True

        'Runs the loading phrase generation subroutine
        Lies()

        'Runs the random number generator subroutine
        Randomize()

        'Declares the variable used for the first half of the loading phrases
        Dim Lie1 As Integer = CInt(Int(5 * Rnd()) + 1)
        'Declares the variable used for the second half of the loading phrases
        Dim Lie2 As Integer = CInt(Int(5 * Rnd()) + 1)

        'Creates a loading phrase
        lblLies.Text = LiesP1(Lie1) & " " & LiesP2(Lie2)
    End Sub

    'Subroutine runs when called in the form load sub
    Sub Lies()
        'Fills the first half of the loading phrase with possible words
        LiesP1(1) = "Triangulating"
        LiesP1(2) = "Decoding"
        LiesP1(3) = "Turtling"
        LiesP1(4) = "Calculating"
        LiesP1(5) = "Transcoding"
        LiesP1(6) = "Observing"
        LiesP1(7) = "Translating"
        LiesP1(8) = "Hypothesising"
        LiesP1(9) = "Polymorphing"
        LiesP1(10) = "Flipping"
        LiesP1(11) = "Reticulating"

        'Fills the second half of the loading phrase with possible words
        LiesP2(1) = "code"
        LiesP2(2) = "sums"
        LiesP2(3) = "hexagons"
        LiesP2(4) = "rolls"
        LiesP2(5) = "beeps"
        LiesP2(6) = "compulsion"
        LiesP2(7) = "singularity engine"
        LiesP2(8) = "boops"
        LiesP2(9) = "the machine spirit"
        LiesP2(10) = "observance"
        LiesP2(11) = "splines"
    End Sub

    'Subroutine runs when timer is enabled
    Private Sub tmrLoading_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrLoading.Tick
        'Runs the random number generator subroutine
        Randomize()

        'Declares the variable used for determining how big a step to take
        Dim Tick As Integer
        'Declares the variable used for keeping the timer from stepping out-of-bounds
        Dim Check As Integer = 10

        'If there is more than 10 counts remaining in the timer...
        If progLoading.Value < 990 Then
            'Sets the step size as a random number between 0-9
            Tick = CInt(Int(10 * Rnd()))
            'If there are only 10 counts remaining...
        Else
            'Sets the step size as a random number within the bounds of the timer
            Tick = CInt(Int((1000 - progLoading.Value) * Rnd()) + 1)
        End If

        'On each tick of the timer, 'Count' is increased by the step size
        Count = Count + Tick
        'Along with this, the progress bar increments by the step size
        progLoading.Value = progLoading.Value + Tick

        'If the timer is at a value divisible by 10...
        If Count Mod Check = 0 Then
            'Declares the variable used for the first half of the loading phrases
            Dim Lie1 As Integer = CInt(Int(5 * Rnd()) + 1)
            'Declares the variable used for the second half of the loading phrases
            Dim Lie2 As Integer = CInt(Int(5 * Rnd()) + 1)

            'Creates a loading phrase
            lblLies.Text = LiesP1(Lie1) & " " & LiesP2(Lie2)

            ''Check' increments by 10
            Check = Check + 10
        End If

        'If the timer has reached its limit...
        If Count = 1000 Then
            'Disable the timer
            tmrLoading.Enabled = False
            'Show the login form
            frmLogin.Show()
        End If
    End Sub

End Class
