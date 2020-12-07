namespace play_with_me.common.Models
{
    public class SearchItem
    {
        public string Url { get; set; }
        public string Title { get; set; }
        public bool Instock { get; set; }
    }

    public class TargetSearchItem : SearchItem
    {
        public string Status { get; set; }
        public string Tcin { get; set; }
        public string Dpci { get; set; }
        public string Upc { get; set; }
    }
}