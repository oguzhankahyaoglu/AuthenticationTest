<%@ Page Language="C#" MasterPageFile="Site1.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="FormsAuthTest.Admin.Login" %>

<asp:Content runat="server" ContentPlaceHolderID="cphContent">
    Login page
        <br />
    <asp:TextBox runat="server" ID="txtUsername" /><br />
    <asp:TextBox runat="server" ID="txtPassword" TextMode="Password" /><br />
    <asp:Button runat="server" Text="Login" OnClick="OnClick" /><br />

</asp:Content>
