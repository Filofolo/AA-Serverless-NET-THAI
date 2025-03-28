namespace sample1;
using Newtonsoft.Json;

class Program
{
    static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            Personne me = new Personne("Florian", 23);
            string json = JsonConvert.SerializeObject(me, Formatting.Indented);
            Console.WriteLine(json);

            ImageProcessor processor = new ImageProcessor();

            // Chemins des fichiers
            string inputImagePath = @"./images/";
            string outputImagePath = @"./images2/";

            // Paramètres de redimensionnement
            int newWidth = 600;
            int newHeight = 200;

            // Liste de paramètres pour les images
            List<Tuple<string, string, int, int>> imageList = new List<Tuple<string, string, int, int>>
            {
                Tuple.Create(inputImagePath + "/imageFrance1.jpg", outputImagePath + "/imageFrance1_redim.jpg", newWidth, newHeight),
                Tuple.Create(inputImagePath + "/imageFrance2.jpg", outputImagePath + "/imageFrance2_redim.jpg", newWidth, newHeight),
                Tuple.Create(inputImagePath + "/imageFrance3.jpg", outputImagePath + "/imageFrance3_redim.jpg", newWidth, newHeight)
            };

            // Version parallèle
            processor.ResizeImagesParallel(imageList);
            Console.WriteLine();

            // Version séquentielle
            processor.ResizeImagesSequential(imageList);
            Console.WriteLine();

        }
}
