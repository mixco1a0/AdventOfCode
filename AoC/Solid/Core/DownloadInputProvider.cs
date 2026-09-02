using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AoC.Solid.Core.Interfaces;
using AoC.Solid.Utils;

namespace AoC.Solid.Core;

public class DownloadInputProvider(int year, int day) : IInputProvider
{
    private readonly int _year = year;
    private readonly int _day = day;
    private string _input = string.Empty;

    public IEnumerable<string> GetInput()
    {
        return Input.ConvertFromInput(_input);
    }

    public void DownloadInput()
    {
        if (string.IsNullOrEmpty(_input))
        {
            Task task = Task.Run(async () => await DownloadInputAsync());
            task.Wait();
        }
    }

    private async Task DownloadInputAsync()
    {
        string session = Puzzle.GetEnvironmentVariable(Puzzle.SessionKey);

        const string baseAddress = "https://adventofcode.com";
        Uri uri = new(baseAddress);
        CookieContainer cookieContainer = new();
        cookieContainer.Add(uri, new Cookie("session", session));

        using HttpClientHandler httpClientHandler = new() { CookieContainer = cookieContainer };
        using HttpClient httpClient = new(httpClientHandler) { BaseAddress = uri };

        string url = $"{_year}/day/{_day}/input";
        HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(url);
        httpResponseMessage.EnsureSuccessStatusCode();
        _input = await httpResponseMessage.Content.ReadAsStringAsync();
    }
}