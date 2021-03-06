using System;
using System.Collections.Generic;
using System.Linq;
using SecSoul.Model.Entity;

namespace SecSoul.WebAPI.Helpers
{
    public class HtmlHelper
    {
        public string AddDirbResults(string HtmlContent, ScanRequest request)
        {
            var HtmlContentParsedUnchanged = HtmlContent.Split('\n').ToList();
            var HtmlContentParsed = HtmlContentParsedUnchanged.Select(s => s.Replace("Nmap", "External")).ToList();

            var SmStart = HtmlContentParsed.FindIndex(x => x.Contains("Scan Summary"));
            var Smdone = HtmlContentParsed.FindIndex(x => x.Contains("External done"));
            
            HtmlContentParsed.RemoveRange(SmStart, Smdone - SmStart);
            
            var startCount = HtmlContentParsed.Count;
            var bodyStart = HtmlContentParsed.FindIndex(x => x.Contains("<body>"));
            
            HtmlContentParsed.Insert(bodyStart + (HtmlContentParsed.Count - startCount) + 1,"<div>");
            HtmlContentParsed.Insert(bodyStart + (HtmlContentParsed.Count - startCount) + 1,"<h1>Page Enumeration Results</h1>");
            HtmlContentParsed.Insert(bodyStart + (HtmlContentParsed.Count - startCount) + 1,"</div>");

            if (request.ScanDirb.Any())
            {
                HtmlContentParsed.Insert(bodyStart + (HtmlContentParsed.Count - startCount) + 1,"<div>");
                HtmlContentParsed.Insert(bodyStart + (HtmlContentParsed.Count - startCount) + 1,"<h2>Pages found:</h2>");

                if (request.ScanDirb.Any(x => !x.IsDirectory))
                {
                    HtmlContentParsed.Insert(bodyStart + (HtmlContentParsed.Count - startCount) + 1,"<table>");
                    HtmlContentParsed.Insert(bodyStart + (HtmlContentParsed.Count - startCount) + 1,"<tr class=\"head\">");
                    HtmlContentParsed.Insert(bodyStart + (HtmlContentParsed.Count - startCount) + 1,"<td>Url</td>");
                    HtmlContentParsed.Insert(bodyStart + (HtmlContentParsed.Count - startCount) + 1,"<td>Http Status</td>");
                    HtmlContentParsed.Insert(bodyStart + (HtmlContentParsed.Count - startCount) + 1,"</tr>");
                    
                    foreach (var item in request.ScanDirb.Where(x => !x.IsDirectory))
                    {
                        HtmlContentParsed.Insert(bodyStart + (HtmlContentParsed.Count - startCount) + 1,"<tr>");
                        HtmlContentParsed.Insert(bodyStart + (HtmlContentParsed.Count - startCount) + 1,$"<td><a href=\"{item.FoundUrl}\">{item.FoundUrl}</a></td>");
                        HtmlContentParsed.Insert(bodyStart + (HtmlContentParsed.Count - startCount) + 1,$"<td>{item.HttpStatus}</td>");
                        HtmlContentParsed.Insert(bodyStart + (HtmlContentParsed.Count - startCount) + 1,"</tr>");
                    }
                    HtmlContentParsed.Insert(bodyStart + (HtmlContentParsed.Count - startCount) + 1,"</table>");
                }
                else
                {
                    HtmlContentParsed.Insert(bodyStart + (HtmlContentParsed.Count - startCount) + 1,"<h3>No pages found</h3>");
                }
                HtmlContentParsed.Insert(bodyStart + (HtmlContentParsed.Count - startCount) + 1,"</div>");
                HtmlContentParsed.Insert(bodyStart + (HtmlContentParsed.Count - startCount) + 1,"<div>");
                HtmlContentParsed.Insert(bodyStart + (HtmlContentParsed.Count - startCount) + 1,"<h2>Directories found:</h2>");
                if (request.ScanDirb.Any(x => x.IsDirectory))
                {
                    HtmlContentParsed.Insert(bodyStart + (HtmlContentParsed.Count - startCount) + 1,"<table>");
                    HtmlContentParsed.Insert(bodyStart + (HtmlContentParsed.Count - startCount) + 1,"<tr class=\"head\">");
                    HtmlContentParsed.Insert(bodyStart + (HtmlContentParsed.Count - startCount) + 1,"<td>Url</td>");
                    HtmlContentParsed.Insert(bodyStart + (HtmlContentParsed.Count - startCount) + 1,"<td>Is Listable</td>");
                    HtmlContentParsed.Insert(bodyStart + (HtmlContentParsed.Count - startCount) + 1,"</tr>");
                    foreach (var item in request.ScanDirb.Where(x => x.IsDirectory))
                    {
                        HtmlContentParsed.Insert(bodyStart + (HtmlContentParsed.Count - startCount) + 1,"<tr>");
                        HtmlContentParsed.Insert(bodyStart + (HtmlContentParsed.Count - startCount) + 1,$"<td><a href=\"{item.FoundUrl}\">{item.FoundUrl}</a></td>");
                        HtmlContentParsed.Insert(bodyStart + (HtmlContentParsed.Count - startCount) + 1,$"<td>{item.IsListable}</td>");
                        HtmlContentParsed.Insert(bodyStart + (HtmlContentParsed.Count - startCount) + 1,"</tr>");
                    }
                    HtmlContentParsed.Insert(bodyStart + (HtmlContentParsed.Count - startCount) + 1,"</table>");
                }
                else
                {
                    HtmlContentParsed.Insert(bodyStart + (HtmlContentParsed.Count - startCount) + 1,"<h3>No Directories Found</h3>");
                }
                HtmlContentParsed.Insert(bodyStart + (HtmlContentParsed.Count - startCount) + 1,"</div>");
            }
            else
            {
                HtmlContentParsed.Insert(bodyStart + (HtmlContentParsed.Count - startCount) + 1,"<h2>No Results gathered</h2>");
            }
            var result = String.Join('\n', HtmlContentParsed.ToArray());
            return String.Join('\n', HtmlContentParsed.ToArray());
        }

