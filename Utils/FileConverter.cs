namespace OnlineShop.Utils
{
    using System.IO;
    using System.Web;

    public static class FileConverter
    {
        public  static byte[] ConvertFileToByteArray(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                MemoryStream target = new MemoryStream();
                file.InputStream.CopyTo(target);
                byte[] data = target.ToArray();
                return data;
            }

            return null;
        }
    }
}