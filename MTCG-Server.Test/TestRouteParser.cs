using MTCGServer.API.RouteCommands;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace MTCGServer.Test
{
    internal class TestRouteParser
    {
       
            private RouteParser _routeParser;

            [OneTimeSetUp]
            public void Initialize()
            {
                _routeParser = new RouteParser();
            }

            [Test]
            public void TestIsMatch_WithValidUsername()
            {
                // Arrange
                var resourcePath = "/users/john";
                var routePattern = "/users/{username}";

                // Act
                var result = _routeParser.IsMatch(resourcePath, routePattern);

                // Assert
                Assert.IsTrue(result);
            }

            [Test]
            public void TestIsMatch_WithInvalidUsername()
            {
                // Arrange
                var resourcePath = "/users/john?age=20";
                var routePattern = "/users/{username}";

                // Act
                var result = _routeParser.IsMatch(resourcePath, routePattern);

                // Assert
                Assert.IsTrue(result);
            }

            [Test]
            public void TestIsMatch_WithValidId()
            {
                // Arrange
                var resourcePath = "/tradings/123";
                var routePattern = "/tradings/{id}";

                // Act
                var result = _routeParser.IsMatch(resourcePath, routePattern);

                // Assert
                Assert.IsTrue(result);
            }

            [Test]
            public void TestIsMatch_WithInvalidId()
            {
                // Arrange
                var resourcePath = "/tradings/123?action=cancel";
                var routePattern = "/tradings/{id}";

                // Act
                var result = _routeParser.IsMatch(resourcePath, routePattern);

                // Assert
                Assert.IsTrue(result);
            }

            [Test]
            public void TestIsMatch_WithValidDeck()
            {
                // Arrange
                var resourcePath = "/deck";
                var routePattern = "/deck{query}";

                // Act
                var result = _routeParser.IsMatch(resourcePath, routePattern);

                // Assert
                Assert.IsTrue(result);
            }

            [Test]
            public void TestIsMatch_WithInvalidDeck()
            {
                // Arrange
                var resourcePath = "/deck?format=html";
                var routePattern = "/deck{query}";

                // Act
                var result = _routeParser.IsMatch(resourcePath, routePattern);

                // Assert
                Assert.IsFalse(result);
            }

         
        
    }
        }
