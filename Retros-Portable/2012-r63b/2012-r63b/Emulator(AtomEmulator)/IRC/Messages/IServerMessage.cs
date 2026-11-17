namespace Atom.IRC.Messages
{
    interface IServerMessage
    {
        string SerializeForChannel(string channel);
        string SerializeForUser(string username);
    }
}
