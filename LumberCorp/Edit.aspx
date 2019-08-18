<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="Edit.aspx.cs" Inherits="LumberCorp.Edit" %>
<%@ Register TagPrefix="uc" TagName="AdminControl" Src="~/Controls/AdminControl.ascx" %>
<!DOCTYPE html>
<html lang="en">
    <head id="Head1" runat="server">
        <meta http-equiv="X-UA-Compatible" content="IE=IE8" />
        <meta charset="utf-8" />
        <title><%: CurrentNode.Title %></title>
        <link rel="Stylesheet" href="/css/default.css" />

        <script src="http://code.jquery.com/jquery-1.10.2.min.js" type=""></script>
        <script src="/tinymce/tinymce/js/tinymce/tinymce.min.js"></script>
        <%--<script src="//tinymce.cachefly.net/4.0/tinymce.min.js"></script>--%>
<script type="text/javascript">
    tinyMCE.init({
        // General options 
        mode: "textareas",
        // theme: "advanced",
        plugins: "code,image,pagebreak,layer,table,save,insertdatetime,preview,media,searchreplace,print,contextmenu,paste,directionality,fullscreen,noneditable,visualchars,nonbreaking,template,wordcount,advlist,autosave",
        // Theme options 
        theme_advanced_buttons1: "save,newdocument,|,bold,italic,underline,strikethrough,|,justifyleft,justifycenter,justifyright,justifyfull,styleselect,formatselect,fontselect,fontsizeselect",
        theme_advanced_buttons2: "cut,copy,paste,pastetext,pasteword,|,search,replace,|,bullist,numlist,|,outdent,indent,blockquote,|,undo,redo,|,link,unlink,anchor,image,cleanup,help,code,|,insertdate,inserttime,preview,|,forecolor,backcolor",
        theme_advanced_buttons3: "tablecontrols,|,hr,removeformat,visualaid,|,sub,sup,|,charmap,iespell,media,advhr,|,print,|,ltr,rtl,|,fullscreen",
        theme_advanced_buttons4: "insertlayer,moveforward,movebackward,absolute,|,styleprops,|,cite,abbr,acronym,del,ins,attribs,|,visualchars,nonbreaking,template,pagebreak,restoredraft",
        theme_advanced_toolbar_location: "top",
        theme_advanced_toolbar_align: "left",
        theme_advanced_statusbar_location: "bottom",
        theme_advanced_resizing: true,
        // Example content CSS (should be your site CSS) 
        // content_css: "/css/default.css",
        // Drop lists for link/image/media/template dialogs 
        // template_external_list_url: "lists/template_list.js",
        // external_link_list_url: "lists/link_list.js",
        //external_image_list_url: "lists/image_list.js",
        // media_external_list_url: "lists/media_list.js",
        // Style formats 
        style_formats: [
        { title: 'Bold text', inline: 'b' },
        { title: 'Red text', inline: 'span', styles: { color: '#ff0000' } },
        { title: 'Red header', block: 'h1', styles: { color: '#ff0000' } },
        { title: 'Example 1', inline: 'span', classes: 'example1' },
        { title: 'Example 2', inline: 'span', classes: 'example2' },
        { title: 'Table styles' },
        { title: 'Table row 1', selector: 'tr', classes: 'tablerow1' }
        ],
        // Replace values for the template plugin 
        template_replace_values: {
            username: "Some User",
            staffid: "991234"
        }
    });
</script>
    </head>
  <body>
    <div class="wrapper">
        <form id="editform" method="post" action="/edit/<% Response.Write(Page.RouteData.Values["page"]); %>" enctype="multipart/form-data" >
      <div class="header">
        <div class="logo"> <img src="/images/logo.png"> </div>
        <hr style="clear:both">
        <div style="margin-top:-10px;">
          <div style="clear: both;">
    <div> 
        <table id="images-table">
        <%
            int i = 0;
            foreach (LumberCorp.Image image in CurrentNode.Images)
            { 
                Response.Write("<tr>\n");
                Response.Write("<td><button type=\"button\" id=\"remove-image\">-</button></td>\n");
                Response.Write("<td><input type=\"hidden\" id=\"imageid"+i+"\" name=\"imageid"+i+"\" value=\""+image.Id+"\" /></td>\n");
                Response.Write("<td><input type=\"text\" name=\"imagepriority" + i + "\" id=\"imagepriority" + i + "\" cols=5 value=\"" + image.Priority.ToString() + "\"></td>\n");
                Response.Write("<td><input type=\"text\" name=\"imageurl" + i + "\" id=\"imageurl" + i + "\" cols=50 value=\"" + image.Url + "\"></td>\n");
                Response.Write( "</tr>\n" );
                i++;
            }                    
        %>
            
        </table>
        <% Response.Write("<input type=\"hidden\" value=\"" + (i).ToString() + "\" id=\"imagescount\" name=\"imagescount\">\n"); %><br />
         <button type="button" id="add-image">+</button>
         </div>
    <hr style="clear:both">
          </div>
          <hr> </div>
      </div>
      <div class="main">
        
    <div class="left-panel"> 
        <% int contentId = 0;
           string contentHtml = " ";
            if( CurrentNode.Contents.Count >= 2 )
            {
                contentId = CurrentNode.Contents[1].Id;
                contentHtml = CurrentNode.Contents[1].Html;
            }

            Response.Write("<input type=\"hidden\" value=\"" + contentId + "\" id=\"contentid1\" name=\"contentid1\">\n");
            Response.Write("<textarea id=\"contenthtml1\" name=\"contenthtml1\" rows=\"15\" cols=\"100\" style=\"width: 95%\">");
            Response.Write(contentHtml); 
            Response.Write("</textarea>\n");
            %>
            <input name="file" type="file" />
         <input id="file-button" type="button" value="Upload" /><br />
         <progress value="0" max="100"></progress>

        <uc:AdminControl id="adminControl" runat="server" />       
    </div>
    <div class="content">
        <% contentId = 0;
           contentHtml = " ";
           if( CurrentNode.Contents.Count >= 1 )
           {
               contentId = CurrentNode.Contents[0].Id;
               contentHtml = CurrentNode.Contents[0].Html;
           }
           Response.Write("<input type=\"hidden\" value=\"" + contentId + "\" id=\"contentid0\" name=\"contentid0\">\n");
           Response.Write("<textarea id=\"contenthtml0\" name=\"contenthtml0\" rows=\"15\" cols=\"10\" style=\"width: 95%\">");
           Response.Write(contentHtml); 
           Response.Write("</textarea>\n");
         %>  

    </div>

      </div>
      <div style="padding-top:80px;margin-top:10px;clear: both;">
        <div class="footer">
         <div class="save"> <input type="submit" value="submit" /> </div>
          <div class="legals"> <a href="#">Disclaimer</a> | <a href="#">Privacy Policy</a> </div>
        </div>
      </div>
         </form>
    </div>


