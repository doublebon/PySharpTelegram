using PySharpTelegram.Core.Services.AccessGroups;
using Telegram.Bot.Types;

namespace TestApp.AccessGroups;

public class AccessGroup : IAccessGroup
{
    public Task<User[]> GetGroupMembersAsync(params string[] accessGroupName)
    {
        return Task.FromResult(Array.Empty<User>());
    }
    
    public Task<bool> AddMembersToGroupAsync<T>(string accessGroupName, params User[] members)
    {
        return Task.FromResult(false);
    }

    public Task<bool> RemoveMemberFromGroupAsync<T>(string accessGroupName, params User[] members)
    {
        return Task.FromResult(false);
    }
}