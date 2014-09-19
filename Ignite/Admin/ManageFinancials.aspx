<%@ Page Title="" Language="C#" MasterPageFile="~/SiteChild.master" AutoEventWireup="true" CodeBehind="ManageFinancials.aspx.cs" Inherits="Ignite.Admin.ManageFinancials" %>

<asp:Content ID="Content1" ContentPlaceHolderID="NestedContent" runat="server">

    <div class="panel panel-info">
        <div class="panel-heading" style="background-color: #6AA6D6">
            <h5 style="color: white">Admin User Management<a style="color: white" href="#myModal" data-toggle="modal" type="button" class="pull-right">&#x2B;Add New Administrator</a></h5>

        </div>
        <div class="panel-body">
            <!--Table here-->
            <asp:GridView ID="dgvAdminUsers" runat="server" AutoGenerateColumns="false" CssClass="table table-hover table-striped" HorizontalAlign="Center" GridLines="None" BorderStyle="Ridge" OnRowEditing="dgvAdminUsers_OnRowEditing">
                <Columns>
                    <asp:BoundField DataField="id" HeaderText="Id" SortExpression="Id" Visible="True" />
                    <asp:BoundField DataField="description" HeaderText="Name" SortExpression="Name" />
                    <asp:CommandField ShowEditButton="True" EditText="  Edit">
                        <ControlStyle CssClass="btn btn-sm btn-info btn-outline glyphicon glyphicon-edit pull-right" />
                    </asp:CommandField>
                    <asp:CommandField ShowDeleteButton="True" DeleteText="  Delete">
                        <ControlStyle CssClass="fa fa-trash-o" ForeColor="red"/>
                    </asp:CommandField>
                </Columns>
            </asp:GridView>
        </div>
    </div>

    <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true" style="display: none;">
        <div class="modal-dialog">
            <div class="modal-content" style="width:700px">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title" id="myModalLabel">Modal title</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                    <div class="col-md-10">
                    <asp:TextBox ID="result" runat="server" CssClass="form-control" ValidationGroup="valid"></asp:TextBox>
                    
                    </div>
                    <div class="col-md-2">
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="result" ErrorMessage="error!" ValidationGroup="valid"></asp:RequiredFieldValidator>
                    </div>
                        </div>
                    </div>
                <div class="modal-footer">
                    <asp:LinkButton ID="btnCloseModal" data-dismiss="modal" aria-hidden="true" CssClass="btn btn-default" ClientIDMode="Static" runat="server">Cancel</asp:LinkButton>
                  <asp:LinkButton ID="btnsavefacility" CssClass="btn btn-primary" ClientIDMode="Static" runat="server" ValidationGroup="valid">Save</asp:LinkButton>
                </div>
            </div>
            <!-- /.modal-content -->
        </div>
        <!-- /.modal-dialog -->
    </div>
    

</asp:Content>
