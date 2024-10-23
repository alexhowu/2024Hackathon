// Copyright (c) Microsoft. All rights reserved.
#pragma warning disable VSTHRD111 // Use ConfigureAwait(bool)
#pragma warning disable CA1050 // Declare types in namespaces
#pragma warning disable CA2007 // Consider calling ConfigureAwait on the awaited task

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.OData.Query;
using Microsoft.SemanticKernel;
using Newtonsoft.Json.Linq;

namespace TimePlugin.Plugins;
public class Sage300Plugin
{
    [KernelFunction, Description("Retrieves Acccount Set information from Sage300.")]
    public async Task<string> GetAccountSetsAsync([Description("ODataQueryOptions object to find and search for Account set (AccountSetCode) information")] string oDataQueryOptions)
    {
        //var authenticationString = $"ADMIN:Admin123!";
        //var base64EncodedAuthenticationString = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(authenticationString));
        var base64EncodedAuthenticationString = "QURNSU46QWRtaW4xMjMh";

        var url = "http://localhost/Sage300WebApi/v1.0/-/SAMLTD/AP/APAccountSets" +
            (string.IsNullOrEmpty(oDataQueryOptions) ? string.Empty : $"?{oDataQueryOptions}");
        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);

        var response = await httpClient.GetAsync(new Uri(url));
        var stringContent = await response.Content.ReadAsStringAsync();

        return stringContent;
    }

    [KernelFunction, Description("Retrieves Customer (with customer number) purchase history from Sage300.")]
    public async Task<string> GetReceiptAsync(
        [Description("ODataQueryOptions object to find and search for customer (CustomerNumber) information and only select fields of CustomerNumber ReceiptDate CustomerReceiptAmount")] string oDataQueryOptions)
    {
        //var authenticationString = $"ADMIN:Admin123!";
        //var base64EncodedAuthenticationString = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(authenticationString));
        var base64EncodedAuthenticationString = "QURNSU46QWRtaW4xMjMh";

        var url = "http://localhost/Sage300WebApi/v1.0/-/SAMLTD/AR/ARPostedReceipts" +
            (string.IsNullOrEmpty(oDataQueryOptions) ? string.Empty : $"?{oDataQueryOptions}");
        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);

        var response = await httpClient.GetAsync(new Uri(url));
        var stringContent = await response.Content.ReadAsStringAsync();

        return stringContent;
    }

    [KernelFunction, Description("Retrieves all Customer purchase history from Sage300.")]
    public async Task<string> GetAllReceiptAsync(
        [Description("ODataQueryOptions object to find only select fields of CustomerNumber ReceiptDate CustomerReceiptAmount")] string oDataQueryOptions)
    {
        //var authenticationString = $"ADMIN:Admin123!";
        //var base64EncodedAuthenticationString = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(authenticationString));
        var base64EncodedAuthenticationString = "QURNSU46QWRtaW4xMjMh";

        var url = "http://localhost/Sage300WebApi/v1.0/-/SAMLTD/AR/ARPostedReceipts" +
            (string.IsNullOrEmpty(oDataQueryOptions) ? string.Empty : $"?{oDataQueryOptions}");
        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);

        var response = await httpClient.GetAsync(new Uri(url));
        var stringContent = await response.Content.ReadAsStringAsync();

        return stringContent;
    }


}
