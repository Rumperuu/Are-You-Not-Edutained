﻿Imports MySql.Data.MySqlClient

Public Class frmAddTeacher

    'Declares the variable used for generating the username of the new student
    Dim Uname As String
    'Declares the variable used for getting the image of the new student
    Dim Filepath As String

    'Subroutine runs when the form loads
    Private Sub frmAddTeacher_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Establishes the connection to the database
        OpenDB()

        'Indicates that no picture has been selected
        picPic.Tag = 0
    End Sub

    'Subroutine runs when the teacher first name text box is clicked
    Private Sub txtTeacherFirstName_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtTeacherFirstName.Click
        'Blanks out the text box
        txtTeacherFirstName.Text = ""
    End Sub

    'Subroutine runs when the teacher last name text box is clicked
    Private Sub txtTeacherLastName_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtTeacherLastName.Click
        'Blanks out the text box
        txtTeacherLastName.Text = ""
    End Sub

    'Subroutine runs when the add teacher button is clicked
    Private Sub btnAddTeacher_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddTeacher.Click
        'Declares the variable used for getting the result of the message box
        Dim Result As MsgBoxResult
        'Declares the variable used for detecting invalid data entry
        Dim Errors As String = ""

        'Assembles an error report if any invalid data entry detected
        If txtTeacherFirstName.Text = "" Then
            Errors = Errors & "No first name input" & vbCrLf
        End If
        If txtTeacherLastName.Text = "" Then
            Errors = Errors & "No last name input" & vbCrLf
        End If
        If picPic.Tag = 0 Then
            Errors = Errors & "No picture input" & vbCrLf
        End If


        Dim sql As String
        Dim dbcomm As MySqlCommand
        Dim dbread As MySqlDataReader

        'If no invalid data entry is detected...
        If Errors = "" Then
            'Runs the username generation subroutine
            Username()
            'Displays a validation message box before saving the data to the database
            Result = MsgBox("Are you sure all these details are correct? Remember, spelling is vital." & vbCrLf & vbCrLf & "Details:" & vbCrLf & txtTeacherFirstName.Text & " " & txtTeacherLastName.Text, MsgBoxStyle.YesNo)
            'If the data is approved by the user...
            If Result = MsgBoxResult.Yes Then
                'Builds SQL query to execute
                sql = "INSERT INTO `cl51-ben`.`tblteachers` (`Fname`, `Lname`, `Username`, `Password`) VALUES ('" & txtTeacherFirstName.Text & "', '" & txtTeacherLastName.Text & "', '" & Uname & "', 'password');"

                dbcomm = New MySqlCommand(sql, DBConn)
                dbread = dbcomm.ExecuteReader()
                dbread.Close()

                'Creates the question creation text file for the teacher
                Dim file As System.IO.FileStream
                file = System.IO.File.Create(Uname & ".txt")

                'Copies the image to the folder for student images and renames it to the new student's username
                My.Computer.FileSystem.CopyFile(Filepath, Uname & ".jpg", FileIO.UIOption.AllDialogs, FileIO.UICancelOption.DoNothing)
            End If
            'If any invalid data entry is detected...
        Else
            'Displays a message box with any detected invalid data entry
            MsgBox("Invalid input:" & vbCrLf & vbCrLf & Errors)
        End If
    End Sub

    'Subroutine runs when called in the add teacher button sub
    Sub Username()

        'Declares the variable used to get the first letter of the new teacher's first name
        Dim L1 As String = Mid(txtTeacherFirstName.Text, 1, 1)
        'Declares the variable used to get the first letter of the new teacher's last name
        Dim L2 As String = Mid(txtTeacherLastName.Text, 1, 1)
        'Declares the variable used to get the numbers at the end of the new teacher's username
        Dim Numbers As Integer = 11
        Dim sql As String
        Dim dbcomm As MySqlCommand
        Dim dbread As MySqlDataReader
        'Assembles the beginning and middle of the username
        Uname = "st" & L1 & L2

        'Builds SQL query to execute
        sql = "SELECT * FROM tblteachers WHERE Username='" & Uname & Numbers & "'"

        dbcomm = New MySqlCommand(sql, DBConn)
        dbread = dbcomm.ExecuteReader()
        'Fills the various properties of the LoggedInStudent object with their respective values from the database
        While dbread.Read
            dbread.Close()
            'Increases the numbers on the end of the username
            Numbers = Numbers + 1
            'Opens a new recordset to check if the new username isn't taken
            sql = "SELECT * FROM tblteachers WHERE Username='" & Uname & Numbers & "'"
            dbcomm = New MySqlCommand(sql, DBConn)
            dbread = dbcomm.ExecuteReader()
        End While
        dbread.Close()
        'Assembles the username
        Uname = Uname & Numbers
    End Sub

    'Subroutine runs when the browse pic button is clicked
    Private Sub btnBrowsePic_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowsePic.Click
        'Sets the filter of the browse window to only allow image files
        diaPic.Filter = "Image Files|*.jpg;*.gif;*.bmp;*.png;*.jpeg|All Files|*.*"
        'Sets the starting directory of the browse window to the C: drive
        diaPic.InitialDirectory = "C:\"
        'Sets the filter index of the browse window
        diaPic.FilterIndex = 1
        'Sets the title of the browse window
        diaPic.Title = "Open File"
        'If okay button of browse window is clicked...
        If (diaPic.ShowDialog() = Windows.Forms.DialogResult.OK) Then
            'Sets filepath to image
            Filepath = diaPic.FileName
            'Sets displayed image to selected image file
            picPic.Image = Image.FromFile(Filepath)
            'Sets picture box tag to indicate that a picture has been selected
            picPic.Tag = 1
        End If
    End Sub

End Class