<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Up-researchInnov.aspx.vb" Inherits="Dashboard.Up_researchInnov" %>

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

        .datepicker {
            z-index: 9999 !important;
        }
    </style>

</head>
<body data-layout="horizontal" data-topbar="colored" data-layout-size="boxed">
    <form id="form" runat="server">
        <div id="basic-form" method="post" novalidate>
            <%--<div class="container-fluid mt-4">--%>
            <asp:Label ID="LabelFund" runat="server" Style="display: none"></asp:Label>
<div class="banner-box">
    <img src="minible/layouts/assets/images/ปก/3.png" alt="banner">
</div>
            <asp:Panel ID="panelUpsource" runat="server" Visible="True">
                <div class="row">
                    <div class="col-lg-12">

                        <div class="card">
                            <div class="card-body">
                                


                                <asp:GridView ID="GridViewInnov" runat="server"
                                    AutoGenerateColumns="False"
                                    DataKeyNames="no"
                                    AllowPaging="True"
                                    OnPageIndexChanging="OnPaging"
                                    OnRowEditing="GridViewInnov_RowEditing"
                                    OnRowCancelingEdit="GridViewInnov_RowCancelingEdit"
                                    OnRowUpdating="GridViewInnov_RowUpdating"
                                    OnRowCommand="GridViewInnov_RowCommand"
                                    ShowFooter="True"
                                    CssClass="table"
                                    EmptyDataText="No records has been added." CellPadding="4" ForeColor="#333333" GridLines="None">

                                    <AlternatingRowStyle BackColor="White" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="type">
                                            <ItemTemplate>
                                                <%# Eval("type_name") %>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DropDownList ID="ddlEditType" runat="server" CssClass="form-control">
                                                    <asp:ListItem Value="1">สิทธิบัตร</asp:ListItem>
                                                    <asp:ListItem Value="2">อนุสิทธิบัตร</asp:ListItem>
                                                    <asp:ListItem Value="3">ลิขสิทธิ์</asp:ListItem>
                                                    <asp:ListItem Value="4">ขอสิทธิบัตร</asp:ListItem>
                                                    <asp:ListItem Value="5">ขออนุสิทธิบัตร</asp:ListItem>
                                                </asp:DropDownList>
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:DropDownList ID="ddlNewType" runat="server" CssClass="form-control">
                                                    <asp:ListItem Value="0">-- type --</asp:ListItem>
                                                    <asp:ListItem Value="1">สิทธิบัตร</asp:ListItem>
                                                    <asp:ListItem Value="2">อนุสิทธิบัตร</asp:ListItem>
                                                    <asp:ListItem Value="3">ลิขสิทธิ์</asp:ListItem>
                                                    <asp:ListItem Value="4">ขอสิทธิบัตร</asp:ListItem>
                                                    <asp:ListItem Value="5">ขออนุสิทธิบัตร</asp:ListItem>
                                                </asp:DropDownList>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="เลขที่ยื่น">
                                            <ItemTemplate>
                                                <%# Eval("request_number") %>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtRenumber" runat="server" CssClass="form-control" Text='<%# Bind("request_number") %>' />
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="txtRenumber" runat="server" CssClass="form-control" Placeholder="ระบุเลขที่" />
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="เลขที่">
                                            <ItemTemplate>
                                                <%# Eval("number") %>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtNumber" runat="server" CssClass="form-control" Text='<%# Bind("number") %>' />
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="txtNumber" runat="server" CssClass="form-control" Placeholder="ระบุเลขที่" />
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="วันที่">
                                            <ItemTemplate>
                                                <%# Eval("request_date", "{0:dd MMM yyyy}") %>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <div class="position-relative">
                                                    <asp:TextBox ID="txtEditDate" runat="server" autocomplete="off" CssClass="form-control datepicker-popup" Text='<%# Bind("request_date", "{0:dd MMM yyyy}") %>' />
                                                    <i class="mdi mdi-calendar position-absolute" style="right: 10px; top: 50%; transform: translateY(-50%); pointer-events: none;"></i>
                                                </div>
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <div class="position-relative">
                                                    <asp:TextBox ID="txtNewDate" runat="server" autocomplete="off" CssClass="form-control datepicker-popup" placeholder="เลือกวันที่" />
                                                    <i class="mdi mdi-calendar position-absolute" style="right: 10px; top: 50%; transform: translateY(-50%); pointer-events: none;"></i>
                                                </div>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ชื่อเจ้าของผลงาน">
                                            <ItemTemplate>
                                                <%# Eval("fullname") %>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DropDownList ID="ddlEditUser" runat="server" CssClass="form-control select2" />
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:DropDownList ID="ddlNewUser" runat="server" CssClass="form-control select2" />
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ชื่อเรื่อง">
                                            <ItemTemplate>
                                                <%# Eval("title") %>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txttitle" runat="server" CssClass="form-control" Text='<%# Bind("title") %>' />
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="txttitle" runat="server" CssClass="form-control" Placeholder="ระบุชื่อเรื่อง" />
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <FooterTemplate>
                                                <asp:Button ID="btnAddNew" runat="server" CommandName="AddNew" CssClass="btn btn-success btn-sm" Text="เพิ่มข้อมูล" />
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:CommandField ShowEditButton="True" />
                                    </Columns>
                                    <EditRowStyle BackColor="#2461BF" />
                                    <FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True" />
                                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                    <%--<PagerStyle BackColor="#2461BF" ForeColor="Black" HorizontalAlign="Center" />--%>
                                    <RowStyle BackColor="#EFF3FB" />
                                    <SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" Font-Bold="True" />
                                    <SortedAscendingCellStyle BackColor="#F5F7FB" />
                                    <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                                    <SortedDescendingCellStyle BackColor="#E9EBEF" />
                                    <SortedDescendingHeaderStyle BackColor="#4870BE" />
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
        <link href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.9.0/css/bootstrap-datepicker.min.css" rel="stylesheet" />
        <script src="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.9.0/js/bootstrap-datepicker.min.js"></script>

        <script>
            function initPopupDatepicker() {
                $('.datepicker-popup').datepicker({
                    format: 'dd M yyyy',
                    autoclose: true,
                    todayHighlight: true,
                    orientation: "bottom auto",   // เด้งลงอัตโนมัติ
                    container: 'body'              // 🔥 ทำให้ popup ลอยออกจาก Grid
                });
            }

            // โหลดครั้งแรก
            $(document).ready(function () {
                initPopupDatepicker();
            });

            // โหลดใหม่ทุกครั้งหลัง PostBack (Edit / Paging / Add)
            Sys.Application.add_load(function () {
                initPopupDatepicker();
            });
        </script>


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
