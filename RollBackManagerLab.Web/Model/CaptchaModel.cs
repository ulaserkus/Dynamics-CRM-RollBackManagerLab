namespace RollBackManagerLab.Web.Model
{
    public class CaptchaModel
    {
        public string Text { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public bool ValidateCaptcha(string expectedCaptchaText)
        {
            return Text.ToUpper() == expectedCaptchaText.ToUpper();
        }
    }
}
