using Libwebp.Net;
using Libwebp.Net.utility;
using Libwebp.Standard;
using Microsoft.AspNetCore.Http;
using SelectPdf;
using SkiaSharp;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Utility.Helpers
{
    public class MediaHelper
    {
       
        //public static async Task ConvertImageToWebpNew(string filePath, string fileName, string fileExt)
        //{
        //    //if (file == null)
        //    //   throw new FileNotFoundException();

        //    //you can handle file checks ie. extensions, file size etc..

        //    //creating output file name
        //    // your name can be a unique Guid or any name of your choice with .webp extension..eg output.webp
        //    //in my case i am removing the extension from the uploaded file and appending
        //    // the .webp extension
        //    var path = Path.Combine(filePath, fileName);
        //      // get file to encode
             
        //    var oFileName = $"{Path.GetFileNameWithoutExtension("test")}.webp";

        //    // create your webp configuration
        //    var config = new WebpConfigurationBuilder()
        //       .Preset(Preset.PHOTO)
        //       .Output(oFileName)
        //       .Build();

        //    //create an encoder and pass in your configuration
        //    var encoder = new WebpEncoder(config);

        //    //copy file to memory stream
        //    var ms = new MemoryStream();
        //    file.CopyTo(ms);

        //    //call the encoder and pass in the Memorystream and input FileName
        //    //the encoder after encoding will return a FileStream output

        //    //Optional cast to Stream to return file for download
        //    Stream fs = await encoder.EncodeAsync(ms, fileName);//File.FileName);

        //    /*Do whatever you want with the file....download, copy to disk or 
        //      save to cloud*/
        //     // File.Create(fs, "application/octet-stream", oFileName);
        //   // return   File(fs, "application/octet-stream", oFileName);

        //    //string path = Path.Combine(filePath, fileName);
        //    using (FileStream outputFileStream = new FileStream(oFileName, FileMode.Create))
        //    {
        //        fs.CopyTo(outputFileStream);
        //    }
        //    using (var fileStream = File.Create(oFileName))
        //    {
        //        myOtherObject.InputStream.Seek(0, SeekOrigin.Begin);
        //        myOtherObject.InputStream.CopyTo(fileStream);
        //    }
        //}

     

        public static void SaveStreamAsFile(string filePath, Stream inputStream, string fileName)
        {
            DirectoryInfo info = new DirectoryInfo(filePath);
            if (!info.Exists)
            {
                info.Create();
            }

            string path = Path.Combine(filePath, fileName);
            using (FileStream outputFileStream = new FileStream(path, FileMode.Create))
            {
                inputStream.CopyTo(outputFileStream);
            }

            //using (FileStream fs = File.Create(actualPath))
            //{
            //    file.CopyTo(fs);
            //}

        }
        //public static void ConvertImageToWebP(string filePath,string fileName,string extension)
        //{
        //    //AppDomain.CurrentDomain.BaseDirectory + "SAMPLE.png"
        //    var path = Path.Combine(filePath,fileName);
        //    Bitmap bmp = new Bitmap(path);
        //    //Imazen.WebP.SimpleEncoder encoder = new Imazen.WebP.SimpleEncoder();
        //    //using (var outStream = new System.IO.MemoryStream())
        //    //{
        //    //    encoder.Encode(bmp, outStream, -1);
        //    //    outStream.Close();

        //    //    var webpBytes = outStream.ToArray();
        //    //    File.WriteAllBytes(AppDomain.CurrentDomain.BaseDirectory + "SAMPLE.webp", webpBytes);
        //    //}
        //    byte[] rawWebP;
        //    // get the picture box image AppDomain.CurrentDomain.BaseDirectory

        //    //Test simple encode in lossly mode in memory with quality 75
        //    string lossyFileName = Path.Combine(filePath, fileName + ".webp");
        //    using (WebP webp = new())
        //        rawWebP = webp.EncodeLossy(bmp, 75);
        //    File.WriteAllBytes(lossyFileName, rawWebP);
        //    var msg = "Made " + lossyFileName + "Simple lossy";

        //}

        public static string SaveDocToFile(IFormFile file, string shortPath, string folder)
        {
            string filename = string.Empty;
            string[] metaData = file.FileName.Split(".");
            if (metaData.Length > 1)
            {
                filename = Guid.NewGuid() + "." + metaData[metaData.Length - 1];
                var directory = Directory.GetCurrentDirectory();
                var actualPath = directory + "/" + folder;
                var filePath = actualPath + "/" + filename;

                //new feature for direcotry creation, if not exists
                if (!new FileInfo(actualPath).Exists)
                {
                    new DirectoryInfo(actualPath).Create();
                }

                using (FileStream fs = File.Create(filePath))
                {
                    file.CopyTo(fs);
                }

            }

            return filename;
        }


        public static string SaveSVGToFile(IFormFile file, string shortPath)
        {
            string filename = string.Empty;
            string[] metaData = file.FileName.Split(".");
            if (metaData.Length > 1)
            {
                filename = Guid.NewGuid() + "." + metaData[metaData.Length - 1];
                var directory = Directory.GetCurrentDirectory();
                var actualPath = directory + "/" + shortPath;
                var filePath = actualPath + "/" + filename;

                //new feature for direcotry creation, if not exists
                if (!new FileInfo(actualPath).Exists)
                {
                    new DirectoryInfo(actualPath).Create();
                }

                using (FileStream fs = File.Create(filePath))
                {
                    file.CopyTo(fs);
                }

            }

            return filename;
        }

        public static void SaveImage(ref dynamic model, string path, string resolution)
        {
            if (model.Image != null && model.Image.Length > 0)
            {
                var fileName = SaveImageToFile(model.Image, "/" + path, resolution);
                if (!string.IsNullOrEmpty(fileName))
                    model.ImageName = fileName;
            }
        }

        public static string SaveVideo(string oldFileName, IFormFile file, string shortPath)
        {
            if (file == null)
                throw new FileNotFoundException();
             
            var FileExtension = new FileInfo(file.FileName).Extension;
            //var oFileName = $"{Path.GetFileNameWithoutExtension(file.FileName)}.webp";
              
            //Remove old file if exists
            if (!string.IsNullOrEmpty(oldFileName))
            {
                var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), shortPath.Replace("/", "\\"), oldFileName);
                if (File.Exists(oldFileName))
                {
                    File.Delete(oldFilePath);
                }
            }
              
            string directory = Directory.GetCurrentDirectory();
            var newFileName = Guid.NewGuid() + FileExtension;
            var filePath = Path.Combine(directory, shortPath.Replace("/", "\\"), newFileName);

            string path = Path.Combine(Directory.GetCurrentDirectory(), shortPath.Replace("/", "\\"));
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            ////new feature for direcotry creation, if not exists
            //if (!new FileInfo(directory + shortPath).Exists)
            //{
            //    new DirectoryInfo(directory + shortPath).Create();
            //}
            using (FileStream fs = File.Create(filePath))
            {
                file.CopyTo(fs);
            }
              
            return newFileName;
        }

        public static  string  ConvertImageToWebp(string oldFileName, IFormFile file, string shortPath)
        {
            if (file == null)
                throw new FileNotFoundException();

            //you can handle file checks ie. extensions, file size etc..

            //creating output file name
            // your name can be a unique Guid or any name of your choice with .webp extension..eg output.webp
            //in my case i am removing the extension from the uploaded file and appending
            // the .webp extension
            var oFileName = $"{Path.GetFileNameWithoutExtension(file.FileName)}.webp";

            // create your webp configuration
            var config = new WebpConfigurationBuilder()
               .Preset(Preset.PHOTO)
               .Output(oFileName)
               .Build();

            //create an encoder and pass in your configuration
            var encoder = new WebpEncoder(config);

             //Remove old file if exists
            if (!string.IsNullOrEmpty(oldFileName))
            {
                var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), shortPath.Replace("/", "\\"), oldFileName);
                if (File.Exists(oldFileName))
                {
                    File.Delete(oldFilePath);
                }
            }


            string path = Path.Combine(Directory.GetCurrentDirectory(), shortPath.Replace("/", "\\"));
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            //Create new file
            var newFileName = Guid.NewGuid() + ".webp"; 
            var filePath = Path.Combine(Directory.GetCurrentDirectory() , shortPath.Replace("/","\\"), newFileName);
            

            //copy file to memory stream
            var ms = new MemoryStream();
            file.CopyTo(ms);

            //call the encoder and pass in the Memorystream and input FileName
            //the encoder after encoding will return a FileStream output

            //Optional cast to Stream to return file for download
            Stream fs =   encoder.EncodeAsync(ms, file.FileName).GetAwaiter().GetResult();

            //write to new file
            using (FileStream fs1 = File.Create(filePath))
            {
                fs.CopyTo(fs1);
            }

            /*Do whatever you want with the file....download, copy to disk or 
              save to cloud*/

            //return File(fs, "application/octet-stream", oFileName);

            return newFileName;
        }

        public static string SaveImageToFile(string oldFileName, IFormFile file, string shortPath, string resolution = "")
        {
           
            string filename = string.Empty;
            string[] metaData =  file.FileName.Split(".");
            if (metaData.Length > 1)
            {
                filename = Guid.NewGuid() + "." + metaData[metaData.Length - 1];
                var directory = Directory.GetCurrentDirectory();
                var newFilePath = directory + shortPath + filename;
                var oldFilePath = directory + shortPath + oldFileName;
                //remove old file if exits
                if (oldFileName != null)
                {
                    var path = directory + shortPath.Replace("/", "\\");
                    var oldResizedPath = path + @"ResizedImage\" ;
                    if (new FileInfo(path + oldFileName).Exists)
                    { File.Delete(Path.Combine(path, oldFileName)); 
                    }
                    if (new FileInfo(oldResizedPath + oldFileName).Exists)
                    {
                        File.Delete(Path.Combine(oldResizedPath, oldFileName));
                    }
                }

                //new feature for direcotry creation, if not exists
                if (!new FileInfo(directory + shortPath).Exists)
                {
                    new DirectoryInfo(directory + shortPath).Create();
                }
                using (FileStream fs = File.Create(newFilePath))
                {
                    file.CopyTo(fs);
                }

                int width = 0;
                int height = 0;
                if (!string.IsNullOrEmpty(resolution))
                {
                    var arrResolution = resolution.Split("*");
                    if (arrResolution.Length == 2)
                    {
                        int.TryParse(arrResolution[0], out width);
                        int.TryParse(arrResolution[1], out height);
                    }
                }

                var resizedPath = directory + shortPath + @"\ResizedImage\" + filename;
                if (!new FileInfo(directory + shortPath + @"\ResizedImage\").Exists)
                {
                    new DirectoryInfo(directory + shortPath + @"\ResizedImage\").Create();
                }
                if (width > 0 && height > 0)
                {
                    using var image = SKBitmap.Decode(newFilePath);
                    var codec = SKCodec.Create(newFilePath);
                    var format = codec.EncodedFormat;
                    var pictureBinary = ImageResize(image, format, width, height);
                    File.WriteAllBytesAsync(resizedPath, pictureBinary);
                }
                else
                {
                    using (FileStream fs = System.IO.File.Create(resizedPath))
                    {
                        file.CopyTo(fs);
                    }
                }
            }

            return filename;
        }

        public static string SaveImageToFile(IFormFile file, string shortPath, string resolution = "")
        {
            string filename = string.Empty;
            string[] metaData = file.FileName.Split(".");
            if (metaData.Length > 1)
            {
                filename = Guid.NewGuid() + "." + metaData[metaData.Length - 1];
                var directory = Directory.GetCurrentDirectory();
                var actualPath = directory + shortPath + filename;

                //new feature for direcotry creation, if not exists
                if (!new FileInfo(directory + shortPath).Exists)
                {
                    new DirectoryInfo(directory + shortPath).Create();
                }
                using (FileStream fs = File.Create(actualPath))
                {
                    file.CopyTo(fs);
                }

                int width = 0;
                int height = 0;
                if (!string.IsNullOrEmpty(resolution))
                {
                    var arrResolution = resolution.Split("*");
                    if (arrResolution.Length == 2)
                    {
                        int.TryParse(arrResolution[0], out width);
                        int.TryParse(arrResolution[1], out height);
                    }
                }

                var resizedPath = directory + shortPath + @"\ResizedImage\" + filename;
                if (!new FileInfo(directory + shortPath + @"\ResizedImage\").Exists)
                {
                    new DirectoryInfo(directory + shortPath + @"\ResizedImage\").Create();
                }
                if (width > 0 && height > 0)
                {
                    using var image = SKBitmap.Decode(actualPath);
                    var codec = SKCodec.Create(actualPath);
                    var format = codec.EncodedFormat;
                    var pictureBinary = ImageResize(image, format, width, height);
                    File.WriteAllBytesAsync(resizedPath, pictureBinary);
                }
                else
                {
                    using (FileStream fs = System.IO.File.Create(resizedPath))
                    {
                        file.CopyTo(fs);
                    }
                }
            }

            return filename;
        }
        protected static byte[] ImageResize(SKBitmap image, SKEncodedImageFormat format, int targetSize)
        {
            if (image == null)
                throw new ArgumentNullException("Image is null");

            float width, height;
            if (image.Height > image.Width)
            {
                // portrait
                width = image.Width * (targetSize / (float)image.Height);
                height = targetSize;
            }
            else
            {
                // landscape or square
                width = targetSize;
                height = image.Height * (targetSize / (float)image.Width);
            }

            if ((int)width == 0 || (int)height == 0)
            {
                width = image.Width;
                height = image.Height;
            }
            try
            {
                using var resizedBitmap = image.Resize(new SKImageInfo((int)width, (int)height), SKFilterQuality.Medium);
                using var cropImage = SKImage.FromBitmap(resizedBitmap);

                //In order to exclude saving pictures in low quality at the time of installation, we will set the value of this parameter to 80 (as by default)
                return cropImage.Encode(format, 80).ToArray();
            }
            catch
            {
                return image.Bytes;
            }
        }
        protected static byte[] ImageResize(SKBitmap image, SKEncodedImageFormat format, int width, int height)
        {
            if (image == null)
                throw new ArgumentNullException("Image is null");

            if ((int)width == 0 || (int)height == 0)
            {
                width = image.Width;
                height = image.Height;
            }
            try
            {
                using var resizedBitmap = image.Resize(new SKImageInfo((int)width, (int)height), SKFilterQuality.Medium);
                using var cropImage = SKImage.FromBitmap(resizedBitmap);

                //In order to exclude saving pictures in low quality at the time of installation, we will set the value of this parameter to 80 (as by default)
                return cropImage.Encode(format, 80).ToArray();
            }
            catch
            {
                return image.Bytes;
            }
        }
        public static async Task<String> ImageToBase64(string filepath)
        {
            var directory = Directory.GetCurrentDirectory();
            var serverpath = Path.Combine(directory, filepath);
            if (System.IO.File.Exists(serverpath))
            {
                var contents = await System.IO.File.ReadAllBytesAsync(serverpath);
                return $"data:image/png;base64,{Convert.ToBase64String(contents)}";
            }
            else
            {
                return "image file not found";
            }
        }

        public static string HtmlToImage(string htmlContent, string filepath)
        {
            string imageName = string.Empty;
            try
            {
                int webPageWidth = 1024;
                int webPageHeight = 0;

                // instantiate a html to image converter object
                HtmlToImage imgConverter = new HtmlToImage();

                // set converter options
                imgConverter.WebPageWidth = webPageWidth;
                imgConverter.WebPageHeight = webPageHeight;

                // create a new image converting an url
                System.Drawing.Image image = imgConverter.ConvertHtmlString(htmlContent);

                // save image
                imageName = Guid.NewGuid() + "." + ImageFormat.Png.ToString();
                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), filepath);
                imagePath = imagePath + "/" + imageName;
                image.Save(imagePath);

                var imageResizePath = Path.Combine(Directory.GetCurrentDirectory(), filepath + @"\ResizedImage\");
                imageResizePath = imageResizePath + "/" + imageName;
                image.Save(imageResizePath);
            }
            catch { }

            return imageName;
        }
        public static string HtmlToPdf(string htmlContent, string filepath)
        {
            var url = string.Empty;
            try
            {
                // instantiate the html to pdf converter
                HtmlToPdf converter = new HtmlToPdf();

                //converter.Options.WebPageHeight = 1050;
                converter.Options.WebPageWidth = 1200;

                PdfDocument doc = converter.ConvertHtmlString(htmlContent);

                // save pdf document
                var pdfFileName = Guid.NewGuid() + ".pdf";
                var pdfPath = Path.Combine(Directory.GetCurrentDirectory(), filepath);
                pdfPath = pdfPath + "/" + pdfFileName;
                doc.Save(pdfPath);

                // close pdf document
                doc.Close();

                url = filepath + "/" + pdfFileName;
            }
            catch { }

            return url;
        }
        public static string HtmlToPdfFront(string htmlContent, string filepath, string header)
        {
            var url = string.Empty;
            try
            {
                // instantiate the html to pdf converter
                HtmlToPdf converter = new HtmlToPdf();

                //converter.Options.WebPageHeight = 1050;
                converter.Options.WebPageWidth = 1200;
                converter.Options.MarginTop = 20;
                converter.Options.MarginBottom = 10;

                if (!string.IsNullOrEmpty(header))
                {
                    // header settings
                    converter.Options.DisplayHeader = true;
                    converter.Header.DisplayOnFirstPage = true;
                    converter.Header.DisplayOnOddPages = true;
                    converter.Header.DisplayOnEvenPages = true;
                    converter.Header.Height = 60;

                    // add some html content to the header
                    PdfHtmlSection headerHtml = new PdfHtmlSection(header, "");
                    headerHtml.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
                    converter.Header.Add(headerHtml);
                }

                PdfDocument doc = converter.ConvertHtmlString(htmlContent);

                // save pdf document
                var pdfFileName = Guid.NewGuid() + ".pdf";
                var pdfPath = Path.Combine(Directory.GetCurrentDirectory(), filepath);
                pdfPath = pdfPath + "/" + pdfFileName;
                doc.Save(pdfPath);

                // close pdf document
                doc.Close();

                url = filepath + "/" + pdfFileName;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return url;
        }
        public static string StringToHtml(string htmlContent, string filepath)
        {
            var url = string.Empty;
            try
            {
                // save pdf document
                var pdfFileName = Guid.NewGuid() + ".html";

                var pdfPath = Path.Combine(Directory.GetCurrentDirectory(), filepath);
                pdfPath = pdfPath + "/" + pdfFileName;

                File.WriteAllText(pdfPath, htmlContent);

                url = filepath + "/" + pdfFileName;
            }
            catch { }

            return url;
        }
    }
}
