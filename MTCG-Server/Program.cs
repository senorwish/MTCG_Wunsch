using MTCGServer.API.RouteCommands;
using MTCGServer.BLL;
using MTCGServer.Core.Server;
using MTCGServer.DAL;
using System.Net;

string connectionString = "Server=localhost;Port=10002;User Id=postgres;Password=admin;Database=MTCGDB;";
var database = new Database(connectionString);
var userDao = database.UserDao;
var packageDao = database.PackageDao;
var cardDao = database.CardDao;
var gameDao = database.GameDao;
var tradeDao = database.TradeDao;

var userManager = new UserManager(userDao);
var packageManager = new PackageManager(packageDao);
var cardsManager = new CardManager(cardDao);
var gameManager = new GameManager(gameDao);
var tradingManager = new TradingManager(tradeDao);

Lobby lobby = new Lobby();
Router router = new Router(userManager, packageManager, cardsManager, gameManager, tradingManager, lobby);
HttpServer server = new HttpServer(IPAddress.Any, 10001, router);
server.Start();