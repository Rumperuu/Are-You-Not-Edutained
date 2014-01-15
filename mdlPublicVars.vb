'Add a reference to COM Microsoft ActiveX Data Objects 6.1
Imports MySql.Data.MySqlClient

Module mdlPublicVars

    '//Connect Four\\
    'Declares the variable used for storing the current player of a game of Connect Four
    Public C4Player As String
    'Declares the variables used for determining the questions to display in a game of Connect Four
    Public C4HSubject, C4HDifficulty, C4HTopic As String

    '//Noughts and Crosses\\
    'Declares the variable used for storing the current player of a game of Noughts and Crosses
    Public NaCPlayer As String
    'Declares the variables used for determining the questions to display in a game of Noughts and Crosses
    Public NaCSubject, NaCDifficulty, NaCTopic As String

    '//Rock, Paper, Scissors\\
    'Declares the variable used for storing the current player of a game of Rock, Paper, Scissors
    Public RPSPlayer As String
    'Declares the variables used for determining the questions to display in a game of Rock, Paper, Scissors
    Public RPSSubject, RPSDifficulty, RPSTopic As String
    'Declares the variables used to store the selected weapons of each player
    Public LoggedInWeapon, OppWeapon As Integer

    '//General\\

    '/General\

    'Declares the variable used for storing the path to the database
    Public DBPath As String
    'Declares the variable used to connect to mySQL database
    Public DBConn As MySqlConnection
    'Subroutine runs when called
    Public Sub OpenDB()
        Try
            'Builds the connection string for the database
            DBConn = New MySqlConnection
            DBConn.ConnectionString = "STUFF"
            DBConn.Open()
        Catch ex As Exception
            MsgBox("Database is borked")
        End Try
    End Sub


    'Declares the variable used for storing the GameID for network play
    Public GameID As String

    '/Students\

    'Declares the variables used for storing the details of both players
    Public FnameStudent, LnameStudent, FnameOppStudent, LnameOppStudent, Form, ImageStudentLoc, ImageOppStudentLoc As String
    'Declares the variable used for storing the StudentID of a player
    Public StudentID As Integer
    'Declares the class used for both players
    Public Class Student
        'Declares the variables used for storing the details of the student
        Public Fname, Lname, Form, Username, C4Player, NaCPlayer As String
        Public StudentID, Wins, Losses, Draws, RPSPlayer As Integer
    End Class
    'Creates two objects of the student class
    Public LoggedInStudent As New Student
    Public OppStudent As New Student
    'Used to populate the Student Account form with the correct data
    Public Viewing As Integer = 1

    '/Teachers\

    'Declares the variables used for storing the details of the teacher
    Public FnameTeacher, LnameTeacher, imageteacherloc As String
    'Declares the variable used for storing the TeacherID
    Public TeacherID As Integer
    'Declares the class used for the teacher
    Public Class Teacher
        'Declares the variables used for storing the details of the teacher
        Public Fname, Lname, Username As String
        Public TeacherID As Integer
    End Class
    'Creates an object of the teacher class
    Public LoggedInTeacher As New Teacher

End Module

