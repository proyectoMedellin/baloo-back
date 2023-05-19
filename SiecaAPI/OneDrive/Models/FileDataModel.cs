namespace SiecaAPI.OneDrive.Models;

internal class FileDataModel
{
    public string ItemId { get; set; }

    public string DownloadUrl { get; set; }

    public string Name { get; set; }

    public string NewName { get; set; }

    public bool IsDownloaded { get; set; }

    public byte[] Bytes { get; set; }
}
