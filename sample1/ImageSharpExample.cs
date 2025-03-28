using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics;

namespace sample1
{
    public class ImageProcessor
    {
        public void ResizeImagesParallel(List<Tuple<string, string, int, int>> imageParams)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            // for(int i=0; i< 200; i++)
            // {
            Parallel.ForEach(imageParams, param =>
                {
                    string inputPath = param.Item1;
                    string outputPath = param.Item2;
                    int width = param.Item3;
                    int height = param.Item4;

                    using (var image = Image.Load(inputPath))
                    {
                        image.Mutate(x => x.Resize(width, height));
                        image.Save(outputPath);
                    }
                });
            // }

            stopwatch.Stop();
            Console.WriteLine($"Temps d'exécution en parallèle : {stopwatch.ElapsedMilliseconds} ms");
        }

        public void ResizeImagesSequential(List<Tuple<string, string, int, int>> imageParams)
        {
            
            Stopwatch stopwatch = Stopwatch.StartNew();

            // for (int i = 0; i < 200; i++)
            // {
                foreach (var param in imageParams)
                {
                    string inputPath = param.Item1;
                    string outputPath = param.Item2;
                    int width = param.Item3;
                    int height = param.Item4;

                    using (var image = Image.Load(inputPath))
                    {
                        image.Mutate(x => x.Resize(width, height));
                        image.Save(outputPath);
                    }
                }
            // }

            stopwatch.Stop();
            Console.WriteLine($"Temps d'exécution en séquentiel : {stopwatch.ElapsedMilliseconds} ms");
        }
    }
}