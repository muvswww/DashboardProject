<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Up-research.aspx.vb" Inherits="Dashboard.Up_research" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <title>อัพเดตข้อมูลของงานวิจัย</title>
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

    <link href="minible/layouts/assets/css/GridviewStyle.css" rel="stylesheet" />
    <!-- Lightbox css -->
    <link href="minible/layouts/assets/libs/magnific-popup/magnific-popup.css" rel="stylesheet" type="text/css" />
    <link href="minible/layouts/assets/libs/select2/css/select2.min.css" rel="stylesheet" />
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
    <%--<script src="minible/datetimepicker/jquery-2.1.3.min.js"></script>--%>
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
             .indent {
    margin-left: 25px;
}
    </style>
</head>
<body data-layout="horizontal" data-topbar="colored" data-layout-size="boxed">
    <form id="form" runat="server">
        <div id="basic-form" method="post" novalidate>
            <div class="container-fluid mt-4">
                <asp:Label ID="Labelpub" runat="server" Style="display: none"></asp:Label>
                <asp:Panel ID="panelPub" runat="server">
                    <div class="row">
                        <div class="col-12">
                            <div class="card">
                                <div class="card-body">
                                    <div class="banner-box">
                                        <img src="minible/layouts/assets/images/ปก/1.png" alt="banner">
                                    </div>

                                    <div class="row align-items-center mb-3 mt-3">
                                        <div class="col-6 col-md-6">
                                            <asp:Button ID="addPub" runat="server"
                                                Text="+ เพิ่มข้อมูล"
                                                CssClass="btn btn-primary waves-effect waves-light" />
                                        </div>

                                        <div class="col-6 col-md-6 text-end">
                                            <asp:DropDownList ID="ddlYearPub1" runat="server"
                                                CssClass="form-select d-inline-block w-auto"
                                                AutoPostBack="true"
                                                OnSelectedIndexChanged="ddlYearPub1_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </div>

                                    </div>

                                    <asp:GridView ID="GridViewPub" runat="server" AutoGenerateColumns="False"
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
                                            <asp:TemplateField HeaderStyle-CssClass="align-content-center" HeaderText="ชื่อเรื่อง">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSub" runat="server" Text='<%# Eval("title") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="align-content-center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="editPub" runat="server" AutoPostBack="True" CommandArgument="<%# Container.DataItemIndex %>" OnClick="LinkButton1_Click"><i class="fas fa-edit"></i></asp:LinkButton>
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

                <asp:Panel ID="panelUpPub" runat="server" Visible="True">
                    <div class="row">
                        <div class="col-lg-12">

                            <div class="card">
                                <div class="card-body">
                                    <div>
                                        <h4 class="mb-4" style="font-weight: bolder">อัพเดตข้อมูลผลงานวิจัยที่ตีพิมพ์</h4>
                                    </div>

                                    <div class="mb-3 row">
                                        <label class="form-label col-md-3">ปีงบประมาณ</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="ddlYearPub2" runat="server" CssClass="form-select"
                                                AutoPostBack="true" OnSelectedIndexChanged="ddlYearPub2_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="mb-3 row align-items-center">
                                        <label class="form-label col-md-3">เดือน</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="ddlMonth" runat="server" CssClass="form-select">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="mb-3 row">
                                        <label class="form-label col-md-3">ประเภทสิ่งตีพิมพ์</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="ddlType" runat="server" CssClass="form-select">
                                                <asp:ListItem Value="0">--- เลือกประเภทสิ่งตีพิมพ์ ---</asp:ListItem>
                                                <asp:ListItem Value="1">ผลงานวิจัยระดับชาติ</asp:ListItem>
                                                <asp:ListItem Value="2">ผลงานวิจัยระดับนานาชาติ</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>

                                    <div class="mb-3 row">
                                        <label for="example-text-input" class="col-md-3 col-form-label ">ชื่อของข้อมูล</label>
                                        <div class="col-md-6">
                                            <asp:TextBox ID="txtTitle" CssClass="form-control" runat="server" type="text" autocomplete="off"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="mb-3 row">
                                        <label for="example-text-input" class="col-md-3 col-form-label ">ชื่อผู้แต่ง</label>
                                        <div class="col-md-6">
                                            <asp:TextBox ID="txtAuthors" CssClass="form-control" runat="server" type="text" autocomplete="off"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="mb-3 row">
                                        <label for="example-text-input" class="col-md-3 col-form-label ">Scopus_source</label>
                                        <div class="col-md-6">
                                            <asp:TextBox ID="txtScopus" CssClass="form-control" runat="server" type="text" autocomplete="off"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="mb-3 row">
                                        <label for="example-text-input" class="col-md-3 col-form-label ">Volume</label>
                                        <div class="col-md-3">
                                            <asp:TextBox ID="txtVolume" CssClass="form-control" runat="server" type="text" autocomplete="off"></asp:TextBox>
                                        </div>
                                        <label for="example-text-input" class="col-md-1 col-form-label ">Issue</label>
                                        <div class="col-md-2">
                                            <asp:TextBox ID="txtIssue" CssClass="form-control" runat="server" type="text" autocomplete="off"></asp:TextBox>
                                        </div>
                                    </div>


                                    <div class="mb-3 row">
                                        <label for="example-text-input" class="col-md-3 col-form-label ">Pages</label>
                                        <div class="col-md-6">
                                            <asp:TextBox ID="txtPages" CssClass="form-control" runat="server" type="text" autocomplete="off"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="mb-3 row">
                                        <label for="example-text-input" class="col-md-3 col-form-label ">DOI</label>
                                        <div class="col-md-6">
                                            <asp:TextBox ID="txtDOI" CssClass="form-control" runat="server" type="text" autocomplete="off"></asp:TextBox>
                                        </div>
                                    </div>

                                    <!-- International -->
                                    <div class="mb-3 row">
                                        <div class="col-md-3">
                                            <asp:CheckBox ID="chkInter" runat="server" Text="International Collaboration"
                                                AutoPostBack="true" OnCheckedChanged="chkInter_CheckedChanged" />
                                        </div>


                                    </div>

                                    <asp:Panel ID="pnlInter" runat="server" Visible="false">

                                        <div class="row mb-2">

                                            <div class="col-md-2">
                                            </div>

                                            <div class="col-md-4">
                                                <asp:TextBox ID="txtInterCollab" runat="server" CssClass="form-control" placeholder="University"></asp:TextBox>
                                            </div>

                                            <div class="col-md-4">
                                                <asp:DropDownList ID="ddlCountryInter"
                                                    runat="server"
                                                    CssClass="form-control select2"
                                                    Style="width: 100%;">
                                                </asp:DropDownList>
                                                <%-- <asp:DropDownList ID="ddlCountryInter" runat="server" CssClass="form-control select2">
