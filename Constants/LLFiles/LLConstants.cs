
namespace PeakLogix.PickProApi.Common.Constants.LLFiles;
public static class LLConstants
{
    // File Extensions
    public const string EXT_TXT = "txt";
    public const string EXT_CSV = "csv";
    public const string EXT_PDF = "pdf";
    public const string EXT_XLSX = "xlsx";
    public const string EXT_HTML = "html";
    public const string EXT_JSON = "json";
    public const string EXT_PNG = "png";
    public const string EXT_LST = "lst";
    public const string EXT_LBL = "lbl";
    public const string DOT_EXT_LST = ".lst";
    public const string DOT_EXT_LBL = ".lbl";

    // Report Tokens
    public const string TOKEN_LST = "-lst";
    public const string TOKEN_PRV = "-prv";
    public const string TOKEN_LBL = "-lbl";
    public const string KEYWORD_FILENAME = "filename";
    public const string KEYWORD_FILENAME_PREFIX = "filename:"; // used in print path

    // Symbols
    public const string DOT_SEPARATOR = ".";
    public const string HYPHEN_SEPARATOR = "-";
    public const string SPACE_CHAR = " ";
    public const string BRACKET_OPEN = "[";
    public const string BRACKET_CLOSE = "]";
    public const string LITERAL_NULL = "null";
    public const string AT_PREFIX = "@";

    // SQL Fragments
    public const string SQL_SELECT_ONE = "SELECT 1";
    public const string SQL_SELECT_ALL_FROM_FMT = "select * from [{0}]";
    public const string SQL_WHERE_PREFIX = " where ";

    // Data
    public const string TABLE_DATA = "Data";

    //Client Custom Data Pattern 
    public const string AllowedClientCustomDataPattern = @"^[a-zA-Z0-9._-]+$";
}
