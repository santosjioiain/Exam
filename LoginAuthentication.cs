﻿using HtmlAgilityPack;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WebScraping
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        string url = "https://en.wikipedia.org/wiki/List_of_programmers";
        var response = CallUrl(url).Result;
        var linkList = ParseHtml(response);
        WriteToCsv(linkList);

        return View();
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    private static async Task<string> CallUrl(string fullUrl)
    {
        HttpClient client = new HttpClient();
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls13;
        client.DefaultRequestHeaders.Accept.Clear();
        var response = client.GetStringAsync(fullUrl);
        return await response;
    }

    private List<string> ParseHtml(string html)
    {
        HtmlDocument htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(html);
        var programmerLinks = htmlDoc.DocumentNode.Descendants("li")
                .Where(node => !node.GetAttributeValue("class", "").Contains("tocsection")).ToList();

        List<string> wikiLink = new List<string>();

        foreach (var link in programmerLinks)
        {
            if (link.FirstChild.Attributes.Count > 0)
                wikiLink.Add("https://en.wikipedia.org/" + link.FirstChild.Attributes[0].Value);
        }

        return wikiLink;

    }

    private void WriteToCsv(List<string> links)
    {
        StringBuilder sb = new StringBuilder();
        foreach (var link in links)
        {
            sb.AppendLine(link);
        }

        System.IO.File.WriteAllText("links.csv", sb.ToString());
    }
}
}
