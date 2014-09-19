<%@ Page Title="" Language="C#" MasterPageFile="~/SiteChild.master" AutoEventWireup="true" CodeBehind="EventSessions.aspx.cs" Inherits="Ignite.Organisations.Events.EventSessions" %>

<asp:Content ID="Content1" ContentPlaceHolderID="NestedContent" runat="server">

    <div class="panel panel-info">
        <div class="panel-heading" style="background-color: #6AA6D6">
            <asp:LinkButton ID="btnEventCategories" ForeColor="white" runat="server" Text="Categories" CssClass="backTo" OnClick="btnEventCategories_OnClick" />
            <asp:LinkButton ID="btnEventCoordinator" ForeColor="white" runat="server" Text="Coordinator" CssClass="backTo" OnClick="btnEventCoordinator_OnClick" />
            <asp:LinkButton ID="btnEventInterests" ForeColor="white" runat="server" Text="Interests" CssClass="backTo" OnClick="btnEventInterests_OnClick" />
            <asp:LinkButton ID="btnEventSessions" ForeColor="white" runat="server" Text="Sessions" CssClass="backTo" OnClick="btnEventSessions_OnClick" />
            <asp:LinkButton ID="btnEventSocialMediaPage" runat="server" ForeColor="white" Text="Social Media Page" CssClass="backTo" OnClick="btnEventSocialMediaPage_OnClick" />
            <asp:LinkButton ID="btnScheduleAlerts" ForeColor="white" runat="server" Text="Schedule Alerts" CssClass="backTo" OnClick="btnScheduleAlerts_OnClick" />
            <asp:LinkButton ID="btnEventPricing" ForeColor="white" runat="server" Text="Pricing" CssClass="backTo" OnClick="btnEventPricing_OnClick" />
            <asp:LinkButton ID="btnViewAttendance" ForeColor="white" runat="server" Text="View Attendance" CssClass="backTo" OnClick="btnViewAttendance_OnClick" />
            <asp:LinkButton ID="btnFeedback" ForeColor="white" runat="server" Text="Respond to Feedback" CssClass="backTo" OnClick="btnFeedback_OnClick" />
            <asp:LinkButton ID="btnMonitirinterest" ForeColor="white" runat="server" Text="Monitor Interest" CssClass="backTo" OnClick="btnMonitirinterest_OnClick" />

        </div>
    </div>



    <div class="panel panel-info">
        <div class="panel-heading" style="background-color: #6AA6D6">
            <h5 style="color: white">Session(s) for
                <asp:Label runat="server" ID="lblEventName" /><asp:LinkButton runat="server" ForeColor="white" ID="btnNew" CssClass="pull-right" OnClick="btnNew_OnClick">&#x2B;Add New Session</asp:LinkButton></h5>

        </div>
        <div class="panel-body">
            <asp:GridView ID="dgvSessions" EmptyDataText="No Sessions." runat="server" DataKeyNames="Id" AutoGenerateColumns="false" CssClass="table table-hover table-striped" HorizontalAlign="Center" GridLines="None" BorderStyle="Solid" OnRowEditing="dgvSessions_OnRowEditing" OnRowDeleting="dgvSessions_OnRowDeleting">
                <Columns>
                    <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn" />
                    <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                    <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description"/>
                    <asp:BoundField DataField="BeginTime" HeaderText="Begins" SortExpression="BeginTime" />
                    <asp:BoundField DataField="EndTime" HeaderText="Ends" SortExpression="EndTime" />
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

    <asp:TextBox runat="server" ID="timeBeginCheck" TextMode="Time" ValidationGroup="eventValidation" CssClass="hideGridColumn" />
    <asp:TextBox runat="server" ID="timeEndCheck" TextMode="Time" ValidationGroup="eventValidation" CssClass="hideGridColumn" />

    <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true" style="display: none;">
        <div class="modal-dialog" style="width: 800px">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title" id="myModalLabel">Add/Edit Session</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="form-group col-md-12">
                            <div class="col-md-3">
                                <label class="control-label">Name:</label>
                            </div>
                            <div class="col-md-8">
                                <asp:TextBox runat="server" ID="txtName" CssClass="form-control" ValidationGroup="sessionValidation"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Name Required!" Display="Dynamic" ValidationGroup="sessionValidation" CssClass="label label-danger" ControlToValidate="txtName"></asp:RequiredFieldValidator>
                            </div>
                            <div class="col-md-1">
                                <asp:Label runat="server" Text="*" ForeColor="red"></asp:Label>
                            </div>
                        </div>
                        <div class="form-group col-md-12">
                            <div class="col-md-3">
                                <label class="control-label">Description:</label>
                            </div>
                            <div class="col-md-8">
                                <asp:TextBox runat="server" ID="txtDescription" TextMode="MultiLine" Rows="5" CssClass="form-control" ValidationGroup="sessionValidation"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Description Required!" Display="Dynamic" ValidationGroup="sessionValidation" CssClass="label label-danger" ControlToValidate="txtDescription"></asp:RequiredFieldValidator>
                            </div>
                            <div class="col-md-1">
                                <asp:Label ID="Label3" runat="server" Text="*" ForeColor="red"></asp:Label>
                            </div>
                        </div>
                        <div class="form-group col-md-12">
                            <div class="col-md-3">
                                <label class="control-label">Begin Time:</label>
                            </div>
                            <div class="col-md-8">
                                <asp:TextBox runat="server" ID="timeBegin" CssClass="form-control" TextMode="Time" ValidationGroup="sessionValidation"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Begin Time required!" Display="Dynamic" ValidationGroup="sessionValidation" CssClass="label label-danger" ControlToValidate="timeBegin"></asp:RequiredFieldValidator>
                                <asp:CompareValidator runat="server" ErrorMessage="Session can not start before the event!" CssClass="label label-danger" Display="Dynamic" ValidationGroup="sessionValidation" ControlToValidate="timeBegin" ControlToCompare="timeBeginCheck" Operator="GreaterThanEqual" />
                            </div>
                            <div class="col-md-1">
                                <asp:Label ID="Label1" runat="server" Text="*" ForeColor="red"></asp:Label>
                            </div>
                        </div>
                        <div class="form-group col-md-12">
                            <div class="col-md-3">
                                <label class="control-label">End Time:</label>
                            </div>
                            <div class="col-md-8">
                                <asp:TextBox runat="server" ID="timeEnd" CssClass="form-control" TextMode="Time" ValidationGroup="sessionValidation"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="End time required!" Display="Dynamic" ValidationGroup="sessionValidation" CssClass="label label-danger" ControlToValidate="timeEnd"></asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="Session can not end after the event!" CssClass="label label-danger" Operator="LessThanEqual" Display="Dynamic" ValidationGroup="sessionValidation" ControlToValidate="timeEnd" ControlToCompare="timeEndCheck" />
                                <asp:CompareValidator ID="CompareValidator2" runat="server" ErrorMessage="End time must be greater than begin time." CssClass="label label-danger" Operator="GreaterThan" Display="Dynamic" ValidationGroup="sessionValidation" ControlToValidate="timeEnd" ControlToCompare="timeBegin" />
                            </div>
                            <div class="col-md-1">
                                <asp:Label ID="Label2" runat="server" Text="*" ForeColor="red"></asp:Label>
                            </div>
                        </div>
                        <div class="form-group col-md-12">
                            <div class="col-md-3">
                                <label class="control-label">Active:</label>
                            </div>
                            <div class="col-md-8">
                                <asp:CheckBox runat="server" ID="chkActive" CssClass="form-control" Enabled="False" Checked="True" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:LinkButton ID="btnCloseModal" CssClass="btn btn-default" ClientIDMode="Static" runat="server" OnClick="btnCloseModal_OnClick">Cancel</asp:LinkButton>
                    <asp:LinkButton ID="btnSaveSession" CssClass="btn btn-success" ClientIDMode="Static" runat="server" ValidationGroup="sessionValidation" OnClick="btnSaveSession_OnClick">Save</asp:LinkButton>
                </div>
            </div>
            <!-- /.modal-content -->
        </div>
        <!-- /.modal-dialog -->
    </div>



</asp:Content>
