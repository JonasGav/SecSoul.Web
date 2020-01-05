using System;
using System.Linq;
using SecSoul.Model.Entity;

namespace SecSoul.WebAPI.Helpers
{
    public class HtmlHelper
    {
        public string AddDirbResults(string HtmlContent, ScanRequest request)
        {
            var HtmlContentParsed = HtmlContent.Split('\n').ToList();
            HtmlContentParsed.Insert(HtmlContentParsed.Count - 2,"<div>");
            HtmlContentParsed.Insert(HtmlContentParsed.Count - 2,"<h1>Page Enumeration Results</h1>");
            HtmlContentParsed.Insert(HtmlContentParsed.Count - 2,"</div>");

            if (request.ScanDirb.Any())
            {
                HtmlContentParsed.Insert(HtmlContentParsed.Count - 2,"<div>");
                HtmlContentParsed.Insert(HtmlContentParsed.Count - 2,"<h2>Pages found:</h2>");

                if (request.ScanDirb.Any(x => !x.IsDirectory))
                {
                    HtmlContentParsed.Insert(HtmlContentParsed.Count - 2,"<table>");
                    HtmlContentParsed.Insert(HtmlContentParsed.Count - 2,"<tr class=\"head\">");
                    HtmlContentParsed.Insert(HtmlContentParsed.Count - 2,"<td>Url</td>");
                    HtmlContentParsed.Insert(HtmlContentParsed.Count - 2,"<td>Http Status</td>");
                    HtmlContentParsed.Insert(HtmlContentParsed.Count - 2,"</tr>");
                    
                    foreach (var item in request.ScanDirb.Where(x => !x.IsDirectory))
                    {
                        HtmlContentParsed.Insert(HtmlContentParsed.Count - 2,"<tr>");
                        HtmlContentParsed.Insert(HtmlContentParsed.Count - 2,$"<td><a href=\"{item.FoundUrl}\">{item.FoundUrl}</a></td>");
                        HtmlContentParsed.Insert(HtmlContentParsed.Count - 2,$"<td>{item.HttpStatus}</td>");
                        HtmlContentParsed.Insert(HtmlContentParsed.Count - 2,"</tr>");
                    }
                    HtmlContentParsed.Insert(HtmlContentParsed.Count - 2,"</table>");
                }
                else
                {
                    HtmlContentParsed.Insert(HtmlContentParsed.Count - 2,"<h3>No pages found</h3>");
                }
                HtmlContentParsed.Insert(HtmlContentParsed.Count - 2,"</div>");
                HtmlContentParsed.Insert(HtmlContentParsed.Count - 2,"<div>");
                HtmlContentParsed.Insert(HtmlContentParsed.Count - 2,"<h2>Directories found:</h2>");
                if (request.ScanDirb.Any(x => x.IsDirectory))
                {
                    HtmlContentParsed.Insert(HtmlContentParsed.Count - 2,"<table>");
                    HtmlContentParsed.Insert(HtmlContentParsed.Count - 2,"<tr class=\"head\">");
                    HtmlContentParsed.Insert(HtmlContentParsed.Count - 2,"<td>Url</td>");
                    HtmlContentParsed.Insert(HtmlContentParsed.Count - 2,"<td>Is Listable</td>");
                    HtmlContentParsed.Insert(HtmlContentParsed.Count - 2,"</tr>");
                    foreach (var item in request.ScanDirb.Where(x => x.IsDirectory))
                    {
                        HtmlContentParsed.Insert(HtmlContentParsed.Count - 2,"<tr>");
                        HtmlContentParsed.Insert(HtmlContentParsed.Count - 2,$"<td><a href=\"{item.FoundUrl}\">{item.FoundUrl}</a></td>");
                        HtmlContentParsed.Insert(HtmlContentParsed.Count - 2,$"<td>{item.IsListable}</td>");
                        HtmlContentParsed.Insert(HtmlContentParsed.Count - 2,"</tr>");
                    }
                    HtmlContentParsed.Insert(HtmlContentParsed.Count - 2,"</table>");
                }
                else
                {
                    HtmlContentParsed.Insert(HtmlContentParsed.Count - 2,"<h3>No Directories Found</h3>");
                }
                HtmlContentParsed.Insert(HtmlContentParsed.Count - 2,"</div>");
            }
            else
            {
                HtmlContentParsed.Insert(HtmlContentParsed.Count - 2,"<h2>No Results gathered</h2>");
            }
            var result = String.Join('\n', HtmlContentParsed.ToArray());
            return String.Join('\n', HtmlContentParsed.ToArray());
        }

