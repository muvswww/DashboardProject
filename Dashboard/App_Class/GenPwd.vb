Option Explicit On
Option Strict On

Imports Microsoft.VisualBasic

'Imports System.Data
'Imports System.Data.SqlClient
'Imports System.Web.Configuration
'Imports System.Threading
'Imports System.Security.Cryptography

'Imports System.IO
'Imports System.Net
'Imports System.Web.Script.Serialization

Public Class GenPwd

    Public Shared Function GeneratePassword(ByVal length As Integer, ByVal numberOfNonAlphanumericCharacters As Integer) As String
        'http://www.4guysfromrolla.com/articles/101205-1.aspx

        'Make sure length and numberOfNonAlphanumericCharacters are valid....
        If ((length < 1) OrElse (length > 128)) Then
            Throw New ArgumentException("Membership_password_length_incorrect")
        End If

        If ((numberOfNonAlphanumericCharacters > length) OrElse (numberOfNonAlphanumericCharacters < 0)) Then
            Throw New ArgumentException("Membership_min_required_non_alphanumeric_characters_incorrect")
        End If

        Do While True
            Dim i As Integer
            Dim nonANcount As Integer = 0
            Dim buffer1 As Byte() = New Byte(length - 1) {}

            'chPassword contains the password's characters as it's built up
            Dim chPassword As Char() = New Char(length - 1) {}

            'chPunctionations contains the list of legal non-alphanumeric characters
            'Dim chPunctuations As Char() = "!@@$%^^*()_-+=[{]};:>|./?".ToCharArray() 'array มี 25 ตัว ?? ทำไม ไม่รู้ error
            Dim chPunctuations As Char() = "1234567890@$%1234567890@$".ToCharArray()

            'Get a cryptographically strong series of bytes
            Dim rng As New System.Security.Cryptography.RNGCryptoServiceProvider
            rng.GetBytes(buffer1)

            For i = 0 To length - 1
                'Convert each byte into its representative character
                Dim rndChr As Integer = (buffer1(i) Mod 87)
                If (rndChr < 10) Then
                    chPassword(i) = Convert.ToChar(Convert.ToUInt16(48 + rndChr))
                Else
                    If (rndChr < 36) Then
                        chPassword(i) = Convert.ToChar(Convert.ToUInt16((65 + rndChr) - 10))
                    Else
                        If (rndChr < 62) Then
                            chPassword(i) = Convert.ToChar(Convert.ToUInt16((97 + rndChr) - 36))
                        Else
                            Try
                                chPassword(i) = chPunctuations(rndChr - 62)
                                nonANcount += 1
                            Catch ex As Exception
                                'Dim message As String = String.Format("Message: {0}\n\n", ex.Message)
                                'message &= String.Format("StackTrace: {0}\n\n", ex.StackTrace.Replace(Environment.NewLine, String.Empty))
                                'message &= String.Format("Source: {0}\n\n", ex.Source.Replace(Environment.NewLine, String.Empty))
                                'message &= String.Format("TargetSite: {0}", ex.TargetSite.ToString().Replace(Environment.NewLine, String.Empty))
                                'ClientScript.RegisterStartupScript(Me.GetType(), "alert", "alert(""" & message & """);", True)
                            End Try
                        End If
                    End If
                End If
            Next

            If nonANcount < numberOfNonAlphanumericCharacters Then
                Dim rndNumber As New Random
                For i = 0 To (numberOfNonAlphanumericCharacters - nonANcount) - 1
                    Dim passwordPos As Integer
                    Do
                        passwordPos = rndNumber.Next(0, length)
                    Loop While Not Char.IsLetterOrDigit(chPassword(passwordPos))
                    chPassword(passwordPos) = chPunctuations(rndNumber.Next(0, chPunctuations.Length))
                Next
            End If

            Return New String(chPassword)
        Loop

        Return Nothing
    End Function

End Class
