package com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.intent;

/**
 * Specifies the Intent Architect instruction mode for the specific targets
 * Instructions can be combined by using the | bitwise operation. For example: @IntentManageClass(members = Mode.CanAdd | Mode.CanRemove)
 * In this case, Intent Architect will only add or remove members, but never update existing ones.
 */
public final class Mode {
    /**
     *  Inherit instructions (do nothing)
     */
    public static final int Default = 0;
    /**
     *  Instructs Intent Architect to ignore these elements
     */
    public static final int Ignore = 1;
    /**
     *  Allows Intent Architect to add or update, but not remove, elements
     */
    public static final int Merge = 2;
    /**
     *  Allows Intent Architect to add, update, and remove elements
     */
    public static final int Manage = 4;
    /**
     *  Allows Intent Architect to add elements
     */
    public static final int CanAdd = 8;
    /**
     *  Allows Intent Architect to update elements
     */
    public static final int CanUpdate = 16;
    /**
     *  Allows Intent Architect to remove elements
     */
    public static final int CanRemove = 32;
}