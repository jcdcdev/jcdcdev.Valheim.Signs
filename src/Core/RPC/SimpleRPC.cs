using System.Collections;
using Jotunn;
using Jotunn.Entities;
using Jotunn.Managers;

namespace jcdcdev.Valheim.Core.RPC;

public abstract class SimpleRPC : ISimpleRPC
{
    private CustomRPC _rpc = null!;

    public void Send(long peerId, ZPackage pkg)
    {
        Logger.LogDebug($"\n\nRPC SENT - {_rpc.Name}\n\nRecipient: {peerId}\n");
        _rpc.SendPackage(peerId, pkg);
    }

    public void Send(long peerId, string? message) => Send(peerId, CreatePackage(message));
    public void Send(long peerId, object? model) => Send(peerId, JsonHelper.ToJson(model));
    public void SendAll(ZPackage pkg) => Send(ZRoutedRpc.Everybody, pkg);
    public void SendAll(string? message) => Send(ZRoutedRpc.Everybody, message);
    public void SendAll(object? model) => Send(ZRoutedRpc.Everybody, model);
    public void SendToServer(ZPackage pkg) => Send(ZRoutedRpc.instance.GetServerPeerID(), pkg);
    public void SendToServer(string? message) => SendToServer(CreatePackage(message));
    public void SendToServer(object? model) => SendToServer(JsonHelper.ToJson(model));

    public IEnumerator ClientInternal(long sender, ZPackage? pkg)
    {
        Logger.LogDebug($"\n\nRPC RECEIVED - {_rpc.Name}\n\nRecipient Type: CLIENT\nSender: {sender}\n");
        yield return Client(sender, pkg);
    }

    public IEnumerator ServerInternal(long sender, ZPackage? pkg)
    {
        Logger.LogDebug($"\n\nRPC RECEIVED - {_rpc.Name}\n\nRecipient Type: SERVER\nSender: {sender}\n");
        yield return Server(sender, pkg);
    }

    public void Initialise(NetworkManager instance)
    {
        var name = GetType().Name;
        Logger.LogInfo($"Initialising RPC for {name}");
        _rpc = NetworkManager.Instance.AddRPC(name, Server, Client);
    }

    protected static IEnumerator Noop(long sender, ZPackage? pkg) { yield break; }

    private static ZPackage CreatePackage(string? message)
    {
        var package = new ZPackage();
        package.Write(message);
        return package;
    }

    public abstract IEnumerator Client(long sender, ZPackage? pkg);

    public abstract IEnumerator Server(long sender, ZPackage? pkg);
}
