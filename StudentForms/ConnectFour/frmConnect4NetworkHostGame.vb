Imports MySql.Data.MySqlClient

Public Class frmConnect4NetworkHostGame
    'Declares the variable used to store the chosen SubjectID
    Dim SubjectID As Integer

    'Subroutine runs on form load
    Private Sub frmConnect4NetworkHostGame_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Runs the OpenDB subroutine
        OpenDB()

        'Runs the Populate subroutine
        Populate()
    End Sub

    'Subroutine runs then the host game button is clicked
    Private Sub btnHost_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnHost.Click
        'Declares the variable used for getting the result of the message box
        Dim Result As MsgBoxResult
        'Declares the variable used for detecting invalid data entry
        Dim Errors As String = ""

        Dim sql As String
        Dim dbcomm As MySqlCommand
        Dim dbread As MySqlDataReader

        'Assembles an error report if any invalid data entry detected
        If txtGameName.Text = "" Then
            Errors = Errors & "No game name input" & vbCrLf
        End If
        If cmboSubject.SelectedItem = "" Then
            Errors = Errors & "No subject selected" & vbCrLf
        End If
        If cmboDifficulty.SelectedItem = "" Then
            Errors = Errors & "No difficulty selected" & vbCrLf
        End If
        If cmboTopic.SelectedItem = "" Then
            Errors = Errors & "No topic selected" & vbCrLf
        End If

        'If no invalid data entry is detected...
        If Errors = "" Then
            'Displays a validation message box before saving the data to the database
            Result = MsgBox("Are you sure all these details are correct? Remember, spelling is vital." & vbCrLf & vbCrLf & "Details:" & vbCrLf & txtGameName.Text & vbCrLf & cmboSubject.SelectedItem & " (" & cmboDifficulty.SelectedItem & ")" & vbCrLf & cmboTopic.SelectedItem, MsgBoxStyle.YesNo)
            'If the data is approved by the user...
            If Result = MsgBoxResult.Yes Then
                'Builds SQL query to execute
                sql = "INSERT INTO `cl51-ben`.`tblconnect4` (`GameName`, `HostUsername`, `CurrentPlayer`, `C4NSubject`, `C4NDifficulty`, `C4NTopic`) VALUES ('" & txtGameName.Text & "', '" & LoggedInStudent.Username & "', 'Red', '" & SubjectID & "', '" & cmboDifficulty.SelectedItem & "', '" & cmboTopic.SelectedItem & "');"

                dbcomm = New MySqlCommand(sql, DBConn)
                dbread = dbcomm.ExecuteReader()
                dbread.Close()
            End If
                'If any invalid data entry is detected...
            Else
                'Displays a message box with any detected invalid data entry
                MsgBox("Invalid input:" & vbCrLf & vbCrLf & Errors)
        End If
    End Sub

    'Subroutine runs when called in form load subroutine
    Sub Populate()
        Dim sql As String
        Dim dbcomm As MySqlCommand
        Dim dbread As MySqlDataReader

        'Builds SQL query to execute
        sql = "SELECT * FROM tblsubjects"

        dbcomm = New MySqlCommand(sql, DBConn)
        dbread = dbcomm.ExecuteReader()
        'Fills the various properties of the LoggedInStudent object with their respective values from the database
        While dbread.Read
            cmboSubject.Items.Add(dbread("Subject"))
        End While
        dbread.Close()
    End Sub

    'Subroutine runs when the selected item in the subject combobox is changed
    Private Sub cmboSubject_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmboSubject.SelectedIndexChanged
        'Declares the StreamReader used to read the topic text file
        Dim Reader As System.IO.StreamReader

        'Enables the topic combobox and clears it of any data
        cmboTopic.Enabled = True
        cmboTopic.Items.Clear()

        'Sets the path to where the file is
        Reader = My.Computer.FileSystem.OpenTextFileReader(cmboSubject.SelectedItem & "Topics.txt")
        'Whilst not at the end of the text file...
        While Not Reader.EndOfStream
            'Add the topic to the combobox
            cmboTopic.Items.Add(Reader.ReadLine)
        End While

        'Closes the streamreader
        Reader.Close()

        Dim sql As String
        Dim dbcomm As MySqlCommand
        Dim dbread As MySqlDataReader

        'Builds SQL query to execute
        sql = "SELECT * FROM tblsubjects WHERE Subject='" & cmboSubject.SelectedItem & "'"

        dbcomm = New MySqlCommand(sql, DBConn)
        dbread = dbcomm.ExecuteReader()
        'Fills the various properties of the LoggedInStudent object with their respective values from the database
        While dbread.Read
            SubjectID = dbread("SubjectID")
        End While
        dbread.Close()
    End Sub
End Class