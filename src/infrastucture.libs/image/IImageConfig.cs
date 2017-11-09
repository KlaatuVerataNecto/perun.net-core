namespace infrastucture.libs.image
{
    public interface IImageConfig
    {
        
        string SourceFilePath { get; }
        
        string SaveFilePath { get; }
        
        int Quality { get; }
        
        int? Width { get; }
        
        int? Height { get; }
        
    }
}