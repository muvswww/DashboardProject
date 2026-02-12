<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="WebForm2.aspx.vb" Inherits="Dashboard.WebForm2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
       <meta charset="utf-8" />
   <title>อัพเดตข้อมูลทุนวิจัย และบริการวิชาการ</title>
   <meta name="viewport" content="width=device-width, initial-scale=1.0" />
   <meta content="Premium Multipurpose Admin & Dashboard Template" name="description" />
   <meta content="Themesbrand" name="author" />
   <!-- App favicon -->
   <link rel="shortcut icon" href="assets/images/favicon.ico" />

  
    
</head>
<body data-layout="horizontal" data-topbar="colored" data-layout-size="boxed">
 
 <asp:DropDownList ID="ddlYear" runat="server"></asp:DropDownList>
<asp:Button ID="btnExport" runat="server" Text="ดาวน์โหลดไฟล์พร้อมข้อมูลไตรมาส" />

</body>
</html>
