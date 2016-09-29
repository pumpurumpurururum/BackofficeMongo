using System;

namespace BaseCms.Views.List.ListViewMetadata.Enums
{
    [Flags]
    public enum ActionButtons
    {
        EditButton = 1,
        DeleteButton = 2,
        UnlinkButton = 4,
        MoveButtons = 8,
        CustomButtons = 16
    }
}
