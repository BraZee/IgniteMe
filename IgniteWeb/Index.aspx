<%@ Page Title="Home" Language="C#" MasterPageFile="~/MasterPages/Site.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="IgniteWeb.Index" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpMain" runat="server">

    <!-- Slider block begin -->
    <section class="slider-wrapper">
        <div class="container">
            <div class="row">

                <!-- Slider begin (bootstrap carousel) -->
                <div class="col-md-12 slider">
                    <div id="slider" class="carousel slide" data-ride="carousel">
                        <ol class="carousel-indicators">
                            <li data-target="#slider" data-slide-to="0" class="active"></li>
                            <li data-target="#slider" data-slide-to="1"></li>
                            <li data-target="#slider" data-slide-to="2"></li>
                            <li data-target="#slider" data-slide-to="3"></li>
                        </ol>

                        <!-- Carousel items begin -->
                        <div class="carousel-inner">
                            <div class="active item">
                                <div class="post-thumbnail-1">
                                    <div class="tag-style-1">
                                        <p>Design </p>
                                        <i class="fa fa-picture-o"></i>
                                    </div>
                                    <figure>
                                        <a href="#">
                                            <img class="img-responsive" src="images/slide1.jpg" alt="slide-1"></a>
                                    </figure>
                                    <h3>Desktop Wallpaper Calendars: April 2014</h3>
                                    <p class="meta-tags">
                                        <span><i class="fa fa-clock-o"></i>04.09.2014</span>
                                        <span><i class="fa fa-comments"></i>15</span>
                                        <span><i class="fa fa-user"></i>Frank</span>
                                        <span><i class="fa fa-heart"></i>23</span>
                                    </p>
                                </div>
                            </div>
                            <div class="item">
                                <div class="post-thumbnail-1">
                                    <div class="tag-style-1">
                                        <p>Design </p>
                                        <i class="fa fa-picture-o"></i>
                                    </div>
                                    <figure>
                                        <a href="#">
                                            <img class="img-responsive" src="images/slide2.jpg" alt="slide-2"></a>
                                    </figure>
                                    <h3>Costum Bi Fold Brochure Template</h3>
                                    <p class="meta-tags">
                                        <span><i class="fa fa-clock-o"></i>04.09.2014</span>
                                        <span><i class="fa fa-comments"></i>15</span>
                                        <span><i class="fa fa-user"></i>Frank</span>
                                        <span><i class="fa fa-heart"></i>23</span>
                                    </p>
                                </div>
                            </div>
                            <div class="item">
                                <div class="post-thumbnail-1">
                                    <div class="tag-style-1">
                                        <p>Design </p>
                                        <i class="fa fa-picture-o"></i>
                                    </div>
                                    <figure>
                                        <a href="#">
                                            <img class="img-responsive" src="images/slide3.jpg" alt="slide-3"></a>
                                    </figure>
                                    <h3>Sky Blok Psd Text Effect</h3>
                                    <p class="meta-tags">
                                        <span><i class="fa fa-clock-o"></i>04.09.2014</span>
                                        <span><i class="fa fa-comments"></i>15</span>
                                        <span><i class="fa fa-user"></i>Frank</span>
                                        <span><i class="fa fa-heart"></i>23</span>
                                    </p>
                                </div>
                            </div>
                            <div class="item">
                                <div class="post-thumbnail-1">
                                    <div class="tag-style-1">
                                        <p>Design </p>
                                        <i class="fa fa-picture-o"></i>
                                    </div>
                                    <figure>
                                        <a href="#">
                                            <img class="img-responsive" src="images/slide4.jpg" alt="slide-4"></a>
                                    </figure>
                                    <h3>Basic Stationery Branding Vol 1</h3>
                                    <p class="meta-tags">
                                        <span><i class="fa fa-clock-o"></i>04.09.2014</span>
                                        <span><i class="fa fa-comments"></i>15</span>
                                        <span><i class="fa fa-user"></i>Frank</span>
                                        <span><i class="fa fa-heart"></i>23</span>
                                    </p>
                                </div>
                            </div>
                        </div>
                        <!-- Carousel's item end -->

                    </div>
                </div>
                <!-- Slider end -->
            </div>
        </div>
    </section>

</asp:Content>
