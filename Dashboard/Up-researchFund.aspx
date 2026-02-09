<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Up-researchFund.aspx.vb" Inherits="Dashboard.Up_researchFund" %>

<!DOCTYPE html>


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <title>อัพเดตข้อมูลทุนวิจัย และบริการวิชาการ</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <meta content="Premium Multipurpose Admin & Dashboard Template" name="description" />
    <meta content="Themesbrand" name="author" />
    <!-- App favicon -->
    <link rel="shortcut icon" href="minible/layouts/assets/images/favicon.ico" />
    <!-- plugin css -->
    <link href="minible/layouts/assets/libs/select2/css/select2.min.css" rel="stylesheet" type="text/css" />
    <link href="minible/layouts/assets/libs/spectrum-colorpicker2/spectrum.min.css" rel="stylesheet" type="text/css" />
    <link href="minible/layouts/assets/libs/bootstrap-datepicker/css/bootstrap-datepicker.min.css" rel="stylesheet" />
    <link href="minible/layouts/assets/libs/bootstrap-touchspin/jquery.bootstrap-touchspin.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="minible/layouts/assets/libs/%40chenfengyuan/datepicker/datepicker.min.css" />
    <!-- Bootstrap Css -->
    <link href="minible/layouts/assets/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <!-- Icons Css -->
    <link href="minible/layouts/assets/css/icons.min.css" rel="stylesheet" type="text/css" />
    <!-- App Css-->
    <link href="minible/layouts/assets/css/app.min.css" rel="stylesheet" type="text/css" />
    <link href="minible/layouts/assets/css/GridviewStyle.css" rel="stylesheet" />
    <!-- JAVASCRIPT -->
    <script src="minible/layouts/assets/libs/jquery/jquery.min.js"></script>
    <script src="minible/layouts/assets/libs/bootstrap/js/bootstrap.bundle.min.js"></script>
    <script src="minible/layouts/assets/libs/metismenu/metisMenu.min.js"></script>
    <script src="minible/layouts/assets/libs/simplebar/simplebar.min.js"></script>
    <script src="minible/layouts/assets/libs/node-waves/waves.min.js"></script>
    <script src="minible/layouts/assets/libs/waypoints/lib/jquery.waypoints.min.js"></script>
    <script src="minible/layouts/assets/libs/jquery.counterup/jquery.counterup.min.js"></script>
    <script src="minible/layouts/assets/js/app.js"></script>
    <link href="minible/layouts/assets/fonts/DB%20X%20v3.2/style.css" rel="stylesheet" />

    <!-- parsleyjs -->
    <link href="minible/layouts/assets/libs/parsleyjs2/css/parsley.css" rel="stylesheet" />
    <script src="minible/layouts/assets/libs/parsleyjs2/js/parsley.min.js"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            $('#form').parsley();
        });
        function cancelValidation() {
            $('#form').parsley().destroy();
        }
    </script>
    <style>
        input[type="checkbox"] + label {
            margin-left: 6px;
        }

        .table td, .table th {
            border: 1px solid #ccc;
            /*border-left: 1px solid #ccc;
            border-right: 1px solid #ccc;
            border-top: none;
            border-bottom: none;*/
        }

        .banner-box {
            width: 100%;
            height: 200px; /* ความสูงแบนเนอร์ */
            display: flex;
            justify-content: center;
            align-items: center;
            overflow: hidden;
            background-color: #fff; /* เผื่อภาพไม่เต็ม */
        }

            .banner-box img {
                width: 100%;
                height: 100%;
                object-fit: contain; /* เห็นภาพครบ */
            }
    </style>

