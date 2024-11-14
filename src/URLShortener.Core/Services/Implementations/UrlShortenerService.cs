using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;
using URLShortener.Abstractions.Services.Interfaces;
using URLShortener.Core.Aggregates;
using URLShortener.Core.Exceptions;
using URLShortener.Core.Helpers.Interfaces;
using URLShortener.Core.Repositories;
using URLShortener.Shared.DTOS.Input;
using URLShortener.Shared.DTOS.Output;

namespace URLShortener.Core.Services.Implementations;

internal class UrlShortenerService(IUrlShortenerRepository repository, IIdGeneratorHelper idGeneratorHelper, ILogger<UrlShortenerService> logger) : IUrlShortenerService
{
    private static readonly Regex IdRegex = new(@"^[a-zA-Z0-9]+$", RegexOptions.Compiled);

    public async Task<string?> GetById(string id)
    {
        try
        {
            var entity = await repository.GetById(id);

            return entity?.ExpiresAt < DateTime.UtcNow ? null : entity?.OriginalUrl;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to get shortened URL by ID");
            throw new UrlRetrievalFailedException();
        }
    }

    public async Task<ShortenedUrlDto> Create(CreateShortenedUrlDto dto)
    {
        ShortenedUrlAggregate aggregate;

        if (!string.IsNullOrEmpty(dto.Id))
        {
            if (!IdRegex.IsMatch(dto.Id))
            {
                throw new IdPatternNotMatchedException(dto.Id);
            }

            var entity = await repository.GetById(dto.Id);

            if (entity is not null && (entity.ExpiresAt is null || entity.ExpiresAt.Value > DateTime.UtcNow))
            {
                throw new IdAlreadyExistsException(dto.Id);
            }

            try
            {
                if (entity is not null)
                {
                    aggregate = new ShortenedUrlAggregate(entity);
                    aggregate.SetNewUrl(dto.OriginalUrl);
                    await repository.Update(aggregate.ToEntity());
                }
                else
                {
                    aggregate = new ShortenedUrlAggregate(dto);
                    await repository.Create(aggregate.ToEntity());
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to create shortened URL");
                throw new UrlCreationFailedException();
            }
        }
        else
        {
            try
            {
                string id = await idGeneratorHelper.GenerateId();
                aggregate = new ShortenedUrlAggregate(id, dto);
                await repository.Create(aggregate.ToEntity());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to create shortened URL");
                throw new UrlCreationFailedException();
            }
        }

        return new ShortenedUrlDto(aggregate.Id, aggregate.OriginalUrl);
    }



    public async Task Delete(string id)
    {
        var entity = await repository.GetById(id);

        if (entity is null)
        {
            throw new EntityNotFoundException(id);
        }

        try
        {
            await repository.Delete(entity);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to delete shortened URL");
            throw new UrlDeletionFailedException();
        }
    }
}