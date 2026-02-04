<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="WebForm1.aspx.vb" Inherits="Dashboard.WebForm1" %>

<!DOCTYPE html>


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <title>อัพเดตข้อมูลทุนวิจัย และบริการวิชาการ</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <meta content="Premium Multipurpose Admin & Dashboard Template" name="description" />
    <meta content="Themesbrand" name="author" />
    <link href="Quark/quark_fontface.css" rel="stylesheet" />
    
    <%--dropzone--%>
    <link href="minible/layouts/assets/libs/dropzone/min/dropzone.min.css" rel="stylesheet" />
    <!-- App favicon -->
    <link rel="shortcut icon" href="minible/layouts/assets/images/favicon.ico" />
    <!-- Bootstrap Css -->
    <link href="minible/layouts/assets/css/bootstrap.min.css" rel="stylesheet" />
    <!-- Icons Css -->
    <link href="minible/layouts/assets/css/icons.min.css" rel="stylesheet" />
    <!-- App Css-->
    <link href="minible/layouts/assets/css/app.min.css" rel="stylesheet" />
    <link href="minible/GridviewStyle.css" rel="stylesheet" />
    <!-- Lightbox css -->
    <link href="minible/layouts/assets/libs/magnific-popup/magnific-popup.css" rel="stylesheet" type="text/css" />
    <!-- JAVASCRIPT -->
    <script src="minible/layouts/assets/libs/jquery/jquery.min.js"></script>
    <script src="minible/layouts/assets/libs/bootstrap/js/bootstrap.bundle.min.js"></script>
    <script src="minible/layouts/assets/libs/metismenu/metisMenu.min.js"></script>
    <script src="minible/layouts/assets/libs/simplebar/simplebar.min.js"></script>
    <script src="minible/layouts/assets/libs/node-waves/waves.min.js"></script>
    <script src="minible/layouts/assets/libs/waypoints/lib/jquery.waypoints.min.js"></script>
    <script src="minible/layouts/assets/libs/jquery.counterup/jquery.counterup.min.js"></script>

    <script src="minible/layouts/assets/js/app.js"></script>

    <!-- ckeditor -->
    <script src="minible/layouts/assets/libs/%40ckeditor/ckeditor5-build-classic/build/ckeditor.js"></script>

    <!--tinymce js-->
    <script src="minible/layouts/assets/libs/tinymce/tinymce.min.js"></script>

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
    <script src="minible/datetimepicker/jquery-2.1.3.min.js"></script>
    <link href="minible/datetimepicker/dist/css/bootstrap-datepicker.css" rel="stylesheet" type="text/css" />
    <script src="minible/datetimepicker/dist/js/bootstrap-datepicker-custom.js" type="text/javascript"></script>
    <script src="minible/datetimepicker/js/bootstrap-datepicker-thai.js" type="text/javascript"></script>
    <script type="text/javascript">
        var $j213 = $.noConflict(true);

        $j213(function () {

            $j213(".clDate").datepicker({
                autoclose: true,
                format: "dd/mm/yyyy",
                todayHighlight: true,
                todayBtn: "linked",
                language: "th",             //เปลี่ยน label ต่างของ ปฏิทิน ให้เป็น ภาษาไทย   (ต้องใช้ไฟล์ bootstrap-datepicker.th.min.js นี้ด้วย)
                useCurrent: false,
                thaiyear: true              //Set เป็นปี พ.ศ.
            }); //.datepicker("setDate", "0");  //กำหนดเป็นวันปัจุบัน
        });
    </script>
    <!-- Flatpickr CSS -->
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/flatpickr/dist/flatpickr.min.css" />
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <style>
        input[type="checkbox"] + label {
            margin-left: 6px;
        }

        .table td, .table th {
            border-left: 1px solid #ccc;
            border-right: 1px solid #ccc;
            border-top: none;
            border-bottom: none;
        }
    </style>
