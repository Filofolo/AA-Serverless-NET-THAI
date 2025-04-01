using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace THAI_HttpTrigger
{
    public static class ResizeHttpTrigger
    {
        // TODO N'accepter que le POST
        [FunctionName("ResizeHttpTrigger")]
        // public static async Task<IActionResult> Run(
        //     [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
        //     ILogger log)
        // {
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Image resize function processing request.");

            try
            {
                // int w = 0; // TODO récupérer le paramètre w
                // int h = 0; // TODO récupérer le paramètre h

                // Récupération et validation des paramètres de taille
                if (!req.Query.TryGetValue("w", out var widthParam) || !int.TryParse(widthParam, out int w) || w <= 0)
                {
                    return new BadRequestObjectResult("Please provide a valid positive 'w' (width) parameter in the query string");
                }

                if (!req.Query.TryGetValue("h", out var heightParam) || !int.TryParse(heightParam, out int h) || h <= 0)
                {
                    return new BadRequestObjectResult("Please provide a valid positive 'h' (height) parameter in the query string");
                }

                // Vérification que le corps de la requête contient des données
                if (req.Body == null || req.Body.Length == 0)
                {
                    return new BadRequestObjectResult("Please provide an image in the request body");
                }

                byte[] targetImageBytes;
                using (var msInput = new MemoryStream())
                {
                    // Récupère le corps du message en mémoire
                    await req.Body.CopyToAsync(msInput);
                    msInput.Position = 0;

                    // Charge l'image       
                    using (var image = Image.Load(msInput))
                    {
                        // Effectue la transformation
                        image.Mutate(x => x.Resize(w, h));

                        // Sauvegarde en mémoire               
                        using (var msOutput = new MemoryStream())
                        {
                            image.SaveAsJpeg(msOutput);
                            targetImageBytes = msOutput.ToArray();
                        }
                    }
                }
                // Renvoie le contenu avec le content-type correspondant à une image jpeg
                // TODO renvoyer les octets de l'image
                // TODO ... ainsi que le content-type correspondant à une image Jpeg
                return new FileContentResult(targetImageBytes, "image/jpeg");
            }
            catch (UnknownImageFormatException)
            {
                return new BadRequestObjectResult("The provided file is not a valid image format");
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Error processing image");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}

// Après déployement, en AuthorizationLevel.Anonymous on peut tester la fonction avec la commande suivante : curl --data-binary "@image.jpg" -X POST "https://thaiflorian-fa.azurewebsites.net/api/ResizeHttpTrigger?w=100&h=100" -v > output.jpeg
// Après déployement, en AuthorizationLevel.Function on peut tester la fonction avec la commande suivante : curl --data-binary "@image.jpg" -X POST "https://thaiflorian-fa.azurewebsites.net/api/ResizeHttpTrigger?w=100&h=100" -H "x-functions-key: YOUR_FUNCTION_KEY " -v > output.jpeg

/* pour tester la logic app, écrire dans le cli : 
florian [ ~ ]$ wget https://a.travel-assets.com/findyours-php/viewfinder/images/res70/106000/106681-Metz.jpg -O test.jpg
florian [ ~ ]$ az storage blob upload   --account-name thaiserverlessnet8467   --container-name thaiflorian-la-container   --file test.jpg   --name test.jpg   --auth-mode key   --account-key YOUR_ACCESS_KEY
*/