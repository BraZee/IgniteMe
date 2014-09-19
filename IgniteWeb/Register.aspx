<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="IgniteWeb.Register" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpMain" runat="server">

    <style type="text/css">
        .zeeForms {
            padding-top: 20px;
        }

        .Attention {
            border: 4px solid green;
            padding: 10px 0;
            width: 100px;
            margin: auto;
            display: block;
            text-align: center;
        }
    </style>


    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div id="TableWrapper">
            <div class="row" ID="registration" runat="server" style="margin-top: 40px">
                <div class="col-md-3"></div>
                <div class="col-md-6 white-card extra-padding">
                    <div class="form-group col-md-12">
                        <div class="control-group col-md-12 zeeForms">
                            <div class="col-md-12">
                                <label class="control-label">First Name</label>
                            </div>
                            <div class="col-md-12">
                                <asp:TextBox runat="server" ToolTip="First Name" ID="txtFName" placeholder="Your first name..." CssClass="form-control" />
                                <asp:RequiredFieldValidator Display="Dynamic" runat="server" ErrorMessage="Please enter your first name!" CssClass="label label-danger" ControlToValidate="txtFName" ValidationGroup="registerValidation" />
                            </div>
                        </div>
                        <div class="control-group col-md-12 zeeForms">
                            <div class="col-md-12">
                                <label class="control-label">Last Name</label>
                            </div>
                            <div class="col-md-12">
                                <asp:TextBox runat="server" ToolTip="Last Name" ID="txtLName" placeholder="Your last name..." CssClass="form-control" />
                                <asp:RequiredFieldValidator Display="Dynamic" ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please enter your last name!" CssClass="label label-danger" ControlToValidate="txtLName" ValidationGroup="registerValidation" />
                            </div>
                        </div>
                        <div class="control-group col-md-12 zeeForms">
                            <div class="col-md-12">
                                <label class="control-label">Username</label>
                            </div>
                            <div class="col-md-12">
                                <asp:TextBox runat="server" ToolTip="Username" ClientIDMode="Static" ID="txtUsername" placeholder="Your username..." CssClass="form-control" /><asp:Label runat="server" Text="Username has already been taken" CssClass="label label-danger" Visible="False" ID="usernaemUnav"/>
                                <asp:RequiredFieldValidator Display="Dynamic" ID="RequiredFieldValidator" runat="server" ErrorMessage="Please enter your username!" CssClass="label label-danger" ControlToValidate="txtUsername" ValidationGroup="registerValidation" />
                            </div>
                        </div>
                        <div class="control-group">
                            <div class=" col-md-6 zeeForms">
                                <div class="col-md-12">
                                    <label class="control-label">Email</label>
                                </div>
                                <div class="col-md-12">
                                    <asp:TextBox runat="server" TextMode="Email" ToolTip="Email" ID="txtEmail" placeholder="Email..." CssClass="form-control" />
                                    <asp:RequiredFieldValidator Display="Dynamic" ID="RequiredFieldValidator2" runat="server" ErrorMessage="Please enter your email address!" CssClass="label label-danger" ControlToValidate="txtEmail" ValidationGroup="registerValidation" />
                                    <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="txtEmail" ID="RegularExpressionValidator1" ValidationExpression="\w+([-+.’]\w+)*@\w+([-.]
