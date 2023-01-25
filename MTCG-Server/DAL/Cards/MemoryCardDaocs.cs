using MTCGServer.Models;

namespace MTCGServer.DAL.Cards
{
    public class MemoryCardDaocs : ICardDao
    {
        public bool ConfigureDeck(User user, List<Guid> guids)
        {
            throw new NotImplementedException();
        }

        public List<Card> GetDeck(User user)
        {
            throw new NotImplementedException();
        }

        public List<Card> GetStack(User user)
        {
            throw new NotImplementedException();
        }
    }
}
