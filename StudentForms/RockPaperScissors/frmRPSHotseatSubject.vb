Imports MySql.Data.MySqlClient

Public Class frmRPSHotseatSubject
    'Declares the variable used to store the chosen SubjectID
    Dim SubjectID As Integer

    'Subroutine runs on form load
    Private Sub frmNaCHotseatSubject_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Runs the OpenDB subroutine
        OpenDB()

        'Runs the Populate subroutine
        Populate()
    End Sub

    'Subroutine runs then the okay button is clicked
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        'If everything has been selected...
        If (cmboDifficulty.SelectedItem <> "") And (cmboSubject.SelectedItem <> "") And (cmboTopic.SelectedItem <> "") Then

            'Sets the chosen subject, difficulty and topic variables
            RPSSubject = SubjectID
            RPSDifficulty = cmboDifficulty.SelectedItem
            RPSTopic = cmboTopic.SelectedItem

            'Shows the Connect Four hotseat player selection form
            frmRPSHotseat.Show()
            'Closes this form
            Me.Close()
        Else
            'Otherwise display an error message
            MsgBox("Incorrect values")
        End If
    End Sub

    'Subroutine runs when called at form load
    Sub Populate()
        Dim sql As String
        Dim dbcomm As MySqlCommand
        Dim dbread As MySqlDataReader

        sql = "SELECT * FROM tblsubjects;"
        dbcomm = New MySqlCommand(sql, DBConn)
        dbread = dbcomm.ExecuteReader()
        While dbread.Read
            cmboSubject.Items.Add(dbread("Subject"))
        End While
        dbread.Close()
    End Sub

    'Subroutine runs when the selected item of the subject combobox is changed
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

        sql = "SELECT * FROM tblsubjects WHERE Subject='" & cmboSubject.SelectedItem & "';"
        dbcomm = New MySqlCommand(sql, DBConn)
        dbread = dbcomm.ExecuteReader()
        While dbread.Read
            SubjectID = dbread("SubjectID")
        End While
        dbread.Close()
    End Sub

    'Subroutine runs when the cancel button is clicked
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        'Shows the Rock, Paper, Scissors hotseat login form
        frmRPSHotseatLogin.Show()
        'Closes this form
        Me.Close()
    End Sub

End Class