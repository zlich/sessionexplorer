<%@ Import namespace="SessionExplorer.Entities"%>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SessionReport.ascx.cs" Inherits="SessionExplorer.Web.Controls.SessionReport" %>

<%@ Register TagPrefix="uc" TagName="DateLinks" Src="~/Controls/DateLinks.ascx" %>
<%@ Register TagPrefix="uc" TagName="PageLinks" Src="~/Controls/PageLinks.ascx" %>
<%@ Register TagPrefix="uc" TagName="ObjectList" Src="~/Controls/ObjectList.ascx" %>

<h1>Session Log - <asp:Label runat="server" ID="EnvironmentLabel" /> Environment</h1>
<h2><asp:Label runat="server" ID="ReportLabel" /></h2>
<p><uc:DateLinks runat="server" ID="DateLinks1" /></p>
<uc:PageLinks runat="server" ID="PageLinks1" />
<asp:GridView runat="server" ID="SessionReportGridView" AutoGenerateColumns="false" AlternatingRowStyle-BackColor="#eeeeee" BorderColor="White" BorderStyle="Solid" HeaderStyle-BackColor="Black" HeaderStyle-ForeColor="White" Width="100%">
    <Columns>
        <asp:TemplateField HeaderText="Session" >
            <ItemTemplate>
                <%# DataBinder.Eval(Container.DataItem, "SessionId") %>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField HeaderText="Created" DataField="Created" DataFormatString="{0:HH:mm:ss}" HtmlEncode="false" />
        <asp:BoundField HeaderText="Expires" DataField="Expires" DataFormatString="{0:HH:mm:ss}" HtmlEncode="false" />
        <asp:TemplateField HeaderText="Items" >
            <ItemTemplate>
                <%--<asp:BulletedList runat="server" DataSource='<%# DataBinder.Eval(Container.DataItem, "Items") %>' DataTextField="Description" />--%>
                <uc:ObjectList runat="server" DataSource='<%# DataBinder.Eval(Container.DataItem, "Items") %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Size">
            <ItemTemplate>
                <%# GetSize((int)DataBinder.Eval(Container.DataItem, "Size")) %>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
<uc:PageLinks runat="server" ID="PageLinks2" />
<p><asp:Label runat="server" ID="RetrievalStats" /></p>