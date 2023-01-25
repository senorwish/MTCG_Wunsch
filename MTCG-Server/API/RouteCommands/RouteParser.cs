using System.Data;
using System.Text.RegularExpressions;

namespace MTCGServer.API.RouteCommands
{
    public class RouteParser : IRouteParser
    {
        public bool IsMatch(string resourcePath, string routePattern)
        {
            string patternUsername = "^" + routePattern.Replace("{username}", ".*").Replace("/", "\\/") + "(\\?.*)?$";
            string patternTradingId = "^" + routePattern.Replace("{id}", ".*").Replace("/", "\\/") + "(\\?.*)?$";

            switch (routePattern)
            {
                case "/users/{username}": return Regex.IsMatch(resourcePath, patternUsername); //return true if the resource Path matches the pattern
                case "/tradings/{id}": return Regex.IsMatch(resourcePath, patternTradingId);
                case "/deck{query}": return (resourcePath == "/deck" || resourcePath == "/deck?format=plain" || resourcePath == "/deck?format=json");

                default: throw new NotImplementedException();
            }

        }

        public Dictionary<string, string> ParseParameters(string resourcePath, string routePattern)
        {
            // query parameters
            var parameters = ParseQueryParameters(resourcePath);
            // id parameter
            var parameter = GetParameterValue(resourcePath, routePattern);
            if (parameter != null)
            {
                switch (routePattern)
                {
                    case "/users/{username}":
                        {
                            parameters["username"] = parameter;
                            return parameters;
                        }
                    case "/tradings/{id}":
                        {
                            parameters["id"] = parameter;
                            return parameters;
                        }
                    default: break;
                }

            }
            return parameters;
        }

        private string? GetParameterValue(string resourcePath, string routePattern)
        {
            string patternUsername = "^" + routePattern.Replace("{username}", "(?<username>[^\\?\\/]*)").Replace("/", "\\/") + "$";
            string patternTradingId = "^" + routePattern.Replace("{id}", "(?<id>[^\\?\\/]*)").Replace("/", "\\/") + "$";

            switch (routePattern)
            {
                case "/users/{username}":
                    {
                        var result = Regex.Match(resourcePath, patternUsername);
                        return result.Groups["username"].Success ? result.Groups["username"].Value : null;
                    }
                case "/tradings/{id}":
                    {
                        var result = Regex.Match(resourcePath, patternTradingId);
                        return result.Groups["id"].Success ? result.Groups["id"].Value : null;
                    }

                default: throw new NotImplementedException();
            }
        }

        private Dictionary<string, string> ParseQueryParameters(string route)
        {
            var parameters = new Dictionary<string, string>();

            var query = route
                .Split("?", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                .Skip(1)
                .FirstOrDefault();

            if (query != null)
            {
                var keyValues = query.Split("&")
                    .Select(p => p.Split("="))
                    .Where(p => p.Length == 2);

                foreach (var p in keyValues)
                {
                    parameters[p[0].Trim()] = p[1].Trim();
                }
            }

            return parameters;
        }
    }
}
