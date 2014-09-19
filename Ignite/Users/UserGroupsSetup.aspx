<%@ Page Title="" Language="C#" MasterPageFile="~/SiteChild.master" AutoEventWireup="true" CodeBehind="UserGroupsSetup.aspx.cs" Inherits="Ignite.Users.UserGroupsSetup" %>
<asp:Content ID="Content1" ContentPlaceHolderID="NestedContent" runat="server">
    <div class="panel panel-info">
        <div class="panel-heading" style="background-color: #6AA6D6">
            <h5 style="color: white">User Group Setup<asp:LinkButton runat="server" ForeColor="white" ID="btnNew" CssClass="pull-right" OnClick="btnNew_OnClick">&#x2B;Add New User Group</asp:LinkButton></h5>

        </div> 
        <div class="panel-body">
            <!--Table here-->
            <asp:GridView ID="dgvUserGroups" DataKeyNames="Id" runat="server"  OnRowEditing="dgvUserGroups_OnRowEditing"  AutoGenerateColumns="false" CssClass="table table-hover table-striped" HorizontalAlign="Center" GridLines="None" BorderStyle="Solid" OnRowDeleting="dgvUserGroups_OnRowDeleting">
                <Columns>
                    <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="False" />
                    <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                    <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description" />
                    <asp:BoundField DataField="Active" HeaderText="Active" SortExpression="Active" />
                    <asp:CommandField ShowEditButton="True" EditText="">
                        <ControlStyle CssClass="fa fa-edit" />
                    </asp:CommandField>
                    <asp:CommandField ShowDeleteButton="True" DeleteText="">
                        <ControlStyle CssClass="fa fa-trash-o" ForeColor="red"/>
                    </asp:CommandField>
                </Columns>
            </asp:GridView>
        </div>
    </div>

    <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true" style="display: none;">
        <div class="modal-dialog">
            <div class="modal-content" style="width: 700px">
                <div class="modal-header">
                   <h4 class="modal-title" id="myModalLabel">Add/Edit User Group</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="form-group col-md-12">
                                <div class="col-md-3">
                                    <label class="control-label">Name:</label>
                                </div>
                                <div class="col-md-8">
                                    <asp:TextBox runat="server" ID="txtName" CssClass="form-control"  ValidationGroup="userGroupValidation"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Name Required!" CssClass="label label-danger" Display="Dynamic" ValidationGroup="userGroupValidation" ControlToValidate="txtName"></asp:RequiredFieldValidator>
                                </div>
                                 <div class="col-md-1 pull-left">
                                    <asp:Label ID="Label1" runat="server" ForeColor="red" Text="*"/>
                                </div>
                            </div>
                            <div class="form-group col-md-12">
                                <div class="col-md-3">
                                    <label class="control-label">Description:</label>
                                </div>
                                <div class="col-md-8">
<%--                                    <asp:TextBox Rows="5" runat="server" ID="txtFirstName" CssClass="form-control" Display="Dynamic" ValidationGroup="userGroupValidation"></asp:TextBox>--%>
                                    <textarea runat="server" ID="txtDescription" class="form-control" ></textarea>
                                </div>
                            </div>
                            <div class="form-group col-md-12">
                                <div class="col-md-3">
                                    <label class="control-label">Active:</label>
                                </div>
                                <div class="col-md-8">
                                    <asp:CheckBox runat="server" ID="chkActive" Enabled="False" Checked="True"></asp:CheckBox>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:LinkButton ID="btnCloseModal" CssClass="btn btn-default" ClientIDMode="Static" runat="server" OnClick="btnCloseModal_OnClick">Cancel</asp:LinkButton>
                    <asp:LinkButton ID="btnSaveUserGroup" CssClass="btn btn-primary" ClientIDMode="Static" runat="server" ValidationGroup="userGroupValidation" OnClick="btnSaveUserGroup_OnClick">Save</asp:LinkButton>
                </div>
            </div>
            <!-- /.modal-content -->
        </div>
        <!-- /.modal-dialog -->
    </div>

</asp:Content>
