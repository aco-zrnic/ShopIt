namespace Server.Util
{
    public class ImageCheck
    {
        public static string IsImage(byte[] data)
        {
            // Check for a valid image file header (e.g., PNG, JPEG)
            if (data.Length >= 2 && data[0] == 0xFF && data[1] == 0xD8)
                return ".jpeg";

            // Compare the first 8 bytes of the data with the PNG signature
            byte[] pngSignature = { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A };
            var isPng = true;

            for (int i = 0; i < pngSignature.Length; i++)
            {
                if (data[i] != pngSignature[i])
                    isPng = false;
            }

            if (isPng)
                return ".png";

            return string.Empty;
        }
    }
}
