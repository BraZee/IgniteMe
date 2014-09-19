<%@ Page Title="" Language="C#" MasterPageFile="~/SiteChild.master" AutoEventWireup="true" CodeBehind="EventSocialMediaPage.aspx.cs" Inherits="Ignite.Organisations.Events.EventSocialMediaPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="NestedContent" runat="server">
    
     <div class="panel panel-info">
        <div class="panel-heading" style="background-color: #6AA6D6">
            <asp:LinkButton ID="btnEventCategories" ForeColor="white" runat="server" Text="Categories" CssClass="backTo" OnClick="btnEventCategories_OnClick"/>
            <asp:LinkButton ID="btnEventCoordinator" ForeColor="white" runat="server" Text="Coordinator" CssClass="backTo" OnClick="btnEventCoordinator_OnClick"/>
            <asp:LinkButton ID="btnEventInterests" ForeColor="white" runat="server" Text="Interests" CssClass="backTo" OnClick="btnEventInterests_OnClick"/>
            <asp:LinkButton ID="btnEventSessions" Visible="False" ForeColor="white" runat="server" Text="Sessions" CssClass="backTo" OnClick="btnEventSessions_OnClick"/>
            <asp:LinkButton ID="btnEventSocialMediaPage" runat="server" ForeColor="white" Text="Social Media Page" CssClass="backTo" OnClick="btnEventSocialMediaPage_OnClick"/>
            <asp:LinkButton ID="btnScheduleAlerts" ForeColor="white" runat="server" Text="Schedule Alerts" CssClass="backTo" OnClick="btnScheduleAlerts_OnClick"/>
            <asp:LinkButton ID="btnEventPricing" ForeColor="white" runat="server" Text="Pricing" CssClass="backTo" OnClick="btnEventPricing_OnClick"/>
            <asp:LinkButton ID="btnViewAttendance" ForeColor="white" runat="server" Text="View Attendance" CssClass="backTo" OnClick="btnViewAttendance_OnClick"/>
            <asp:LinkButton ID="btnFeedback" ForeColor="white" runat="server" Text="Respond to Feedback" CssClass="backTo" OnClick="btnFeedback_OnClick"/>
            <asp:LinkButton ID="btnMonitirinterest" ForeColor="white" runat="server" Text="Monitor Interest" CssClass="backTo" OnClick="btnMonitirinterest_OnClick" />
            
        </div>
    </div>
    

       
    <div class="panel panel-info">
        <div class="panel-heading" style="background-color: #6AA6D6">
            <h5 style="color: white">Social Media Page(s) for <asp:Label runat="server" ID="lblEventName"/><asp:LinkButton runat="server" ForeColor="white" ID="btnNew" CssClass="pull-right" OnClick="btnNew_OnClick">&#x2B;Add New Social Media Page</asp:LinkButton></h5>
        </div>
        <div class="panel-body">
            <asp:GridView ID="dgvSocialMedia" EmptyDataText="No Social Media Page(s)." runat="server" DataKeyNames="Id" AutoGenerateColumns="false" CssClass="table table-hover table-striped" HorizontalAlign="Center" GridLines="None" BorderStyle="Solid" OnRowEditing="dgvSocialMedia_OnRowEditing" OnRowDeleting="dgvSocialMedia_OnRowDeleting">
                <Columns>
                    <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn"/>
                    <asp:BoundField DataField="Type" HeaderText="Social Media" SortExpression="Type" />
                    <asp:BoundField DataField="URL" HeaderText="URL" SortExpression="URL" />
                    <asp:BoundField DataField="Active" HeaderText="Active" SortExpression="Active" />
                    <asp:CommandField ShowEditButton="True" EditText="">
                            <ControlStyle CssClass=" fa fa-pencil" ForeColor="blue"/>
                        </asp:CommandField>
                        <asp:CommandField ShowDeleteButton="True" DeleteText="">
                            <ControlStyle CssClass="fa fa-trash-o" ForeColor="red"/>
                        </asp:CommandField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
    
   
    <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true" style="display: none;">
        <div class="modal-dialog" style="width: 800px">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title" id="myModalLabel">Add/Edit Social Media Page</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="form-group col-md-12">
                                <div class="col-md-3">
                                    <label class="control-label">URL:</label>
                                </div>
                                <div class="col-md-8 ">
                                    <asp:TextBox runat="server" ID="txtURL" CssClass="form-control" ValidationGroup="socialMediaValidation"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Name Required!" Display="Dynamic" ValidationGroup="socialMediaValidation" CssClass="label label-danger" ControlToValidate="txtURL"></asp:RequiredFieldValidator>
                                </div>
                            <div class="col-md-1">
                                    <asp:Label ID="Label1" runat="server" Text="*" ForeColor="red"></asp:Label>
                                </div>
                            </div>
                            <div class="form-group col-md-12">
                                    <div class="col-md-3">
                                        <label class="control-label">Social Media Type:</label>
                                    </div>
                                    <div class="col-md-9">
                                        <asp:DropDownList runat="server" ID="cbType" CssClass="form-control"/>
                                    </div>
                                </div>
                                <div class="form-group col-md-12">
                                    <div class="col-md-3">
                                        <label class="control-label">Active:</label>
                                    </div>
                                    <div class="col-md-9">
                                        <asp:CheckBox runat="server" ID="chkActive" CssClass="form-control" Enabled="False" Checked="True"/>
                                  </div>
                                </div>
                            </div>
                      </div>
                <div class="modal-footer">
                    <asp:LinkButton ID="btnCloseModal" CssClass="btn btn-default" ClientIDMode="Static" runat="server" OnClick="btnCloseModal_OnClick">Cancel</asp:LinkButton>
                    <asp:LinkButton ID="btnSaveSocialMediaPage" CssClass="btn btn-success" ClientIDMode="Static" runat="server" ValidationGroup="Validation" OnClick="btnSaveSession_OnClick">Save</asp:LinkButton>
               </div>
            </div>
            <!-- /.modal-content -->
        </div>
        <!-- /.modal-dialog -->
    </div>
    

</asp:Content>
