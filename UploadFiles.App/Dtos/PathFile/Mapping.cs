namespace UploadFiles.App.Dtos.PathFile;

public static class Mapping
{
    #region Output
    public static IEnumerable<PathFileOutputDto> ToListPathFileDto(this IEnumerable<Domain.Entities.PathFile> pathFiles)
    {
        return [.. from pathFile in pathFiles
                   select new PathFileOutputDto(
                        pathFile.Id,
                        pathFile.Path
                   )
        ];
    }
    public static IEnumerable<Domain.Entities.PathFile> ToListPathFileDto(this IEnumerable<PathFileOutputDto> pathFileOutputDtos)
        => pathFileOutputDtos.Select(s => s.ToPathFile());

    public static Domain.Entities.PathFile ToPathFile(this PathFileOutputDto pathFileOutputDto)
        => new(
            pathFileOutputDto.Id,
            pathFileOutputDto.PathFile
        );

    public static PathFileOutputDto ToPathFileOutputDto(this Domain.Entities.PathFile pathFileOutputDto)
        => new(
            pathFileOutputDto.Id,
            pathFileOutputDto.Path
        );
    #endregion

    #region Create
    public static PathFileCreateDto ToPathFileCreateDto(this Domain.Entities.PathFile pathFile)
        => new(pathFile.Path);

    public static Domain.Entities.PathFile ToPathFileCreate(this PathFileCreateDto pathFileCreateDto)
        => new(pathFileCreateDto.PathFile);
    #endregion

    #region Update
    public static Domain.Entities.PathFile ToPathFileUpdate(this PathFileUpdateDto pathFileUpdateDto)
        => new(
            pathFileUpdateDto.Id,
            pathFileUpdateDto.PathFile
        );
    #endregion
}
