<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Default.aspx.cs" Inherits="_Default" Title="PSW Inc: Home" %>

<%--asp--%>
<asp:Content ID="Content1" ContentPlaceHolderID="mainContent" runat="server">
<!--

POSTER PHOTO

-->
    <div id="poster-photo-container">
        <img src="images/photo-poster.jpg" alt="" class="poster-photo-image" />
        <div id="feature-area-home">
            Our best widget ever: <a href="Items.aspx">Widget 010</a><br />
            just released to fantastic reviews.
        </div>
    </div>
    <!--

CONTENT CONTAINER

-->
    <div id="content-container-two-column">
        <!--

CONTENT MAIN COLUMN

-->
        <div id="content-main-two-column">
            <h1>World class, custom widgets for your industry</h1>
            <p>
                Consectetuer adipiscing elit, sed diam nonummy nibh euismod tincidunt ut laoreet
                dolore magna aliquam erat volutpat.
            </p>
            <div id="three-column-container">
                <div id="three-column-side1">
                    <a href="Items.aspx">
                        <img src="images/home-photo-1.jpg" class="photo-border" alt="Enter Alt Text Here" /></a>
                    <h2>Our Products</h2>
                    <p>
                        Amet, consectetuer adipiscing elit, sed diam tincidunt ut laoreet. Consectetuer
                        adipiscing elit, sed diam nonummy nibh euismod tincidunt ut laoreet dolore magna
                        aliquam erat volutpat. Ut wisi enim ad minim veniam.
                    </p>
                    <a href="Items.aspx">read more</a><img class="arrow" src="images/arrow.gif" alt="" />
                </div>
                <div id="three-column-side2">
                    <a href="#">
                        <img src="images/home-photo-3.jpg" class="photo-border" alt="Enter Alt Text Here" /></a>
                    <h2>Customer Support</h2>
                    <p>
                        Amet, consectetuer adipiscing elit, sed diam tincidunt ut laoreet. Consectetuer
                        adipiscing elit, sed diam nonummy nibh euismod tincidunt ut laoreet dolore magna
                        aliquam erat volutpat. Ut wisi enim ad minim veniam.
                    </p>
                    <a href="#">read more</a><img class="arrow" src="images/arrow.gif" alt="" />
                </div>
                <div id="three-column-middle">
                    <a href="#">
                        <img src="images/home-photo-2.jpg" class="photo-border" alt="Enter Alt Text Here" /></a>
                    <h2>About PSW, Inc.</h2>
                    <p>
                        Amet, consectetuer adipiscing elit, sed diam tincidunt ut laoreet. Consectetuer
                        adipiscing elit, sed diam nonummy nibh euismod tincidunt ut laoreet dolore magna
                        aliquam erat volutpat. Ut wisi enim ad minim veniam.
                    </p>
                    <a href="#">read more</a><img class="arrow" src="images/arrow.gif" alt="" />
                </div>
            </div>
        </div>
        <!--

CONTENT SIDE COLUMN

-->
        <div id="content-side-two-column">
            <h2>PSW in Business Magazine!</h2>
            <p>
                Ectetuer adipiscing elit, sed diam nonummy nibh euismod tincidunt ut laoreet dolore
                magna aliquam erat volutpat. Ut wisi enim ad minim veniam, quis nost.
            </p>
            <h3>
                <a href="#">Recent News </a>
            </h3>
            <ul class="list-of-links">
                <li><a href="#">Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diam nonummy
                    nibh euismod tincidunt.</a></li>
                <li><a href="#">Sed diam nonummy</a></li>
                <li><a href="#">Nibh euismod</a></li>
                <li><a href="#">Ut wisi enim ad minim veniam, quis nostrud</a></li>
            </ul>
        </div>
        <div class="clear">
        </div>
    </div>
</asp:Content>
