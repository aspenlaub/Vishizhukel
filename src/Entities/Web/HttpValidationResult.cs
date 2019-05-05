namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Entities.Web {
    public class HtmlValidationResult {
        public bool Success;
        public string ErrorMessage;

        public HtmlValidationResult() {
            Success = false;
            ErrorMessage = "Invalid";
        }
    }
}
