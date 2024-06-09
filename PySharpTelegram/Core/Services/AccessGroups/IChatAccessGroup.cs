using Telegram.Bot.Types;

namespace PySharpTelegram.Core.Services.AccessGroups;

public interface IChatAccessGroup
{
    public Task<User[]> GetGroupMembersAsync(params string[] accessGroupName);
    public Task<bool> AddMembersToGroupAsync<T>(string accessGroupName, params User[] members);
    public Task<bool> RemoveMemberFromGroupAsync<T>(string accessGroupName, params User[] members);
}