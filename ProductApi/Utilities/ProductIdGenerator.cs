using System.Security.Cryptography;

namespace ProductApi.Utilities
{
    public class ProductIdGenerator
    {
        private static readonly RandomNumberGenerator rng = RandomNumberGenerator.Create();

        public static string GenerateUniqueId()
        {
            byte[] bytes = new byte[4]; // 32-bit
            int id;

            do
            {
                rng.GetBytes(bytes);
                id = BitConverter.ToInt32(bytes, 0);
                id = Math.Abs(id % 1_000_000); // Keep it 6 digits
            }
            while (id < 100_000); // Ensure it's not less than 6 digits

            return id.ToString("D6"); // Pad with 0s if needed
        }
    }
}
