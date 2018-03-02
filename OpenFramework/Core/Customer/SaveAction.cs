namespace OpenFramework.Customer
{
    /// <summary>Enumeration of save actions available</summary>
    public enum SaveAction
    {
        /// <summary>Save item and maintains page</summary>
        SaveOnly = 0,

        /// <summary>Save item and leave page</summary>
        SaveAndClose = 1,

        /// <summary>Combine "save only" and "save and close"</summary>
        Both = 2,

        /// <summary>Save item and prepare page for a new item</summary>
        SaveAndNew = 3
    }
}