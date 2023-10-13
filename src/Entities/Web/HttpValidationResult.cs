namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Entities.Web {
    public class HtmlValidationResult {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }

        public HtmlValidationResult() {
            Success = false;
            ErrorMessage = "Invalid";
        }
    }
}
