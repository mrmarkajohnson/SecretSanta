using Application.Areas.Partners.BaseModels;
using Global.Abstractions.Areas.Partners;
using Global.Extensions.System;
using Microsoft.AspNetCore.Mvc.Rendering;
using static Global.Settings.PartnerSettings;

namespace ViewLayer.Models.Partners;

public class RelationshipVm : RelationshipBase, IRelationship
{
    public override bool SuggestedByCurrentUser { get; set; }
    public string SharedGroupsDisplay => string.Join(", ", SharedGroupNames);

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
            RelationshipStatus.ToBeConfirmed => [RelationshipStatus.ToBeConfirmed, RelationshipStatus.EndedBeforeConfirmation],
            RelationshipStatus.ToConfirm => [RelationshipStatus.ToConfirm, RelationshipStatus.Active, RelationshipStatus.Ended, RelationshipStatus.IgnoreNonRelationship, RelationshipStatus.Avoid],
            RelationshipStatus.Active => [RelationshipStatus.Active, RelationshipStatus.Ended, RelationshipStatus.IgnoreOld],
            RelationshipStatus.Ended => [RelationshipStatus.Ended, RelationshipStatus.IgnoreOld],
            RelationshipStatus.EndedBeforeConfirmation => [RelationshipStatus.EndedBeforeConfirmation, RelationshipStatus.IgnoreOld],
            RelationshipStatus.IgnoreOld => [RelationshipStatus.Ended, RelationshipStatus.IgnoreOld],
            _ => [Status]
        };
    }
}
