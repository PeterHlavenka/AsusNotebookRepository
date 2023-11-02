// See https://aka.ms/new-console-template for more information

Main().Wait();

static async Task Main()
{
    var url = "https://www.zsplesivec.cz/data/files/343plan-8.a.pdf";
    var baseFileName = "Plan-8A.pdf"; // The base filename
    var folderPath = @"C:\Users\phlavenka\Documents\PlanyPlesivec\"; // Folder where the file will be saved
    var emailRecipient = "peter.x.hlavenka.consultant@nielsen.com";

    try
    {
        using (var client = new HttpClient())
        {
            // Send a GET request to the URL
            var response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                // Initialize a counter to add to the filename if it already exists
                int fileCounter = 1;
                string fileName = Path.Combine(folderPath, baseFileName);

                // Check if the file already exists, and if it does, increment the counter
                while (File.Exists(fileName))
                {
                    fileName = Path.Combine(folderPath, $"{Path.GetFileNameWithoutExtension(baseFileName)} ({fileCounter}){Path.GetExtension(baseFileName)}");
                    fileCounter++;
                }

                // Read the content of the response as a byte array
                var fileData = await response.Content.ReadAsByteArrayAsync();

                // Save the byte array to the local file with the updated name
                File.WriteAllBytes(fileName, fileData);

                Console.WriteLine($"Downloaded '{fileName}' successfully.");

                // Send the PDF file as an email attachment
               // SendEmailWithAttachment(emailRecipient, fileName);
            }
            else
            {
                Console.WriteLine($"Failed to download the file. Status code: {response.StatusCode}");
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred: {ex.Message}");
    }
}