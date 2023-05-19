namespace SiecaAPI.DTO.Data
{
    public class DtoMenu
    {
        public Guid Id { get; set; }
        public Guid OrganizationId { get; set; }
        public string MenuLabel { get; set; }
        public string? Description { get; set; }
        public Guid? ParentMenuId { get; set; }
        public int Position { get; set; }
        public string? ExternalUrl { get; set; }
        public string? Route { get; set; }
        public Guid PermissionId { get; set; }
        public string CreatedBy { get; set; }

        public DtoMenu(Guid organizationId, string menuLabel, string? description, Guid? parentMenuId, int Position, string? externalUrl,
             string? route, Guid permissionId, string createdBy)
        {
            OrganizationId = organizationId;
            MenuLabel = menuLabel;
            Description = description;
            ParentMenuId = parentMenuId;
            ExternalUrl = externalUrl;
            Route = route;
            PermissionId = permissionId;
            CreatedBy = createdBy;
        }
        public DtoMenu(Guid id, Guid organizationId, string menuLabel, string description, Guid? parentMenuId, int Position, string externalUrl,
             string route, Guid permissionId, string createdBy)
        {
            Id = id;
            OrganizationId = organizationId;
            MenuLabel = menuLabel;
            Description = description;
            ParentMenuId = parentMenuId;
            ExternalUrl = externalUrl;
            Route = route;
            PermissionId = permissionId;
            CreatedBy = createdBy;
        }
    }
}
