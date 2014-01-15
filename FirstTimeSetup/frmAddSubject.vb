Imports MySql.Data.MySqlClient

Public Class frmAddSubject

    'Subroutine runs when the form loads
    Private Sub frmAddSubject_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Establishes the connection to the database
        OpenDB()
    End Sub

    'Subroutine runs when the add subject button is clicked
    Private Sub btnAddSubject_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddSubject.Click
        'Declares the variable used for detecting invalid data entry
        Dim Errors As String = ""
        'Declares the variable used for getting the result of the message box
        Dim Result As MsgBoxResult
        Dim sql As String
        Dim dbcomm As MySqlCommand
        Dim dbread As MySqlDataReader

        'Assembles an error report if any invalid data entry detected
        If txtSubjectName.Text = "" Then
            Errors = Errors & "No subject name input" & vbCrLf
        End If

        'If no invalid data entry is detected...
        If Errors = "" Then
            'Displays a validation message box before saving the data to the database
            Result = MsgBox("Are you sure all these details are correct? Remember, spelling is vital." & vbCrLf & vbCrLf & "Details:" & vbCrLf & txtSubjectName.Text, MsgBoxStyle.YesNo)
            'If the data is approved by the user...
            If Result = MsgBoxResult.Yes Then
                'Builds SQL query to execute
                sql = "INSERT INTO `cl51-ben`.`tblsubjects` (`Subject`) VALUES ('" & txtSubjectName.Text & "');"

                dbcomm = New MySqlCommand(sql, DBConn)
                dbread = dbcomm.ExecuteReader()
                dbread.Close()

                'Creates the topics text file for the subject
                Dim file As System.IO.FileStream
                file = System.IO.File.Create(txtSubjectName.Text & "Topics.txt")
                file.Close()
            End If
            'If any invalid data entry is detected...
        Else
            'Displays a message box with any detected invalid data entry
            MsgBox("Invalid input:" & vbCrLf & vbCrLf & Errors)
        End If
    End Sub

    'Subroutine runs when the subject name text box is clicked
    Private Sub txtSubjectName_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSubjectName.Click
        'Blanks out the text box
        txtSubjectName.Text = ""
    End Sub

End Class