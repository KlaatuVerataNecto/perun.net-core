namespace infrastucture.libs.image
{
    public interface IImageConfig
    {
        
        string SourceFilePath { get; }
        
        string SaveFilePath { get; }
        
        int Quality { get; }
        
        int? X { get; }
        
        int? Y { get; }
        
        int? Width { get; }
        
        int? Height { get; }

        int? MaxWidth { get; }

        int? MaxHeight { get; }
    }
}