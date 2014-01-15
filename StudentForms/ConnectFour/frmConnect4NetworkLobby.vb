Imports MySql.Data.MySqlClient

Public Class frmConnect4NetworkLobby
    'Declares the variable used to determine if a game has a second player or not
    Dim NoOpp As Boolean = True
    'Declares the variable used to determine if the logged-in player is the host of a game
    Dim Host As Boolean = True

    'Subroutine runs on form load
    Private Sub frmConnect4NetworkLobby_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Runs the OpenDB subroutine
        OpenDB()

        'Runs the UpdateList subroutine
        UpdateList()
    End Sub

    'Subroutine runs when called on form load or btnRefresh_Click subroutines
    Sub UpdateList()
        'Clears the lobby listbox of data
        lstLobby.Items.Clear()

        'Declares the variable used to determine if the logged-in player is currently playing a given game
        Dim CurrPlaying As String = ""

        Dim sql As String
        Dim dbcomm As MySqlCommand
        Dim dbread As MySqlDataReader

        'Builds SQL query to execute
        sql = "SELECT * FROM tblconnect4 WHERE OppUsername ='' OR OppUsername ='" & LoggedInStudent.Username & "' OR HostUsername ='" & LoggedInStudent.Username & "';"

        dbcomm = New MySqlCommand(sql, DBConn)
        dbread = dbcomm.ExecuteReader()
        Dim foo As Integer = 0
        'Fills the various properties of the LoggedInStudent object with their respective values from the database
        While dbread.Read
            'If there is an opponent in the game...
            If dbread("OppUsername") <> "" Then
                'Sets CurrPlaying to positive
                CurrPlaying = " - CURRENTLY PLAYING"
            Else
                'Otherwise CurrPlaying is negative
                CurrPlaying = ""
            End If
            'Add the game details to the lobby listbox
            lstLobby.Items.Add(dbread("GameID") & " - " & dbread("GameName") & " - " & dbread("HostUsername") & CurrPlaying)
            foo = 1
        End While
        dbread.Close()
    End Sub

    'Subroutine runs when the host game button is clicked
    Private Sub btnHostGame_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnHostGame.Click
        'Shows the Connect Four network hosting form
        frmConnect4NetworkHostGame.Show()
    End Sub

    'Subroutine runs when the your games button is clicked
    Private Sub btnYourGames_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnYourGames.Click
        'Shows the Connect Four network your games form
        frmConnect4NetworkYourGames.Show()
    End Sub

    'Subroutine runs when the selected item of the lobby listbox is changes
    Private Sub lstLobby_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstLobby.SelectedIndexChanged
        foob()
    End Sub

    Sub foob()
        'Determine the GameID of the selected game
        Dim EndofGamID As Integer = InStr(1, lstLobby.SelectedItem, " ", CompareMethod.Text)
        Dim GamID As String = Mid(lstLobby.SelectedItem, 1, EndofGamID - 1)

        'Declares the variables used to determine the usernames of both players in the selected game
        Dim HUsername As String
        Dim Username As String

        Dim sql As String
        Dim dbcomm As MySqlCommand
        Dim dbread As MySqlDataReader
        'Builds SQL query to execute
        sql = "SELECT * FROM tblconnect4 WHERE GameID=" & GamID & ";"

        'Sets the selected GameID
        GameID = GamID

        dbcomm = New MySqlCommand(sql, DBConn)
        dbread = dbcomm.ExecuteReader()

        'Fills the various properties of the LoggedInStudent object with their respective values from the database
        While dbread.Read
            'If the logged-in user isn't the host of the selected game...
            If dbread("HostUsername") <> LoggedInStudent.Username Then
                'Sets the host flag to false
                Host = False

                'Sets the players
                LoggedInStudent.C4Player = "Yellow"
                OppStudent.C4Player = "Red"

                'Sets the opponent as the host
                Username = dbread("HostUsername")

                'Closes the recordset
                dbread.Close()

                'Builds SQL query to execute
                sql = "SELECT * FROM tblstudents WHERE Username='" & Username & "';"

                dbcomm = New MySqlCommand(sql, DBConn)
                dbread = dbcomm.ExecuteReader()

                While dbread.Read
                    'Populates the properties of the OppStudent object with the host's data
                    With OppStudent
                        .Fname = dbread("Fname")
                        .Lname = dbread("Lname")
                        .StudentID = dbread("StudentID")
                        .Username = dbread("Username")
                    End With
                End While

                'Sets the no opponent flag to false
                NoOpp = False
                dbread.Close()
            Else
                'If the logged-in user is the host, sets the host flag to true
                Host = True
                'Sets the players
                LoggedInStudent.C4Player = "Red"
                OppStudent.C4Player = "Yellow"

                'Sets the usernames of the players
                HUsername = dbread("HostUsername")
                Username = dbread("OppUsername")

                'Closes the recordset
                dbread.Close()

                'Builds SQL query to execute
                sql = "SELECT * FROM tblstudents WHERE Username='" & Username & "';"

                dbcomm = New MySqlCommand(sql, DBConn)
                dbread = dbcomm.ExecuteReader()

                While dbread.Read
                    'Populates the properties of the OppStudent object with data
                    With OppStudent
                        .Fname = dbread("Fname")
                        .Lname = dbread("Lname")
                        .StudentID = dbread("StudentID")
                        .Username = dbread("Username")
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
    End Sub

    'Subroutine runs when join game button is clicked
    Private Sub btnJoinGame_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnJoinGame.Click
        Dim sql As String
        Dim dbcomm As MySqlCommand
        Dim dbread As MySqlDataReader

        'If there is an opponent in the game...
        If NoOpp = False Then
            If Host = False Then
                'Builds the SQL query to execute
                sql = "UPDATE  `tblconnect4` SET  `OppUsername`='" & LoggedInStudent.Username & "' WHERE  `GameID`='" & GameID & "';"
                dbcomm = New MySqlCommand(sql, DBConn)
                dbread = dbcomm.ExecuteReader()
                dbread.Close()
            End If

            'Shows the Connect Four network game form
            frmConnect4Network.Show()
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