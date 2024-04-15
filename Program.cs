//using SixLabors.ImageSharp;
//using SixLabors.ImageSharp.PixelFormats;

//using System;

//class Program
//{
//    static void Main(string[] args)
//    {
//        // Путь к контейнерному изображению и изображению для скрытия
//        string containerImagePath = "container.png";
//        string secretImagePath = "secret_image.png";

//        // Загрузка изображений
//        using (Image<Rgba32> containerImage = Image.Load<Rgba32>(containerImagePath))
//        using (Image<Rgba32> secretImage = Image.Load<Rgba32>(secretImagePath))
//        {
//            // Проверка соответствия размеров изображений
//            if (containerImage.Width < secretImage.Width || containerImage.Height < secretImage.Height)
//            {
//                Console.WriteLine("Размеры контейнерного изображения должны быть больше размеров изображения для скрытия.");
//                return;
//            }

//            // Скрытие изображения
//            EmbedImage(containerImage, secretImage);

//            // Сохранение контейнерного изображения с встроенным изображением
//            containerImage.Save("container_image_with_secret.png");

//            // Извлечение скрытого изображения
//            Image<Rgba32> extractedImage = ExtractImage("container_image_with_secret.png", secretImage.Width, secretImage.Height);

//            // Сохранение извлеченного изображения
//            extractedImage.Save("extracted_secret_image.png");

//            Console.WriteLine("Процесс завершен. Проверьте директорию для результатов.");
//        }
//    }

//    // Метод для скрытия изображения в контейнерном изображении
//    static void EmbedImage(Image<Rgba32> containerImage, Image<Rgba32> secretImage)
//    {
//        int width = Math.Min(containerImage.Width, secretImage.Width);
//        int height = Math.Min(containerImage.Height, secretImage.Height);

//        for (int y = 0; y < height; y++)
//        {
//            for (int x = 0; x < width; x++)
//            {
//                // Получаем пиксели изображений
//                Rgba32 containerPixel = containerImage[x, y];
//                Rgba32 secretPixel = secretImage[x, y];

//                // Меняем младшие биты цветовых компонентов контейнерного пикселя
//                containerPixel.R = (byte)((containerPixel.R & 0xFE) | ((secretPixel.R >> 7) & 0x01));
//                containerPixel.G = (byte)((containerPixel.G & 0xFE) | ((secretPixel.G >> 7) & 0x01));
//                containerPixel.B = (byte)((containerPixel.B & 0xFE) | ((secretPixel.B >> 7) & 0x01));

//                // Обновляем пиксель в контейнерном изображении
//                containerImage[x, y] = containerPixel;
//            }
//        }
//    }

//    // Метод для извлечения скрытого изображения из контейнерного изображения
//    static Image<Rgba32> ExtractImage(string containerImagePath, int width, int height)
//    {
//        // Загружаем контейнерное изображение
//        using (Image<Rgba32> containerImage = Image.Load<Rgba32>(containerImagePath))
//        {
//            // Создаем новое изображение для извлечения скрытого изображения
//            Image<Rgba32> extractedImage = new Image<Rgba32>(width, height);

//            // Проходимся по каждому пикселю извлекаемого изображения
//            for (int y = 0; y < height; y++)
//            {
//                for (int x = 0; x < width; x++)
//                {
//                    Rgba32 containerPixel = containerImage[x, y];

//                    // Создаем пустой пиксель для извлеченного изображения
//                    Rgba32 extractedPixel = new Rgba32();

//                    // Извлекаем младшие биты каждой компоненты пикселя контейнерного изображения и восстанавливаем их
//                    byte extractedR = (byte)((containerPixel.R & 0x01) * 255);
//                    byte extractedG = (byte)((containerPixel.G & 0x01) * 255);
//                    byte extractedB = (byte)((containerPixel.B & 0x01) * 255);

//                    // Устанавливаем пиксель в извлеченном изображении
//                    extractedImage[x, y] = new Rgba32(extractedR, extractedG, extractedB);
//                }
//            }

//            return extractedImage;
//        }
//    }
//}



using System;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

class Program
{
    static void Main(string[] args)
    {
        string containerImagePath = "container1.png";
        string secretImagePath = "secret_image.png";
        string outputImagePath = "output.png";

        HideImage(containerImagePath, secretImagePath, outputImagePath);
        Console.WriteLine("Изображение успешно спрятано.");

        string extractedImagePath = "extracted.png";
        ExtractImage(outputImagePath, extractedImagePath, Image.Load<Rgba32>(secretImagePath).Width, Image.Load<Rgba32>(secretImagePath).Height);
        Console.WriteLine("Изображение успешно извлечено.");
    }

    static void HideImage(string containerImagePath, string secretImagePath, string outputImagePath)
    {
        using (Image<Rgba32> containerImage = Image.Load<Rgba32>(containerImagePath))
        using (Image<Rgba32> secretImage = Image.Load<Rgba32>(secretImagePath))
        {
            if (containerImage.Width < secretImage.Width || containerImage.Height < secretImage.Height)
            {
                Console.WriteLine("Размеры контейнерного изображения должны быть больше размеров изображения для скрытия.");
                return;
            }

            for (int y = 0; y < secretImage.Height; y++)
            {
                for (int x = 0; x < secretImage.Width; x++)
                {
                    Rgba32 containerColor = containerImage[x, y];
                    Rgba32 secretColor = secretImage[x, y];

                    byte newR = (byte)((containerColor.R & 0xF8) | ((secretColor.R >> 5) & 0x07));
                    byte newG = (byte)((containerColor.G & 0xF8) | ((secretColor.G >> 5) & 0x07));
                    byte newB = (byte)((containerColor.B & 0xF8) | ((secretColor.B >> 5) & 0x07));

                    containerImage[x, y] = new Rgba32(newR, newG, newB);
                }
            }

            containerImage.Save(outputImagePath);
        }
    }

    static void ExtractImage(string containerImagePath, string outputImagePath, int width, int height)
    {
        using (Image<Rgba32> containerImage = Image.Load<Rgba32>(containerImagePath))
        {
            using (Image<Rgba32> extractedImage = new Image<Rgba32>(width, height))
            {
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        Rgba32 containerColor = containerImage[x, y];

                        byte extractedR = (byte)((containerColor.R & 0x07) << 5);
                        byte extractedG = (byte)((containerColor.G & 0x07) << 5);
                        byte extractedB = (byte)((containerColor.B & 0x07) << 5);

                        extractedImage[x, y] = new Rgba32(extractedR, extractedG, extractedB);
                    }
                }

                extractedImage.Save(outputImagePath);
            }
        }
    }
}
