<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<% if (!Request.IsAuthenticated) 
   {%>
        <div class="uc_wapper">
            <div class="uc_title">
                <h2>Đăng nhập hệ thống</h2>
            </div>
            <div class="dk_content">
                <div class="validation-summary-errors"><%=ViewData["Error"]%></div>
                <div class="LabelInput1">
                    <label for="UserName">Tên đăng nhập:</label>
                </div>

                <div align="Center" class="LabelInput1">
                    <input type="text" value="<%:ViewData["UserName"]%>" style="width:230px" name="UserNameLogin" id="UserName"/>
                    <span id="UserName_validationMessage" class="field-validation-valid"></span>
                </div>
                
                <div class="LabelInput1">
                    <label for="Password">Mật khẩu:</label>
                </div>

                <div align="Center" class="LabelInput1">
                    <input type="password" style="width:230px" name="PasswordLogin" value="<%:ViewData["Password"]%>" id="Password"/>
                    <span id="Password_validationMessage" class="field-validation-valid"></span>
                </div>

                <div class="LabelInput1">
                    <input type="checkbox" value="true" name="RememberMe" id="RememberMe"/>
                    <input type="hidden" value="false" name="RememberMe"/>
                    <label for="RememberMe">Ghi nhớ?</label>
            
                </div>
                <div style="text-align:center" class="LabelInput1">
                    <a onclick="LogOn('QuanTriHeThong','Home','Index', 'LogOn', '','', '','');" class="LogonRegistry">Đăng nhập</a>
                    <a onclick="Submitform('QuanTriHeThong','Home','RegistryFree', '', '','', '','');" class="LogonRegistry">Đăng ký</a>
                </div>
            </div>
        </div>
<%}%>