</head>
<body data-layout="horizontal" data-topbar="colored" data-layout-size="boxed">
    <form id="form" runat="server">
        <div id="basic-form" method="post" novalidate>
            <div class="container-fluid mt-4">
                <asp:Label ID="LabelFund" runat="server" Style="display: none"></asp:Label>
                <asp:Panel ID="panelFund" runat="server">
                   <%-- <div class="col-12 text-end">
                                        <a href="portal.aspx" class="font-size-20">
                                            <div class="uil-times">ปิด</div> 
                                        </a>
                                    </div>--%>
                    <div class="row">
                        <div class="col-12">
                            <div class="card">
                                <div class="card-body">
                                    
                                    <div class="banner-box">
                                        <img src="minible/layouts/assets/images/ปก/2.png" alt="banner">
                                    </div>
                                    <div class="row align-items-center mb-3 mt-3">
                                        <div class="col-6 col-md-2">
                                            <asp:Button ID="addFund" runat="server"
                                                Text="+ เพิ่มโครงการงานวิจัย"
                                                CssClass="btn btn-primary waves-effect waves-light" />
                                        </div>

                                        <div class="col-6 col-md-4">
                                            <asp:Button ID="addFundsource" runat="server"
                                                Text="+ เพิ่มแหล่งทุน"
                                                CssClass="btn btn-warning waves-effect waves-light" />
                                        </div>

                                        <div class="col-6 col-md-6 text-end">
                                            <asp:DropDownList ID="ddlYearFund1" runat="server"
                                                CssClass="form-select d-inline-block w-auto"
                                                AutoPostBack="true"
                                                OnSelectedIndexChanged="ddlYearFund1_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </div>

                                    </div>
                                    <asp:GridView ID="GridViewFund" runat="server" AutoGenerateColumns="False"
                                        DataKeyNames="NO" AllowPaging="True" OnPageIndexChanging="OnPaging"
                                        EmptyDataText="No records has been added."
                                        CssClass="table" PageSize="200" CellPadding="4" BackColor="White" BorderColor="#CC9966" BorderStyle="None" BorderWidth="1px">
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-CssClass="align-content-center" HeaderText="ปี">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblyear" runat="server" Text='<%# Eval("year") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="align-content-center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-CssClass="align-content-center" HeaderText="ประเภท">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSec" runat="server" Text='<%# Eval("type_name") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="align-content-center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-CssClass="align-content-center" HeaderText="หัวหน้าโครงการ">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSub" runat="server" Text='<%# Eval("fullname") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="align-content-center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-CssClass="align-content-center" HeaderText="ชื่อโครงการ">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTitle" runat="server" Text='<%# Eval("title") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="align-content-center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="editFund" runat="server" AutoPostBack="True" CommandArgument="<%# Container.DataItemIndex %>" OnClick="LinkButton1_Click"><i class="fas fa-edit"></i></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                        </Columns>

                                        <FooterStyle BackColor="#FFFFCC" ForeColor="#330099" />
                                        <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="#FFFFCC" />
                                        <PagerStyle ForeColor="#330099" HorizontalAlign="Center" BackColor="#FFFFCC" />
                                        <RowStyle BackColor="White" ForeColor="#330099" />
                                        <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="#663399" />
                                        <SortedAscendingCellStyle BackColor="#FEFCEB" />
                                        <SortedAscendingHeaderStyle BackColor="#AF0101" />
                                        <SortedDescendingCellStyle BackColor="#F6F0C0" />
                                        <SortedDescendingHeaderStyle BackColor="#7E0000" />

                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:Panel>

                <asp:Panel ID="panelUpFund" runat="server" Visible="True">
                    <div class="row">
                        <div class="col-lg-12">

                            <div class="card">
                                <div class="card-body">
                                    <div>
                                        <h4 class="mb-4" style="font-weight: bolder">อัพเดตข้อมูลทุนวิจัย & บริการวิชาการ</h4>
                                    </div>

                                    <%--<div class="mb-3 row">
                                        <label class="form-label col-md-3">ปีงบประมาณ</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="ddlYearFund2" runat="server" CssClass="form-select"
                                                AutoPostBack="true" OnSelectedIndexChanged="ddlYearFund2_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </div>
                                    </div>--%>
                                    <div class="mb-3 row">
                                        <label for="example-text-input" class="col-md-3 col-form-label ">ปีงบประมาณ</label>
                                        <div class="col-md-6">
                                            <asp:TextBox ID="txtyear" CssClass="form-control" runat="server" type="text" autocomplete="off"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="mb-3 row">
                                        <label class="form-label col-md-3">ประเภทงานวิจัย</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="ddlType" runat="server" CssClass="form-select">
                                                <asp:ListItem Value="0">--- เลือกประเภทงานวิจัย ---</asp:ListItem>
                                                <asp:ListItem Value="1">ประเภททุนวิจัย</asp:ListItem>
                                                <asp:ListItem Value="2">ประเภทบริการวิชาการ</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="mb-3 row">
                                        <label class="form-label col-md-3">หัวหน้าโครงการ</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="ddlUserFund" runat="server" CssClass="form-control select2"
                                                AutoPostBack="true" OnSelectedIndexChanged="ddlUserFund_SelectedIndexChanged">
                                            </asp:DropDownList>

                                        </div>
                                    </div>
                                    <div class="mb-3 row">
                                        <label for="example-text-input" class="col-md-3 col-form-label ">หน่วยงาน</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="ddlDept" runat="server" CssClass="form-control select2"
                                                AutoPostBack="true">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="mb-3 row">
                                        <label for="example-text-input" class="col-md-3 col-form-label ">ชื่อโครงการ</label>
                                        <div class="col-md-6">
                                            <asp:TextBox ID="txtTitle" CssClass=" form-control two-line-ellipsis" runat="server" type="text" autocomplete="off" TextMode="MultiLine"
                                                Rows="3"></asp:TextBox>

                                        </div>
                                    </div>
                                    <div class="mb-3 row">
                                        <label class="form-label col-md-3">แหล่งทุน</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="ddlSource" runat="server" CssClass="form-control select2"
                                                AutoPostBack="true">
                                            </asp:DropDownList>
                                        </div>
                                    </div>


                                    <div class="mb-3 row">
                                        <label for="example-text-input" class="col-md-3 col-form-label ">วันที่เริ่มต้น - วันที่สิ้นสุดของสัญญา</label>
                                        <div class="col-md-6">
                                            <label class="form-label">Date Range</label>
                                            <div class="input-daterange input-group" id="datepicker6" data-date-format="dd M, yyyy" data-date-autoclose="true" data-provide="datepicker" data-date-container='#datepicker6'>
                                                <input type="text" id="txtStartDate" runat="server" class="form-control" name="start" placeholder="Start Date" autocomplete="off" />
                                                <input type="text" id="txtEndDate" runat="server" class="form-control" name="end" placeholder="End Date" autocomplete="off" />
                                            </div>
                                        </div>
                                    </div>

                                    <div class="mb-3 row">
                                        <label for="example-text-input" class="col-md-3 col-form-label ">ขยายเวลา (* ถ้ามี)</label>
                                        <div class="col-md-6">
                                            <div class="input-group" id="datepicker1">
                                                <input type="text" id="txtExtendDate" runat="server" class="form-control" placeholder="dd M, yyyy"
                                                    data-date-format="dd M, yyyy" data-date-container='#datepicker1' data-provide="datepicker" autocomplete="off" />

                                                <span class="input-group-text"><i class="mdi mdi-calendar"></i></span>
                                            </div>
                                        </div>
                                    </div>


                                    <div class="mb-3 row">
                                        <label for="example-text-input" class="col-md-3 col-form-label ">จำนวนเงินทุน</label>
                                        <div class="col-md-6">
                                            <asp:TextBox ID="txtamount" CssClass="form-control" runat="server" type="text" autocomplete="off"></asp:TextBox>
                                        </div>
                                    </div>

                                </div>
                            </div>
                        </div>
                    </div>
                    <asp:HiddenField ID="hfFundNo" runat="server" />

                    <div class="text-center mt-4">
                        <asp:Button ID="btnsubmitFund" runat="server" Text="Save" CssClass="btn bg-soft-success waves-effect waves-light" Width="120px" Visible="False" />
                        <asp:Button ID="btnupdateFund" runat="server" Text="Update" CssClass="btn bg-soft-primary waves-effect waves-light" Width="120px" Visible="False" />
                        <asp:Button ID="btnCancelFund" runat="server" Text="Cancel" CssClass="btn bg-soft-danger waves-effect waves-light" Width="120px" OnClientClick="cancelValidation(); return true;" Visible="True" />

                    </div>
                </asp:Panel>

                <asp:Panel ID="panelUpsource" runat="server" Visible="False">
                    <div class="row">
                        <div class="col-lg-12">

                            <div class="card">
                                <div class="card-body">
                                    <div>
                                        <h4 class="mb-4" style="font-weight: bolder">แหล่งทุนของงานวิจัย & บริการวิชาการ</h4>
                                    </div>
                                  <asp:RadioButtonList ID="rblFundType" runat="server"
    AutoPostBack="True"
    OnSelectedIndexChanged="rblFundType_SelectedIndexChanged"
    RepeatDirection="Horizontal"
    CssClass="fund-radio mb-2" >
    <asp:ListItem Value="0" Selected="True">ทั้งหมด</asp:ListItem>
    <asp:ListItem Value="1">ภายใน</asp:ListItem>
    <asp:ListItem Value="2">ภายนอก</asp:ListItem>
