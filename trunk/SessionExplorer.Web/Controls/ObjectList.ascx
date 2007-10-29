<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ObjectList.ascx.cs" Inherits="SessionExplorer.Web.Controls.ObjectList" %>

<%@ Register TagPrefix="uc" TagName="ObjectVisualiser" Src="~/Controls/ObjectVisualiser.ascx" %>

<asp:Repeater ID="SessionItemRepeater" runat="server">
    <ItemTemplate>
        <uc:ObjectVisualiser runat="server" TargetName='<%# (string)DataBinder.Eval(Container.DataItem, "Key") %>' Target='<%# DataBinder.Eval(Container.DataItem, "Value") %>' />
    </ItemTemplate>
</asp:Repeater>