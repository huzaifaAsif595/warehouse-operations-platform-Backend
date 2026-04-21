using Microsoft.Extensions.Logging;

namespace PeakLogix.PickProApi.Common.Helpers;

/// <summary>
/// Provides reusable validation methods that can be used across repositories and services
/// </summary>
public static class ValidationHelper
{
    /// <summary>
    /// Validates that string parameters are not null or whitespace
    /// </summary>
    /// <param name="parameters">Array of tuples containing the value and parameter name</param>
    /// <returns>Tuple indicating success status and error message if validation fails</returns>
    public static (bool Success, string? ErrorMessage) ValidateStringParameters(params (string value, string paramName)[] parameters)
    {
        foreach (var (value, paramName) in parameters)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return (false, $"{paramName} cannot be null or empty");
            }
        }
        return (true, null);
    }

    /// <summary>
    /// Validates string parameters and logs warnings if validation fails
    /// </summary>
    /// <param name="logger">Logger instance for logging warnings</param>
    /// <param name="methodName">Name of the calling method for logging context</param>
    /// <param name="parameters">Array of tuples containing the value and parameter name</param>
    /// <returns>True if all parameters are valid, false otherwise</returns>
    public static bool ValidateStringParametersWithLogging(ILogger logger, string methodName, params (string value, string paramName)[] parameters)
    {
        foreach (var (value, paramName) in parameters)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                logger.LogWarning("{MethodName} called with null or empty {ParameterName} parameter", methodName, paramName);
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// Validates that a collection is not null or empty
    /// </summary>
    /// <param name="collection">Collection to validate</param>
    /// <param name="paramName">Parameter name for error message</param>
    /// <returns>Tuple indicating success status and error message if validation fails</returns>
    public static (bool Success, string? ErrorMessage) ValidateCollection<T>(IEnumerable<T>? collection, string paramName)
    {
        if (collection == null)
        {
            return (false, $"{paramName} cannot be null");
        }

        if (!collection.Any())
        {
            return (false, $"{paramName} cannot be empty");
        }

        return (true, null);
    }

    /// <summary>
    /// Validates that an object is not null
    /// </summary>
    /// <param name="obj">Object to validate</param>
    /// <param name="paramName">Parameter name for error message</param>
    /// <returns>Tuple indicating success status and error message if validation fails</returns>
    public static (bool Success, string? ErrorMessage) ValidateNotNull(object? obj, string paramName)
    {
        if (obj == null)
        {
            return (false, $"{paramName} cannot be null");
        }
        return (true, null);
    }

    /// <summary>
    /// Validates that a numeric value is within a specified range
    /// </summary>
    /// <param name="value">Value to validate</param>
    /// <param name="min">Minimum allowed value (inclusive)</param>
    /// <param name="max">Maximum allowed value (inclusive)</param>
    /// <param name="paramName">Parameter name for error message</param>
    /// <returns>Tuple indicating success status and error message if validation fails</returns>
    public static (bool Success, string? ErrorMessage) ValidateRange(int value, int min, int max, string paramName)
    {
        if (value < min || value > max)
        {
            return (false, $"{paramName} must be between {min} and {max}");
        }
        return (true, null);
    }

    /// <summary>
    /// Validates that a numeric value is within a specified range
    /// </summary>
    /// <param name="value">Value to validate</param>
    /// <param name="min">Minimum allowed value (inclusive)</param>
    /// <param name="max">Maximum allowed value (inclusive)</param>
    /// <param name="paramName">Parameter name for error message</param>
    /// <returns>Tuple indicating success status and error message if validation fails</returns>
    public static (bool Success, string? ErrorMessage) ValidateRange(decimal value, decimal min, decimal max, string paramName)
    {
        if (value < min || value > max)
        {
            return (false, $"{paramName} must be between {min} and {max}");
        }
        return (true, null);
    }

    /// <summary>
    /// Validates that a string matches a specific format using regex
    /// </summary>
    /// <param name="value">String to validate</param>
    /// <param name="pattern">Regex pattern to match</param>
    /// <param name="paramName">Parameter name for error message</param>
    /// <returns>Tuple indicating success status and error message if validation fails</returns>
    public static (bool Success, string? ErrorMessage) ValidateFormat(string value, string pattern, string paramName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return (false, $"{paramName} cannot be null or empty");
        }

        if (!System.Text.RegularExpressions.Regex.IsMatch(value, pattern))
        {
            return (false, $"{paramName} does not match the required format");
        }

        return (true, null);
    }

    /// <summary>
    /// Validates multiple conditions and returns the first error encountered
    /// </summary>
    /// <param name="validations">Array of validation results</param>
    /// <returns>Tuple indicating success status and error message if any validation fails</returns>
    public static (bool Success, string? ErrorMessage) ValidateMultiple(params (bool Success, string? ErrorMessage)[] validations)
    {
        foreach (var validation in validations)
        {
            if (!validation.Success)
            {
                return validation;
            }
        }
        return (true, null);
    }

    /// <summary>
    /// Validates that a column name exists in the allowed list of columns (Whitelisting approach)
    /// This is the most secure method to prevent SQL injection through column names
    /// </summary>
    /// <param name="columnName">The column name to validate</param>
    /// <param name="allowedColumns">List of allowed column names</param>
    /// <returns>The validated column name from the whitelist</returns>
    /// <exception cref="ArgumentException">Thrown when column name is not in whitelist or is invalid</exception>
    public static string ValidateColumnWhitelist(string columnName, IEnumerable<string> allowedColumns)
    {
        if (string.IsNullOrWhiteSpace(columnName))
            throw new ArgumentException("Column name cannot be null or empty", nameof(columnName));
        
        if (allowedColumns == null || !allowedColumns.Any())
            throw new ArgumentException("Allowed columns list cannot be null or empty", nameof(allowedColumns));
        
        // Case-insensitive comparison for column names
        var allowedColumnsList = allowedColumns.Select(c => c.Trim()).ToList();
        var matchedColumn = allowedColumnsList.FirstOrDefault(c => 
            c.Equals(columnName.Trim(), StringComparison.OrdinalIgnoreCase));
        
        if (matchedColumn == null)
            throw new ArgumentException($"Column '{columnName}' is not in the allowed columns list", nameof(columnName));
        
        return matchedColumn; // Return the exact column name from whitelist
    }
}
