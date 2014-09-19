<%@ Page Title="" Language="C#" MasterPageFile="~/SiteChild.master" AutoEventWireup="true" CodeBehind="EventAlerts.aspx.cs" Inherits="Ignite.Organisations.Events.EventAlerts" %>

<asp:Content ID="Content1" ContentPlaceHolderID="NestedContent" runat="server">

    <div class="panel panel-info">
        <div class="panel-heading" style="background-color: #6AA6D6">
            <asp:LinkButton ID="btnEventCategories" ForeColor="white" runat="server" Text="Categories" CssClass="backTo" OnClick="btnEventCategories_OnClick" />
            <asp:LinkButton ID="btnEventCoordinator" ForeColor="white" runat="server" Text="Coordinator" CssClass="backTo" OnClick="btnEventCoordinator_OnClick" />
            <asp:LinkButton ID="btnEventInterests" ForeColor="white" runat="server" Text="Interests" CssClass="backTo" OnClick="btnEventInterests_OnClick" />
            <asp:LinkButton ID="btnEventSessions" ForeColor="white" runat="server" Text="Sessions" Visible="False" CssClass="backTo" OnClick="btnEventSessions_OnClick" />
            <asp:LinkButton ID="btnEventSocialMediaPage" runat="server" ForeColor="white" Text="Social Media Page" CssClass="backTo" OnClick="btnEventSocialMediaPage_OnClick" />
            <asp:LinkButton ID="btnScheduleAlerts" ForeColor="white" runat="server" Text="Schedule Alerts" CssClass="backTo" OnClick="btnScheduleAlerts_OnClick" />
            <asp:LinkButton ID="btnEventPricing" ForeColor="white" runat="server" Text="Pricing" CssClass="backTo" OnClick="btnEventPricing_OnClick" />
            <asp:LinkButton ID="btnViewAttendance" ForeColor="white" runat="server" Text="View Attendance" CssClass="backTo" OnClick="btnViewAttendance_OnClick" />
            <asp:LinkButton ID="btnFeedback" ForeColor="white" runat="server" Text="Respond to Feedback" CssClass="backTo" OnClick="btnFeedback_OnClick" />
            <asp:LinkButton ID="btnMonitirinterest" ForeColor="white" runat="server" Text="Monitor Interest" CssClass="backTo" OnClick="btnMonitirinterest_OnClick" />

        </div>
    </div>

    <asp:UpdatePanel runat="server" ChildrenAsTriggers="False" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="row">
                <div class="col-md-3">
                    <div class="list-group">
                        <a class="list-group-item list-group-item-heading" style="background-color: #6AA6D6; color: white">Message Types</a>
                        <asp:RadioButtonList ID="messagetypes" runat="server" CssClass="list-group-item" AutoPostBack="True" OnSelectedIndexChanged="messagetypes_OnSelectedIndexChanged">
                            <asp:ListItem Text="Pre Event Message" Value="1" />
                            <asp:ListItem Text="Event Message" Value="2" />
                            <asp:ListItem Text="Post Event Message" Value="3" />
                        </asp:RadioButtonList>
                    </div>

                    <div class="list-group" style="padding-top: 20px">
                        <a class="list-group-item list-group-item-heading" style="background-color: #6AA6D6; color: white">Placeholders</a>
                        <a class="list-group-item" runat="server" id="recipientName" onserverclick="appendPlaceholder">1. Recipient Name</a>
                        <a class="list-group-item" runat="server" id="eventName" onserverclick="appendPlaceholder">2. Event Name</a>
                        <a class="list-group-item" runat="server" id="location" onserverclick="appendPlaceholder">3. Location</a>
                        <a class="list-group-item" runat="server" id="beginTime" onserverclick="appendPlaceholder">4. Begin Time</a>
                        <a class="list-group-item" runat="server" id="beginDate" onserverclick="appendPlaceholder">5. Begin Date</a>
                        <a class="list-group-item" runat="server" id="siteURL" onserverclick="appendPlaceholder">6. Site URL</a>
                        <a class="list-group-item" runat="server" id="price" onserverclick="appendPlaceholder">7. Price</a>
                        <a class="list-group-item" runat="server" id="endDate" onserverclick="appendPlaceholder">8. End Date</a>
                        <a class="list-group-item" runat="server" id="endTime" onserverclick="appendPlaceholder">9. End Time</a>
                    </div>
                </div>

                <div class="col-md-6">
                    <div class="col-md-12">
                        <h4 class="control-label"><u>SMS</u></h4>
                        <asp:HiddenField runat="server" ID="smsId" />
                    </div>
                    <div class="col-md-12">
                        <asp:TextBox runat="server" TextMode="MultiLine" ID="txtSMS" Rows="5" placeholder="without placeholders limit should be ~160. 227-placeholders.length()" CssClass="form-control" onblur="saveCaretPos(this,'sms');" onfocus="SetCursorToTextEnd(this)"/>
                        <asp:Label runat="server" CssClass="label label-danger" ID="smsOverflow" Text="SMS too long." Visible="False"></asp:Label>
                    </div>
                    <div class="col-md-12" style="padding-top: 20px">
                        <asp:Button runat="server" ID="btnSaveAlertSMS" CssClass="btn btn-success pull-right" Text="Save SMS" OnClick="btnSaveAlertSMS_OnClick" />
                        <asp:Button runat="server" ID="btnSMSPreview" CssClass="btn btn-info" Text="SMS Preview" OnClick="btnSMSPreview_OnClick"/>
                    </div>
                    <div class="col-md-12" style="padding-top: 10px">
                        <h4 class="control-label"><u>Email</u></h4>
                        <asp:HiddenField runat="server" ID="emailId" />
                    </div>
                    <div class="col-md-12">
                        <label class="control-label"><em>Subject</em></label>
                    </div>
                    <div class="col-md-12">
                        <asp:TextBox runat="server" ID="txtSubject" CssClass="form-control" onblur="saveCaretPos(this,'subject');" onfocus="SetCursorToTextEnd(this)"/>
                    </div>
                    <div class="col-md-12">
                        <label class="control-label"><em>Body</em></label>
                    </div>
                    <div class="col-md-12">
                        <asp:TextBox runat="server" TextMode="MultiLine" ID="txtBody" Rows="7" CssClass="form-control" onblur="saveCaretPos(this,'body');" onfocus="SetCursorToTextEnd(this)"></asp:TextBox>
                    </div>
                    <div class="col-md-12" style="padding-top: 20px">
                        <asp:Button runat="server" ID="btnEmailPreview" CssClass="btn btn-info" Text="Email Preview" OnClick="btnEmailPreview_OnClick"/>
                        <asp:Button runat="server" ID="btnSaveAlertEmail" CssClass="btn btn-lg pull-right btn-success" Text="Save Email" OnClick="btnSaveAlertEmail_OnClick" />
                    </div>
                </div>


                <div class="col-md-3">
                    <div class="list-group col-md-12">
                        <a class="list-group-item list-group-item-heading" style="background-color: #6AA6D6; color: white">Recipients</a>
                        <asp:RadioButtonList runat="server" ID="recipients" AutoPostBack="True" CssClass="list-group-item" OnSelectedIndexChanged="recipients_OnSelectedIndexChanged">
                            <asp:ListItem Text="Mobile Users" Value="True" />
                            <asp:ListItem Text="Event Coordinators" Value="False" />
                        </asp:RadioButtonList>
                    </div>
                    <div class="col-md-12">
                        <h4 class="control-label"><u>Days Before/After(SMS)</u></h4>
                    </div>
                    <div class="col-md-12">
                        <asp:TextBox runat="server" TextMode="Number" ID="txtDaysBeforeAfterSMS" CssClass="form-control" ValidationGroup="alertValidation" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Please provide number of days!" CssClass="label label-danger" Display="Dynamic" ValidationGroup="eventValidation" ControlToValidate="txtDaysBeforeAfterSMS"></asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="CompareValidator1" runat="server" Type="Integer" ErrorMessage="Must be an integer!" CssClass="label label-danger" Display="Dynamic" ValidationGroup="eventValidation" ControlToValidate="txtDaysBeforeAfterSMS" Operator="DataTypeCheck"></asp:CompareValidator>
                    </div>
                    <div class="col-md-12" style="padding-top: 20px">
                        <h4 class="control-label"><u>Days Before/After(Email)</u></h4>
                    </div>
                    <div class="col-md-12">
                        <asp:TextBox runat="server" TextMode="Number" ID="txtDaysBeforeAfterEmail" CssClass="form-control" ValidationGroup="alertValidation" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please provide number of days!" CssClass="label label-danger" Display="Dynamic" ValidationGroup="eventValidation" ControlToValidate="txtDaysBeforeAfterEmail"></asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="CompareValidator2" runat="server" Type="Integer" ErrorMessage="Must be an integer!" CssClass="label label-danger" Display="Dynamic" ValidationGroup="eventValidation" ControlToValidate="txtDaysBeforeAfterEmail" Operator="DataTypeCheck"></asp:CompareValidator>
                    </div>
                </div>
            </div>
            
            
            
            
            <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true" style="display: none;">
        <div class="modal-dialog" style="width: 800px">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title" id="myModalLabel">Message Preview</h4>
                </div>
                <div class="modal-body">
                    <h3 runat="server" ID="subjHeading" Visible="False"><strong>Subject</strong></h3>
                    <asp:Label runat="server" ID="msgSubject"/>
                    <br/>
                    <h3 runat="server" ID="bodyHeading" Visible="False"><strong>Body</strong></h3>
                    <asp:Label runat="server" ID="msgPreview"/>
                </div>
                <div class="modal-footer">
                    <asp:LinkButton ID="btnCloseModal" CssClass="btn btn-default" ClientIDMode="Static" runat="server">Cancel</asp:LinkButton>
                    <asp:LinkButton ID="btnSaveSession" CssClass="btn btn-success" ClientIDMode="Static" runat="server" ValidationGroup="sessionValidation">Save</asp:LinkButton>
                </div>
            </div>
            <!-- /.modal-content -->
        </div>
        <!-- /.modal-dialog -->
    </div>
            
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="beginDate" EventName="ServerClick"/>
            <asp:AsyncPostBackTrigger ControlID="beginTime" EventName="ServerClick" />
            <asp:AsyncPostBackTrigger ControlID="recipients" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="messagetypes" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="endDate" EventName="ServerClick" />
            <asp:AsyncPostBackTrigger ControlID="endTime" EventName="ServerClick" />
            <asp:AsyncPostBackTrigger ControlID="eventName" EventName="ServerClick" />
            <asp:AsyncPostBackTrigger ControlID="location" EventName="ServerClick" />
            <asp:AsyncPostBackTrigger ControlID="price" EventName="ServerClick" />
            <asp:AsyncPostBackTrigger ControlID="recipientName" EventName="ServerClick" />
            <asp:AsyncPostBackTrigger ControlID="siteURL" EventName="ServerClick" />
            <asp:PostBackTrigger ControlID="btnSMSPreview"/>
            <asp:PostBackTrigger ControlID="btnEmailPreview"/>
            <asp:PostBackTrigger ControlID="btnSaveAlertSMS"/>
            <asp:PostBackTrigger ControlID="btnSaveAlertEmail"/>
        </Triggers>
    </asp:UpdatePanel>

    <%--   <div class="row">
        <div class="col-md-3"></div>
        <div class="col-md-6">
            <div class="col-md-12">
                <asp:Button runat="server" ID="btnSaveAlerts" CssClass="btn btn-block btn-success" Text="Save" />
            </div>
        </div>
        <div class="col-md-3"></div>
    </div>--%>


    <asp:HiddenField runat="server" ID="CaretPos" />
    <asp:HiddenField runat="server" ID="selectedBox" />
    <asp:HiddenField runat="server" ID="placeholderLength"/>

    <script type="text/javascript">
        /*
                tinymce.init({
                    selector: "textarea"
                });*/


        function saveCaretPos(txt,name) {
            document.getElementById('<% =CaretPos.ClientID %>').value = getCaret(txt);
            document.getElementById('<% =selectedBox.ClientID %>').value = name;

        }
        function getCaret(el) {
            if (el.selectionStart) {
                return el.selectionStart;
            } else if (document.selection) {
                el.focus();

                var r = document.selection.createRange();
                if (r == null) {
                    return 0;
                }

                var re = el.createTextRange(),
                    rc = re.duplicate();
                re.moveToBookmark(r.getBookmark());
                rc.setEndPoint('EndToStart', re);

                return rc.text.length;
            }
            return 0;
        }

        function SetCursorToTextEnd(text) {
            //var text = document.getElementById(textControlID);
            if (text != null && text.value.length > 0) {
                if (text.createTextRange) {
                    var range = text.createTextRange();
                    range.moveStart('character', text.value.length);
                    range.collapse();
                    range.select();
                }
                else if (text.setSelectionRange) {
                    var textLength = text.value.length;
                    text.setSelectionRange(textLength, textLength);
                }
            }
        }

    </script>

</asp:Content>