</asp:DropDownList>--%>
                                            </div>

                                            <div class="col-md-2">
                                                <asp:Button ID="btnAddInter" runat="server" Text="เพิ่ม" CssClass="btn btn-primary" OnClick="btnAddInter_Click" />
                                            </div>
                                        </div>
                                        <div class="row mb-2">
                                            <div class="col-md-2">
                                            </div>
                                            <div class="col-md-8">
                                                <asp:Label ID="lblInter" runat="server" CssClass="form-control" Text="กรณีที่มี ระบุ International Collaboration"></asp:Label>
                                            </div>
                                        </div>
                                    </asp:Panel>


                                    <!-- Academic -->
                                    <div class="mb-3 row mt-3">
                                        <div class="col-md-3">
                                            <asp:CheckBox ID="chkAcademic" runat="server" Text="Academic-Corporate Collaboration"
                                                AutoPostBack="true" OnCheckedChanged="chkAcademic_CheckedChanged" />
                                        </div>
                                    </div>

                                    <asp:Panel ID="pnlAcademic" runat="server" Visible="false">

                                        <div class="row mb-2">
                                            <div class="col-md-2">
                                            </div>

                                            <div class="col-md-4">
                                                <asp:TextBox ID="txtacademicCollab" runat="server" CssClass="form-control" placeholder="University"></asp:TextBox>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:DropDownList ID="ddlCountryAcademic"
                                                    runat="server"
                                                    CssClass="form-control select2"
                                                    Style="width: 100%;">
                                                </asp:DropDownList>
                                            </div>

                                            <div class="col-md-2">
                                                <asp:Button ID="btnAddAcademic" runat="server" Text="เพิ่ม" CssClass="btn btn-primary" OnClick="btnAddAcademic_Click" />
                                            </div>

                                        </div>
                                        <div class="row mb-2">

                                            <div class="col-md-2">
                                            </div>
                                            <div class="col-md-8">
                                                <asp:Label ID="lblAcademic" runat="server" CssClass="form-control" Text="กรณีที่มี ระบุ Academic-Corporate Collaboration"></asp:Label>
                                            </div>
                                        </div>
                                    </asp:Panel>

                                    <%--                                    <div class="mb-3 row">
                                        <label class="form-label col-md-3">KPI</label>
                                        <div class="col-md-6">

                                            <asp:CheckBoxList ID="cblProject" runat="server" CssClass="form-check">
                                            </asp:CheckBoxList>
                                        </div>
                                    </div>--%>
                                    <!-- ====== Q & Ranking ====== -->
                                    <div class="mb-3 row">
                                        <label class="col-md-3 col-form-label">Quartile / Ranking</label>

                                        <div class="col-md-6">
                                            <asp:CheckBoxList ID="cblQ" runat="server"
    RepeatDirection="Vertical">

    <asp:ListItem Value="Q1">Q1</asp:ListItem>

    <asp:ListItem Value="Top1" Attributes-CssClass="indent">Top1</asp:ListItem>
    <asp:ListItem Value="Top10" Attributes-CssClass="indent">Top10</asp:ListItem>

    <asp:ListItem Value="Q2">Q2</asp:ListItem>
    <asp:ListItem Value="Q3">Q3</asp:ListItem>
    <asp:ListItem Value="Q4">Q4</asp:ListItem>

