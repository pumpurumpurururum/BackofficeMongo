using System;
using System.Text;

namespace BaseCms.Helpers
{
    /// <summary>
    /// Класс для перевода русскоязычного текста в транслитерат («Строка текста» -> «stroka_teksta»).
    /// </summary>
    public static class Transliteration
    {
        #region Transliteration tables
        /// <summary>
        /// Русскоязычные символы
        /// </summary>
        private static readonly Char[] ChOrigTable = new[] {
            'а', 'б', 'в', 'г', 'д', 'е', 'ё', 'ж', 'з', 'и',
            'й', 'к', 'л', 'м', 'н', 'о', 'п', 'р', 'с', 'т',
            'у', 'ф', 'х', 'ц', 'ч', 'ш', 'щ', 'ъ', 'ы', 'ь',
            'э', 'ю', 'я', ' '
        };

        /// <summary>
        /// Англоязычные аналоги русскоязычных символов
        /// </summary>
        private static readonly Char[] ChSafeTable = new[] {
            'a', 'b', 'v', 'g', 'd', 'e', 'e', 'j', 'z', 'i',
            'i', 'k', 'l', 'm', 'n', 'o', 'p', 'r', 's', 't',
            'y', 'f', 'h', 'c', 'c', 's', 's', '_', 'i', '_',
            'e', 'u', 'a', '-'
        };
        #endregion

        /// <summary>
        /// Преобразование русскоязычного текста в транслитерат.
        /// <example>
        /// <code>
        /// Console.WriteLine(Transliteration.Transform("Строка текста"));
        /// </code>
        /// >> stroka_teksta
        /// </example>
        /// </summary>
        /// <param name="value">Русскоязычный текст</param>
        /// <returns>Транслитерат</returns>
        public static string Transform(string value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            var sb = new StringBuilder(value.ToLowerInvariant());
            for (int i = 0, end = sb.Length; i < end; i++)
            {
                if (char.IsDigit(sb[i]))
                    continue;
                if (char.IsLetter(sb[i]))
                {
                    for (int i2 = 0, end2 = (Math.Min(ChOrigTable.Length, ChSafeTable.Length)); i2 < end2; i2++)
                    {
                        if (ChOrigTable[i2].Equals(sb[i]))
                            sb[i] = ChSafeTable[i2];
                    }
                }
                else
                    if (!(sb[i] == '.' || sb[i] == '-'))
                    sb[i] = '-';
            }

            return sb.ToString();
        }
    }
}
