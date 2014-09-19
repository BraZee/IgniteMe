<%@ Page ValidateRequest="false" Title="" Language="C#" MasterPageFile="~/SiteChild.master" AutoEventWireup="true" CodeBehind="SendEmail.aspx.cs" Inherits="Ignite.SendEmail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="NestedContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="False" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="panel panel-info">
                <div class="panel-heading" style="background-color: #6AA6D6">
                    <h5 style="color: white">Send Email</h5>
                </div>
                <div class="panel-body">
                    <div class="row">
                        <div class="col-md-3">
                            <div class="list-group">
                                <a class="list-group-item list-group-item-heading" style="background-color: #6AA6D6; color: white">Placeholders</a>
                                <a class="list-group-item" runat="server" id="recipientName" onserverclick="appendPlaceholder">1. Recipient Name</a>
<%--                                <a class="list-group-item" runat="server" id="eventName" onserverclick="appendPlaceholder">2. Event Name</a>--%>
<%--                                <a class="list-group-item" runat="server" id="location" onserverclick="appendPlaceholder">3. Location</a>--%>
<%--                                <a class="list-group-item" runat="server" id="beginTime" onserverclick="appendPlaceholder">4. Begin Time</a>--%>
<%--                                <a class="list-group-item" runat="server" id="beginDate" onserverclick="appendPlaceholder">5. Begin Date</a>--%>
<%--                                <a class="list-group-item" runat="server" id="siteURL" onserverclick="appendPlaceholder">6. Site URL</a>--%>
<%--                                <a class="list-group-item" runat="server" id="price" onserverclick="appendPlaceholder">7. Price</a>--%>
<%--                                <a class="list-group-item" runat="server" id="endDate" onserverclick="appendPlaceholder">8. End Date</a>--%>
<%--                                <a class="list-group-item" runat="server" id="endTime" onserverclick="appendPlaceholder">9. End Time</a>--%>
                            </div>
                        </div>
                        <div class="col-md-9">
                            <div class="form-group col-md-12">
                                <div class="col-md-2">
                                    <label class="control-label">Recipients:</label>
                                </div>
                                <div class="col-md-4">
                                    <asp:DropDownList runat="server" ID="cbRecipients" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="cbRecipients_OnSelectedIndexChanged"/>
                                </div>
                                <div class="col-md-2" style="margin-right: 0">
                                    <label class="control-label pull-right">Filter:</label>
                                </div>
                                <div class="col-md-4">
                                    <asp:DropDownList runat="server" ID="cbFilter" CssClass="form-control" Enabled="False" />
                                </div>
                            </div>
                            <div class="form-group col-md-12">
                                <div class="col-md-2">
                                    <label class="control-label">Subject:</label>
                                </div>
                                <div class="col-md-10">
                                    <asp:TextBox runat="server" ID="txtSubject" CssClass="form-control" onblur="saveCaretPos(this,'subject');" onfocus="SetCursorToTextEnd(this)"/>
                                </div>
                            </div>

                            <div class="form-group col-md-12">
                                <div class="col-md-12">
                                    <asp:TextBox runat="server" ID="txtMessage" TextMode="MultiLine" Rows="10" CssClass="form-control" onblur="saveCaretPos(this,'body');" onfocus="SetCursorToTextEnd(this)" />
                                </div>
                            </div>
                            <div class="form-group col-md-12">
                                <div class="col-md-2">
                                </div>
                                <div class="col-md-10">
                                    <asp:Button runat="server" ID="btnSend" Text="Send" CssClass="btn btn-lg btn-success pull-right" OnClick="btnSend_OnClick" />
                                </div>
                            </div>

                        </div>

                    </div>
                </div>
            </div>
            
        </ContentTemplate>
        <Triggers>
<%--            <asp:AsyncPostBackTrigger ControlID="beginDate" EventName="ServerClick"/>--%>
<%--            <asp:AsyncPostBackTrigger ControlID="beginTime" EventName="ServerClick" />--%>
           <asp:AsyncPostBackTrigger ControlID="cbRecipients" EventName="SelectedIndexChanged" />
<%--            <asp:AsyncPostBackTrigger ControlID="endDate" EventName="ServerClick" />--%>
<%--            <asp:AsyncPostBackTrigger ControlID="endTime" EventName="ServerClick" />--%>
<%--            <asp:AsyncPostBackTrigger ControlID="eventName" EventName="ServerClick" />--%>
<%--            <asp:AsyncPostBackTrigger ControlID="location" EventName="ServerClick" />--%>
<%--            <asp:AsyncPostBackTrigger ControlID="price" EventName="ServerClick" />--%>
            <asp:AsyncPostBackTrigger ControlID="recipientName" EventName="ServerClick" />
<%--            <asp:AsyncPostBackTrigger ControlID="siteURL" EventName="ServerClick" />--%>
            <asp:PostBackTrigger ControlID="btnSend"/>
        </Triggers>
    </asp:UpdatePanel>
    
    <asp:HiddenField runat="server" ID="CaretPos" />
    <asp:HiddenField runat="server" ID="selectedBox" />

    <script type="text/javascript">
        /*
                tinymce.init({
                    selector: "textarea"
                });*/


        function saveCaretPos(txt, name) {
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
