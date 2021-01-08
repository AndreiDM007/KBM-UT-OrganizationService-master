using System.Text.RegularExpressions;

namespace Kebormed.Core.OrganizationService.Web.Business.Errors
{
    public static class ErrorHandlingHelper
    {
        private const string MatchPattern = "(?<=.)([A-Z])";
        private const string SnakeCasePattern = "_$0";

        /// <summary>
        /// Converts "CamelCase" pattern to "lower_case_with_underscore" pattern
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToLowerSnakeCase(this string input) => Regex.Replace(input, MatchPattern, SnakeCasePattern, RegexOptions.Compiled);
    }
}
