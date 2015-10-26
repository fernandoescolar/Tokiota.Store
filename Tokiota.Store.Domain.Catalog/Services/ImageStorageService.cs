namespace Tokiota.Store.Domain.Catalog.Services
{
    using System.IO;
    using System.Web;

    internal class ImageStorageService : IImageStorageService
    {
        public string SaveImage(string fileName, Stream stream)
        {
            var imageUrl = Path.Combine("~/images", Path.GetFileName(fileName));
            var destination = HttpContext.Current.Server.MapPath(imageUrl);

            if (File.Exists(destination)) File.Delete(destination);

            using (var reader = new BinaryReader(stream))
            {
                using (var writer = new BinaryWriter(new FileStream(destination, FileMode.CreateNew)))
                {
                    var buffer = new byte[1024 * 4];
                    var read = 0;
                    while ((read = reader.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        writer.Write(buffer, 0, read);
                    }
                }
            }

            return imageUrl;
        }

        public void DeleteImage(string imageUrl)
        {
            var destination = HttpContext.Current.Server.MapPath(imageUrl);
            if (File.Exists(destination)) File.Delete(destination);
        }
    }
}
