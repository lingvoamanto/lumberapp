<%@ Title="Cart" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="True" CodeBehind="Cart.aspx.cs" Inherits="LumberCorp.Cart" %>
<%@ Register TagPrefix="uc" TagName="StockControl" Src="StockControl.ascx" %>
<%@ Register TagPrefix="uc" TagName="DetailControl" Src="DetailControl.ascx" %>
<%@ Import namespace="System.Data.OleDb" %>
<%@ Import namespace="System.Configuration" %>
<%@ Import namespace="LumberCorp" %>
<%@ Import Namespace="System.Web.Script.Serialization" %>

<asp:Content runat="server" ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent">
    <div id="slideshow"> 
        <%
            Response.Write( "<img src=\"/images/front-1.png\" alt=\"\" class=\"active\" />" );
        %>
    </div>
    <hr style="clear:both">
</asp:Content>
<asp:Content runat="server" ID="MainContent" ContentPlaceHolderID="MainContent">
    <div class="left-panel"> 
        <uc:StockControl id="stockControl" runat="server" />
        <uc:DetailControl id="detailControl" runat="server" />
    </div>
    <div class="content">
        <% 
            CartItems cartItems;
            HttpCookie responseCookie = Request.Cookies.Get("cart");
            HttpCookie cartCookie = responseCookie;
            if (cartCookie!= null)
            {
                string escapedString = cartCookie.Value;
                string jsonString = HttpUtility.UrlDecode(escapedString);
                cartItems = new JavaScriptSerializer().Deserialize<CartItems>(jsonString);
            }
            else
                cartItems = new CartItems();


        List<StockItem> stockItems = StockItems;

        if (cartItems.data == null)
        {
            Response.Write("There is nothing in your cart.");
        }
        else
        {
            bool white = false;

            Response.Write("<table>\n<thead>");
            Response.Write("<tr>");
            Response.Write("<td>Width</td>");
            Response.Write("<td>Thickness</td>");
            Response.Write("<td>Length</td>");
            Response.Write("<td>Grade</td>");
            Response.Write("<td>Treatment</td>");
            Response.Write("<td>Dryness</td>");
            Response.Write("<td>Finish</td>");
            Response.Write("<td style=\"text-align:right\">Packs</td>");
            Response.Write("<td>Cube</td>");
            Response.Write("<td style=\"text-align:right\">Order</td>");
            Response.Write("</tr>\n");
            Response.Write("</thead>\n<tbody>");
            int i=0;
            foreach (CartItem cartItem in cartItems.data)
            {
                if (cartItem.sku == null)
                    continue;

                if (!white)
                    Response.Write("<tr data-id=\""+i.ToString()+"\" id=\"item-"+i.ToString()+"\">");
                else
                    Response.Write("<tr data-id=\"" + i.ToString() + "\" id=\"item-" + i.ToString() + "\" style=\"background-color:#FFFFFF;\">");

                
                foreach (StockItem stockItem in stockItems)
                {
                    if (stockItem.SKU == cartItem.sku)
                    {
                        Response.Write("<td style=\"text-align:right\">" + stockItem.Width + "</td>");
                        Response.Write("<td style=\"text-align:right\">" + stockItem.Thickness + "</td>");
                        Response.Write("<td style=\"text-align:right\">" + stockItem.Length + "</td>");
                        Response.Write("<td>" + stockItem.Grade + "</td>");
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

                        Response.Write("<td><input style=\"text-align:right;\" class=\"sku-input\" type=\"text\" data=\"" + stockItem.SKU + "\" value=\"" + cartItem.quantity + "\"></td>");
                        Response.Write("<td><button class=\"remove\" data-id=\""+i.ToString()+"\" type=\"button\">-</button></td>");
                        Response.Write("</tr>\n");

                        break;
                    }
                }

                i++;
                white = !white;
            }

            Response.Write("</tbody></table>\n");
            Response.Write("<p>Add any additional notes:</p>\n");
            Response.Write("<textarea name=\"notes\" id=\"notes\" cols=\"70\" rows=\"5\"></textarea>");
            Response.Write("<button id=\"email\" type=\"button\">Order</button>");
        }
        %>
    </div>
</asp:Content>

<asp:Content runat="server" ID="ScriptContent" ContentPlaceHolderID="ScriptContent">
    <script>
        function setCookie(c_name, value, exdays) {
            var exdate = new Date();
            exdate.setDate(exdate.getDate() + exdays);
            var c_value = escape(value) + ((exdays == null) ? "" : "; expires=" + exdate.toUTCString());
            document.cookie = c_name + "=" + c_value;
        }

        function getCookie(c_name) {
            var c_value = document.cookie;
            var c_start = c_value.indexOf(" " + c_name + "=");
            if (c_start == -1) {
                c_start = c_value.indexOf(c_name + "=");
            }
            if (c_start == -1) {
                c_value = null;
            }
            else {
                c_start = c_value.indexOf("=", c_start) + 1;
                var c_end = c_value.indexOf(";", c_start);
                if (c_end == -1) {
                    c_end = c_value.length;
                }
                c_value = unescape(c_value.substring(c_start, c_end));
            }
            return c_value;
        }

        $(document).ready(function () {
            $(".sku-input").keyup(function (event) {
                var number = $(this).val();
                var isNumber = /^([1-9][0-9]*)/.test(number);
                console.log('' + number + ' ' + isNumber);
                if (isNumber) {
                    $(this).css('color', 'black');
                }
                else {
                    $(this).css('color', 'red');
                }
            });

            $(".remove").click(function (event) {
                var id = $(this).data("id");
                $('#item-' + id).remove();
            });

            $('#email').click(function () {
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
                    if (quantity != "") {
                        var found = false;
                        for (var i = 0; i < length; i++) {
                            var item = cart[i];
                            if (item.sku == sku) {
                                item.quantity += quantity;
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

                var notes = $('#notes').val();
                var data = { cartItems: { data: cart }, notes: notes };

                $.ajax({
                    type: "POST",
                    contentType: 'application/json',
                    url: "LumberService.asmx/SendOrder",
                    dataType: 'json',
                    data: JSON.stringify(data),
                    success: function (result) {
                        console.log("result.d = " + result.d);
                        if (result.d == "OK") {
                            alert('Pre-order has been successfuly sent.');
                            $('#order-text').html("Successfully pre-order.");
                            setCookie('cart', '', -1);
                        }
                        else {
                            alert('Unable to pre-order because ' + result.d);
                            $('#order-text').html("Unable to pre-order.");
                        }
                    },
                    error: function (jqXHR, status, error) {
                        $('#order-text').html("Unable to order. Please, try later.");
                    }
                });
            });
        });
    </script>
</asp:Content>
