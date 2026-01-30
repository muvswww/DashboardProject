<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Up-research.aspx.vb" Inherits="Dashboard.Up_research" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <title>อัพเดตOIT</title>
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
</head>
<body data-layout="horizontal" data-topbar="colored" data-layout-size="boxed">
    <form id="form" runat="server">
            <div id="basic-form" method="post" novalidate>
                <div class="container-fluid mt-4">
                    <asp:Panel ID="Panel1" runat="server" Visible="False">
                        <div class="row">
                            <div class="col-12">
                                <div class="card">
                                    <div class="card-body">
                                            <div style="height: 200px; width: 100%; background-image: url('minible/layouts/assets/images/bg.png');">
</div>
                                        <div class="md-4 ">
                                            <div class="mt-3 ">
                                                <asp:Button ID="Add" runat="server" Text=" + Add Data OIT" CssClass="btn btn-success waves-effect waves-light mb-4" />
                                            </div>
                                        </div>

                                        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False"
                                            DataKeyNames="ID" AllowPaging="True" OnPageIndexChanging="OnPaging"
                                            EmptyDataText="No records has been added."
                                            CssClass="table" CellPadding="5" ForeColor="#333333" GridLines="None" PageSize="200">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>
                                                <asp:TemplateField HeaderStyle-CssClass="align-content-center" HeaderText="ปี">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblyear" runat="server" CssClass="table-warning" Text='<%# Eval("year") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="align-content-center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-CssClass="align-content-center" HeaderText="ข้อมูล">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSec" runat="server" CssClass="table-warning" Text='<%# Eval("section") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="align-content-center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-CssClass="align-content-center" HeaderText="ข้อมูลย่อย">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSub" runat="server" Text='<%# Eval("SubSec") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="align-content-center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="Edit" runat="server" AutoPostBack="True" CommandArgument="<%# Container.DataItemIndex %>" OnClick="LinkButton1_Click"><i class="fas fa-edit"></i></asp:LinkButton>
                                                          </ItemTemplate>
                                                </asp:TemplateField>
                                                 <asp:TemplateField>
     <ItemTemplate>
         <asp:LinkButton ID="Delete" runat="server" AutoPostBack="True" CommandArgument="<%# Container.DataItemIndex %>" OnClick="lbtDelete_Click" OnClientClick="return confirmDelete();"><i class="fas fa-trash-alt"></i></asp:LinkButton>
     </ItemTemplate>
 </asp:TemplateField>
                                            </Columns>
                                            <EditRowStyle BackColor="#999999" />
                                            <FooterStyle BackColor="#5D7B9D" ForeColor="White" Font-Bold="True" />
                                            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                            <PagerStyle ForeColor="White" HorizontalAlign="Center" BackColor="#284775" />
                                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                            <SelectedRowStyle BackColor="#E2DED6" ForeColor="#333333" Font-Bold="True" />
                                            <SortedAscendingCellStyle BackColor="#E9E7E2" />
                                            <SortedAscendingHeaderStyle BackColor="#506C8C" />
                                            <SortedDescendingCellStyle BackColor="#FFFDF8" />
                                            <SortedDescendingHeaderStyle BackColor="#6F8DAE" />

                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                     <asp:Label ID="Label1" runat="server" Text="" Style="display: none"></asp:Label>
                    <asp:Panel ID="Panel2" runat="server" Visible="True">
                        <div class="row">
                            <div class="col-lg-12">

                                <div class="card">
                                    <div class="card-body">
                                            <div>
                                                <h4 class="mb-4" style="font-weight: bolder">อัพเดตข้อมูล OIT</h4>
                                            </div>

                                            <div class="mb-3 row">
                                                <label for="example-text-input" class="col-md-3 col-form-label ">ประจำปี</label>
                                                <div class="col-md-6">
                                                    <asp:TextBox ID="txtyear" CssClass="form-control" runat="server" type="text" required data-parsley-required-message="* กรุณาระบุปี" autocomplete="off"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="mb-3 row">
                                                <label class="col-md-3 col-form-label ">หัวข้อของข้อมูล</label>
                                                <div class="col-md-6">
                                                    <asp:DropDownList ID="ddlTitle" runat="server" CssClass="form-select" required data-parsley-min="1" data-parsley-required-message="* กรุณาระบุประเภท">
                                                        <asp:ListItem Value="0">ระบุหัวข้อ</asp:ListItem>
                                                        <asp:ListItem Value="1">ข้อมูลพื้นฐาน</asp:ListItem>
                                                        <asp:ListItem Value="2">การประชาสัมพันธ์</asp:ListItem>
                                                        <asp:ListItem Value="3">การปฏิสัมพันธ์ข้อมูล</asp:ListItem>
                                                        <asp:ListItem Value="4">แผนการดำเนินงานและงบประมาณ</asp:ListItem>
                                                        <asp:ListItem Value="5">การปฏิบัติงาน</asp:ListItem>
                                                        <asp:ListItem Value="6">การให้บริการและการติดต่อประสานงาน</asp:ListItem>
                                                        <asp:ListItem Value="7">แผนการใช้จ่ายงบประมาณประจำปี</asp:ListItem>
                                                        <asp:ListItem Value="8">การจัดซื้อจัดจ้างหรือการจัดหาพัสดุ</asp:ListItem>
                                                        <asp:ListItem Value="9">การบริหารและพัฒนาทรัพยากรบุคคล</asp:ListItem>
                                                        <asp:ListItem Value="10">การจัดการเรื่องร้องเรียนการทุจริตและประพฤติมิชอบ</asp:ListItem>
                                                        <asp:ListItem Value="11">การเปิดโอกาสให้เกิดการมีส่วนร่วม</asp:ListItem>
                                                        <asp:ListItem Value="12">นโยบาย No Gift Policy</asp:ListItem>
                                                        <asp:ListItem Value="13">การประเมินความเสี่ยงเพื่อป้องกันการทุจริต</asp:ListItem>
                                                        <asp:ListItem Value="14">การเสริมสร้างวัฒนธรรมองค์กร</asp:ListItem>
                                                        <asp:ListItem Value="15">แผนป้องกันการทุจริต</asp:ListItem>
                                                        <asp:ListItem Value="16">มาตรการส่งเสริมความโปร่งใใสและป้องกันการทุจริตภายในหน่วยงาน</asp:ListItem>
                                                        <asp:ListItem Value="17">ผลการดำเนินงาน ITA</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="mb-3 row">
                                                <label for="example-text-input" class="col-md-3 col-form-label ">ชื่อของข้อมูล</label>
                                                <div class="col-md-6">
                                                    <asp:TextBox ID="txtSec" CssClass="form-control" runat="server" type="text" required data-parsley-required-message="* กรุณาระบุชื่อของข้อมูล" autocomplete="off"></asp:TextBox>
                                                </div>
                                            </div>

                                            <div class="mb-3 row">
                                                <label for="example-text-input" class="col-md-3 col-form-label ">ชื่อย่อยของข้อมูล (*ถ้ามีมากกว่า 2 ไฟล์)</label>
                                                <div class="col-md-6">
                                                    <asp:TextBox ID="txtSub" CssClass="form-control" runat="server" type="text" autocomplete="off"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="mb-3 row">
                                                <label for="example-text-input" class="col-md-3 col-form-label ">linkของข้อมูล</label>
                                                <div class="col-md-6">
                                                    <asp:TextBox ID="txtLink" CssClass="form-control" runat="server" type="text" required data-parsley-required-message="* กรุณาระบุLinkของข้อมูล" autocomplete="off"></asp:TextBox>
                                                </div>
                                            </div>
                                            </div>
                                            </div>
                                    </div>
                                    </div>
                    </asp:Panel>

                     <div class="text-center mt-4">
     <asp:Button ID="btnsubmit" runat="server" Text="Save" CssClass="btn bg-soft-success waves-effect waves-light" Width="120px" Visible="False" />
     <asp:Button ID="btnupdate" runat="server" Text="Update" CssClass="btn bg-soft-primary waves-effect waves-light" Width="120px" Visible="False" />
     <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn bg-soft-danger waves-effect waves-light" Width="120px" OnClientClick="cancelValidation(); return true;" Visible="False" />

 </div>



                </div>
        </div>
    </form>

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
 

    <script>
        function confirmDelete() {
            var result = confirm("Are you sure you want to delete?");
            return result;
        }
    </script>
</body>
</html>