\w+)*\.\w+([-.]\w+)*"
                                        runat="server" ErrorMessage="Wrong email format!" ValidationGroup="registerValidation" CssClass="label label-danger" />
                                </div>
                            </div>

                            <div class="col-md-6 zeeForms">
                                <div class="col-md-12">
                                    <label class="control-label">Confirm Email</label>
                                </div>
                                <div class="col-md-12">
                                    <asp:TextBox runat="server" TextMode="Email" ToolTip="Confirm Email" ID="txtConfirmEmail" placeholder="Confirm Email..." CssClass="form-control" />
                                    <%--                        <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="txtPassword" ID="RegularExpressionValidator1" ValidationExpression="^[\s\S]{4,}$" runat="server" ErrorMessage="Minimum 4 characters required." ValidationGroup="registerValidation" CssClass="label label-danger"/>--%>
                                    <asp:CompareValidator ID="CompareValidator2" runat="server" ControlToValidate="txtConfirmEmail" Display="Dynamic" ControlToCompare="txtEmail" CssClass="label label-danger" ErrorMessage="Emails do not match!" ValidationGroup="registerValidation" />
                                </div>
                            </div>
                        </div>
                        <div class="control-group">
                            <div class="col-md-6 zeeForms">
                                <div class="col-md-12">
                                    <label class="control-label">Password</label>
                                </div>
                                <div class="col-md-12">
                                    <asp:TextBox runat="server" TextMode="Password" ToolTip="Password" ID="txtPassword" placeholder="Password..." CssClass="form-control" />
                                    <asp:RequiredFieldValidator Display="Dynamic" ID="RequiredFieldValidator3" runat="server" ErrorMessage="Please enter your password!" CssClass="label label-danger" ControlToValidate="txtPassword" ValidationGroup="registerValidation" />
                                    <asp:RegularExpressionValidator SetFocusOnError="True" Display="Dynamic" ControlToValidate="txtPassword" ID="RegularExpressionValidator2" ValidationExpression="^[\s\S]{4,}$" runat="server" ErrorMessage="Minimum 4 characters required." ValidationGroup="registerValidation" CssClass="label label-danger" />
                                </div>
                            </div>
                            <div class="col-md-6 zeeForms">
                                <div class="col-md-12">
                                    <label class="control-label">Confirm Password</label>
                                </div>
                                <div class="col-md-12">
                                    <asp:TextBox runat="server" TextMode="Password" ToolTip="Confirm Password" ID="txtConfirmPassword" placeholder="Confirm Password..." CssClass="form-control" />
                                    <%--                        <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="txtPassword" ID="RegularExpressionValidator1" ValidationExpression="^[\s\S]{4,}$" runat="server" ErrorMessage="Minimum 4 characters required." ValidationGroup="registerValidation" CssClass="label label-danger"/>--%>
                                    <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="txtConfirmPassword" Display="Dynamic" ControlToCompare="txtPassword" CssClass="label label-danger" ErrorMessage="Passwords do not match!" ValidationGroup="registerValidation" />
                                </div>
                            </div>
                        </div>
                        <div class="control-group col-md-12 zeeForms">
                            <div class="col-md-12">
                                <label class="control-label">Security Question</label>
                            </div>
                            <div class="col-md-12">
                                <asp:DropDownList runat="server" ID="cbSecurityQuestions" ToolTip="Security Question" CssClass="form-control" />
                            </div>
                        </div>
                        <div class="control-group col-md-12 zeeForms">
                            <div class="col-md-12">
                                <label class="control-label">Security Answer</label>
                            </div>
                            <div class="col-md-12">
                                <asp:TextBox runat="server" ToolTip="Security Answer" ID="txtAnswer" placeholder="security Answer..." CssClass="form-control" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" Display="Dynamic" runat="server" ErrorMessage="Please enter an answer!" CssClass="label label-danger" ControlToValidate="txtAnswer" ValidationGroup="registerValidation" />
                            </div>
                        </div>
                        <div class="control-group col-md-12 zeeForms">
                            <div class="col-md-12">
                                <asp:CheckBox runat="server" ID="chkAgreement" />
                                <label>I have read and agreed to the <u><a runat="server" id="Terms">Terms And Conditions</a></u>.</label>
                            </div>
                        </div>
                        <div class="control-group col-md-12 zeeForms">
                            <div class="col-md-12">
                                <asp:Button runat="server" ID="btnRegister" Text="Register" AccessKey="" CssClass="btn btn-green pull-right" ForeColor="white" BackColor="green" ValidationGroup="registerValidation"  OnClick="btnRegister_OnClick"/>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-3"></div>
            </div>
            </div>
            <asp:Label ID="Message" runat="server" CssClass="Attention"
                Text="YOU'RE NOW REGISTERED ON IGNITE!!!" Visible="False" />
            <p runat="server" ID="success" visible="False" style="text-align: center">
                Login To Get Started!
            </p>
            <p runat="server" ID="fail" visible="False" style="text-align: center">
                An error occured, try again.
            </p>
            
           
        </ContentTemplate>
       </asp:UpdatePanel>
    
     <script type="text/javascript">
         $(function () {
             $('form').bind('submit', function () {
                 if (Page_ClientValidate()) {
                     $('#TableWrapper').slideUp(3000);
                     // alert("here");
                 } else {
                     alert("error");
                 }
             });
         });

         function pageLoad() {

             $('.Attention').animate({ width: '600px' }, 3000).fadeOut('slow');
         }
            </script>
</asp:Content>
