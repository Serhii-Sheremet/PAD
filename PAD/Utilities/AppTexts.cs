namespace PAD
{
    public class AppTexts
    {
        public int Id { get; set; }
        public string NativeText { get; set; }
        public string ForeignText { get; set; }
        public string LanguageCode { get; set; }

        public AppTexts ParseFile(string s)
        {
            var row = s.Split(new char[] { '|' });
            return new AppTexts() { NativeText = row[0], ForeignText = row[1], LanguageCode = row[2] };
        }
    }
}
