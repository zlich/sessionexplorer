<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SessionExplorer.Web._Default" %>
<%@ Register TagPrefix="uc" TagName="SessionReport" Src="~/Controls/SessionReport.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Session Explorer</title>
    <style type="text/css" media="all">
        body, p, h1, h2, h3, h4, h5, h6, td, th, input, textarea, select
        {
            font-family: Verdana, Arial;
            font-size: 10px;
        }
        h1 { font-size: 14px; margin: 2px 0; }
        h2 { font-size: 12px; margin: 2px 0; }
        
        .green { color: green; }
        .red { color: red; }
        
        .PagingLink img { vertical-align: -20%; }
        .PagingLink { text-decoration: none }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <p style="text-align: right;"><asp:LinkButton runat="server" ID="FlushButton" Text="Flush Cache" OnClick="FlushButton_Click"/></p>
            <uc:SessionReport runat="server" ID="SessionReport1" />
        </div>
    </form>
</body>
</html>
