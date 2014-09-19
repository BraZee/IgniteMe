<%@ Page Title="" Language="C#" MasterPageFile="~/SiteChild.master" AutoEventWireup="true" CodeBehind="ManageInterests.aspx.cs" Inherits="Ignite.Admin.ManageInterests" %>
<asp:Content ID="Content1" ContentPlaceHolderID="NestedContent" runat="server">
    
      <div class="panel panel-info">
        <div class="panel-heading" style="background-color: #6AA6D6">
            <h5 style="color: white">Interest Management<asp:LinkButton runat="server" ForeColor="white" ID="btnNew" CssClass="pull-right" OnClick="btnNew_OnClick">&#x2B;Add New Interest</asp:LinkButton></h5>

        </div>
        <div class="panel-body">
            <asp:GridView ID="dgvInterests" runat="server" OnRowDeleting="dgvInterests_OnRowDeleting" OnRowEditing="dgvInterests_OnRowEditing" DataKeyNames="Id" AutoGenerateColumns="false" CssClass="table table-hover table-striped" HorizontalAlign="Center" GridLines="None" BorderStyle="Solid">
                <Columns>
                    <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="False" />
                    <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                    <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description" />
                    <asp:BoundField DataField="Active" HeaderText="Active" SortExpression="Active" />
                    <asp:CommandField ShowEditButton="True" EditText="">
                        <ControlStyle CssClass=" fa fa-edit" />
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
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title" id="myModalLabel">Add/Edit Interest</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="form-group col-md-12">
                                <div class="col-md-3">
                                    <label class="control-label">Name:</label>
                                </div>
                                <div class="col-md-8">
                                    <asp:TextBox runat="server" ID="txtName" CssClass="form-control" ValidationGroup="interestValidation"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Name Required!" CssClass="label label-danger" Display="Dynamic" ValidationGroup="interestValidation" ControlToValidate="txtName"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-1">
                                    <asp:Label ID="Label1" runat="server" Text="*" ForeColor="red"></asp:Label>
                                </div>
                            </div>
                            <div class="form-group col-md-12">
                                <div class="col-md-3">
                                    <label class="control-label">Description:</label>
                                </div>
                                <div class="col-md-8">
                                    <%--                                    <asp:TextBox Rows="5" runat="server" ID="txtFirstName" CssClass="form-control" Display="Dynamic" ValidationGroup="interestValidation"></asp:TextBox>--%>
                                    <textarea runat="server" id="txtDescription" class="form-control"></textarea>
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
                    <asp:LinkButton ID="btnSaveInterest" CssClass="btn btn-primary" ClientIDMode="Static" runat="server" ValidationGroup="interestValidation" OnClick="btnSaveInterest_OnClick">Save</asp:LinkButton>
                </div>
            </div>
            <!-- /.modal-content -->
        </div>
        <!-- /.modal-dialog -->
    </div>



</asp:Content>
