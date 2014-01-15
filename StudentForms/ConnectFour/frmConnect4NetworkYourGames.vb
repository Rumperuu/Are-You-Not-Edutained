Imports MySql.Data.MySqlClient

Public Class frmConnect4NetworkYourGames

    'Declares the variable used to determine if a game has a second player or not
    Dim NoOpp As Boolean = True

    'Subroutine runs on form load
    Private Sub frmConnect4NetworkYourGames_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Runs the OpenDB subroutine
        OpenDB()

        'Runs the UpdateList subroutine
        UpdateList()

        'Brings the host game form to the front
        frmConnect4NetworkHostGame.BringToFront()
    End Sub

    'Subroutine runs when called on form load or btnRefresh_Click subroutines
    Sub UpdateList()
        'Clears the lobby listbox of data
        lstYourGames.Items.Clear()

        'Declares the variable used for the result of the confirmation messagebox
        Dim Result As MsgBoxResult
        'Declares the variable used to determine if the logged-in player is currently playing a given game
        Dim CurrPlaying As String = ""

        Dim sql As String
        Dim dbcomm As MySqlCommand
        Dim dbread As MySqlDataReader

        'Builds SQL query to execute
        sql = "SELECT * FROM tblconnect4 WHERE HostUsername = '" & LoggedInStudent.Username & "'"

        dbcomm = New MySqlCommand(sql, DBConn)
        dbread = dbcomm.ExecuteReader()
        'Fills the various properties of the LoggedInStudent object with their respective values from the database
        While dbread.Read
            'If there is an opponent in the game...
            If dbread("OppUsername").Value <> "" Then
                'Sets CurrPlaying to positive
                CurrPlaying = " - Opponent: " & dbread("OppUsername").Value
            Else
                'Otherwise CurrPlaying is negative
                CurrPlaying = " - No opponent"
            End If
            'Add the game details to the lobby listbox
            lstYourGames.Items.Add(dbread("GameID").Value & " - " & dbread("GameName").Value & " - " & dbread("HostUsername").Value & CurrPlaying)
            dbread.Close()
            Exit Sub
        End While
        dbread.Close()
    End Sub

    'Subroutine runs when the host game button is clicked
    Private Sub btnHostGame_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnHostGame.Click
        'Shows the Connect Four network host game form
        frmConnect4NetworkHostGame.Show()
    End Sub

    'Subroutine runs when the selected item of the lobby listbox is changes
    Private Sub lstYourGames_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstYourGames.SelectedIndexChanged
        'Determine the GameID of the selected game
        Dim EndofGamID As Integer = InStr(1, lstYourGames.SelectedItem, " ", CompareMethod.Text)
        Dim GamID As String = Mid(lstYourGames.SelectedItem, 1, EndofGamID)

        'Declares the variable used to determine the username of the host
        Dim Username As String

        Dim sql As String
        Dim dbcomm As MySqlCommand
        Dim dbread As MySqlDataReader

        Try
            'Builds SQL query to execute
            sql = "SELECT * FROM tblConnect4 WHERE GameID='" & GamID & "'"

            'Sets the selected GameID
            GameID = GamID

            dbcomm = New MySqlCommand(sql, DBConn)
            dbread = dbcomm.ExecuteReader()
            'Fills the various properties of the LoggedInStudent object with their respective values from the database
            While dbread.Read
                'If there is an opponent...
                If dbread("OppUsername") <> "" Then
                    'Sets the players
                    LoggedInStudent.C4Player = "Red"
                    OppStudent.C4Player = "Yellow"

                    'Sets the host
                    Username = dbread("OppUsername")

                    'Closes the recordset
                    dbread.Close()

                    'Builds the SQl query to execute
                    sql = "SELECT * FROM tblstudents WHERE username='" & Username & "'"
                    dbcomm = New MySqlCommand(sql, DBConn)
                    dbread = dbcomm.ExecuteReader()

                    'If there are records found...
                    While dbread.Read
                        'Populates the properties of the OppStudent object with the host's data
                        With OppStudent
                            .Fname = dbread("fname")
                            .Lname = dbread("lname")
                            .StudentID = dbread("StudentID")
                            .Username = dbread("username")
                        End With

                        'Sets the no opponent flag to false
                        NoOpp = False
                        dbread.Close()
                        Exit Sub
                    End While
                    'Sets the no opponent flag to true
                    NoOpp = True
                    dbread.Close()
                    Exit Sub
                End If
                'Closes the recordset
                dbread.Close()
                Exit Sub
            End While
            dbread.Close()
        Catch
        End Try
    End Sub

    'Subroutine runs when join game button is clicked
    Private Sub btnJoinGame_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnJoinGame.Click
        'If there is an opponent in the game...
        If NoOpp = False Then
            'Shows the Connect Four network game form
            frmConnect4Network.Show()
            'Closes this form
            Me.Close()
        Else
            'Displays an error message
            MsgBox("No opponent in game")
        End If
    End Sub

    'Subroutine runs when the refresh button is clicked
    Private Sub btnRefresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRefresh.Click
        'Runs the UpdateList subroutine
        UpdateList()
    End Sub
End Class