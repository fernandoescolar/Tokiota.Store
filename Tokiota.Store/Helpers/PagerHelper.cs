namespace Tokiota.Store.Helpers
{
    using Models;
    using System;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;

    public static class PagerHelper
    {
        public static MvcHtmlString Pager(this HtmlHelper html, IPaginatedListModel paginatedListModel)
        {
            int totalPages = CalcTotalPages(paginatedListModel.Total, paginatedListModel.PageSize);
            int previousPage = CalcPreviousPage(paginatedListModel.Page);
            int nextPage = CalcNextPage(paginatedListModel.Page, totalPages);
            int lowerBound = CalcLowerBound(paginatedListModel.Page, 10, totalPages);
            int upperBound = CalcUpperBound(paginatedListModel.Page, 10, totalPages);

            var result = new StringBuilder("<ul class=\"pagination\">");

            if (paginatedListModel.Total != 0)
            {
                //// Show first
                //html.CreateButton(result, "|<", 1, paginatedListModel.Page == 1);

                // Show previous
                html.CreateButton(result, "Prev", previousPage, paginatedListModel.Page == 1);

                for (int i = lowerBound; i <= upperBound; i++)
                {
                    if (i == paginatedListModel.Page)
                    {
                        html.CreateCurrentButton(result, i.ToString());
                    }
                    else
                    {
                        html.CreateButton(result, i.ToString(), i);
                    }
                }

                // Show next
                html.CreateButton(result, "Next", nextPage, paginatedListModel.Page == totalPages);

                //// Show last
                //html.CreateButton(result, ">|", totalPages, paginatedListModel.Page == totalPages);
            }

            result.Append("</ul>");

            return MvcHtmlString.Create(result.ToString());
        }

        private static void CreateButton(this HtmlHelper html, StringBuilder result, string text, int gotoPage, bool disabled = false)
        {
            if (!disabled)
            {
                var urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
                var tagBuilder = new TagBuilder("a");
                tagBuilder.InnerHtml = text;
                tagBuilder.MergeAttribute("href", urlHelper.Action(html.ViewContext.RouteData.Values["action"].ToString(), new { id = gotoPage }));
                //tagBuilder.MergeAttribute("class", "pagerbutton");
                AppendLi(result, tagBuilder.ToString());
            }
        }

        private static void CreateCurrentButton(this HtmlHelper html, StringBuilder result, string text)
        {
            var tagBuilder = new TagBuilder("span");
            tagBuilder.InnerHtml = text;
            //tagBuilder.MergeAttribute("class", "pagerbutton");
            AppendLi(result, tagBuilder.ToString());
        }

        private static void AppendLi(StringBuilder result, string text)
        {
            var tagBuilder = new TagBuilder("li");
            tagBuilder.InnerHtml = text;
            result.Append(tagBuilder.ToString());
        }

        private static int CalcNextPage(int currentPage, int totalPages)
        {
            if (currentPage == totalPages)
            {
                return totalPages;
            }
            else
            {
                return currentPage + 1;
            }
        }

        private static int CalcPreviousPage(int currentPage)
        {
            if (currentPage == 1)
            {
                return 1;
            }
            else
            {
                return currentPage - 1;
            }
        }

        private static int CalcTotalPages(int totalItems, int pageSize)
        {
            double tmp = Math.Ceiling(totalItems / (double)pageSize);
            return Convert.ToInt32(tmp);
        }

        private static int CalcUpperBound(int currentPage, int pagesInPager, int totalPages)
        {
            int lowerBound = CalcLowerBound(currentPage, pagesInPager, totalPages);
            int upperBound = lowerBound + (pagesInPager - 1);

            if (upperBound > totalPages)
            {
                return totalPages;
            }

            return upperBound;
        }

        private static int CalcLowerBound(int currentPage, int pagesInPager, int totalPages)
        {
            double rawIntervalsNumber = totalPages / (double)pagesInPager;
            double tmp = totalPages / (double)currentPage;
            double rawCurrentInvertal = rawIntervalsNumber / tmp;

            if (double.IsNaN(rawCurrentInvertal))
            {
                return 0;
            }

            if (rawCurrentInvertal <= 1)
            {
                return 1;
            }

            int lowerBound = Convert.ToInt32((Math.Floor(rawCurrentInvertal) * pagesInPager) + 1);

            if (lowerBound > currentPage)
            {
                return currentPage - pagesInPager + 1;
            }

            return lowerBound;
        }
    }
}