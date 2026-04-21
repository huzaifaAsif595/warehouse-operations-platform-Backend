using Microsoft.Extensions.Caching.Memory;
using PeakLogix.PickProApi.Common.Constants.LLFiles;

namespace PeakLogix.PickProApi.Common.Helpers
{
    public static class LLHelper
    {
        // Fully qualify to avoid conflicts with any similarly named type in your solution.
        public static readonly IMemoryCache _cache =
            new MemoryCache(new MemoryCacheOptions());
        public static bool EndsWith(string? value, string suffix)
    => value?.EndsWith(suffix, StringComparison.OrdinalIgnoreCase) == true;
        public static bool ContainsCI(string? source, string token) =>
            !string.IsNullOrEmpty(source) &&
            source.IndexOf(token, StringComparison.OrdinalIgnoreCase) >= 0;

        public static string ToFileSetName(string? value) =>
            (value ?? string.Empty).Replace(LLConstants.HYPHEN_SEPARATOR, LLConstants.DOT_SEPARATOR); // "-" -> "."

        // Safe case-insensitive ends-with
        public static bool EndsWithCI(string? value, string suffix) =>
            value?.EndsWith(suffix, StringComparison.OrdinalIgnoreCase) == true;

        // Safe case-insensitive equals
        public static bool EqualsCI(string? a, string? b) =>
            string.Equals(a, b, StringComparison.OrdinalIgnoreCase);

        // Split "name.ext" -> "name"
        public static string SplitBase(string? name)
        {
            if (string.IsNullOrEmpty(name)) return string.Empty;
            var idx = name.IndexOf(LLConstants.DOT_SEPARATOR, StringComparison.Ordinal);
            return idx > 0 ? name.Substring(0, idx) : name;
        }
        public static string SplitBase(string? value, string dot)
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;
            var idx = value.IndexOf(dot, StringComparison.Ordinal);
            return idx > 0 ? value.Substring(0, idx) : value;
        }

        // Replace multiple pairs in sequence
        public static string ReplaceMany(string input, params (string oldVal, string newVal)[] pairs)
        {
            if (string.IsNullOrEmpty(input) || pairs == null || pairs.Length == 0) return input ?? string.Empty;
            var result = input;
            foreach (var (oldVal, newVal) in pairs)
            {
                result = result.Replace(oldVal, newVal);
            }
            return result;
        }

        // Normalize “-lst”->“.lst”, “-lbl”->“.lbl”, remove spaces (keeps your original semantics)
        public static string NormalizeDesignNameExact(string fileSetName) =>
            ReplaceMany(fileSetName ?? string.Empty, (LLConstants.TOKEN_LST, LLConstants.DOT_SEPARATOR + LLConstants.EXT_LST), (LLConstants.TOKEN_LBL, LLConstants.DOT_SEPARATOR + LLConstants.EXT_LBL), (LLConstants.SPACE_CHAR, string.Empty));

        // Is flat export (csv/txt)
        public static bool IsFlatExport(string type) =>
            EqualsCI(type, LLConstants.EXT_CSV) || EqualsCI(type, LLConstants.EXT_TXT);

    }
}