</asp:RadioButtonList>


                                    <asp:GridView ID="GridViewsource" runat="server"
                                        AutoGenerateColumns="False"
                                        DataKeyNames="no"
                                        AllowPaging="True"
                                        OnPageIndexChanging="OnPaging"
                                        OnRowEditing="GridViewsource_RowEditing"
                                        OnRowCancelingEdit="GridViewsource_RowCancelingEdit"
                                        OnRowUpdating="GridViewsource_RowUpdating"
                                        OnRowCommand="GridViewsource_RowCommand"
                                        ShowFooter="True"
                                        CssClass="table"
                                        EmptyDataText="No records has been added." BackColor="White" BorderColor="#CCCCCC" BorderWidth="1px" CellPadding="4" ForeColor="Black" GridLines="Horizontal" BorderStyle="None">

                                        <Columns>
                                            <asp:TemplateField HeaderText="แหล่งทุน">
                                                <ItemTemplate>
                                                    <%# Eval("Fund_source") %>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtEditSource" runat="server" CssClass="form-control" Text='<%# Bind("Fund_source") %>' />
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    <asp:TextBox ID="txtNewSource" runat="server" CssClass="form-control" Placeholder="ชื่อแหล่งทุนใหม่" />
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="ภายใน / ภายนอก">
                                                <ItemTemplate>
                                                    <%# Eval("type_name") %>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:DropDownList ID="ddlEditType" runat="server" CssClass="form-control">
                                                        <asp:ListItem Value="1">ภายใน</asp:ListItem>
                                                        <asp:ListItem Value="2">ภายนอก</asp:ListItem>
                                                    </asp:DropDownList>
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    <asp:DropDownList ID="ddlNewType" runat="server" CssClass="form-control">
                                                        <asp:ListItem Value="1">ภายใน</asp:ListItem>
                                                        <asp:ListItem Value="2">ภายนอก</asp:ListItem>
                                                    </asp:DropDownList>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:CommandField ShowEditButton="True" />
                                            <asp:TemplateField>
                                                <FooterTemplate>
                                                    <asp:Button ID="btnAddNew" runat="server" CommandName="AddNew" CssClass="btn btn-success btn-sm" Text="เพิ่มข้อมูล" />
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                        <HeaderStyle BackColor="#333333" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Right" />
                                        <SelectedRowStyle BackColor="#CC3333" ForeColor="White" Font-Bold="True" />
                                        <SortedAscendingCellStyle BackColor="#F7F7F7" />
                                        <SortedAscendingHeaderStyle BackColor="#4B4B4B" />
                                        <SortedDescendingCellStyle BackColor="#E5E5E5" />
                                        <SortedDescendingHeaderStyle BackColor="#242121" />
                                    </asp:GridView>

                                    <div class="text-end">
                                    <asp:Button ID="btnCancelsource" runat="server" Text="Cancel" CssClass="btn bg-soft-danger waves-effect waves-light" Width="120px" OnClientClick="cancelValidation(); return true;" Visible="True" />
