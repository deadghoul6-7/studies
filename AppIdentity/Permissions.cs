namespace ProjectAspEShop2024.AppIdentity
{
    public static class Permissions
    {
        public static class Reviews
        {
            public const string Edit = "Permissions.Reviews.Edit";
            public const string Create = "Permissions.Reviews.Create";
            public const string Approve = "Permissions.Reviews.Approve";
        }

        public static class Categories
        {
            public const string Edit = "Permissions.Categories.Edit";
            public const string Create = "Permissions.Categories.Create";
        }

        public static class Products
        {
            public const string EditPrice = "Permissions.Products.EditPrice";
            public const string EditQuantity = "Permissions.Products.EditQuantity";
            public const string EditPicture = "Permissions.Products.EditPicture";
        }
    }
}
