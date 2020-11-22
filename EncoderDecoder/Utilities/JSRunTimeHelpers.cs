using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EncoderDecoder.Utilities
{
    public static class JSRunTimeHelpers
    {
        const string success = "success";
        public static async Task<bool> CopyTextToClipboard(this IJSRuntime jSRuntime, string Text)
        {
            var result = (await jSRuntime.InvokeAsync<string>("clipboardCopy.copyText", Text));
            Console.WriteLine($"result => {result}");
            return result == success;
        }
    }
}
