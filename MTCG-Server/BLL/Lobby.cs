using MTCGServer.Models;
using System.Collections.Concurrent;

namespace MTCGServer.BLL
{
    public class Lobby
    {
        private ConcurrentQueue<User> lobby = new ConcurrentQueue<User>();
        private ConcurrentDictionary<User, string> _battleLog = new ConcurrentDictionary<User, string>();

        public Lobby() { }

        public string EnterALobby(User currentUser, ref bool updateUser)
        {
            User? waitingUser = null;
            if (lobby.TryDequeue(out waitingUser))
            {
                Battle battlefield = new Battle(currentUser, waitingUser);
                string battleLog = battlefield.Start_Fight(ref updateUser);
                _battleLog.AddOrUpdate(waitingUser, battleLog, (userToUpdate, oldBattleLog) => battleLog); //die ersten 2 Parameter sind für wenn es noch nicht existiert Der 3. Parameter is eine Funktion
                return battleLog;
            }
            else
            {
                _battleLog.TryAdd(currentUser, "");
                lobby.Enqueue(currentUser);
                string? battleLog = "";
                while (battleLog == "" || battleLog == null)
                {
                    _battleLog.TryGetValue(currentUser, out battleLog);
                }
                return battleLog!;
            }
        }
    }
}
