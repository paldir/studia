<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="News.aspx.cs" Inherits="NewsPage" Title="PSW Inc: News" %>

<%--asp--%>
    <!--

Title Under Menu

-->
    <div id="pagetitle">
        News</div>
    <!--

CONTENT CONTAINER

-->
    <div id="content-container-three-column">
        <!--

  CONTENT SIDE 1 COLUMN

  -->
        <div id="content-side1-three-column">
            <ul class="list-of-links">
                <li class="current"><a href="News.aspx">All News</a></li>
            </ul>
        </div>
        <!--

  CENTER COLUMN

  -->
        <div id="content-main-three-column">
            <h1>
                PSW News</h1>
            <hr />
            <!-- END MAIN COLUMN -->
          <%--asp--%>
        </div>
        <!--

  CONTENT SIDE 2 COLUMN

  -->
        <div id="content-side2-three-column">
            <h3 class="small">
                Magazine pull quotes</h3>
            <blockquote>
                <p>
                    Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diam nonummy nibh
                    euismod tincidunt ut laoreet dolore magna aliquam erat.</p>
                <p>
                    <cite>Magazine, March 24, 2012</cite></p>
            </blockquote>
            <blockquote>
                <p>
                    Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diam nonummy nibh
                    euismod tincidunt ut laoreet dolore magna aliquam erat.</p>
                <p>
                    <cite>Magazine, July 12, 2012</cite></p>
            </blockquote>
        </div>
        <div class="clear">
        </div>
    </div>