</head>
<body data-layout="horizontal" data-topbar="colored" data-layout-size="boxed">
    <form id="form" runat="server">
        <div id="basic-form" method="post" novalidate>
            <div class="container-fluid mt-4">
                <asp:Label ID="LabelFund" runat="server" Style="display: none"></asp:Label>
                <asp:Panel ID="panelFund" runat="server">
                    <div class="row">
                        <div class="col-12">
                            <div class="card">
                                <div class="card-body">
                                    <div style="height: 200px; width: 100%; background-image: url('minible/layouts/assets/images/ปก/2.png');">
                                    </div>
                                    <div class="row align-items-center mb-3 mt-3">
                                        <div class="col-6 col-md-6">
                                            <asp:Button ID="addFund" runat="server"
                                                Text="+ เพิ่มข้อมูล"
                                                CssClass="btn btn-primary waves-effect waves-light" />
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
                                        CssClass="table" PageSize="200" CellPadding="4" ForeColor="#333333" GridLines="None">
                                        <AlternatingRowStyle BackColor="White" />
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-CssClass="align-content-center" HeaderText="ปี">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblyear" runat="server" CssClass="table-warning" Text='<%# Eval("year") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="align-content-center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-CssClass="align-content-center" HeaderText="ประเภท">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSec" runat="server" CssClass="table-warning" Text='<%# Eval("type") %>'></asp:Label>
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
                                                    <asp:Label ID="lblSub" runat="server" Text='<%# Eval("title") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="align-content-center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <%--<asp:LinkButton ID="editFund" runat="server" AutoPostBack="True" CommandArgument="<%# Container.DataItemIndex %>" OnClick="LinkButton1_Click"><i class="fas fa-edit"></i></asp:LinkButton>--%>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                        </Columns>

                                        <EditRowStyle BackColor="#7C6F57" />
                                        <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                                        <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                                        <RowStyle BackColor="#E3EAEB" />
                                        <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                                        <SortedAscendingCellStyle BackColor="#F8FAFA" />
                                        <SortedAscendingHeaderStyle BackColor="#246B61" />
                                        <SortedDescendingCellStyle BackColor="#D4DFE1" />
                                        <SortedDescendingHeaderStyle BackColor="#15524A" />

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
                                        <div class="mb-3 row">
                                            <label for="example-text-input" class="col-md-3 col-form-label ">ชื่อโครงการ</label>
                                            <div class="col-md-6">
                                                <asp:TextBox ID="txtTitle" CssClass="form-control" runat="server" type="text" autocomplete="off"></asp:TextBox>
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
                                                    <input type="text" class="form-control" name="start" placeholder="Start Date" />
                                                    <input type="text" class="form-control" name="end" placeholder="End Date" />
                                                </div>
                                            </div>
                                        </div>

                                        <div class="mb-3 row">
                                            <label for="example-text-input" class="col-md-3 col-form-label ">ขยายเวลา (* ถ้ามี)</label>
                                            <div class="col-md-6">
                                                <div class="input-group" id="datepicker1">
                                                    <input type="text" class="form-control" placeholder="dd M, yyyy"
                                                        data-date-format="dd M, yyyy" data-date-container='#datepicker1' data-provide="datepicker">

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
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn bg-soft-danger waves-effect waves-light" Width="120px" OnClientClick="cancelValidation(); return true;" Visible="True" />

                        </div>
                </asp:Panel>




            </div>
        </div>
    </form>
    
    <!-- Flatpickr JavaScript -->
    <script src="https://cdn.jsdelivr.net/npm/flatpickr"></script>
    <!-- Magnific Popup-->
    <script src="minible/layouts/assets/libs/magnific-popup/jquery.magnific-popup.min.js"></script>

    <!-- lightbox init js-->
    <%--ต้องไว้ข้างล่างเท่านั้น ไว้ข้างบนไม่ทำงาน--%>
    <script src="minible/layouts/assets/js/pages/lightbox.init.js"></script>
    <script src="minible/layouts/assets/js/app.js"></script>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.2/jquery.min.js"></script>
    <script src="minible/layouts/assets/libs/dropzone/min/dropzone.min.js"></script>

    <script type="text/javascript">
        var $j1112 = $.noConflict(true);
        $j1112(document).ready(function () {
            $j1112('#' + btnsubmitClientId).on('click', function (e) {

            });
        });
    </script>

     <%--เลือกเวลา 24ชม.--%>
 <%--<script type="text/javascript">
     document.addEventListener("DOMContentLoaded", function () {
         flatpickr("#<%= txtTimeSt.ClientID %>", {
             enableTime: true,
             noCalendar: true,
             dateFormat: "H:i", // รูปแบบ 24 ชั่วโมง เช่น 14:30
             time_24hr: true
         });
         flatpickr("#<%= txtTimeFn.ClientID %>", {
         enableTime: true,
         noCalendar: true,
         dateFormat: "H:i", // รูปแบบ 24 ชั่วโมง เช่น 14:30
         time_24hr: true
     });
 });
 </script>--%>
 <%--เลือกเวลา 24ชม.--%>

 <script>
     function confirmDelete() {
         var result = confirm("Are you sure you want to delete?");
         return result;
     }
 </script>
</body>
</html>
