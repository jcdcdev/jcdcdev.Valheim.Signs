using System.Collections;
using Jotunn.Managers;

namespace jcdcdev.Valheim.Core.RPC;

public interface ISimpleRPC
{
    void Send(long peerId, ZPackage pkg);
    void Send(long peerId, string? message);
    void Send(long peerId, object? model);
    void SendAll(ZPackage pkg);
    void SendAll(string? message);
    void SendAll(object? model);
    void SendToServer(ZPackage pkg);
    void SendToServer(string? message);
    void SendToServer(object? model);
    IEnumerator ClientInternal(long sender, ZPackage? pkg);
    IEnumerator ServerInternal(long sender, ZPackage? pkg);
    void Initialise(NetworkManager instance);
}