</div>


                                    
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:Panel>




            </div>
        </div>
    </form>
    <!-- JAVASCRIPT -->
    <script src="minible/layouts/assets/libs/jquery/jquery.min.js"></script>
    <script src="minible/layouts/assets/libs/bootstrap/js/bootstrap.bundle.min.js"></script>
    <script src="minible/layouts/assets/libs/metismenu/metisMenu.min.js"></script>
    <script src="minible/layouts/assets/libs/simplebar/simplebar.min.js"></script>
    <script src="minible/layouts/assets/libs/node-waves/waves.min.js"></script>
    <script src="minible/layouts/assets/libs/waypoints/lib/jquery.waypoints.min.js"></script>
    <script src="minible/layouts/assets/libs/jquery.counterup/jquery.counterup.min.js"></script>

    <!-- plugins -->
    <script src="minible/layouts/assets/libs/select2/js/select2.min.js"></script>
    <script src="minible/layouts/assets/libs/spectrum-colorpicker2/spectrum.min.js"></script>
    <script src="minible/layouts/assets/libs/bootstrap-datepicker/js/bootstrap-datepicker.min.js"></script>
    <script src="minible/layouts/assets/libs/bootstrap-touchspin/jquery.bootstrap-touchspin.min.js"></script>
    <script src="minible/layouts/assets/libs/bootstrap-maxlength/bootstrap-maxlength.min.js"></script>
    <script src="minible/layouts/assets/libs/%40chenfengyuan/datepicker/datepicker.min.js"></script>

    <!-- init js -->
    <script src="minible/layouts/assets/js/pages/form-advanced.init.js"></script>

    <script src="minible/layouts/assets/js/app.js"></script>

    <script type="text/javascript">
        var $j1112 = $.noConflict(true);
        $j1112(document).ready(function () {
            $j1112('#' + btnsubmitClientId).on('click', function (e) {

            });
        });
    </script>



    <script>
        function confirmDelete() {
            var result = confirm("Are you sure you want to delete?");
            return result;
        }
    </script>

</body>
</html>