<script>  
    function progressHandlingFunction(e) {
        if (e.lengthComputable) {
            $('progress').attr({ value: e.loaded, max: e.total });
        }
    }

    $(document).ready(function () {
        $(':file').change(function () {
            var file = this.files[0];
            var name = file.name;
            var size = file.size;
            var type = file.type;
            //Your validation
        });

        $('#file-button').click(function () {
            $('progress').attr({ value: 0, max: 100 });
            var formData = new FormData($('form')[0]);
            $.ajax({
                url: '/ImageHandler.ashx',  //Server script to process data
                type: 'POST',
                xhr: function () {  // Custom XMLHttpRequest
                    var myXhr = $.ajaxSettings.xhr();
                    if (myXhr.upload) { // Check if upload property exists
                        myXhr.upload.addEventListener('progress', progressHandlingFunction, false); // For handling the progress of the upload
                    }
                    return myXhr;
                },
                //Ajax events
                // beforeSend: beforeSendHandler,
                success: function (r) {
                    $('progress').attr({ value: 100, max: 100 });
                },
                // error: errorHandler,
                // Form data
                data: formData,
                //Options to tell jQuery not to process data or worry about content-type.
                cache: false,
                contentType: false,
                processData: false
            });
        });

        // Adds a new image to the top of the page
        $('#add-image').click(function (event) {
            var i = $('#imagescount').val();
            console.log("imagescount = " + i);
            var row =   "<tr>\n";
            row = row + "<tr><td><button type=\"button\" id=\"remove-image\">-</button></td></tr>\n";
            row = row + "<td><input type=\"hidden\" id=\"imageid" + i + "\" name=\"imageid" + i + "\" value=\"0\" /></td>\n";
            row = row + "<td><input type=\"text\" name=\"imagepriority" + i + "\" id=\"imagepriority" + i + "\" cols=5 value=\"\"></td>\n";
            row = row + "<td><input type=\"text\" name=\"imageurl" + i + "\" id=\"imageurl" + i + "\" cols=50 value=\"\"></td>\n";
            row = row + "</tr>\n";
            // $('#images-table tr:last').after(row);
            $('#images-table').append(row);
            console.log("row = " + row);
            i++;
            $('#imagescount').val(i);
        });

        // This used to save the current page to the database, we know do this through form submission
        $('#junk').click(function (event) {
            event.preventDefault();
            var page = $('#page').val();
            var contents = Array();
            contents[0] = $('#content0').val();
            contents[1] = $('#content1').val();
            var images = Array();
            var table = document.getElementById("images");
            for (var i = 0, row; row = table.rows[i]; i++) {
                //iterate through rows
                //rows would be accessed using the "row" variable assigned in the for loop

                images[i] = { Priority: row.cells[0], Url: row.cells[1] };
            }
            $.ajax({
                type: "POST",
                contentType: 'application/json',
                url: "LumberService.asmx/SavePage",
                dataType: 'json',
                data: JSON.stringify({
                    page: page, contents: contents, images: images
                }),
                success: function (result) {
                    console.log("result.d = " + result.d);
                    if (result.d != "OK") {
                        $('#problem-text').html(result.d);
                        $('#problem-dialog').dialog('open');
                    }
                    else {
                        $('#thanks-dialog').dialog('open');
                    }
                },
                error: function (jqXHR, status, error) {
                    $('#problem-text').html(error);
                    $('#problem-dialog').dialog('open');
                }
            });
        });
    });
</script>
      </body>
    </html>