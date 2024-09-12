using Microsoft.AspNetCore.Http;

namespace Application.Helper
{
    public class SystemHelper
    {

        //public static byte[] ImageToBytes(Image imageIn)
        //{
        //    using (var stream = new MemoryStream())
        //    {
        //        imageIn.Save(stream, ImageFormat.Png);
        //        return stream.ToArray();
        //    }
        //}

        public static string GetIpAddress()
        {
            IHttpContextAccessor httpContextAccessor = new HttpContextAccessor();
            return httpContextAccessor.HttpContext.Connection?.LocalIpAddress?.ToString() ?? string.Empty;
        }

        public static string GenerateSenderBatchId()
        {
            Random random = new Random();
            /**
             * Generates a unique sender_batch_id based on the current date and a sequential number.
             * 
             * Returns:
             *     A unique sender_batch_id string.
             */
            DateTime today = DateTimeHelper.GetDateTimeNow();
            string dateStr = today.ToString("yyyyMMdd");

            // Retrieve the last used number from a database or a file
            int randomNumber = random.Next(100000, 999999);

            string senderBatchId = $"{dateStr}_{randomNumber.ToString("D5")}";

            return senderBatchId;
        }

        public static int CalculateSimilarity(string tagName, string searchTerm)
        {
            // Convert to lowercase for case-insensitive comparison
            tagName = tagName.ToLower();
            searchTerm = searchTerm.ToLower();

            // If tagName contains searchTerm, return a negative score for priority
            int index = tagName.IndexOf(searchTerm);
            if (index != -1)
            {

                return index;
            }

            // If not contained, use the Levenshtein algorithm to calculate a similarity score
            int[,] dp = new int[tagName.Length + 1, searchTerm.Length + 1];

            for (int i = 0; i <= tagName.Length; i++)
            {
                for (int j = 0; j <= searchTerm.Length; j++)
                {
                    if (i == 0)
                    {
                        dp[i, j] = j;
                    }
                    else if (j == 0)
                    {
                        dp[i, j] = i;
                    }
                    else
                    {
                        dp[i, j] = Math.Min(Math.Min(
                            dp[i - 1, j] + 1,
                            dp[i, j - 1] + 1),
                            dp[i - 1, j - 1] + (tagName[i - 1] == searchTerm[j - 1] ? 0 : 1));
                    }
                }
            }

            return dp[tagName.Length, searchTerm.Length] + 1000; // Add 1000 to give tags that do not contain searchTerm a higher score
        }
    }
}
