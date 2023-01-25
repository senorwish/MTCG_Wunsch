using MTCGServer.Models;

namespace MTCGServer.DAL.Cards
{
    public interface ICardDao
    {
        bool ConfigureDeck(User user, List<Guid> guids);
        List<Card> GetDeck(User user);
        List<Card> GetStack(User user);
    }
}
