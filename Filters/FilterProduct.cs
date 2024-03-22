namespace ProjectAspEShop2024.Filters
{
    public enum SortType
    {
        None = 0,
        Ascending = 1,
        Descending = 2
    }

    public class FilterProduct
    {
        public long? CategoryId { get; set; }

        public long? BrandId { get; set; }

        public SortType SortByName { get; set; }

        public SortType SortByPrice { get; set; }

        public int MinPrice { get; set; }

        public int MaxPrice { get; set; }

        public string? SearchText { get; set; }

        public List<long>? ListSelectedBrandsId { get; set; }

        public FilterProduct()
        {
            SortByName = SortType.None;
            SortByPrice = SortType.None;
            MaxPrice = int.MaxValue;
        }
    }
}