        public string AddVirusTotalResults(string HtmlContent, ScanRequest request)
        {
            var HtmlContentParsed = HtmlContent.Split('\n').ToList();
            HtmlContentParsed.Insert(HtmlContentParsed.Count - 2,"<div>");
            HtmlContentParsed.Insert(HtmlContentParsed.Count - 2,"<h1>Virus Total Results</h1>");
            HtmlContentParsed.Insert(HtmlContentParsed.Count - 2,"</div>");

            if (request.ScanVirusTotal.Any())
            {
                HtmlContentParsed.Insert(HtmlContentParsed.Count - 2,"<table>");
                HtmlContentParsed.Insert(HtmlContentParsed.Count - 2,"<tr class=\"head\">");
                HtmlContentParsed.Insert(HtmlContentParsed.Count - 2,"<td>Provider</td>");
                HtmlContentParsed.Insert(HtmlContentParsed.Count - 2,"<td>Risk detected</td>");
                HtmlContentParsed.Insert(HtmlContentParsed.Count - 2,"</tr>");
                foreach (var item in request.ScanVirusTotal.Where(x => x.ScanResult == true))
                {
                    HtmlContentParsed.Insert(HtmlContentParsed.Count - 2,"<tr>");
                    HtmlContentParsed.Insert(HtmlContentParsed.Count - 2,$"<td>{item.ScanProvider}</td>");
                    HtmlContentParsed.Insert(HtmlContentParsed.Count - 2,$"<td>{item.ScanResult}</td>");
                    HtmlContentParsed.Insert(HtmlContentParsed.Count - 2,"</tr>");
                }
                HtmlContentParsed.Insert(HtmlContentParsed.Count - 2,"</table>");
            }
            else
            {
                HtmlContentParsed.Insert(HtmlContentParsed.Count - 2,"<h2>No Results gathered</h2>");
            }
            var result = String.Join('\n', HtmlContentParsed.ToArray());
            return String.Join('\n', HtmlContentParsed.ToArray());
        }

        public string AddHashCheckResults(string HtmlContent, ScanRequest request)
        {
            var HtmlContentParsed = HtmlContent.Split('\n').ToList();
            HtmlContentParsed.Insert(HtmlContentParsed.Count - 2,"<div>");
            HtmlContentParsed.Insert(HtmlContentParsed.Count - 2,"<h1>File Hash Scan Results</h1>");
            HtmlContentParsed.Insert(HtmlContentParsed.Count - 2,"</div>");

            if (request.ScanHashCheck.Any())
            {
                HtmlContentParsed.Insert(HtmlContentParsed.Count - 2,"<table>");
                HtmlContentParsed.Insert(HtmlContentParsed.Count - 2,"<tr class=\"head\">");
                HtmlContentParsed.Insert(HtmlContentParsed.Count - 2,"<td>File Hash</td>");
                HtmlContentParsed.Insert(HtmlContentParsed.Count - 2,"<td>File Location</td>");
                HtmlContentParsed.Insert(HtmlContentParsed.Count - 2,"<td>Malware Chance Percentage</td>");
                HtmlContentParsed.Insert(HtmlContentParsed.Count - 2,"</tr>");
                foreach (var item in request.ScanHashCheck)
                {
                    HtmlContentParsed.Insert(HtmlContentParsed.Count - 2,"<tr>");
                    HtmlContentParsed.Insert(HtmlContentParsed.Count - 2,$"<td>{item.Hash}</td>");
                    HtmlContentParsed.Insert(HtmlContentParsed.Count - 2,$"<td>{item.Location}</td>");
                    HtmlContentParsed.Insert(HtmlContentParsed.Count - 2,$"<td>{item.MalwarePercentage}</td>");
                    HtmlContentParsed.Insert(HtmlContentParsed.Count - 2,"</tr>");
                }
                HtmlContentParsed.Insert(HtmlContentParsed.Count - 2,"</table>");
            }
            else
            {
                HtmlContentParsed.Insert(HtmlContentParsed.Count - 2,"<h2>No Bad File Hashes Found</h2>");
            }
            
            var result = String.Join('\n', HtmlContentParsed.ToArray());
            return String.Join('\n', HtmlContentParsed.ToArray());
        }
    }
}