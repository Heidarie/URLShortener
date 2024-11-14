using System.Text;
using URLShortener.Core.Helpers.Interfaces;
using URLShortener.Core.Repositories;

namespace URLShortener.Core.Helpers.Implementations;

internal class IdGeneratorHelper(IUrlShortenerRepository repository) : IIdGeneratorHelper
{
    private const string Alphanumeric = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    public async Task<string> GenerateId(int length = 6)
    {
        var random = new Random();
        var sb = new StringBuilder();

        do
        {
            sb.Clear();
            for (int i = 0; i < length; i++)
            {
                sb.Append(Alphanumeric[random.Next(Alphanumeric.Length)]);
            }
        } while (await IsIdTaken(sb.ToString()));

        return sb.ToString();
    }

    private async Task<bool> IsIdTaken(string id)
    {
        var entity = await repository.GetById(id);

        return entity is not null && (entity.ExpiresAt is null || entity.ExpiresAt.Value > DateTime.UtcNow);
    }
}