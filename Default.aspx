<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Query._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <h1>Query Website</h1>
        <p class="lead">1. Click Load Data. It may take about 30 seconds, please be patient. </p>
        <p class="lead">2. Try query by typing in First Name and/or Last Name.</p>
        <p class="lead">3. Click Query after typing. </p>
        <p class="lead">4. Clear Data will remove all the data. </p>
        <p class="lead">
            <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Load Data" />
        </p>
        <p class="lead">
            <asp:Button ID="Button2" runat="server" Text="Clear Data" OnClick="Button2_Click" />
        </p>
        <p class="lead">
            <asp:Button ID="Button3" runat="server" Text="Query"  OnClick="Button3_Click" />
        </p>
        <p class="lead">
            <asp:Label ID="Label3" runat="server" Text="First Name:"></asp:Label><asp:TextBox ID="TextBox1" runat="server" ></asp:TextBox>
&nbsp;&nbsp;&nbsp;
            <asp:Label ID="Label4" runat="server" Text="Last Name:"></asp:Label><asp:TextBox ID="TextBox2" runat="server" ></asp:TextBox>
        </p>
        <p class="lead">
            <asp:Label ID="Label1" runat="server" Text="Query result"></asp:Label>
        &nbsp;</p>
        <p class="lead">
            <asp:ListBox ID="ListBox1" runat="server" Width="900px" Height="170px" OnSelectedIndexChanged="ListBox1_SelectedIndexChanged" ></asp:ListBox>
        </p>
    </div>

    <div class="row">
    </div>

</asp:Content>
