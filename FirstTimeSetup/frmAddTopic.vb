Imports MySql.Data.MySqlClient

Public Class frmAddTopic

    'Subroutine runs when the form loads
    Private Sub frmAddTopic_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Establishes the connection to the database
        OpenDB()

        'Runs the form population subroutine
        Populate()
    End Sub

    'Subroutine runs when the add topic button is clicked
    Private Sub btnAddTopic_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddTopic.Click
        'Runs the form population subroutine
        CheckValid()
    End Sub

    'Subroutine runs when called in the form load sub
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
            'Populates the subject combobox with data
            cmboSubject.Items.Add(dbread("Subject"))
        End While
        dbread.Close()
    End Sub

    'Subroutine runs when called in the add topic button sub
    Sub CheckValid()
        'Declares the variable used for getting the result of the message box
        Dim Result As MsgBoxResult
        'Declares the variable used for detecting invalid data entry
        Dim Errors As String = ""

        'Assembles an error report if any invalid data entry detected
        If cmboSubject.Text = "" Then
            Errors = Errors & "No subject selected" & vbCrLf
        End If
        If cmboYear.Text = "" Then
            Errors = Errors & "No year selected" & vbCrLf
        End If
        If txtTopic.Text = "" Then
            Errors = Errors & "No topic input" & vbCrLf
        End If

        'If no invalid data entry is detected...
        If Errors = "" Then
            'Displays a validation message box before saving the data to the database
            Result = MsgBox("Are you sure all these details are correct? Remember, spelling is vital." & vbCrLf & vbCrLf & "Details:" & vbCrLf & cmboSubject.SelectedItem & " (" & cmboYear.SelectedItem & ")" & vbCrLf & txtTopic.Text, MsgBoxStyle.YesNo)
            'If the data is approved by the user...
            If Result = MsgBoxResult.Yes Then
                'Declares the variable used for writing to the text file
                Dim writer As System.IO.StreamWriter
                'Gets the filepath to the selected subject's topic text file, creating it if it doesn't exist
                writer = My.Computer.FileSystem.OpenTextFileWriter(cmboSubject.Text & "Topics.txt", True)
                'Adds the new topic to the selected subject's topics text file
                writer.WriteLine(txtTopic.Text & " (" & cmboYear.Text & ")")
                'Saves the text file
                writer.Close()
            End If
            'If any invalid data entry is detected... 
        Else
            'Displays a message box with any detected invalid data entry
            MsgBox("Invalid input:" & vbCrLf & vbCrLf & Errors)
        End If
    End Sub

End Class