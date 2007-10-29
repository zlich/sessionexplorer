<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PageLinks.ascx.cs" Inherits="SessionExplorer.Web.Controls.PageLinks" %>

<asp:Table ID="Table1" runat="server" Width="100%">
    <asp:TableRow>
        <asp:TableCell Width="33%" HorizontalAlign="Left">
            <em><asp:Label runat="server" ID="EntriesLabel" /></em>
        </asp:TableCell>
        <asp:TableCell Width="33%" HorizontalAlign="Center">
            <asp:HyperLink runat="server" ID="FirstImageLink" ImageUrl="~/Images/first-active.png" CssClass="PagingLink" />&nbsp;<asp:HyperLink runat="server" ID="FirstLink" Text="first" CssClass="PagingLink" />&nbsp;
            <asp:HyperLink runat="server" ID="PreviousImageLink" ImageUrl="~/Images/previous-active.png" CssClass="PagingLink" />&nbsp;<asp:HyperLink runat="server" ID="PreviousLink" Text="previous" CssClass="PagingLink" />&nbsp;
            <asp:HyperLink runat="server" ID="NextLink" Text="next" CssClass="PagingLink" />&nbsp;<asp:HyperLink runat="server" ID="NextImageLink" ImageUrl="~/Images/next-active.png" CssClass="PagingLink" />&nbsp;
            <asp:HyperLink runat="server" ID="LastLink" Text="last" CssClass="PagingLink" />&nbsp;<asp:HyperLink runat="server" ID="LastImageLink" ImageUrl="~/Images/last-active.png" CssClass="PagingLink" />
        </asp:TableCell>
        <asp:TableCell Width="33%" HorizontalAlign="Right">
            Page <%= PageNumber %> of <%= lastPage %>
        </asp:TableCell>
    </asp:TableRow>
</asp:Table>