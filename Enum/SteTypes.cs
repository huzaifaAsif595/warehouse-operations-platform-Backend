using System.ComponentModel;

namespace PeakLogix.PickProApi.Common.Enum
{
    public static class SteTypes
    {
        public enum ImportPostActions
        {
            None,
            ArchiveFile
        }

        public enum ImportSourceTypes
        {
            WebAPI,
            DB_Table,
            Text_File,
            Socket
        }

        public enum ImportTypesFormats
        {
            CSV,
            FixedLength,
            XML,
            JSON,
            EDI
        }

        public enum ImportDataTypes
        {
            String,
            DateTime,
            Numeric,
            Bool,
            JSON,
            Base64
        }


        public static string ToImportPostActionsString(this ImportPostActions action)
        {
            return action switch
            {
                ImportPostActions.None => string.Empty,
                ImportPostActions.ArchiveFile => "Archive File",
                _ => "Unknown Post Action"
            };
        }

        public static string ToImportTypeFormatsString(this ImportTypesFormats importType)
        {
            return importType switch
            {
                ImportTypesFormats.CSV => "CSV",
                ImportTypesFormats.FixedLength => "Fixed Length",
                ImportTypesFormats.XML => "XML",
                ImportTypesFormats.JSON => "JSON",
                ImportTypesFormats.EDI => "EDI",
                _ => "Unknown Import Type"
            };
        }

        public static string ToImportSourceTypesString(this ImportSourceTypes sourceType)
        {
            return sourceType switch
            {
                ImportSourceTypes.WebAPI => "Web API",
                ImportSourceTypes.DB_Table => "DB Table",
                ImportSourceTypes.Text_File => "Text File",
                ImportSourceTypes.Socket => "Socket",
                _ => "Unknown Source Type"
            };
        }

        public static string ToImportDataTypesString(this ImportDataTypes dataType)
        {
            return dataType switch
            {
                ImportDataTypes.String => "String Type",
                ImportDataTypes.Numeric => "Numeric Type",
                ImportDataTypes.DateTime => "DateTime Type",
                ImportDataTypes.Bool => "Bool Type",
                ImportDataTypes.JSON => "JSON Type",
                ImportDataTypes.Base64 => "Base64 Type",
                _ => "Unknown Data Type"
            };
        }
    }
}
