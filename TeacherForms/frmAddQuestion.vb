Imports MySql.Data.MySqlClient

Public Class frmAddQuestion

    'Declares the variable used for storing the SubjectID
    Dim SubjectID As Integer

    'Subroutine runs when the form loads
    Private Sub frmAddQuestion_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Establishes the connection to the database
        OpenDB()

        'Runs the form population subroutine
        Populate()
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

    'Subroutine runs when the back button is clicked
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        'Closes the form
        Me.Close()
    End Sub

    'Subroutine runs when the selected index in the subject combobox changes
    Private Sub cmboSubject_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmboSubject.SelectedIndexChanged
        'Declares the varibale used to read the topics text file
        Dim Reader As System.IO.StreamReader

        'Enables the topics combobox
        cmboTopic.Enabled = True
        cmboTopic.Items.Clear()

        'Gets the path to the selected subject's topics text file
        Reader = My.Computer.FileSystem.OpenTextFileReader(cmboSubject.SelectedItem & "Topics.txt")
        'Whilst the end of the text file hasn't been reacher...
        While Not Reader.EndOfStream
            'Adds an item to the topics combobox
            cmboTopic.Items.Add(Reader.ReadLine)
        End While

        'Closes the recordset
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
            'Populates the subject combobox with data
            SubjectID = dbread("SubjectID")
        End While
        dbread.Close()
    End Sub

    'Subroutine runs when the question textbox is clicked
    Private Sub txtQuestion_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtQuestion.Click
        'Blanks out the textbox
        txtQuestion.Text = ""
    End Sub

    'Subroutine runs when the answer textbox is clicked
    Private Sub txtAnswer_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAnswer.Click
        'Blanks out the textbox
        txtAnswer.Text = ""
    End Sub

    'Subroutine runs when the submit button is clicked
    Private Sub btnSubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        'Declares the variable used for getting the result of the message box
        Dim Result As MsgBoxResult
        'Declares the variable used for detecting invalid data entry
        Dim Errors As String = ""
        'Assembles an error report if any invalid data entry detected
        If cmboSubject.Text = "" Then
            Errors = Errors & "No subject selected" & vbCrLf
        End If
        If cmboDifficulty.Text = "" Then
            Errors = Errors & "No difficulty selected" & vbCrLf
        End If
        If cmboTopic.Text = "" Then
            Errors = Errors & "No topic selected" & vbCrLf
        End If
        If txtQuestion.Text = "" Then
            Errors = Errors & "No question input" & vbCrLf
        End If
        If txtAnswer.Text = "" Then
            Errors = Errors & "No answer input" & vbCrLf
        End If


        Dim sql As String
        Dim dbcomm As MySqlCommand
        Dim dbread As MySqlDataReader

        'If no invalid data entry is detected...
        If Errors = "" Then
            'Displays a validation message box before saving the data to the database
            Result = MsgBox("Are you sure all these details are correct? Remember, spelling is vital." & vbCrLf & vbCrLf & "Details:" & vbCrLf & cmboDifficulty.SelectedItem & " " & cmboSubject.SelectedItem & vbCrLf & cmboTopic.SelectedItem & vbCrLf & vbCrLf & "Question: ''" & txtQuestion.Text & "''" & vbCrLf & "Answer: ''" & txtAnswer.Text & "''", MsgBoxStyle.YesNo)
            'If the data is approved by the user...
            If Result = MsgBoxResult.Yes Then
                'Builds SQL query to execute
                sql = "INSERT INTO `cl51-ben`.`tblquestions` (`SubjectID`, `TeacherID`, `Question`, `Answer`, `Difficulty`, `Topic`) VALUES ('" & SubjectID & "', '" & LoggedInTeacher.TeacherID & "', '" & txtQuestion.Text & "', '" & txtAnswer.Text & "', '" & cmboDifficulty.SelectedItem & "', '" & cmboTopic.SelectedItem & "');"

                dbcomm = New MySqlCommand(sql, DBConn)
                dbread = dbcomm.ExecuteReader()
                dbread.Close()
            End If
            'If any invalid data entry is detected...
        Else
            'Displays a message box with any detected invalid data entry
            MsgBox("Invalid input:" & vbCrLf & vbCrLf & Errors)
        End If

        'Declares the variable used for writing to the text file
        Dim writer As System.IO.StreamWriter
        'Gets the filepath to the logged-in teacher's question creation log text file
        writer = My.Computer.FileSystem.OpenTextFileWriter(LoggedInTeacher.Username & ".txt", True)
        'Adds the log of the question creation to the logged-in teacher's log file
        writer.WriteLine(LoggedInTeacher.Fname & " " & LoggedInTeacher.Lname & " created a question for " & cmboDifficulty.SelectedItem & " " & cmboSubject.SelectedItem & " (''" & txtQuestion.Text & "'') - " & TimeOfDay & " " & DateValue(Now))
        'Saves the log file
        writer.Close()
    End Sub

End Class