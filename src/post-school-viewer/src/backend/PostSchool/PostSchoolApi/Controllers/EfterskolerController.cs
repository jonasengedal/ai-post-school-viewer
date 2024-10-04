using System.ClientModel;
using System.Text.Json;
using Azure.AI.OpenAI;
using Microsoft.AspNetCore.Mvc;
using OpenAI.Chat;
using PostSchoolApi.Models;


namespace PostSchoolApi.Controllers;
[ApiController]
[Route("api/[controller]")]
public class EfterskolerController : ControllerBase
{

    public EfterskolerController()
    {
        if (!State.Efterskoler.Any())
        {
            var efterskolder = JsonSerializer.Deserialize<List<Efterskole>>(System.IO.File.ReadAllText("MakePostSchoolData1.json"));
            State.Efterskoler = efterskolder?.ToDictionary(x => x.Id) ?? [];
        }
    }

    [HttpGet]
    public ActionResult<IEnumerable<Efterskole>> GetEfterskoler()
    {
        return State.Efterskoler.Values;
    }

    [HttpPut("{id}")]
    public IActionResult UpdateEfterskole(int id, Efterskole efterskole)
    {
        if (id != efterskole.Id) return BadRequest();

        State.Efterskoler[id] = efterskole;

        return NoContent();
    }

    [HttpPost("{id}/load")]
    public async Task<ActionResult<Efterskole>> LoadEfterskoleWithAIAsync(int id)
    {
        try
        {
            string keyFromEnvironment = Environment.GetEnvironmentVariable("AZURE_OPENAI_API_KEY") ?? System.IO.File.ReadAllText("azurekey.txt");

            AzureOpenAIClient azureClient = new(
                new Uri("https://oai-hackathon-swc.openai.azure.com/"),
                new ApiKeyCredential(keyFromEnvironment));
            //new DefaultAzureCredential());

            if (!State.Efterskoler.TryGetValue(id, out var efterskole))
            {
                return NotFound("The efterskole was not found.");
            }
            ChatClient chatClient = azureClient.GetChatClient("gpt-4");
            // Read prompts from input file
            var prompt =
                $"""
            Make me a json document for the danish efterskole with the name "{efterskole.Navn}". 
            Add the following properties: Navn, KortBeskrivelse, LangBeskrivelse, GpsKoordinater, Adresse, Linjefag, Valgfag, ObligatoriskeFag, Type.
            GpsKoordinater is an object with the properties Lat and Long which are numbers.
            Informations must be present and accurate.
            The output must be a JSON dokument.
            """;

            // Process each prompt with Azure OpenAI
            var retries = 0;
            string efterskoleJson = string.Empty;
            while (string.IsNullOrEmpty(efterskoleJson) && ++retries < 5)
            {
                efterskoleJson = await GetCompletionAsync(chatClient, prompt);
            }
            var loadedEfterskole = JsonSerializer.Deserialize<Efterskole>(efterskoleJson);
            if (loadedEfterskole == null)
            {
                return StatusCode(500, "Could not parse AI result");
            }
            loadedEfterskole.Id = id;
            State.Efterskoler[id] = loadedEfterskole;

            return loadedEfterskole;

        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }

    }

    // Method to get prompt completion from Azure OpenAI
    private static async Task<string> GetCompletionAsync(ChatClient client, string prompt)
    {
        try
        {
            // Send the request and get the completion
            var response = await client.CompleteChatAsync(prompt);

            // Extract the response text
            string completion = response.Value.Content.First().Text;

            return completion;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while getting the completion: {ex.Message}");
            return string.Empty;
        }
    }
}

public static class State
{
    public static Dictionary<int, Efterskole> Efterskoler = [];
}
