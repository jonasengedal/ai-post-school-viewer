using System.ClientModel;
using Azure.AI.OpenAI;
using Azure.Identity;
using OpenAI.Chat;



// File paths
string inputFilePath = args[0];
string outputFilePath = args[1];

try
{
string keyFromEnvironment = Environment.GetEnvironmentVariable("AZURE_OPENAI_API_KEY") ?? File.ReadAllText("azurekey.txt");

AzureOpenAIClient azureClient = new(
    new Uri("https://oai-hackathon-swc.openai.azure.com/"),
    new ApiKeyCredential(keyFromEnvironment));
//new DefaultAzureCredential());
    
    ChatClient chatClient = azureClient.GetChatClient("gpt-4");
    // Read prompts from input file
    if (File.Exists(inputFilePath))
    {
        var prompt = await File.ReadAllTextAsync(inputFilePath);
        Console.WriteLine("Prompt loaded successfully.");

        // Process each prompt with Azure OpenAI
        var completedPrompt = await GetCompletionAsync(chatClient, prompt);

        // Write the completed prompts to the output file
        await File.WriteAllTextAsync(outputFilePath, completedPrompt);
        Console.WriteLine($"Completed prompts written to {outputFilePath}");
    }
    else
    {
        Console.WriteLine($"Error: File {inputFilePath} not found.");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"An error occurred: {ex.Message}");
}


// Method to get prompt completion from Azure OpenAI
static async Task<string> GetCompletionAsync(ChatClient client, string prompt)
{
    try
    {
        // Send the request and get the completion
        var response = await client.CompleteChatAsync(prompt);

        // Extract the response text
        string completion = response.Value.Content.First().Text;
        Console.WriteLine($"Completed prompt: {completion}");

        return completion;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred while getting the completion: {ex.Message}");
        return string.Empty;
    }
}