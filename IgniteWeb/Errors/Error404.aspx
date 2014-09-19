<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="Error404.aspx.cs" Inherits="IgniteWeb.Errors.Error404" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpMain" runat="server">
    
    <!-- Content block begin -->
	<section id="content" class="container">
		<div class="row">
			<div class="page-404 col-md-4">
				<img class="img-responsive" src="/images/page17_img01.png" alt="page17_img01">
			</div>
			<div class="page-404 col-md-4">
				<p>We're sorry, but we can't find the page you were looking for. It's probably some thing we've done wrong but now we know about it and we'll try to fix it. In the meantime, try one of these options:</p>
				<ul>
					<li>
						<asp:HyperLink runat="server" ID="previousPage">Go to Previous Page</asp:HyperLink>
					</li>	
					<li>
						<a href="/Index.aspx">Go to Homepage</a>
					</li>
				</ul>
			</div>
            </div>
            </section>

</asp:Content>