</asp:CheckBoxList>
                                        </div>
                                    </div>

                                    <!-- ====== TCI ====== -->
                                    <div class="mb-3 row">
                                        <label class="col-md-3 col-form-label">TCI</label>
                                        <div class="col-md-6">
                                            <asp:CheckBoxList ID="cblTCI" runat="server" RepeatDirection="Vertical">
                                                <asp:ListItem Value="TCI_G1">กลุ่ม1</asp:ListItem>
                                                <asp:ListItem Value="TCI_G2">กลุ่ม2</asp:ListItem>
                                                <asp:ListItem Value="TCI_G3">กลุ่ม3</asp:ListItem>
                                            </asp:CheckBoxList>
                                        </div>
                                    </div>

                                    <!-- ====== SDG ====== -->
                                    <div class="mb-3 row">
                                        <label class="col-md-3 col-form-label">SDG</label>
                                        <div class="col-md-9">
                                            <asp:CheckBoxList ID="cblSDG" runat="server" RepeatDirection="Vertical">
                                                <asp:ListItem Value="1">SDG 1: ขจัดความยากจน (No Poverty)</asp:ListItem>
                                                <asp:ListItem Value="2">SDG 2: ขจัดความหิวโหย (Zero Hunger)</asp:ListItem>
                                                <asp:ListItem Value="3">SDG 3: สุขภาพและความเป็นอยู่ที่ดี (Good Health and Well-being)</asp:ListItem>
                                                <asp:ListItem Value="4">SDG 4: การศึกษาที่มีคุณภาพ (Quality Education)</asp:ListItem>
                                                <asp:ListItem Value="5">SDG 5: ความเท่าเทียมทางเพศ (Gender Equality)</asp:ListItem>
                                                <asp:ListItem Value="6">SDG 6: น้ำสะอาดและสุขาภิบาล (Clean Water and Sanitation)</asp:ListItem>
                                                <asp:ListItem Value="7">SDG 7: พลังงานสะอาดที่ทุกคนเข้าถึงได้ (Affordable and Clean Energy)</asp:ListItem>
                                                <asp:ListItem Value="8">SDG 8: งานที่มีคุณค่าและการเติบโตทางเศรษฐกิจ (Decent Work and Economic Growth)</asp:ListItem>
                                                <asp:ListItem Value="9">SDG 9: อุตสาหกรรม นวัตกรรม และโครงสร้างพื้นฐาน (Industry, Innovation and Infrastructure)</asp:ListItem>
                                                <asp:ListItem Value="10">SDG 10: ลดความเหลื่อมล้ำ (Reduced Inequalities)</asp:ListItem>
                                                <asp:ListItem Value="11">SDG 11: เมืองและชุมชนที่ยั่งยืน (Sustainable Cities and Communities)</asp:ListItem>
                                                <asp:ListItem Value="12">SDG 12: การบริโภคและการผลิตที่ยั่งยืน (Responsible Consumption and Production)</asp:ListItem>
                                                <asp:ListItem Value="13">SDG 13: การรับมือการเปลี่ยนแปลงสภาพภูมิอากาศ (Climate Action)</asp:ListItem>
                                                <asp:ListItem Value="14">SDG 14: ทรัพยากรทางทะเล (Life Below Water)</asp:ListItem>
                                                <asp:ListItem Value="15">SDG 15: ระบบนิเวศบนบก (Life on Land)</asp:ListItem>
                                                <asp:ListItem Value="16">SDG 16: สันติภาพ ความยุติธรรม และสถาบันที่เข้มแข็ง (Peace, Justice and Strong Institutions)</asp:ListItem>
                                                <asp:ListItem Value="17">SDG 17: ความร่วมมือเพื่อการพัฒนาที่ยั่งยืน (Partnerships for the Goals)</asp:ListItem>
                                            </asp:CheckBoxList>
                                        </div>
                                    </div>

                                </div>
                            </div>
                        </div>
                    </div>
                    <asp:HiddenField ID="hfPubNo" runat="server" />

                    <div class="text-center mt-3 mb-4">
                        <asp:Button ID="btnsubmitPub" runat="server" Text="Save" CssClass="btn bg-soft-success waves-effect waves-light" Width="120px" Visible="False" />
                        <asp:Button ID="btnupdatePub" runat="server" Text="Update" CssClass="btn bg-soft-primary waves-effect waves-light" Width="120px" Visible="False" />
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn bg-soft-danger waves-effect waves-light" Width="120px" OnClientClick="cancelValidation(); return true;" Visible="True" />

                    </div>
                </asp:Panel>




            </div>
        </div>
    </form>

    <!-- Magnific Popup-->
    <script src="minible/layouts/assets/libs/magnific-popup/jquery.magnific-popup.min.js"></script>

    <!-- lightbox init js-->
    <%--ต้องไว้ข้างล่างเท่านั้น ไว้ข้างบนไม่ทำงาน--%>
    <script src="minible/layouts/assets/js/pages/lightbox.init.js"></script>
    <script src="minible/layouts/assets/js/app.js"></script>
    <%--<script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.2/jquery.min.js"></script>--%>
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
    <script src="minible/layouts/assets/libs/select2/js/select2.min.js"></script>
    <script>
        document.addEventListener("DOMContentLoaded", function () {

            const cbl = document.getElementById("<%= cblQ.ClientID %>");
        const checkboxes = cbl.querySelectorAll("input[type=checkbox]");

        function getCheckbox(value) {
            for (let cb of checkboxes) {
                if (cb.value === value) return cb;
            }
            return null;
        }

        checkboxes.forEach(cb => {
            cb.addEventListener("change", function () {

                const q1 = getCheckbox("Q1");
                const top1 = getCheckbox("Top1");
                const top10 = getCheckbox("Top10");

                if (this.value === "Top10" && this.checked) {
                    if (top1) top1.checked = true;
                    if (q1) q1.checked = true;
                }

                if (this.value === "Top1" && this.checked) {
                    if (q1) q1.checked = true;
                }

                // ❗ ไม่ต้อง reverse ตามที่คุณต้องการ
            });
        });

    });
    </script>
</body>
</html>
