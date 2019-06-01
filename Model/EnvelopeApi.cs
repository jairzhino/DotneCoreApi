namespace Backend.Model{
    public class EnvelopeApi
    {
        public bool isError { get; set; }
        public int status { get; set; }
        public object data { get; set; }
        public object error { get; set; }
    }
}