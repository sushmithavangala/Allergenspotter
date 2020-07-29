using System;
using System.Collections.Generic;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace Allergenspotter.Services
{
    public class ComputerVisionService
    {

        // private string subscriptionKey = Environment.GetEnvironmentVariable("COMPUTER_VISION_SUBSCRIPTION_KEY");
        // private string endpoint = Environment.GetEnvironmentVariable("COMPUTER_VISION_ENDPOINT");
        // private readonly ComputerVisionClient _client;

        public ComputerVisionService()
        {
            
        }

        // public static ComputerVisionClient Authenticate(string endpoint, string key)
        // {
        //     ComputerVisionClient client =
        //       new ComputerVisionClient(new ApiKeyServiceClientCredentials(key))
        //       { Endpoint = endpoint };
        //     return client;
        // }

        public async Task<List<string>> BatchReadFileUrl(ComputerVisionClient client, string urlImage)
        {
            String fullTextResultList = "";

            // Read text from URL
            
            var textHeaders = await client.BatchReadFileAsync(urlImage);
            // After the request, get the operation location (operation ID)
            string operationLocation = textHeaders.OperationLocation;

            const int numberOfCharsInOperationId = 36;
            string operationId = operationLocation.Substring(operationLocation.Length - numberOfCharsInOperationId);

            // Extract the text
            // Delay is between iterations and tries a maximum of 10 times.
            int i = 0;
            int maxRetries = 10;
            ReadOperationResult results;
            do
            {
                results = await client.GetReadOperationResultAsync(operationId);
                Console.WriteLine("Server status: {0}, waiting {1} seconds...", results.Status, i);
                await Task.Delay(1000);
                if (i == 9)
                {
                    Console.WriteLine("Server timed out.");
                }
            }
            while ((results.Status == TextOperationStatusCodes.Running ||
                results.Status == TextOperationStatusCodes.NotStarted) && i++ < maxRetries);

            var textRecognitionLocalFileResults = results.RecognitionResults;
            foreach (TextRecognitionResult recResult in textRecognitionLocalFileResults)
            {
                foreach (Line line in recResult.Lines)
                {
                    fullTextResultList += line.Text + " ";
                }
            }
            var cleanResults = resultCleanUp(fullTextResultList);
            return cleanResults;
        }

        private static List<string> resultCleanUp(string batchResult)
        {

            /***
                all_lines = ""
                # Print the detected text, line by line
                if get_text_results.status == OperationStatusCodes.succeeded:
                    for text_result in get_text_results.analyze_result.read_results:
                        for line in text_result.lines:
                            all_lines += line.text + " "

                split_lines = all_lines.split(",")
                output = [s.replace('(', "") for s in split_lines]
                output = [s.replace(')', "") for s in output]
                output = [s.strip() for s in output]

                return json.dumps({'ingredients': output})
                ***/

            var splitLines = batchResult.Split(',');
                for(var line = 0; line < splitLines.Length;line++)
                {
                    splitLines[line] = splitLines[line].Replace("(",string.Empty);
                    splitLines[line] = splitLines[line].Replace(")",string.Empty);
                    splitLines[line] = splitLines[line].Trim();
                }

                return splitLines.ToList();
        }

        //public IEnumerable<String> getTextFromImage(String imageUrl)
        //{
        //    ComputerVisionClient client = Authenticate(endpoint, subscriptionKey);

        //}
    }
}