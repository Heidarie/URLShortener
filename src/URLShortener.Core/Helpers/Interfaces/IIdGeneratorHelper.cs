namespace URLShortener.Core.Helpers.Interfaces;

internal interface IIdGeneratorHelper
{
    Task<string> GenerateId(int length = 6);
}