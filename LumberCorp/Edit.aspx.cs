using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LumberCorp
{
    public partial class Edit : System.Web.UI.Page
    {
        Node node = null;

        public Node CurrentNode
        {
            get
            {
                if (node == null)
                {
                    string page = Page.RouteData.Values["page"] as string;
                    if (page == null)
                        node = null;
                    else
                        node = ContentManagementSystem.FindNodeByPage(page);
                }
                // check node for validity
                // there should be two contact pages
                return node;
            }
        }

        private void Authorize()
        {
            User user = (User) HttpContext.Current.Session["user"];

            if( user == null )
                Response.Redirect("/404");

            if (user.Status != 2)
            {
                Response.Redirect("/404");
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Authorize();
            if( CurrentNode == null )
                Response.Redirect("/404");

            if( Request.Form.Count == 0 )
                return;

            for (int i = 0; i < 2; i++)
            {
                Content content;
                if (i < CurrentNode.Contents.Count)
                {
                    content = CurrentNode.Contents[i];
                }
                else
                {
                    content = new Content();
                    content.Node = CurrentNode;
                }
                content.Position = i;
                string contentId = Request.Form["contentid" + i].ToString();
                content.Id = int.Parse( contentId );
                content.Html = Request.Form["contenthtml"+ i].ToString();
                Console.WriteLine(content.Html);
                ContentManagementSystem.SaveContent(content);
            }

            // Remove any images that the user has deleted
            int imagesCount = int.Parse(Request.Form["imagescount"].ToString());
            foreach (Image image in CurrentNode.Images)
            {
                bool found = false;
                
                for( int i=0; i<imagesCount; i++ )
                {
                    int id = int.Parse(Request.Form["imageid" + i].ToString());
                    if (image.Id == id)
                    {
                        found = true;
                        break;
                    }
                }

                if( ! found )
                    ContentManagementSystem.RemoveImage(image);
            }


            // Add in the new images
            node.Images.Clear();
            for (int i = 0; i < imagesCount; i++)
            {
                Image image = new Image();
                image.Node = CurrentNode;
                string imageId = Request.Form["imageid" + i].ToString();
                image.Id = int.Parse(imageId);
                image.Url = Request.Form["imageurl" + i].ToString();
                ContentManagementSystem.SaveImage(image);

                node.Images.Add(image);
            }
        }
    }
}