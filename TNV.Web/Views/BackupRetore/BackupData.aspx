<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<TNV.Web.Models.FileUploadModel>>" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">   
        <table width="100%" border="0px">
        <tr>
            <td width="71%" valign="top">
               <div class="TitleNomal">SAO LƯU DỮ LIỆU</div>
                    <div style="padding-left:100px; padding-right:100px;">
                    <fieldset>
                        <table id="Table2"  width="480px">
                            <tr>
                                <td width="180px" align="right">Ngày sao lưu:</td>
                                <td  width="120px" align="left">
                                    <input type="text" id="BackupDate" name="BackupDate" value="<%:ViewData["BackupDate"] %>" readonly="readonly" style="width: 75px; margin-right:3px" />
                                    <script type="text/javascript">
                                        $(document).ready(function () { $('#BackupDate').datepicker({ showOn: 'button', buttonImage: '/Content/image/calendar.gif', duration: 0 }); });
                                    </script>
                                </td>
                                <td width="180px" align="left">
                                    <img class="ImageButton" onclick="Submitform('QuanTriHeThong','BackupRetore','MakeBackupData', '', '','');"  src='<%: Url.Content("~/Content/Image/SaoLuu.png") %>' onmouseout="this.style.opacity=100" onmouseover="this.style.opacity=0.5"/>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </div>
                <br />
                <div style="padding-left:20px; padding-right:20px;">
                    <fieldset style="width:650px">
                        <h2 class="SmallTitleNomal">HƯỚNG DẪN SAO LƯU DỮ LIỆU</h2>
                        <p class="huongdan">
                            <b>Sao lưu dữ liệu</b> là công việc quan trọng để bảo đảm an toàn dữ liệu khi có sự cố xảy ra. 
                            Sao lưu dữ liệu là việc tạo ra một phiên bản dữ liệu giống hoàn toàn bản gốc và lưu sang một vị trí khác, 
                            khi có sự cố xảy ra có thể đem bản dữ liệu sao lưu ra để sử dụng. <br />
                            &nbsp;&nbsp;&nbsp;&nbsp; 1. Chọn ngày sao lưu <br />
                            &nbsp;&nbsp;&nbsp;&nbsp; 2. Bấm nút "Sao lưu dữ liệu" <br />
                            &nbsp;&nbsp;&nbsp;&nbsp; 3. Download file sao lưu về lưu tại máy tính cá nhân <br />
                            <span class="CacBuoc">Ghi chú:</span> Tên các file sao lưu đều có dạng:<br /> 
                            &nbsp;&nbsp;&nbsp;&nbsp;<b>Backup_ToanThongMinh_1649080447(23_01_2013).bak</b>. <br /> 
                            &nbsp;&nbsp;&nbsp;&nbsp;Trong đó: 23_01_2013 là ngày sao lưu, ToanThongMinh là tên CSDL sao lưu
                        </p>
                        </fieldset> 
                </div>
            </td>
            <td width="29%" valign="top" align="center">
                <%--Thêm nội dung cột bên phải--%>
            </td>
        </tr>
        </table>
</asp:Content>

