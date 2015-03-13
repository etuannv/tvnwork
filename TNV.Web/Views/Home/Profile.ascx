<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TNV.Web.Models.UserModel>" %>
<%if (ViewData["Role"].ToString().Trim()=="3")
       {%>
        <div class="NewsGroup">
            <div class="title">
                Thông tin thành viên
            </div>
            <div class="content">
                <div class="LabelInput">
                    Tên thành viên: <b><%: Model.FullName %></b>
                </div>
                <div class="LabelInput">
                    Loại thành viên: <b><%: Model.RoleDescription%></b>
                </div>
                <div class="LabelInput">
                    Ngày sinh: <b><%: Model.NgaySinh.ToString("dd/MM/yyyy")%></b>
                </div>
                <div class="LabelInput">
                    Địa chỉ Email: <b><%: Model.Email%></b>
                </div>
                <div class="LabelInput">
                    Địa chỉ liên hệ: <b><%: Model.TenHuyen%> - <%: Model.TenTinh%></b>
                </div>
                <div class="LabelInput">
                   Học sinh của trường: <b><%: Model.SchoolName%></b>
                </div>
                <div class="LabelInput">
                    Điện thoại liên hệ: <b><%: Model.MobileAlias%></b>
                </div>
                <div class="LabelInput">
                    Ngày đăng ký: <b><%: Model.CreateDate.ToString("dd/MM/yyyy") %></b>
                </div>
                <div class="LabelInput">
                    Đăng nhập lần thứ: <b><%: Model.LoginNumber %></b>
                </div>
            </div>
        </div>
        <%}
          else if (ViewData["Role"].ToString().Trim() == "2")
           {%>
            <div class="NewsGroup">
                <div class="title">
                    Thông tin thành viên
                </div>
                <div class="content">
                   <div class="LabelInput">
                        Tên thành viên: <b><%: Model.FullName %></b>
                    </div>
                    <div class="LabelInput">
                        Loại thành viên: <b><%: Model.RoleDescription%></b>
                    </div>
                    <div class="LabelInput">
                        Ngày sinh: <b><%: Model.NgaySinh.ToString("dd/MM/yyyy")%></b>
                    </div>
                    <div class="LabelInput">
                        Địa chỉ Email: <b><%: Model.Email%></b>
                    </div>
                    <div class="LabelInput">
                        Địa chỉ liên hệ: <b><%: Model.TenHuyen%> - <%: Model.TenTinh%></b>
                    </div>
                    <div class="LabelInput">
                       Học sinh của trường: <b><%: Model.SchoolName%></b>
                    </div>
                    <div class="LabelInput">
                        Điện thoại liên hệ: <b><%: Model.MobileAlias%></b>
                    </div>
                    <div class="LabelInput">
                        Ngày đăng ký: <b><%: Model.CreateDate.ToString("dd/MM/yyyy") %></b>
                    </div>
                    <div class="LabelInput">
                        Ngày tính phí: <b><%: Model.StartDate.ToString("dd/MM/yyyy") %></b>
                    </div>
                    <div class="LabelInput">
                        Ngày hết hạn: <b><%: Model.ExpiredDate.ToString("dd/MM/yyyy") %></b>
                    </div>
                    <div class="LabelInput">
                        Đăng nhập lần thứ: <b><%: Model.LoginNumber %></b>
                    </div>
                </div>
            </div>
            <%} 
              else
              {%>
                  <div class="NewsGroup">
                    <div class="title">
                        Thông tin thành viên
                    </div>
                    <div class="content">
                        <div class="LabelInput">
                            Tên thành viên: <b><%: Model.FullName %></b>
                        </div>
                        <div class="LabelInput">
                            Loại thành viên: <b><%: Model.RoleDescription %></b>
                        </div>
                        <div class="LabelInput">
                            Đăng nhập lần thứ: <b><%: Model.LoginNumber %></b>
                        </div>
                    </div>
                </div>

              <%} %>

