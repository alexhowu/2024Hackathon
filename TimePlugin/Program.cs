// Copyright (c) Microsoft. All rights reserved.
#pragma warning disable VSTHRD111 // Use ConfigureAwait(bool)
#pragma warning disable CA1050 // Declare types in namespaces
#pragma warning disable CA2007 // Consider calling ConfigureAwait on the awaited task

using System.ComponentModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Newtonsoft.Json.Linq;
using TimePlugin;
using TimePlugin.Plugins;
using winforms_chat;
using static System.Net.Mime.MediaTypeNames;

var config = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .AddEnvironmentVariables()
    .Build()
    ?? throw new InvalidOperationException("Configuration is not provided.");

var kernelBuilder = Kernel.CreateBuilder().AddAzureOpenAIChatCompletion(
    deploymentName: "mygpt",
    endpoint: "https://sage300azureopenai.openai.azure.com/",
    apiKey: "50078e7b63794f029b47a8c3d07b632b"
);

kernelBuilder.Plugins.AddFromType<TimeInformationPlugin>();
kernelBuilder.Plugins.AddFromType<StockInformationPlugin>();
kernelBuilder.Plugins.AddFromType<Sage300Plugin>();
var kernel = kernelBuilder.Build();

// Get chat completion service
var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

// Enable auto function calling
OpenAIPromptExecutionSettings openAIPromptExecutionSettings = new()
{
    ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions
};

System.Windows.Forms.Application.SetHighDpiMode(HighDpiMode.SystemAware);
System.Windows.Forms.Application.EnableVisualStyles();
System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);

ChatHistory chatHistory = [];

var form = new Form1();
form.Load += Form_Load;
form.UserMessage += Form_UserMessage;


async void Form_UserMessage(object sender, string message)
{
    chatHistory.AddUserMessage(message);
    var chatResult = await chatCompletionService.GetChatMessageContentsAsync(chatHistory, openAIPromptExecutionSettings, kernel);
    ((Form1)sender).AddMessage(chatResult.Last().ToString());
}
void Form_Load(object? sender, EventArgs e)
{
    ((Form1)sender).AddMessage("Ask questions to use the Time Plugin such as: What time is it?");
}

System.Windows.Forms.Application.Run(form);

/// <summary>
/// A plugin that returns the current time.
/// </summary>
public class TimeInformationPlugin
{
    /// <summary>
    /// Retrieves the current time in UTC.
    /// </summary>
    /// <returns>The current time in UTC. </returns>
    [KernelFunction, Description("Retrieves the current time in UTC.")]
    public string GetCurrentUtcTime([Description("The name of the city of the current time")] string city)
    {
        return DateTime.UtcNow.ToString("R");
    }
}

/// <summary>
/// A plugin that returns the stock information.
/// </summary>
public class StockInformationPlugin
{
    /// <summary>
    /// Retrieves the current stock price of a company.
    /// </summary>
    /// <param name="stockSymbol"></param>
    /// <returns></returns>
    [KernelFunction, Description("Retrieves the current stock price of a company.")]
    public async Task<string> GetCurrentStockPriceAsync([Description("The stock symbol name of the company")] string stockSymbol)
    {
        const string ApiKey = "2LNMH5UIBXMJAXG1";
        var ApiUrl = $"https://www.alphavantage.co/query?function=TIME_SERIES_INTRADAY&interval=5min&symbol={stockSymbol}&apikey={ApiKey}";

        var client = new HttpClient();
        var response = await client.GetAsync(new Uri(ApiUrl));
        var stockContent = await response.Content.ReadAsStringAsync();

        var stockObject = JObject.Parse(stockContent);
        var stockPrice = stockObject?["Time Series (5min)"]?.First?.First?["1. open"]?.ToString();
        return stockPrice;
    }
}
