// Copyright (c) Martin Costello, 2018. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace TodoApp.Extensions
{
    /// <summary>
    /// A class containing extension methods for the <see cref="CultureInfo"/> class.  This class cannot be inherited.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class CultureInfoExtensions
    {
        /// <summary>
        /// Returns the Unicode flag emoji for the culture.
        /// </summary>
        /// <param name="culture">The <see cref="CultureInfo"/> to get the flag emoji for.</param>
        /// <returns>
        /// A <see cref="string"/> containing the Unicode flag emoji for the culture.
        /// </returns>
        /// <remarks>
        /// See https://alanedwardes.com/blog/posts/country-code-to-flag-emoji-csharp/
        /// </remarks>
        public static string FlagEmoji(this CultureInfo culture)
        {
            string[] split = culture.Name.Split("-");

            if (split.Length == 2)
            {
                string region = split[1].ToUpperInvariant();
                return string.Join(string.Empty, region.Select((p) => char.ConvertFromUtf32(p + 0x1f1a5)));
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
