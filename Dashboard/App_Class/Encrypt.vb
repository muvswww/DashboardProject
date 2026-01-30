Imports Microsoft.VisualBasic
Imports System.Security.Cryptography
Imports System.IO

Public Class Encrypt
    Private Const cryptoKey As String = "czsqO+DxnA1EcyurkKdllA=="
    Private Shared ReadOnly IV As Byte() = New Byte(7) {240, 3, 45, 29, 0, 76, _
     173, 59}

    Public Shared Function EncryptMD5(ByVal strString As String) As String
        Dim ASCIIenc As New ASCIIEncoding
        Dim strReturn As String
        Dim ByteSourceText() As Byte = ASCIIenc.GetBytes(strString)
        Dim Md5Hash As New MD5CryptoServiceProvider
        Dim ByteHash() As Byte = Md5Hash.ComputeHash(ByteSourceText)

        strReturn = ""

        For Each b As Byte In ByteHash
            strReturn = strReturn & b.ToString("x2")
        Next
        Return strReturn
    End Function

    Public Shared Function Encrypt1(ByVal s As String) As String
        If s Is Nothing OrElse s.Length = 0 Then
            Return String.Empty
        End If
        Dim result As String = String.Empty
        Try
            Dim buffer As Byte() = Encoding.[Default].GetBytes(s)
            Dim des As New TripleDESCryptoServiceProvider()
            Dim MD5 As New MD5CryptoServiceProvider()

            des.Key = MD5.ComputeHash(ASCIIEncoding.UTF32.GetBytes("aerwegdfgh"))
            des.IV = IV
            result = Convert.ToBase64String(des.CreateEncryptor().TransformFinalBlock(buffer, 0, buffer.Length))
        Catch ex As Exception
            Throw ex
        End Try
        Return result
    End Function
    Public Shared Function Decrypt1(ByVal s As String) As String
        If s Is Nothing OrElse s.Length = 0 Then
            Return String.Empty
        End If
        Dim result As String = String.Empty
        Try
            Dim buffer As Byte() = Convert.FromBase64String(s)

            Dim des As New TripleDESCryptoServiceProvider()
            Dim MD5 As New MD5CryptoServiceProvider()
            des.Key = MD5.ComputeHash(ASCIIEncoding.UTF32.GetBytes("aerwegdfgh"))
            des.IV = IV
            result = Encoding.[Default].GetString(des.CreateDecryptor().TransformFinalBlock(buffer, 0, buffer.Length))
        Catch ex As Exception
            Throw ex
        End Try

        Return result
    End Function

    Public Shared Function Encrypt2(ByVal s As String) As String
        If s Is Nothing OrElse s.Length = 0 Then
            Return String.Empty
        End If
        Dim result As String = String.Empty
        Try
            Dim buffer As Byte() = Encoding.[Default].GetBytes(s)
            Dim des As New TripleDESCryptoServiceProvider()
            Dim MD5 As New MD5CryptoServiceProvider()

            des.Key = MD5.ComputeHash(ASCIIEncoding.UTF32.GetBytes(cryptoKey))
            des.IV = IV
            result = Convert.ToBase64String(des.CreateEncryptor().TransformFinalBlock(buffer, 0, buffer.Length))
        Catch ex As Exception
            Throw ex
        End Try
        Return result
    End Function

    Public Shared Function Decrypt2(ByVal s As String) As String
        If s Is Nothing OrElse s.Length = 0 Then
            Return String.Empty
        End If
        Dim result As String = String.Empty
        Try
            Dim buffer As Byte() = Convert.FromBase64String(s)

            Dim des As New TripleDESCryptoServiceProvider()
            Dim MD5 As New MD5CryptoServiceProvider()
            des.Key = MD5.ComputeHash(ASCIIEncoding.UTF32.GetBytes(cryptoKey))
            des.IV = IV
            result = Encoding.[Default].GetString(des.CreateDecryptor().TransformFinalBlock(buffer, 0, buffer.Length))
        Catch ex As Exception
            Throw ex
        End Try

        Return result
    End Function

    Public Shared Function EncryptAES(clearText As String) As String
        Dim EncryptionKey As String = "/kjz#dfh}iorghj_-{hseb@flskjo8547t6t^*IObnd!fgop9"
        Dim clearBytes As Byte() = Encoding.Unicode.GetBytes(clearText)
        Using encryptor As Aes = Aes.Create()
            Dim pdb As New Rfc2898DeriveBytes(EncryptionKey, New Byte() {&H49, &H76, &H61, &H6E, &H20, &H4D, _
             &H65, &H64, &H76, &H65, &H64, &H65, _
             &H76})
            encryptor.Key = pdb.GetBytes(32)
            encryptor.IV = pdb.GetBytes(16)
            Using ms As New MemoryStream()
                Using cs As New CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write)
                    cs.Write(clearBytes, 0, clearBytes.Length)
                    cs.Close()
                End Using
                clearText = Convert.ToBase64String(ms.ToArray())
            End Using
        End Using
        Return clearText
    End Function

    Public Shared Function DecryptAES(cipherText As String) As String
        Dim EncryptionKey As String = "/kjz#dfh}iorghj_-{hseb@flskjo8547t6t^*IObnd!fgop9"
        Dim cipherBytes As Byte() = Convert.FromBase64String(cipherText)
        Using encryptor As Aes = Aes.Create()
            Dim pdb As New Rfc2898DeriveBytes(EncryptionKey, New Byte() {&H49, &H76, &H61, &H6E, &H20, &H4D, _
             &H65, &H64, &H76, &H65, &H64, &H65, _
             &H76})
            encryptor.Key = pdb.GetBytes(32)
            encryptor.IV = pdb.GetBytes(16)
            Using ms As New MemoryStream()
                Using cs As New CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write)
                    cs.Write(cipherBytes, 0, cipherBytes.Length)
                    cs.Close()
                End Using
                cipherText = Encoding.Unicode.GetString(ms.ToArray())
            End Using
        End Using
        Return cipherText
    End Function

End Class
