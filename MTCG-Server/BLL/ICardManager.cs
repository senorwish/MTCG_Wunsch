using MTCGServer.Models;

namespace MTCGServer.BLL
{
    public interface ICardManager
    {
        List<Card> GetStack(User user);
        List<Card> GetDeck(User user);
        bool ConfigureDeck(User user, List<Guid> guids);
    }
}
