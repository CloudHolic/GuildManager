using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flurl.Http;
using GuildManager.Types;
using GuildManager.Utils;
using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace GuildManager.Models.Controllers;

public class GuildMemberCrawler
{
    private static string BaseUrl => "https://maple.gg";

    private static string SyncXpath => "//button[@id='btn-sync']";

    private string ServerName { get; }

    private string GuildName { get; }

    public GuildMemberCrawler(string serverName, string guildName)
    {
        ServerName = serverName;
        GuildName = guildName;
    }

    public async Task<List<GuildMember>> GetMembers()
    {
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(await CrawlHtml());

        var rawData = htmlDoc.DocumentNode.InnerText.Split("\r\n")
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select((x, i) => new { Idx = i, Str = x.Trim() }).ToList();

        var list = new List<ValueTuple<string, string, string>>();
        rawData.ForEach(x =>
        {
            var (idx, str) = (x.Idx, x.Str);
            if (str.Contains("마지막 활동일"))
                list.Add((rawData[idx - 2].Str, rawData[idx - 1].Str, rawData[idx].Str));
        });

        return list.Skip(3)
            .Select(ParseGuildMember)
            .ToList();
    }

    public static async Task<int?> CrawlMurung(string nickname)
    {
        var response = $"{BaseUrl}/u/{nickname}".GetAsync().Result;
        var doc = await response.ResponseMessage.Content.ReadAsStringAsync();

        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(doc);

        var rawData = htmlDoc.DocumentNode.SelectNodes("//div[contains(@class, 'text-center')]")[1].InnerText;

        return rawData.Split("층")[0].ExtractInteger();
    }

    private async Task<string> CrawlHtml()
    {
        string pageSource;

        try
        {
            var driverService = ChromeDriverService.CreateDefaultService();
            driverService.HideCommandPromptWindow = true;

            var options = new ChromeOptions();
            options.AddArgument("--headless");

            using var driver = new ChromeDriver(driverService, options);
            driver.Url = $"{BaseUrl}/guild/{ServerName}/{GuildName}";
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2);

            driver.FindElement(By.XPath(SyncXpath)).Click();
            driver.SwitchTo().Alert().Accept();
            await Task.Delay(5000);

            pageSource = driver.PageSource;
        }
        catch (Exception)
        {
            return string.Empty;
        }

        return pageSource;
    }

    private static GuildMember ParseGuildMember(ValueTuple<string, string, string> rawData)
    {
        var (nickname, jobAndLevel, lastActivity) = rawData;
        var job = jobAndLevel.Split("/")[0];
        var level = jobAndLevel.Split("/")[1];

        return new GuildMember(nickname, job, level.ExtractInteger() ?? 0, null, lastActivity.ExtractInteger() ?? 1, Position.Member);
    }
}