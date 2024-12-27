using Application.Santa.Areas.Partners.BaseModels;
using Global.Abstractions.Global.Partners;
using Global.Extensions.System;
using Microsoft.AspNetCore.Mvc.Rendering;
using static Global.Settings.PartnerSettings;

namespace ViewLayer.Models.Partners;

public class RelationshipVm : RelationshipBase, IRelationship
{
    public override bool SuggestedByCurrentUser { get; set; }
    public string SharedGroupsDisplay => string.Join(", ", SharedGroupsDisplay);

    public List<SelectListItem> AvailableStatuses => GetAvailableStatusSelect();

    private List<SelectListItem> GetAvailableStatusSelect()
    {
        List<RelationshipStatus> availableStatuses = GetAvailableStatuses();
        return availableStatuses.ToSelectList();
    }

    private List<RelationshipStatus> GetAvailableStatuses()
    {
        return Status switch
        {
            RelationshipStatus.ToBeConfirmed => [RelationshipStatus.ToBeConfirmed, RelationshipStatus.Ended],
            RelationshipStatus.ToConfirm => [RelationshipStatus.ToConfirm, RelationshipStatus.Active, RelationshipStatus.Ended],
            RelationshipStatus.Active => [RelationshipStatus.Active, RelationshipStatus.Ended, RelationshipStatus.IgnoreOld],
            RelationshipStatus.Ended => [RelationshipStatus.Ended, RelationshipStatus.IgnoreOld],
            RelationshipStatus.IgnoreOld => [RelationshipStatus.Ended, RelationshipStatus.IgnoreOld],
            _ => throw new NotImplementedException()
        };
    }
}