        public string AddVirusTotalResults(string HtmlContent, ScanRequest request)
        {
            var HtmlContentParsed = HtmlContent.Split('\n').ToList();
            
            var startCount = HtmlContentParsed.Count;
            var bodyStart = HtmlContentParsed.FindIndex(x => x.Contains("<body>"));
            
            HtmlContentParsed.Insert(bodyStart + (HtmlContentParsed.Count - startCount) + 1,"<div>");
            HtmlContentParsed.Insert(bodyStart + (HtmlContentParsed.Count - startCount) + 1,"<h1>Website Access Scan Results</h1>");
            HtmlContentParsed.Insert(bodyStart + (HtmlContentParsed.Count - startCount) + 1,"</div>");

            if (request.ScanVirusTotal.Any(x => x.ScanResult == true))
            {
                HtmlContentParsed.Insert(bodyStart + (HtmlContentParsed.Count - startCount) + 1,"<table>");
                HtmlContentParsed.Insert(bodyStart + (HtmlContentParsed.Count - startCount) + 1,"<tr class=\"head\">");
                HtmlContentParsed.Insert(bodyStart + (HtmlContentParsed.Count - startCount) + 1,"<td>Provider</td>");
                HtmlContentParsed.Insert(bodyStart + (HtmlContentParsed.Count - startCount) + 1,"<td>Risk detected</td>");
                HtmlContentParsed.Insert(bodyStart + (HtmlContentParsed.Count - startCount) + 1,"</tr>");
                foreach (var item in request.ScanVirusTotal.Where(x => x.ScanResult == true))
                {
                    HtmlContentParsed.Insert(bodyStart + (HtmlContentParsed.Count - startCount) + 1,"<tr>");
                    HtmlContentParsed.Insert(bodyStart + (HtmlContentParsed.Count - startCount) + 1,$"<td>{item.ScanProvider}</td>");
                    HtmlContentParsed.Insert(bodyStart + (HtmlContentParsed.Count - startCount) + 1,$"<td>{item.ScanResult}</td>");
                    HtmlContentParsed.Insert(bodyStart + (HtmlContentParsed.Count - startCount) + 1,"</tr>");
                }
                HtmlContentParsed.Insert(bodyStart + (HtmlContentParsed.Count - startCount) + 1,"</table>");
            }
            else
            {
                HtmlContentParsed.Insert(bodyStart + (HtmlContentParsed.Count - startCount) + 1,"<h2>No Risks Detected</h2>");
            }
            var result = String.Join('\n', HtmlContentParsed.ToArray());
            return String.Join('\n', HtmlContentParsed.ToArray());
        }

        public string AddHashCheckResults(string HtmlContent, ScanRequest request)
        {
            var HtmlContentParsed = HtmlContent.Split('\n').ToList();
            var startCount = HtmlContentParsed.Count;
            var bodyStart = HtmlContentParsed.FindIndex(x => x.Contains("<body>"));
            
            HtmlContentParsed.Insert(bodyStart + (HtmlContentParsed.Count - startCount) + 1,"<div>");
            HtmlContentParsed.Insert(bodyStart + (HtmlContentParsed.Count - startCount) + 1,"<h1>File Hash Scan Results</h1>");
            HtmlContentParsed.Insert(bodyStart + (HtmlContentParsed.Count - startCount) + 1,"</div>");

            if (request.ScanHashCheck.Any())
            {
                HtmlContentParsed.Insert(bodyStart + (HtmlContentParsed.Count - startCount) + 1,"<table>");
                HtmlContentParsed.Insert(bodyStart + (HtmlContentParsed.Count - startCount) + 1,"<tr class=\"head\">");
                HtmlContentParsed.Insert(bodyStart + (HtmlContentParsed.Count - startCount) + 1,"<td>File Hash</td>");
                HtmlContentParsed.Insert(bodyStart + (HtmlContentParsed.Count - startCount) + 1,"<td>File Location</td>");
                HtmlContentParsed.Insert(bodyStart + (HtmlContentParsed.Count - startCount) + 1,"<td>Malware Chance Percentage</td>");
                HtmlContentParsed.Insert(bodyStart + (HtmlContentParsed.Count - startCount) + 1,"</tr>");
                foreach (var item in request.ScanHashCheck)
                {
                    HtmlContentParsed.Insert(bodyStart + (HtmlContentParsed.Count - startCount) + 1,"<tr>");
                    HtmlContentParsed.Insert(bodyStart + (HtmlContentParsed.Count - startCount) + 1,$"<td>{item.Hash}</td>");
                    HtmlContentParsed.Insert(bodyStart + (HtmlContentParsed.Count - startCount) + 1,$"<td>{item.Location}</td>");
                    HtmlContentParsed.Insert(bodyStart + (HtmlContentParsed.Count - startCount) + 1,$"<td>{item.MalwarePercentage}</td>");
                    HtmlContentParsed.Insert(bodyStart + (HtmlContentParsed.Count - startCount) + 1,"</tr>");
                }
                HtmlContentParsed.Insert(bodyStart + (HtmlContentParsed.Count - startCount) + 1,"</table>");
            }
            else
            {
                HtmlContentParsed.Insert(bodyStart + (HtmlContentParsed.Count - startCount) + 1,"<h2>No Bad File Hashes Found</h2>");
            }
            
            var result = String.Join('\n', HtmlContentParsed.ToArray());
            return String.Join('\n', HtmlContentParsed.ToArray());
        }
    }
}