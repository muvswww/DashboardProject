
Partial Class dashboard
    'Inherits System.Web.UI.Page
    Inherits BasePage

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim MinibleBody As HtmlGenericControl = CType((Me.Master).FindControl("MinibleBody"), HtmlGenericControl)
        MinibleBody.Attributes.Remove("data-layout")
        MinibleBody.Attributes.Remove("data-layout-size")
    End Sub
End Class
