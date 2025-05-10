using System;

namespace Shared.Utils
{
    public static class HexUtils
    {
        /// <summary>
        /// Converts a hexadecimal string to a byte array.
        /// </summary>
        /// <param name="hex">The hex string (must have even length).</param>
        /// <returns>Byte array representation of the hex string.</returns>
        /// <exception cref="ArgumentException">Thrown when the input is null or empty.</exception>
        /// <exception cref="FormatException">Thrown when the input is not a valid hex string.</exception>
        public static byte[] StringToByteArray(string hex)
        {
            if (string.IsNullOrWhiteSpace(hex))
                throw new ArgumentException("Hex string is null or empty.", nameof(hex));

            if (hex.Length % 2 != 0)
                throw new FormatException("Hex string must have an even length.");

            var bytes = new byte[hex.Length / 2];
            for (int i = 0; i < hex.Length; i += 2)
            {
                if (!IsHexChar(hex[i]) || !IsHexChar(hex[i + 1]))
                    throw new FormatException($"Invalid hex character: {hex.Substring(i, 2)}");

                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }
            return bytes;
        }

        /// <summary>
        /// Checks if a character is a valid hex digit.
        /// </summary>
        /// <param name="c">The character to check.</param>
        /// <returns>True if the character is a hex digit; otherwise false.</returns>
        private static bool IsHexChar(char c)
        {
            return (c >= '0' && c <= '9') ||
                   (c >= 'a' && c <= 'f') ||
                   (c >= 'A' && c <= 'F');
        }
    }
}
