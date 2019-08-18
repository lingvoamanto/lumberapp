<%@ Page Title="Stock" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Stock.aspx.cs" Inherits="LumberCorp.Stock" %>
<%@ Register TagPrefix="uc" TagName="StockControl" Src="~/Controls/StockControl.ascx" %>
<%@ Register TagPrefix="uc" TagName="AdminControl" Src="~/Controls/AdminControl.ascx" %>
<%@ Register TagPrefix="uc" TagName="DetailControl" Src="~/Controls/DetailControl.ascx" %>

<%@ Import namespace="System.Data.OleDb" %>
<%@ Import namespace="System.Configuration" %>
<%@ Import namespace="LumberCorp" %>

<asp:Content runat="server" ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent">
    <div id="slideshow"> 
        <%
            Response.Write("<img src=\"/images/IMG_1117-SALES.png\" alt=\"\" class=\"active\" />");
        %>
    </div>
    <hr style="clear:both">
</asp:Content>
<asp:Content runat="server" ID="MainContent" ContentPlaceHolderID="MainContent">
    <div class="left-panel"> 
        <uc:StockControl id="stockControl" runat="server" />
        <uc:AdminControl id="adminControl" runat="server" />
        <uc:DetailControl id="detailControl" runat="server" />
    </div>
    <div class="content">
        <% 
        string category = "";
        bool white = false;
        List<StockItem> stockItems = StockItems;
        foreach (StockItem stockItem in stockItems)
        {
            if (stockItem.Type != Type)
                continue;
            
            if (stockItem.Category != category)
            {
                if (category != "")
                    Response.Write("</tbody></table>\n</details>\n");
                if( stockItem.Category.Replace('_',' ') == Category )
                    Response.Write("<details open=\"open\">");
                else
                    Response.Write("<details>");
                
                Response.Write("<summary>" + stockItem.Category + "</summary>\n");
                Response.Write("<table>\n<thead>");
                Response.Write("<tr>");
                Response.Write("<td>Width</td>");
                Response.Write("<td>Thickness</td>");
                Response.Write("<td>Length</td>");
                Response.Write("<td width=\"150\">Grade</td>");
                Response.Write("<td>Treatment</td>");
                Response.Write("<td>Dryness</td>");
                Response.Write("<td>Finish</td>");
                Response.Write("<td style=\"text-align:right\">Packs</td>");
                Response.Write("<td>Cube</td>");
                Response.Write("<td style=\"text-align:right\">Order</td>");
                Response.Write("</tr>\n");
                Response.Write("</thead>\n<tbody>");
                category = stockItem.Category;
                white = false;
            }

            if( ! white )
                Response.Write("<tr>");
            else
                Response.Write("<tr style=\"background-color:#FFFFFF;\">");
            Response.Write("<td style=\"text-align:right\">" + stockItem.Width + "</td>");
            Response.Write("<td style=\"text-align:right\">" + stockItem.Thickness + "</td>");
            Response.Write("<td style=\"text-align:right\">" + stockItem.Length + "</td>");
            Response.Write("<td width=\"150\">" + stockItem.Grade + "</td>");
            Response.Write("<td>" + stockItem.Treatment + "</td>");
            Response.Write("<td>" + stockItem.Dryness + "</td>");
            Response.Write("<td>" + stockItem.Finish + "</td>");
            Response.Write("<td>" + stockItem.Packs + "</td>");

            if (stockItem.Cube != null && stockItem.Cube != "")
            {
                double cube = double.Parse(stockItem.Cube);
                Response.Write("<td style=\"text-align:right\">" + cube.ToString("0.####") + "</td>");
            }
            else
                Response.Write("<td>.</td>");
                
            Response.Write("<td><input class=\"sku-input\" type=\"text\" data-sku=\"" + stockItem.SKU + "\" ></td>");
            Response.Write("</tr>\n");

            white = !white;
        }
        
        
        if (category != "")
        {
            Response.Write("</tbody></table>\n</details>\n");
            Response.Write("<button id=\"addtocart\" type=\"button\">Add To Cart</button>\n");
        } %>
    </div>

</asp:Content>

<asp:Content runat="server" ID="ScriptContent" ContentPlaceHolderID="ScriptContent">
    <script>
        $(document).ready(function () {
            $(".sku-input").keyup(function (event) {
                var number = $(this).val();
                var isNumber = /^([1-9][0-9]*)/.test(number);
                console.log('' + number + ' ' + isNumber);
                if ( isNumber) {
                    $(this).css('color', 'black');
                }
                else {
                    $(this).css('color', 'red');
                }
            });

            $('#addtocart').click(function () {
                var jsonCart = getCookie('cart');
                var cart;
                if (jsonCart == null || jsonCart == '') {
                    cart = [];
                }
                else {
                    cart = JSON.parse(jsonCart).data;
                }
                var length = cart.length;
                $('.sku-input').each(function (index, value) {
                    var length = cart.length;
                    var quantity = $(this).val();
                    var sku = $(this).data('sku');
                    if( quantity != "" )
                    {
                        var found = false;
                        for (var i = 0; i < length; i++) {
                            var item = cart[i];
                            if (item.sku == sku) {
                                item.quantity = parseInt(quantity)+parseInt(item.quantity);
                                found = true;
                                break;
                            }
                        }

                        if (!found) {
                            var item = { quantity: quantity, sku: sku };
                            cart.push(item);
                        }
                    }
                });

                var data = '{ "data" : ' + JSON.stringify(cart) + '}';
                setCookie('cart', data, 365);
                setCookie('test', "OK", 365);
                document.location = "/cart";
            });
        });
    </script>
</asp:Content>
