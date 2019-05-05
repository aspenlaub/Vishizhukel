using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Entities.Web;

namespace Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Web {
    public interface ISecuredHttpGate {
        Task<HtmlValidationResult> IsHtmlMarkupValidAsync(string markup);
        Task<bool> RegisterDefectAsync(string headline, string description, bool old);
    }
}
