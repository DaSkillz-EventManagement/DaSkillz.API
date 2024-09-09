namespace Application.Abstractions.AvatarApi
{
    public interface IAvatarApiClient
    {
        string GetRandomAvatarUrl();
        string GetRandomBoyAvatarUrl();
        string GetRandomGirlAvatarUrl();
        string GetAvatarUrlWithName(string FullName);
    }
}
