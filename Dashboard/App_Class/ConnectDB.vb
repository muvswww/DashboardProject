Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration

Imports Dashboard.Encrypt
Imports System.Web.Services.Protocols

Public Class ConnectDB
    Private Shared objConn As SqlConnection
    Private Shared objCmd As SqlCommand
    Private Shared Trans As SqlTransaction

    Public Shared dbConn_leave As String = Replace(ConfigurationManager.ConnectionStrings("dbConn_leave").ConnectionString, "password", Decrypt2("2fxKF+rsggSR+BM25c3IJLWBqS1Pu4Y5"))
    Public Shared dbConn_itjobs As String = Replace(ConfigurationManager.ConnectionStrings("dbConn_itjobs").ConnectionString, "password", Decrypt2("2fxKF+rsggSR+BM25c3IJLWBqS1Pu4Y5"))

    '******* Connection แบบแยกเป็น 3 ส่วน *******
    Public Shared dbConn As String = ConfigurationManager.ConnectionStrings("PreFixConn").ConnectionString & _
            Decrypt2(ConfigurationManager.ConnectionStrings("PwdConn").ConnectionString) & _
            ConfigurationManager.ConnectionStrings("PostFixConn").ConnectionString
    '******* Connection แบบแยกเป็น 3 ส่วน *******

    Public Shared dbConn_Surveys As String = Replace(ConfigurationManager.ConnectionStrings("dbConn_Surveys").ConnectionString, "password", Decrypt2("2fxKF+rsggSR+BM25c3IJLWBqS1Pu4Y5"))

    Public Shared dbConnVSAPP As String = ConfigurationManager.ConnectionStrings("PreFixConn").ConnectionString & _
            Decrypt2(ConfigurationManager.ConnectionStrings("PwdConn").ConnectionString) & _
            ConfigurationManager.ConnectionStrings("PostFixConn").ConnectionString


    Public Shared dbConn_VSAPP As String = Replace(ConfigurationManager.ConnectionStrings("dbConn_VSAPP").ConnectionString, "password", Decrypt2("2fxKF+rsggSR+BM25c3IJLWBqS1Pu4Y5"))

    'Public Shared Function QueryDataReader(ByVal strSQL As String, ByVal strConnString As String, ByVal table As String) As SqlDataReader
    '    Dim dtReader As SqlDataReader
    '    objConn = New SqlConnection
    '    With objConn
    '        .ConnectionString = Replace(strConnString, "table", table)
    '        .Open()
    '    End With
    '    objCmd = New SqlCommand(strSQL, objConn)
    '    dtReader = objCmd.ExecuteReader()
    '    Return dtReader '*** Return DataReader ***'
    'End Function

    Public Shared Function QueryDataReader(ByVal strSQL As String, ByVal strConnString As String, ByVal table As String, ByVal parameters As SqlParameter()) As SqlDataReader
        objConn = New SqlConnection
        With objConn
            .ConnectionString = Replace(strConnString, "table", table)
            .Open()
        End With
        Using objCmd As New SqlCommand(strSQL, objConn)
            If parameters IsNot Nothing AndAlso parameters.Length > 0 Then
                objCmd.Parameters.AddRange(parameters)
            End If
            Return objCmd.ExecuteReader()
        End Using
    End Function


    Public Shared Function QueryDataReader2(ByVal strSQL As String, ByVal strConnString As String, ByVal Table As String, ByVal parameters As SqlParameter()) As SqlDataReader
        'Using objConn As New SqlConnection(strConnString) <- connection มันปิดก่อน

        objConn = New SqlConnection

        With objConn
            .ConnectionString = Replace(strConnString, "table", Table)
            .Open()
        End With

        Using objCmd As New SqlCommand(strSQL, objConn)
            ' Add parameters to the command if they are provided
            If parameters IsNot Nothing AndAlso parameters.Length > 0 Then
                objCmd.Parameters.AddRange(parameters)
            End If

            Return objCmd.ExecuteReader()
        End Using
        'End Using
    End Function

    Public Function QueryDataSet(ByVal strSQL As String, ByVal strConnString As String) As DataSet
        Dim ds As New DataSet
        Dim dtAdapter As New SqlDataAdapter
        objConn = New SqlConnection
        With objConn
            .ConnectionString = strConnString
            .Open()
        End With
        objCmd = New SqlCommand
        With objCmd
            .Connection = objConn
            .CommandText = strSQL
            .CommandType = CommandType.Text
        End With
        dtAdapter.SelectCommand = objCmd
        dtAdapter.Fill(ds)
        Return ds '*** Return DataSet ***'
    End Function

    Public Shared Function GetDataTable(ByVal cmd As SqlCommand, ByVal strConnString As String, ByVal table As String) As DataTable
        Dim dt As New DataTable()
        objConn = New SqlConnection
        With objConn
            .ConnectionString = Replace(strConnString, "table", table)
            .Open()
        End With
        Dim dtAdapter As New SqlDataAdapter()
        cmd.CommandType = CommandType.Text
        cmd.Connection = objConn
        dtAdapter.SelectCommand = cmd
        dtAdapter.Fill(dt)
        Return dt

        objConn.Close()

    End Function

    Public Shared Function QueryDataTable2(ByVal strSQL As String, ByVal strConnString As String, ByVal database As String, ByVal parameters As SqlParameter()) As DataTable
        'Dim dt As New DataTable

        Using objConn As New SqlConnection(strConnString)

            With objConn
                .ConnectionString = Replace(strConnString, "database", database)
                .Open()
            End With

            Using cmd As New SqlCommand(strSQL, objConn)
                ' Add parameters to the SqlCommand if provided
                If parameters IsNot Nothing AndAlso parameters.Length > 0 Then
                    cmd.Parameters.AddRange(parameters)
                End If

                Using dtAdapter As New SqlDataAdapter(cmd)
                    Try
                        Dim dt As New DataTable
                        dtAdapter.Fill(dt)
                        Return dt ' Return DataTable
                    Catch ex As Exception
                        Dim message As String = String.Format("Message: {0}\n\n", ex.Message)
                        message &= String.Format("StackTrace: {0}\n\n", ex.StackTrace.Replace(Environment.NewLine, String.Empty))
                        message &= String.Format("Source: {0}\n\n", ex.Source.Replace(Environment.NewLine, String.Empty))
                        message &= String.Format("TargetSite: {0}", ex.TargetSite.ToString().Replace(Environment.NewLine, String.Empty))
                        strSQL = message
                        Dim err As New DataTable()
                        Return err
                    End Try
                End Using ' dtAdapter is automatically disposed of when the block exits
            End Using ' cmd is automatically disposed of when the block exits
        End Using ' objConn is automatically disposed of when the block exits

    End Function


    Public Shared Function QueryDataTable(ByRef strSQL As String, ByVal strConnString As String, ByVal Table As String) As DataTable
        Dim dtAdapter As SqlDataAdapter
        Dim dt As New DataTable
        objConn = New SqlConnection
        With objConn
            .ConnectionString = Replace(strConnString, "table", Table)
            .Open()
        End With

        'MsgBox(strSQL)

        dtAdapter = New SqlDataAdapter(strSQL, objConn)

        Try
            dtAdapter.Fill(dt)

            strSQL = Nothing
            Return dt '*** Return DataTable ***'

        Catch ex As Exception

            Dim message As String = String.Format("Message: {0}\n\n", ex.Message)
            message &= String.Format("StackTrace: {0}\n\n", ex.StackTrace.Replace(Environment.NewLine, String.Empty))
            message &= String.Format("Source: {0}\n\n", ex.Source.Replace(Environment.NewLine, String.Empty))
            message &= String.Format("TargetSite: {0}", ex.TargetSite.ToString().Replace(Environment.NewLine, String.Empty))

            ''https://stackoverflow.com/questions/3930941/how-to-show-exception-variable-value-in-alert-box-in-asp-net-using-c-sharp
            'Response.Write("<script>alert('" + Server.HtmlEncode(ex.Message) + "')</script>")

            strSQL = message

            Dim err As New Data.DataTable()

            Return err

            'Throw
            '****************** get err example ******************
            'If dt.Rows.Count > 0 Then
            '    Response.Write("<script>alert('" + CStr(dt.Rows.Count) + "')</script>")
            'Else
            '    If Not String.IsNullOrEmpty(SQLRN) Then
            '        Response.Write("<script>alert('" + Server.HtmlEncode(SQLRN) + "')</script>")

            '    Else
            '        Response.Write("<script>alert('No Record')</script>")
            '    End If
            'End If
            '****************** get err example ******************

        End Try

        objConn.Close()

    End Function

    Public Shared Function QueryExecuteNonQuery2(ByRef strSQL As String, ByVal strConnString As String, ByVal Table As String) As Boolean
        Try
            Using objConn As New SqlConnection(strConnString)
                With objConn
                    .ConnectionString = Replace(strConnString, "table", Table)
                    .Open()
                End With




                Using objCmd As New SqlCommand()

                    objCmd.Connection = objConn
                    objCmd.CommandType = CommandType.Text
                    objCmd.CommandText = strSQL
                    objCmd.ExecuteNonQuery()

                    Return True '*** Return True ***'
                End Using

            End Using

        Catch ex As Exception
            Dim message As String = String.Format("Message: {0}\n\n", ex.Message)
            message &= String.Format("StackTrace: {0}\n\n", ex.StackTrace.Replace(Environment.NewLine, String.Empty))
            message &= String.Format("Source: {0}\n\n", ex.Source.Replace(Environment.NewLine, String.Empty))
            message &= String.Format("TargetSite: {0}", ex.TargetSite.ToString().Replace(Environment.NewLine, String.Empty))

            'https://stackoverflow.com/questions/3930941/how-to-show-exception-variable-value-in-alert-box-in-asp-net-using-c-sharp
            'Response.Write("<script>alert('" + Server.HtmlEncode(ex.Message) + "')</script>")

            strSQL = message

            Return False '*** Return False ***'
        Finally
            Close_objConn()
        End Try
    End Function

    Public Shared Function QueryExecuteNonQuery3(ByVal strSQL As String,
                                             ByVal strConnString As String,
                                             ByVal Table As String,
                                             Optional ByVal parameters As SqlParameter() = Nothing,
                                             Optional ByRef errorMessage As String = "") As Boolean
        Try
            ' ปรับค่าของ strConnString ให้แทน {table} ด้วยชื่อ Table ที่กำหนด
            Dim finalConnString As String = strConnString.Replace("{table}", Table)

            Using objConn As New SqlConnection(finalConnString)
                objConn.Open()

                Using objCmd As New SqlCommand(strSQL, objConn)
                    objCmd.CommandType = CommandType.Text

                    ' Add parameters to the command if they are provided
                    If parameters IsNot Nothing AndAlso parameters.Length > 0 Then
                        objCmd.Parameters.AddRange(parameters)
                    End If

                    objCmd.ExecuteNonQuery()
                    Return True '*** สำเร็จ ***'
                End Using
            End Using

        Catch ex As Exception
            Dim sb As New Text.StringBuilder()
            sb.AppendLine("Message: " & ex.Message)
            sb.AppendLine("Source: " & ex.Source)
            sb.AppendLine("StackTrace: " & ex.StackTrace)
            sb.AppendLine("TargetSite: " & ex.TargetSite.ToString())
            errorMessage = sb.ToString()

            Return False '*** เกิดข้อผิดพลาด ***'
        End Try
    End Function
    Public Shared Function QueryExecuteNonQuery(ByRef strSQL As String, ByVal strConnString As String) As Boolean
        objConn = New SqlConnection
        With objConn
            .ConnectionString = strConnString
            .Open()
        End With

        Try
            objCmd = New SqlCommand
            With objCmd
                .Connection = objConn
                .CommandType = CommandType.Text
                .CommandText = strSQL
            End With
            objCmd.ExecuteNonQuery()
            Return True '*** Return True ***'

        Catch ex As Exception
            Dim message As String = String.Format("Message: {0}\n\n", ex.Message)
            message &= String.Format("StackTrace: {0}\n\n", ex.StackTrace.Replace(Environment.NewLine, String.Empty))
            message &= String.Format("Source: {0}\n\n", ex.Source.Replace(Environment.NewLine, String.Empty))
            message &= String.Format("TargetSite: {0}", ex.TargetSite.ToString().Replace(Environment.NewLine, String.Empty))

            'https://stackoverflow.com/questions/3930941/how-to-show-exception-variable-value-in-alert-box-in-asp-net-using-c-sharp
            'Response.Write("<script>alert('" + Server.HtmlEncode(ex.Message) + "')</script>")

            strSQL = message

            Return False '*** Return False ***'

        End Try
    End Function

    Public Function QueryExecuteScalar(ByVal strSQL As String, ByVal strConnString As String) As Object
        Dim obj As Object
        objConn = New SqlConnection
        With objConn
            .ConnectionString = strConnString
            .Open()
        End With
        Try
            objCmd = New SqlCommand
            With objCmd
                .Connection = objConn
                .CommandType = CommandType.Text
                .CommandText = strSQL
            End With
            obj = objCmd.ExecuteScalar() '*** Return Scalar ***'
            Return obj
        Catch ex As Exception
            Return Nothing '*** Return Nothing ***'
        End Try
    End Function

    Public Function TransStart(ByVal strConnString As String) As Boolean
        objConn = New SqlConnection
        With objConn
            .ConnectionString = strConnString
            .Open()
        End With
        Try
            Trans = objConn.BeginTransaction(IsolationLevel.ReadCommitted)
            Return True
        Catch ex As Exception
            Return False '*** Return False ***'
        End Try
    End Function

    Public Function TransExecute(ByVal strSQL As String) As Boolean
        objCmd = New SqlCommand
        With objCmd
            .Connection = objConn
            .Transaction = Trans
            .CommandType = CommandType.Text
            .CommandText = strSQL
        End With
        Try
            objCmd.ExecuteNonQuery()
            Return True
        Catch ex As Exception
            Return False
        End Try

    End Function

    Public Function TransRollBack() As Boolean
        Try
            Trans.Rollback()
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Function TransCommit() As Boolean

        Try
            Trans.Commit()
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Shared Sub Close_objConn()
        If objConn IsNot Nothing Then
            If objConn.State = ConnectionState.Open Then
                objConn.Close()
                objConn = Nothing
            End If
        End If
    End Sub

    Function inj(ByVal byString As String) As String
        Return byString.Replace("'", "")
    End Function
End Class