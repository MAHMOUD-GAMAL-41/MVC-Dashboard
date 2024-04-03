namespace Demo.PL.Helper
{
    public static class DocumentSettings
    {
        public static string UploadFile(IFormFile file, string folderName)
        {
            // 1 . 
            var folderPath= Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files", folderName);

            // 2 .
            var fileName = $"{Guid.NewGuid}-{Path.GetFileName(file.FileName)}";

            // 3 .
            var filePath = Path.Combine(folderPath, fileName);

            // 4 .
            using FileStream fileStream = new FileStream(filePath,FileMode.Create);

            file.CopyTo(fileStream);

            return fileName;


        }
        public static void DeleteFile(string folderName, string fileName)
        {
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files", folderName);
            if (File.Exists(folderPath))
            {

                var filePath = Path.Combine(folderPath, fileName);
                if (File.Exists(filePath)) {
                    File.Delete(filePath);
            }
            }
           

        }
    }
}
