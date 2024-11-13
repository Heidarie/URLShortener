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
        } while (await repository.GetById(sb.ToString()) is not null);

        return sb.ToString();
    }
}