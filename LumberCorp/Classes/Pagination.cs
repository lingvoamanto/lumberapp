using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LumberCorp
{
    public class Pagination
    {
        public static void Write(int total, string directory, int sequence)
        {
            int page = sequence / 10 + 1;

            int count = total / 10;
            int remainder = total - count * 10;
            if (remainder > 0)
                count = count + 1;

            int start = page - 2;
            int end = page + 2;
            if (start < 1)
            {
                start = 1;
                end = 5;
                if (end > count)
                    end = count;
            }
            else if (end > count)
            {
                end = count;
                start = end - 5;
                if (start < 1)
                    start = 1;
            }


            System.Web.HttpContext.Current.Response.Write("<a href=\"" + directory + "/1\">&laquo;</a>");
            if (start > 1)
            {
                System.Web.HttpContext.Current.Response.Write("<a href=\"" + directory + "/" + Math.Max(start * 10 - 19, 1).ToString() + "\">&lt;</a>");
            }
            for (int i = start; i <= end; i++)
            {
                System.Web.HttpContext.Current.Response.Write("<a href=\"" + directory + "/" + (i * 10 - 9).ToString() + "\">" + i.ToString() + "</a>");
            }
            if (end < count)
            {
                System.Web.HttpContext.Current.Response.Write("<a href=\"" + directory + "/" + (end * 10 + 1).ToString() + "\">&gt;</a>");
            }
            System.Web.HttpContext.Current.Response.Write("<a href=\"" + directory + "/" + Math.Max(total - 9, 1) + "\">&raquo;</a>");
        }
    }
}