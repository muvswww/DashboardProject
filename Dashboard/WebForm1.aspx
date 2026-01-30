<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="WebForm1.aspx.vb" Inherits="Dashboard.WebForm1" %>

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
                    <asp:Panel ID="Panel1" runat="server">
                        <h2 class="text-center mb-5">อัพเดทข้อมูลส่วนงานวิจัย</h2>
                       <div class="row mt-2">
                    <div class="col-md-6 col-xl-4">
        
    <div class="card">
        <img class="card-img-top img-fluid" src="minible/layouts/assets/images/Research1.png" >
        <div class="card-body">
            <p class="card-text font-size-20 text-center"><b>ผลงานวิจัยที่ตีพิมพ์</b></p>
        </div>
    </div>
        
</div>
                    <div class="col-md-6 col-xl-4">
        
    <div class="card">
        <img class="card-img-top img-fluid" src="minible/layouts/assets/images/Research2.png" >
        <div class="card-body">
            <p class="card-text font-size-20 text-center"><b>ทุนวิจัย และบริการวิชาการ</b></p>
        </div>
    </div>
        
</div>
                    <div class="col-md-6 col-xl-4">
        
    <div class="card">
        <img class="card-img-top img-fluid" src="minible/layouts/assets/images/Research3.png" >
        <div class="card-body">
            <p class="card-text font-size-20 text-center"><b>นวัตกรรม</b></p>
        </div>
    </div>
        
</div>
</div>
</asp:Panel>
                    <asp:Panel ID="Panel_Public" runat="server">

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
