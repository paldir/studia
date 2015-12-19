<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ItemDetail.aspx.cs" Inherits="ItemDetail" Title="PSW Inc: Items" %>

<%--asp--%>
    <!--

Title Under Menu

-->
    <div id="pagetitle">
        Items</div>
    <div id="content-container-three-column">
        <!--

  CONTENT SIDE 1 COLUMN

  -->
        <div id="content-side1-three-column">
            <h2>
                <a href="Items.aspx">All Items</a></h2>
            <ul class="list-of-links">
                <li><a href="Items.aspx">All Widgets</a></li>
                <li><a href="#">Widget systems</a></li>
                <li><a href="#">Widget accessories</a></li>
                <li><a href="#">Widgets for fun</a></li>
            </ul>
            <h2>
                Specials</h2>
            <ul class="list-of-links no-lines">
                <li><a href="#">Special #1</a></li>
                <li><a href="#">Special #2</a></li>
                <li><a href="#">Special #3</a></li>
                <li><a href="#">Special #4</a></li>
            </ul>
            <h2>
                Popular widgets</h2>
            <ul class="list-of-links no-lines">
                <li><a href="#">Popular #1</a></li>
                <li><a href="#">Popular #2</a></li>
                <li><a href="#">Popular #3</a></li>
                <li><a href="#">Popular #4</a></li>
            </ul>
            <h2>
                New Widgets</h2>
            <ul class="list-of-links no-lines">
                <li><a href="#">New Widget #1</a></li>
                <li><a href="#">New Widget #2</a></li>
                <li><a href="#">New Widget #3</a></li>
                <li><a href="#">New Widget #4</a></li>
            </ul>
            <h2>
                Upgrade your widget with these great accessories</h2>
            <a href="#">
                <img src="images/product-small-12.jpg" alt="Product 12 name" class="photo-border" /></a>
            <a href="#">
                <img src="images/product-small-11.jpg" alt="Product 11 name" class="photo-border" /></a>
        </div>
        <!--

  CENTER COLUMN

  -->
        <div id="content-main-three-column">
            <%--asp--%>
            <%--asp--%>                
            <a href="Items.aspx">Return to All Items</a><img src="images/arrow.gif" alt="" />
        </div>
        <!-- END MAIN COLUMN -->
        <!--

  CONTENT SIDE 2 COLUMN

  -->
        <div id="content-side2-three-column">
            <h3 class="small">
                <%--asp--%>
            <blockquote>
                <p>
                    Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diam nonummy nibh
                    euismod tincidunt ut laoreet dolore magna aliquam erat.</p>
                <p>
                    <cite>Jane Doe, Phoenix, AZ</cite></p>
            </blockquote>
            <blockquote>
                <p>
                    Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diam nonummy nibh
                    euismod tincidunt ut laoreet dolore magna aliquam erat.</p>
                <p>
                    <cite>John Doe, New York, NY</cite></p>
            </blockquote>
            <blockquote>
                <p>
                    Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diam nonummy nibh
                    euismod tincidunt ut laoreet dolore magna aliquam erat.</p>
                <p>
                    <cite>Jane Doe, Phoenix, AZ</cite></p>
            </blockquote>
            <blockquote>
                <p>
                    Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diam nonummy nibh
                    euismod tincidunt ut laoreet dolore magna aliquam erat.</p>
                <p>
                    <cite>John Doe, New York, NY</cite></p>
            </blockquote>
        </div>
        <div class="clear">
        </div>
    </div>

