namespace SiecaAPI.DTO.Response
{
    public class DtoMenuResp
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

        public DtoMenuResp(Guid organizationId, string menuLabel, string? description, Guid? parentMenuId, int Position, string? externalUrl,
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
        public DtoMenuResp(Guid id, Guid organizationId, string menuLabel, string description, Guid? parentMenuId, int Position, string externalUrl,
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
